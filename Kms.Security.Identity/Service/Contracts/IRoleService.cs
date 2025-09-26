using Kms.Security.Common.Domain;
using System;

namespace Kms.Security.Identity
{
    public  interface IRoleService:IServiceBase<LabxandRole,RoleDto>
    {        
        void Delete(Guid id);
        string GetTrackEntity();
    }
}
