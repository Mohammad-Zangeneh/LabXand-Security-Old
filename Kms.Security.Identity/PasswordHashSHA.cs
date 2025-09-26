using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kms.Security.Identity
{
    public class PasswordHashSHA
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static PasswordVerificationResult VerifyPassword(string hashPassword, string password)
        {
            byte[] sentHashValue = Encoding.UTF8.GetBytes(hashPassword);
            byte[] compareHashValue;

            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                compareHashValue = Encoding.UTF8.GetBytes(builder.ToString());
            }

            bool same = true;
            //Compare the values of the two byte arrays.
            for (int x = 0; x < sentHashValue.Length; x++)
            {
                if (sentHashValue[x] != compareHashValue[x])
                {
                    same = false;
                    break;
                }

            }

            //Display whether or not the hash values are the same.
            if (same)
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;

        }

        /// <summary>
        /// Calculates a number based on user's hashed password value that is used to preventing changing password from
        /// the database. When a hacker changes the PasswordHash in the database, the LoginValue field is still
        /// unchanged and also the hacker can not replicate the LoginValue number because the super secret calculation
        /// algorithm is not revealed to them. This method should be used when user changes their password from inside
        /// the system or when admin resets the user password.
        /// </summary>
        /// <param name="password">hashed value of the password (PasswordHash)</param>
        /// <returns></returns>
        public static int CalculateLoginValue(string password)
        {
            int sumOfCharAscii = 0;
            for (int i = 0; i <= password.Length-1; i++)
            {
                sumOfCharAscii += (int)password[i];
            }

            var value = (sumOfCharAscii + 8) * 7 + ((sumOfCharAscii + 6) * 5) / 4 + 321;
            if (value < 0)
                value = (0 - value);

            return value;
        }
    }
}
