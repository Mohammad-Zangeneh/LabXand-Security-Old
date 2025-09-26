using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kms.Security.Util
{
    public interface IUsernameValidator
    {
        bool ValidateUsername(string username);
    }

    public class UsernameValidator : IUsernameValidator
    {
        private const string usernamePattern = @"^[A-Za-z0-9._-]{3,15}$";
        public bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new NullReferenceException("نام کاربری اجباری است.");

            var regex = new Regex(usernamePattern);
            if (!regex.IsMatch(username))
                throw new FormatException("نام کاربری فقط باید شامل حروف الفبا لاتین، اعداد 0-9 و خط تیره و خط زیرین باشد.");

            return true;
        }
    }

    public interface IPersonnelNumberValidator
    {
        bool ValidatePersonnelNumber(string personnelCode);
    }

    public class PersonnelNumberValidator : IPersonnelNumberValidator
    {
        private const string OnlyDigitPattern = @"^\d+$";
        public bool ValidatePersonnelNumber(string personnelCode)
        {
            if (ConfigurationManager.AppSettings["CustomerName"] == "Behdasht")
                return true;
            if (string.IsNullOrWhiteSpace(personnelCode))
                throw new NullReferenceException("شماره پرسنلی اجباری است.");

            var regex = new Regex(OnlyDigitPattern);
            if (!regex.IsMatch(personnelCode))
                throw new FormatException("شماره پرسنلی فقط باید شامل اعداد 0-9 باشد.");

            return true;
        }
    }
    public interface IPhoneNumberValidator
    {
        bool ValidatePhoneNumber(string phoneNumber);
    }

    public class PhoneNumberValidator : IPhoneNumberValidator
    {
        private const string OnlyDigitPattern = @"^\d+$";
        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new NullReferenceException("شماره تلفن اجباری است.");

            if (phoneNumber[0] != '0')
                throw new FormatException("شماره تلفن باید با صفر شروع گردد.");

            if (phoneNumber.Length != 11)
                throw new FormatException("شماره تلفن باید شامل 11 کاراکتر عددی باشد.");

            var regex = new Regex(OnlyDigitPattern);
            if (!regex.IsMatch(phoneNumber))
                throw new FormatException("شماره تلفن فقط باید شامل اعداد 0-9 باشد.");

            return true;
        }
    }

    public interface IEmailValidator
    {
        bool ValidateEmailAddress(string emailAddress);
    }

    public class EmailValidator : IEmailValidator
    {
        private const string EmailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        public bool ValidateEmailAddress(string emailAddress)
        {
            var isValid = true;

            try
            {
                var email = new MailAddress(emailAddress);
            }
            catch
            {
                isValid = false;
            }

            if (!isValid)
                throw new FormatException("فرمت ایمیل درست نیست.");

            var regex = new Regex(EmailPattern);
            if(!regex.IsMatch(emailAddress))
                throw new FormatException("فرمت ایمیل درست نیست.");

            return isValid;
        }
    }
}
