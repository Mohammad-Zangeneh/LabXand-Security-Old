using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.DataContract.Zar
{
    public class ZarApiEnterprisePositonDto
    {
        public string OrganizationUnitCode { get; set; }
        public string OrganizationUnitText { get; set; }
        public string ParentOrganizationunitCode { get; set; }
        public string ParentOrganizationUnitText { get; set; }
    }
}
