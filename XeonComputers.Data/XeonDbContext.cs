using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeonComputers.Models;

namespace XeonComputers.Data
{
    public class XeonDbContext : IdentityDbContext<XeonUser>
    {
        public XeonDbContext(DbContextOptions<XeonDbContext> options)
            : base(options)
        {
        }
    }
}
