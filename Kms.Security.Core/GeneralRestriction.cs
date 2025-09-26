using System;
using System.Linq.Expressions;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace Kms.Security.Core
{

	public class GeneralRestriction<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier> where TDomain : DomainEntityBase<TIdentifier>
	{
		private Expression<Func<TDomain, bool>> _condition;

		public GeneralRestriction(Expression<Func<TDomain, bool>> condition, IUserContextDetector<KmsUserContext> userContextDetector = null)
		{
			if (userContextDetector == null)
			{
				_condition = condition;
			}
			else if (userContextDetector.UserContext.IsSuperAdmin)
			{
				_condition = (TDomain domain) => true;
			}
			else
			{
				_condition = condition;
			}
		}

		public Expression<Func<TDomain, bool>> Get()
		{
			return _condition;
		}
	}
}