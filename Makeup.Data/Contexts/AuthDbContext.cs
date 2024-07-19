using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makeup.Data.Contexts
{
    public class AuthDbContext : IdentityDbContext<IdentityUser, IdentityRole,
    string>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) :
       base(options)
        {
        }
    }
}
