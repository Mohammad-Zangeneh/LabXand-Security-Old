using Kms.Security.Common.DataContract.Zar;
using Kms.Security.Common.Domain;
using Kms.Security.Identity.Service.Contracts;
using Kms.Security.Util;
using LabXand.Core;
using LabXand.DistributedServices;
using LabXand.Infrastructure.Data.Redis;
using LabXand.Logging.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Service
{
    public class ZarService : IZarService
    {
        private readonly IOrganizationService _organizationService;
        private IEnterprisePositionService _enterprisePositionService;
        private readonly IKmsApplicationUserManager _kmsApplicationUserManager;
        private readonly IRoleService _roleService;
        private readonly IEntityMapper<ApplicationUser, MemberDto> _memberMapper;
        private readonly IRedisCacheService _cacheManager;

        public ZarService(
            IKmsApplicationUserManager kmsApplicationUserManager,
            IEnterprisePositionService enterprisePositionService,
            IOrganizationService organizationService,
            IRoleService roleService,
            IEntityMapper<ApplicationUser, MemberDto> memberMapper,
            IRedisCacheService redisCacheService)
        {
            _kmsApplicationUserManager = kmsApplicationUserManager;
            _enterprisePositionService = enterprisePositionService;
            _organizationService = organizationService;
            _roleService = roleService;
            _memberMapper = memberMapper;
            _cacheManager = redisCacheService;
        }

        public IList<ZarApiDto> GetZarRawData()
        {
            var data = HttpUtil.PerformHttpGet(ConfigurationManager.AppSettings["ZarUsersWebService"]);
            List<ZarApiDto> result;
            using (var reader = new StreamReader(data.GetResponseStream(), Encoding.UTF8))
            {
                var responseText = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(responseText))
                {
                    throw new Exception();
                }

                result = JsonConvert.DeserializeObject<List<ZarApiDto>>(responseText);
            }
            result = result
                .Where(t => !string.IsNullOrWhiteSpace(t.CompanyCode))
                .Where(t => !string.IsNullOrWhiteSpace(t.OrganizationUnitText))
                .Where(t => !string.IsNullOrWhiteSpace(t.PersonelFirstName))
                .Where(t => !string.IsNullOrWhiteSpace(t.PersonelLastName))
                .Where(t => t.PersonalNumber != 0)
                .ToList();

            return result;
        }

        public void SyncData()
        {
            var zarData = GetZarRawData();
            var allowedOrganizationsCode = new List<string>() { "1010" };
            zarData = zarData.Where(z => allowedOrganizationsCode.Contains(z.CompanyCode)).ToList();
            SyncOrganizations(zarData, allowedOrganizationsCode);
            SyncEnterprisePositions(zarData, allowedOrganizationsCode);
            SyncUsers(zarData, allowedOrganizationsCode);
            new FileLogger().Log("C:\\temp\\Kms\\ZarDataSync", "Sync data " + Guid.NewGuid().ToString(), "txt", "Data Synced." + "\n" + DateTime.Now.ToString());
        }

        public void SyncOrganizations(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode)
        {
            var allowedOrganizations = zarData
                .Where(t => allowedOrganizationsCode.Contains(t.CompanyCode))
                .Select(t => new ZarCompanyDto { Name = t.CompanyText, Code = int.Parse(t.CompanyCode) })
                .ToList();
            allowedOrganizations = DistinctCompany(allowedOrganizations);
            var organizationsInDb = _organizationService.GetAll();
            var organizationsInDbCodes = organizationsInDb.Select(t => t.SortingNumber.ToString()).ToList();
            var organizationsToInsert = allowedOrganizations.Where(t => !organizationsInDbCodes.Contains(t.Code.ToString())).ToList();

            foreach (var org in organizationsToInsert)
            {
                var newOrgToSave = new OrganizationDto()
                {
                    Id = Guid.NewGuid(),
                    Name = org.Name,
                    SortingNumber = org.Code,
                };
                _organizationService.Save(newOrgToSave);
            }
            _cacheManager.RemoveDatabase(RedisDbType.Organization);
        }

        public void SyncEnterprisePositions(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode)
        {
            InsertNewEnterprisePositions(zarData, allowedOrganizationsCode);
            UpdateEnterprisePositions(zarData, allowedOrganizationsCode);
            _cacheManager.RemoveDatabase(RedisDbType.EnterprisePosition);
        }

        public void SyncUsers(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode)
        {
            var membersInDb = _kmsApplicationUserManager.GetAllWithEntAndOrg();
            InsertUsers(zarData, membersInDb);
            UpdateUsers(zarData, membersInDb);
            _cacheManager.RemoveDatabase(RedisDbType.User);
        }

        public void UpdateUsers(IList<ZarApiDto> zarData, List<MemberDto> membersInDb)
        {
            var membersToUpdate = membersInDb
                .Join(zarData, m => m.PersonnelNumber, z => z.PersonalNumber.ToString(), (m, z) => new { DbMember = m, ApiDto = z })
                .Where(t =>
                t.DbMember.FirstName != t.ApiDto.PersonelFirstName ||
                t.DbMember.LastName != t.ApiDto.PersonelLastName ||
                t.DbMember.EnterprisePosition.SortingNumber != int.Parse(t.ApiDto.OrganizationUnitCode) ||
                t.DbMember.Organization.SortingNumber != int.Parse(t.ApiDto.CompanyCode)).ToList();

            _enterprisePositionService.ClearNavigationProperty();
            var enterprisePositionsInDb = _enterprisePositionService.GetAll();
            _organizationService.ClearNavigationProperty();
            var organizationsInDb = _organizationService.GetAll();

            foreach (var member in membersToUpdate)
            {
                member.DbMember.FirstName = member.ApiDto.PersonelFirstName;
                member.DbMember.LastName = member.ApiDto.PersonelLastName;
                member.DbMember.EnterprisePosition = enterprisePositionsInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(member.ApiDto.OrganizationUnitCode));
                member.DbMember.EnterprisePositionId = enterprisePositionsInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(member.ApiDto.OrganizationUnitCode)).Id;
                member.DbMember.Organization = organizationsInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(member.ApiDto.CompanyCode));
                member.DbMember.OrganizationId = organizationsInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(member.ApiDto.CompanyCode)).Id;
                try
                {
                    _kmsApplicationUserManager.EditUser(_memberMapper.CreateFrom(member.DbMember), false, ConfigurationManager.AppSettings["SecretKey"]);
                }
                catch (Exception ex)
                {
                    new FileLogger().Log("C:\\temp\\Kms\\ZarDataSync", "updating Users " + Guid.NewGuid().ToString(), "txt", "on Performing user Save in api --> " + member.DbMember.UserName + " \n" + ex.Message + "\n\n");
                }
                Thread.Sleep(2001);
            }
        }

        public void InsertUsers(IList<ZarApiDto> zarData, List<MemberDto> membersInDb)
        {
            _enterprisePositionService.ClearNavigationProperty();
            _enterprisePositionService.HasNavigationProperty(t => t.Organization);
            var enterprisePositionsInDb = _enterprisePositionService.GetAll();
            _enterprisePositionService.ClearNavigationProperty();

            _organizationService.ClearNavigationProperty();
            var organizationInDb = _organizationService.GetAll();

            var daneshkarRole = _roleService.GetAll().First(t => t.Name == "Daneshkar" || t.Title == "دانشکار");
            var newUsers = zarData
                .Where(t => !membersInDb.Any(m => m.PersonnelNumber == t.PersonalNumber.ToString()))
                .Where(t => !string.IsNullOrWhiteSpace(t.CompanyCode))
                .Where(t => !string.IsNullOrWhiteSpace(t.OrganizationUnitCode))
                .ToList();

            foreach (var user in newUsers)
            {
                var member = new MemberDto()
                {
                    FirstName = user.PersonelFirstName,
                    LastName = user.PersonelLastName,
                    Email = user.PersonalNumber + "Email@zar.ir",
                    CellphoneNumber = '0' + user.PersonalNumber.ToString(),
                    Password = "123456",
                    PersonnelNumber = user.PersonalNumber.ToString(),
                    Roles = new List<RoleDto>() { (daneshkarRole) },
                    UserName = user.PersonalNumber.ToString(),
                    EnterprisePositionId = enterprisePositionsInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(user.OrganizationUnitCode) && t.Organization.SortingNumber.ToString() == user.CompanyCode).Id,
                    OrganizationId = organizationInDb.SingleOrDefault(t => t.SortingNumber == int.Parse(user.CompanyCode)).Id,
                    UserStatus = UserStatus.Active,
                    IsSuperAdmin = false
                };

                if(member.CellphoneNumber.Length < 11)
                    member.CellphoneNumber += new string('0', 11 - member.CellphoneNumber.Length);

                try
                {
                    var res = _kmsApplicationUserManager.CreateMemberAsync(_memberMapper.CreateFrom(member), member.Password, ConfigurationManager.AppSettings["SecretKey"]).Result;
                }
                catch (Exception ex)
                {
                    new FileLogger().Log("C:\\temp\\Kms\\ZarDataSync", "updating Users " + Guid.NewGuid().ToString(), "txt", "on Performing user Save in api --> " + member.UserName + " \n" + ex.Message + "\n\n");
                }
                Thread.Sleep(2001);
            }
        }

        public void UpdateEnterprisePositions(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode)
        {
            _enterprisePositionService.ClearNavigationProperty();
            _enterprisePositionService.HasNavigationProperty(t => t.Parent);
            foreach (var orgCode in allowedOrganizationsCode)
            {
                var enterprisePositionsInDb = _enterprisePositionService.GetList(CreateEqualitySpecification<EnterprisePositionDto>(FilterOperations.Equal, int.Parse(orgCode), "Organization.SortingNumber").GetCriteria());
                var zarEnterprisePositionsFromData = DeriveEnterprisePositionsTree(zarData, orgCode);
                var changedEnterprisePositions = enterprisePositionsInDb.Where(t => CheckEnterprisePositionChange(t, zarEnterprisePositionsFromData)).ToList();

                foreach (var enterprisePosition in changedEnterprisePositions)
                {
                    enterprisePosition.Name = zarEnterprisePositionsFromData.Where(t => int.Parse(t.OrganizationUnitCode) == enterprisePosition.SortingNumber).First().OrganizationUnitText;
                    if (!string.IsNullOrWhiteSpace(zarEnterprisePositionsFromData.Where(z => enterprisePosition.SortingNumber == int.Parse(z.OrganizationUnitCode)).First().ParentOrganizationunitCode))
                    {
                        enterprisePosition.ParentId = enterprisePositionsInDb
                            .Where(t => int.Parse(zarEnterprisePositionsFromData.Where(z => enterprisePosition.SortingNumber == int.Parse(z.OrganizationUnitCode)).First().ParentOrganizationunitCode) == t.SortingNumber)
                            .First().Id;
                        enterprisePosition.Parent = enterprisePositionsInDb
                            .Where(t => int.Parse(zarEnterprisePositionsFromData.Where(z => enterprisePosition.SortingNumber == int.Parse(z.OrganizationUnitCode)).First().ParentOrganizationunitCode) == t.SortingNumber)
                            .First();
                    }
                    else
                    {
                        enterprisePosition.ParentId = null;
                        enterprisePosition.Parent = null;
                    }
                }

                foreach (var enterprisePosition in changedEnterprisePositions)
                {
                    try
                    {
                        _enterprisePositionService.Save(enterprisePosition);
                    }
                    catch (Exception ex)
                    {
                        new FileLogger().Log("C:\\temp\\Kms\\ZarDataSync", "updating EnterprisePositions " + Guid.NewGuid().ToString(), "txt", "on Performing EnterprisePosition Save in api --> " + enterprisePosition.Name + " " + enterprisePosition.SortingNumber + " \n" + ex.Message + "\n\n");
                    }
                }
            }
            _enterprisePositionService.ClearNavigationProperty();
        }

        public void InsertNewEnterprisePositions(IList<ZarApiDto> zarData, IList<string> allowedOrganizationsCode)
        {
            foreach (var orgCode in allowedOrganizationsCode)
            {
                var zarEnterprisePositionsFromData = DeriveEnterprisePositionsTree(zarData, orgCode);
                var specificationOfOrganizationInDb = CreateEqualitySpecification<OrganizationDto>(FilterOperations.Equal, int.Parse(orgCode), "SortingNumber");
                var organizationInDb = _organizationService.Get(specificationOfOrganizationInDb.GetCriteria());
                var enterprisePositionsInDb = organizationInDb.EnterprisePositions.ToList();
                var uninsertedZarApiEnterprisePositions = GetUninsertedEnterprisePositions(enterprisePositionsInDb, zarEnterprisePositionsFromData);
                var uninsertedEnterprisePositions = new List<EnterprisePositionDto>();
                foreach (var EnterprisePosition in uninsertedZarApiEnterprisePositions)
                {
                    uninsertedEnterprisePositions.Add(CreateFromZarApiEnterprisePositionDto(EnterprisePosition, organizationInDb.Id));
                }

                _enterprisePositionService.RangeInsert(uninsertedEnterprisePositions);
            }
        }

        public EnterprisePositionDto CreateFromZarApiEnterprisePositionDto(ZarApiEnterprisePositonDto zarApiEnterprisePositonDto, Guid organizationId)
        {
            return new EnterprisePositionDto()
            {
                Id = Guid.NewGuid(),
                Name = zarApiEnterprisePositonDto.OrganizationUnitText,
                OrganizationId = organizationId,
                SortingNumber = int.Parse(zarApiEnterprisePositonDto.OrganizationUnitCode)
            };
        }

        public List<ZarApiEnterprisePositonDto> GetUninsertedEnterprisePositions(List<EnterprisePositionDto> enterprisePositionsInDb, List<ZarApiEnterprisePositonDto> enterprisePositionFromZar)
        {
            var insertedEnterprisePositions = enterprisePositionFromZar
                .Join(enterprisePositionsInDb,
                ez => int.Parse(ez.OrganizationUnitCode),
                ed => ed.SortingNumber,
                (ez, ed) => ez)
                .ToList();

            return enterprisePositionFromZar.Where(t => !insertedEnterprisePositions.Any(i => i.OrganizationUnitCode == t.OrganizationUnitCode)).ToList();
        }

        public SpecificationOfDataList<T> CreateEqualitySpecification<T>(FilterOperations filterOperation, object filterValue, string propertyName) where T : class
        {
            return new SpecificationOfDataList<T>()
            {
                FilterSpecifications = new List<FilterSpecification<T>>()
                    {
                        new FilterSpecification<T>()
                        {
                            FilterOperation = filterOperation,
                            FilterValue = filterValue,
                            PropertyName = propertyName
                        }
                    }
            };
        }

        public List<ZarApiEnterprisePositonDto> DeriveEnterprisePositionsTree(IList<ZarApiDto> zarData, string orgCode)
        {
            var orgData = zarData.Where(t => t.CompanyCode == orgCode);
            var enterprisePositionsTree = orgData
                .GroupJoin(orgData, z1 => z1.ParentApproverOrganizationPositionCode, z2 => z2.OrganizationPositionCode, (z1, z2) => new ZarApiEnterprisePositonDto
                {
                    OrganizationUnitCode = z1.OrganizationUnitCode,
                    OrganizationUnitText = z1.OrganizationUnitText,
                    ParentOrganizationunitCode = z2.FirstOrDefault()?.OrganizationUnitCode,
                    ParentOrganizationUnitText = z2.FirstOrDefault()?.OrganizationUnitText,
                })
                .Where(t => t.OrganizationUnitCode != t.ParentOrganizationunitCode)
                .ToList();

            return DistinctZarApiEnterprisePositonDto(enterprisePositionsTree);
        }

        public List<ZarApiEnterprisePositonDto> DistinctZarApiEnterprisePositonDto(IList<ZarApiEnterprisePositonDto> zarApiEnterprises)
        {
            var result = new List<ZarApiEnterprisePositonDto>();
            ZarApiEnterprisePositonDto distinctZarApiEnterprisePositonDto;
            while (zarApiEnterprises.Count > 0)
            {
                distinctZarApiEnterprisePositonDto = zarApiEnterprises.First();
                result.Add(distinctZarApiEnterprisePositonDto);
                zarApiEnterprises = zarApiEnterprises
                    .Where(t => !(t.OrganizationUnitText == distinctZarApiEnterprisePositonDto.OrganizationUnitText && t.OrganizationUnitCode == distinctZarApiEnterprisePositonDto.OrganizationUnitCode))
                    .ToList();
            }

            return result;
        }

        public List<ZarCompanyDto> DistinctCompany(IList<ZarCompanyDto> zarCompanies)
        {
            var result = new List<ZarCompanyDto>();
            ZarCompanyDto distinctCompany;
            while (zarCompanies.Count > 0)
            {
                distinctCompany = zarCompanies.First();
                result.Add(distinctCompany);
                zarCompanies = zarCompanies.Where(t => !(t.Name == distinctCompany.Name)).ToList();
            }

            return result;
        }

        public bool CheckEnterprisePositionChange(EnterprisePositionDto enterprisePosition, List<ZarApiEnterprisePositonDto> zarApiEnterprisePositonDtos)
        {
            if (!zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber))
                return false;

            if (zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber && z.OrganizationUnitText != enterprisePosition.Name))
                return true;

            if (zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber && z.ParentOrganizationunitCode == null && enterprisePosition.Parent == null))
                return false;

            if (zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber && z.ParentOrganizationunitCode == null && enterprisePosition.Parent != null))
                return true;

            if (zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber && z.ParentOrganizationunitCode != null && enterprisePosition.Parent == null))
                return true;

            if (zarApiEnterprisePositonDtos.Any(z => int.Parse(z.OrganizationUnitCode) == enterprisePosition.SortingNumber && int.Parse(z.ParentOrganizationunitCode) != enterprisePosition.Parent.SortingNumber))
                return true;

            return false;
        }
    }
}
