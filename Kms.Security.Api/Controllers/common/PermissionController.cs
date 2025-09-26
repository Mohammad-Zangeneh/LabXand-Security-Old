using Kms.Security.Common.Domain;
using Kms.Security.Identity;
using Kms.Security.Jwt;
using Kms.Security.Util;
using Kms.Security.WebApi.RateLimit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kms.Security.Api.Controllers
{
    [RateLimit]
    public class PermissionController: ApiController
    {
        IPermissionService _service;
        IIdentityUnitOfWork _unitOfWork;
        IPermissionCategoryService _permissionCategoryService;
        public PermissionController(IPermissionService service, IIdentityUnitOfWork unitOfWork
            , IPermissionCategoryService permissionCategoryService
            )
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _permissionCategoryService = permissionCategoryService;
        }

        [Description("دریافت")]
        [HttpGet]
        [Route("api/permission/get", Name = "permissionGet")]
        [JwtAuthorize(Permission= "RoleManagement")]
        public IList<PermissionDto> Get( )
        {
            var permissions =  _service.GetAll();
            var permissionCategory = _permissionCategoryService.GetAll();
            if(permissionCategory!= null)
                foreach (var item in permissionCategory)
                {
                    permissions.Add(
                        new PermissionDto() { Id = item.Id, PermissionCategoryId = item.ParentId, Title = item.Name }
                        );
                }
            return permissions;
        }

        [Description("دریافت نوع مجوز")]
        [HttpGet]
        [Route("api/PermissionType/Get")]
        [JwtAuthorize(Permission = "RoleManagement")]
        public IList<StatusDescription>  GetPermissionType ()
        {
            return _service.GetPermissionTypeForCombo();
        }

        [Description("ذخیره")]
        [HttpPost]
        [Route("api/permission/Save", Name = "SavePermission")]
        [JwtAuthorizeAttribute(Permission = "PermissionManagement")]
        public PermissionDto Save(PermissionDto domain)
        {
            var result = _service.Save(domain);
            return result;
        }


    }
}