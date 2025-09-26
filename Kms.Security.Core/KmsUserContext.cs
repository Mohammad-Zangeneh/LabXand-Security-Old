using System;
using System.Collections.Generic;
using LabXand.Security.Core;
using Newtonsoft.Json;
using WorkFlow.Security;

namespace Kms.Security.Core
{

	public class KmsUserContext : UserContextBase, IWorkflowUserContext
	{
		public Guid MemberId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public Guid OrganizationId { get; set; }

		public string OrganizationTitle { get; set; }

		public string Token { get; set; }

		public List<string> AuthorizedOperations { get; set; }

		public List<string> Posts { get; set; }

		public Guid EnterprisePositionId { get; set; }

		public bool IsSuperAdmin { get; set; }

		public override bool IsAuthorizedFor(string claim)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}