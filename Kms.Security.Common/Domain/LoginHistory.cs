using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Common.Domain
{
    public class LoginHistory : IDomainBase<Guid>
    {

        public LoginHistory(Guid id, Guid userId, DateTime date, Boolean isSuccess, string fingerPrint, DateTime? expireDateToken, DateTime? logOutDate)
        {
            Id = id;
            UserId = userId;
            Date = date;
            IsSuccess = isSuccess;
            FingerPrint = fingerPrint;
            ExpireDateToken = expireDateToken;
            LogOutDate = logOutDate;
        }
        protected LoginHistory() { }

        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public DateTime Date { get; protected set; }
        public Boolean IsSuccess { get; protected set; }
        public string FingerPrint { get; protected set; }
        public DateTime? ExpireDateToken { get; protected set; }
        public DateTime? LogOutDate { get; protected set; }

        public bool IdIsEmpty()
        {
            if (this.Id == Guid.Empty)
                return true;
            return false;
        }

        public void SetNewId()
        {
            Id = Guid.NewGuid();
        }

        public void setExpireDate(DateTime date)
        {
            ExpireDateToken = date;
        }

        public void setLogOutDate(DateTime date)
        {
            LogOutDate = date;
        }

        public void SetDate()
        {
            this.Date = DateTime.Now;
        }
    }
}
