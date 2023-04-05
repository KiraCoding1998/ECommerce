using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Server.Helpers
{
    public class PasswordManager
    {
        public byte[]? PasswordSalt { get; private set; }
        public byte[]? HashedPassword { get; private set; }



        public static PasswordManager CreatePassowrdObject(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordManager passwordHash = new PasswordManager
                {
                    PasswordSalt = hmac.Key,
                    HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
                };

                return passwordHash;
            }

        }
        public static bool IsPasswordVerified(string enteredPassword , byte[] passwordSalt , byte[] hashedPassword)
        {
            using (var hmac = new HMACSHA512(key:passwordSalt))
            {
                byte[] enterdPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

                return enterdPasswordHash.SequenceEqual(hashedPassword);
            }
        }
    }
}
