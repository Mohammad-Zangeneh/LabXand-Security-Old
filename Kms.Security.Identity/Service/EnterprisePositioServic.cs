using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kms.Security.Common.Domain;
using LabXand.DistributedServices;
using System.Data.Entity;
using LabXand.DomainLayer.Core;
using System.Configuration;

namespace Kms.Security.Identity
{
    public class EnterprisePositioServic : ServiceBase<Guid, EnterprisePosition, EnterprisePositionDto>, IEnterprisePositionService
    {
     
        public EnterprisePositioServic(IEntityMapper<EnterprisePosition, EnterprisePositionDto> mapper) : base(mapper)
        {
            
        }

        private readonly List<EnterprisePosition> childs = new List<EnterprisePosition>();
        private IList<EnterprisePosition> enterprisePosition;

        public IList<EnterprisePosition> GetAllChild(Guid parentId)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                enterprisePosition = dbContext.EnterprisePositions.ToList();
                GetChild(parentId);
                return childs;
            }
        }
        void GetChild(Guid parentId)
        {
            var list = enterprisePosition.Where(p => p.ParentId == parentId);
            childs.AddRange(list);
            foreach (var item in list)
            {
                GetChild(item.Id);
            }
        }
        //sadeghi546
        public override EnterprisePositionDto Save(EnterprisePositionDto domainDto)
        {
            PreventTreeRuin(domainDto.Id, domainDto.ParentId);
            return base.Save(domainDto);
        }
        //sadeghi546
        public void PreventTreeRuin(Guid id, Guid? parentid)
        {
            if (id != Guid.Empty)
            {
                var listOfChildren = GetAllChild(id).Select<EnterprisePosition, Guid?>(e => e.Id).ToList();
                var IsIncluded = listOfChildren.Contains(parentid);

                if (id == parentid || IsIncluded)
                    throw new Exception(" حلقه مخرب در ساختار درختی");
            }

        }

        public Dictionary<Guid, string> GetAllEnterprisePositionName()
        {
            var enterpriseList = GetAll();
            var enterpriseNames = new Dictionary<Guid, string>();
            var enterprises = enterpriseList.ToList();
            foreach (var enterprise in enterprises)
            {
                // check config
                if (ConfigurationManager.AppSettings["CustomerName"] == "Barez")
                {
                    var root = enterprise;
                    string rootParent = null;
                    while (root?.ParentId != null)
                    {
                        root = enterprises.FirstOrDefault(p => p.Id == root.ParentId);
                        rootParent = root?.Name;
                    }
                    enterpriseNames.Add(enterprise.Id, $"{rootParent ?? enterprise.Organization?.Name} ⇽ {enterprise.Name}");
                }
                else
                    enterpriseNames.Add(enterprise.Id, $"{enterprise.Name}");
            }

            return enterpriseNames;
        }
    }
    
}
