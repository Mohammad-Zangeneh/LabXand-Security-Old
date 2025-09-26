using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabXand.DistributedServices;
using System.Data.Entity;

namespace Kms.Security.Identity
{
    public class EnterprisePositionPostService : ServiceBase<Guid, EnterprisePositionPost, EnterprisePositionPostDto>, IEnterprisePositionPostService
    {
        public EnterprisePositionPostService(IEntityMapper<EnterprisePositionPost, EnterprisePositionPostDto> mapper) : base(mapper)
        {
            this.HasNavigationProperty(p => p.EnterprisePosition);
            this.HasNavigationProperty(t => t.Permissions);
            this.HasNavigationProperty(t => t.EnterprisePosition.Organization);
        }

        public override EnterprisePositionPostDto Save(EnterprisePositionPostDto domainDto)
        {
            var domain = _mapper.CreateFrom(domainDto);
            using (var dbContext = new ApplicationDbContext())
            {
                if (domain.Permissions != null)
                {
                    var permissions = domain.Permissions.Where(p => true).ToList();
                    var ids = permissions.Select(p => p.Id);
                    domain.SetPermissionList(dbContext.Permissions.Where(p => ids.Any(x => x.Equals(p.Id))).ToList());
                }


                if (domain.IdIsEmpty())
                {
                    domain.SetNewId();
                    dbContext.Set<EnterprisePositionPost>().Add(domain);
                }
                else
                {
                    //dbContext.Set<EnterprisePositionPost>().Attach(domain);
                    //dbContext.Entry(domain).State = EntityState.Modified;
                    var test = dbContext.EnterprisePositionPosts.Include(t => t.Permissions).FirstOrDefault(t => t.Id == domain.Id);
                    if (test != null)
                    {
                        test.SetPermissionList(domain.Permissions);
                        test.Title = domain.Title;
                        test.EnterprisePositionId = domain.EnterprisePositionId;
                        test.Description = domain.Description;
                        domain = test;
                    }
                    //addbyhamed
                    else
                    {

                        test = new EnterprisePositionPost(domain.Id, domain.Title, domain.EnterprisePositionId, domain.Description);
                        domain = test;
                        dbContext.Set<EnterprisePositionPost>().Add(domain);
                    }
                    //addbyhamed

                }
                //foreach (var item in domain.Permissions)
                //{
                //    //if (selectedPermissions.Any(p => p.Id == item.Id))
                //    dbContext.Set<Permission>().Attach(item);
                //}
                dbContext.SaveAllChanges();

                //dbContext.SaveAllChanges();
                return _mapper.MapTo(domain);
            }
        }






        //public override EnterprisePositionPostDto Save(EnterprisePositionPostDto domainDto)
        //{
        //    var domain = _mapper.CreateFrom(domainDto);
        //    using (var dbContext = new ApplicationDbContext())
        //    {
        //        if (domain.Permissions != null)
        //        {
        //            var permissions = domain.Permissions.Where(p => true).ToList();
        //            var ids = permissions.Select(p => p.Id);
        //            domain.SetPermissionList(dbContext.Permissions.Where(p => ids.Any(x => x.Equals(p.Id))).ToList());
        //        }
        //        if (domain.IdIsEmpty())
        //        {
        //            domain.SetNewId();
        //            dbContext.Set<EnterprisePositionPost>().Add(domain);
        //        }
        //        else
        //        {
        //            //dbContext.Set<EnterprisePositionPost>().Attach(domain);
        //            //dbContext.Entry(domain).State = EntityState.Modified;
        //            var test = dbContext.EnterprisePositionPosts.Include(t => t.Permissions).FirstOrDefault(t => t.Id == domain.Id);
        //            test.SetPermissionList(domain.Permissions);
        //            test.Title = domain.Title;
        //            test.EnterprisePositionId = domain.EnterprisePositionId;
        //            test.Description = domain.Description;
        //            domain = test;
        //        }
        //        //foreach (var item in domain.Permissions)
        //        //{
        //        //    //if (selectedPermissions.Any(p => p.Id == item.Id))
        //        //    dbContext.Set<Permission>().Attach(item);
        //        //}

        //        dbContext.SaveAllChanges();

        //        //dbContext.SaveAllChanges();
        //        return _mapper.MapTo(domain);
        //    }
        //}
    }
}
