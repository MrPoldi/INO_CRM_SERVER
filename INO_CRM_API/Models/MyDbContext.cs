using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INO_CRM_API.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public MyDbContext() : base()
        {

        }

        public DbSet<BranchModel> Branches { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<ContactPersonModel> ContactPeople { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<TradeNoteModel> TradeNotes { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
