using System;
using System.Collections.Generic;
using System.Linq;
using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using Kms.Security.Util;
using Microsoft.Owin.Security;

namespace Kms.Security.Identity
{
    public class OrganizationService : ServiceBase<Guid, Organization, OrganizationDto>, IOrganizationService
    {
        private readonly IAuthenticationManager _authenticationManager;
        public OrganizationService(IEntityMapper<Organization, OrganizationDto> mapper, IAuthenticationManager authenticationManager) : base(mapper)
        {
            _authenticationManager = authenticationManager;
            this.HasNavigationProperty(p => p.EnterprisePositions);
            //_jwtUtil = jwtUtil;
            //if (!jwtUtil.HasRestriction("SuperAdminPermisionManagement"))
            if (_authenticationManager != null && _authenticationManager.User != null)
            {
                var organizationIdCliam = _authenticationManager.User.Claims.FirstOrDefault(t => t.Type == LabxandClaimTypes.OrganizationId);
                var superAdmin = _authenticationManager.User.Claims.FirstOrDefault(t => t.Type == LabxandClaimTypes.IsSuperAdmin)?.Value;
                if (organizationIdCliam == null || (superAdmin != null && Convert.ToBoolean(superAdmin) == true))
                    return;
                var test = new Guid(organizationIdCliam.Value);

                var org = GetAllChild(test).Select(t => t.Id).ToList();
                org.Add(test);
                this.HasRestriction(p => org.Any(t => t == p.Id));
            }

        }

        private readonly List<Organization> childs = new List<Organization>();
        private IList<Organization> organizations;

        private void GetChild(Guid parentId)
        {
            var list = organizations.Where(p => p.ParentId == parentId);
            childs.AddRange(list);
            foreach (var item in list)
            {
                GetChild(item.Id);
            }
        }
        public override IList<OrganizationDto> GetAll()
        {

            var organizationDtoList = base.GetAll();
            var root = organizationDtoList.FirstOrDefault(t => t.ParentId == null);
            var parentIds = organizationDtoList.Where(t => t.ParentId != null).Select(t => t.ParentId).ToList();
            foreach (var item in parentIds)
            {
                var organizationDto = organizationDtoList.FirstOrDefault(t => t.Id == item);
                if (organizationDto == null)
                    foreach (var organn in organizationDtoList.Where(t => t.ParentId == item))
                    {
                        organn.ParentId = null;
                    }
            }
            return organizationDtoList;
        }
        public IList<Organization> GetAllChild(Guid parentId)
        {

            using (var dbContext = new ApplicationDbContext())
            {
                organizations = dbContext.Organizations.ToList();
                GetChild(parentId);
                return childs;
            }
        }
        //sadeghi546
        public override OrganizationDto Save(OrganizationDto domainDto)
        {
            PreventTreeRuin(domainDto.Id, domainDto.ParentId);
            return base.Save(domainDto);
        }
        //sadeghi546
        public void PreventTreeRuin(Guid id , Guid? parentid)
        {
            if (id != Guid.Empty)
            {
                var listOfChildren = GetAllChild(id).Select<Organization, Guid?>(o => o.Id).ToList();
                var IsIncluded = listOfChildren.Contains(parentid);

                if (id == parentid || IsIncluded)
                    throw new Exception(" حلقه مخرب در ساختار درختی");
            }
        }

        public OrganizationDto GetRoot()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var root = dbContext.Organizations.FirstOrDefault(x=>x.ParentId == null);
                return _mapper.MapTo(root);
            }
        }

        public IList<OrganizationDto> GetAllWithoutNavigation()
        {
            ClearNavigationProperty();
            return GetAll();
        }
    }
}
