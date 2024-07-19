using Makeup.Entities;
using Makeup.Services.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Services
{
    public class ApiAuthService
    {
        private readonly Makeup2024Context _dbContext;
        public ApiAuthService(Makeup2024Context dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiUser> Authenticate(LoginDto login)
        {
            var user = await _dbContext.ApiUsers.FirstOrDefaultAsync(x => x.Username
           == login.Username);
            if (user == null)
            {
                return null!;
            }
            var hash = Hash($"{login.Password}{user.Salt}");
            if (user.Password == hash)
            {
                return user;
            }
            else return null!;
        }
        private static string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
