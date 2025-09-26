module nodak.common.models {
    export class CompanyMapper extends nodak.models.EntityMapper<Company, CompanyDto>{
        constructor() {
            super();
        }

        MapToDto(entity: Company): CompanyDto {
            let companyDto = new CompanyDto();
            companyDto = ObjectAssign<CompanyDto>(new CompanyDto(), entity);

            return companyDto;
        }

        MapToEntity(dto: CompanyDto): Company {
            let company = new Company();
            company = ObjectAssign<Company>(new Company(), dto);
            return company;
        };
    }
}
angular.module('common.services').service('common.company.mapper', nodak.common.models.CompanyMapper);