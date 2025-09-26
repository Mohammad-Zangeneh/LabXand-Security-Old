using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.DataContract.Zar
{
    public class ZarApiDto
    {
        public string ParentPositionDescription { get; set; }
        public string ParentApproverPersonalNumber { get; set; }
        public string ParentApproverOrganizationPositionCode { get; set; }
        public string Birthdate { get; set; }
        public int PersonalNumber { get; set; }
        public string ParentApproverName { get; set; }
        public string PersonelFirstName { get; set; }
        public string PersonelLastName { get; set; }
        public string ParentApproverUser { get; set; }
        public string EnglishPersonelFirstName { get; set; }
        public string EnglishPersonelLastName { get; set; }
        public string GenderCode { get; set; }
        public string GenderText { get; set; }
        public string Country { get; set; }
        public string PlaceOfBirth { get; set; }
        public string FatherName { get; set; }
        public string Nationality { get; set; }
        public string OrganizationPositionCode { get; set; }
        public string OrganizationPositionText { get; set; }
        public string JobKeyCode { get; set; }
        public string JobKeyText { get; set; }
        public string OrganizationUnitCode { get; set; }
        public string OrganizationUnitText { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterText { get; set; }
        public string SalariedGroup { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyText { get; set; }
    }
}
