using System;
using System.Linq.Expressions;
using LabXand.DomainLayer.Core;

namespace Kms.Security.Core
{

	public class ConditionRestriction<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier> where TDomain : DomainEntityBase<TIdentifier>
	{
		private Expression<Func<TDomain, bool>> _condition;

		public ConditionRestriction(Expression<Func<TDomain, bool>> condition)
		{
			_condition = condition;
		}

		public Expression<Func<TDomain, bool>> Get()
		{
			return _condition;
		}
	}
}