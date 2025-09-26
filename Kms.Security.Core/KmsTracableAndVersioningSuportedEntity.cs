using System;
using LabXand.DomainLayer.Core;

namespace Kms.Security.Core
{

	public abstract class KmsTracableAndVersioningSuportedEntity<TIdentifier, TVersion> : KmsTraceableEntity<TIdentifier>, ITracableAndVersioningSuportedEntity<TIdentifier, TVersion, KmsTraceData, KmsUserContext>, IVersioningSupportedEntity<TIdentifier, TVersion>, IComparable, IDomainEntity<TIdentifier>, IEquatable<IDomainEntity<TIdentifier>>, IDomainEntity, ITracableEntity<KmsTraceData, KmsUserContext>, ITracableEntity<KmsUserContext>
	{
		public abstract TVersion Version { get; }

		public abstract void IncreaseVersion(TVersion currentVersion);

		public abstract int CompareTo(object obj);
	}
}