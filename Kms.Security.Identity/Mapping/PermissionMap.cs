using Kms.Security.Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity.Mapping
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            this.HasKey(p => p.Id);
          //  this.HasOptional(p => p.Parent).WithMany().HasForeignKey(p => p.ParentId);
            this.Property(p => p.Code).IsRequired()
                                        .HasMaxLength(60)
                                        .HasColumnAnnotation(
                                        IndexAnnotation.AnnotationName,
                                        new IndexAnnotation(
                                            new IndexAttribute("IX_Code") { IsUnique = true, Order=1 }));
            this.Property(p => p.CompanyId).IsRequired()
                                     .HasColumnAnnotation(
                                     IndexAnnotation.AnnotationName,
                                     new IndexAnnotation(
                                         new IndexAttribute("IX_Code") { IsUnique = true, Order = 2 })).IsOptional();

            this.Property(p => p.Title).IsRequired();
            this.HasOptional(p => p.Company).WithMany().HasForeignKey(p => p.CompanyId);

        }
    }
}
