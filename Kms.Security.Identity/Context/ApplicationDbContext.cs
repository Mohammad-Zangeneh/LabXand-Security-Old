using Kms.Security.Common.Domain;
using Kms.Security.Identity.Mapping;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Kms.Security.Identity
{
    public class ApplicationDbContext :
      IdentityDbContext<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>,
      IIdentityUnitOfWork
    {
        public ApplicationDbContext()
            : base("IdentityConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            //   Database.SetInitializer<ApplicationDbContext>(null);
            Database.SetInitializer(new KmsSecurityInitializer());
        }

        public DbSet<Permission> Permissions { set; get; }
        public DbSet<PermissionRole> PermissionRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<EnterprisePosition> EnterprisePositions { get; set; }
        public DbSet<EnterprisePositionPost> EnterprisePositionPosts { get; set; }
        public DbSet<LabxandRole> LabxandRoles { set; get; }
        public DbSet<Company> Companes { get; set; }
        public DbSet<PermissionCategory> PermissionCategories { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<ClientIPAccess> ClientIPAccess { get; set; }
        public DbSet<SecurityConfiguration> SecurityConfigurations { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(builder);
            builder.Configurations.Add(new RoleMap());
            builder.Configurations.Add(new PermissionRoleMap());
            builder.Configurations.Add(new PermissionMap());
            builder.Configurations.Add(new EnterprisePositionPostMap());
            builder.Configurations.Add(new MemberMap());
            builder.Configurations.Add(new PermissionCategoryMap());
            builder.Configurations.Add(new UserTokenMap());
            builder.Configurations.Add(new LoginHistoryMap());
            builder.Configurations.Add(new ClientIPAccessMap());
            builder.Configurations.Add(new SecurityConfigurationMap());

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<CustomRole>().ToTable("CustomRoles");
            builder.Entity<CustomUserClaim>().ToTable("UserClaims");
            builder.Entity<CustomUserRole>().ToTable("UserCustomRoles");
            builder.Entity<CustomUserLogin>().ToTable("UserLogins");
            //builder.Entity<SecurityConfiguration>().ToTable("SecurityConfigurations");
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public int SaveAllChanges()
        {
            return base.SaveChanges();
        }
        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>)this.Set<TEntity>()).AddRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }
        //public void MarkAsUnChanged<TEntity>(TEntity entity) where TEntity : class
        //{
        //    Entry(entity).State = EntityState.d;
        //}

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public void ForceDatabaseInitialize()
        {
            this.Database.Initialize(force: true);
        }


    }
}
