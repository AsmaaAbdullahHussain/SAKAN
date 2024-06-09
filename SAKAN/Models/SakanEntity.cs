
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SAKAN.Models
{
    public class SakanEntity : IdentityDbContext<ApplicationUser>
    {
        public SakanEntity() { }
        public SakanEntity(DbContextOptions<SakanEntity> options) : base(options) { }

        public DbSet<Owner> Owner{ get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Flat> Flat { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<FlatImage> FlatImage { get; set; }
        public DbSet<RoomImage> RoomImage { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<StudentInRoom> StudentInRoom { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User");//.Ignore(u => u.PhoneNumberConfirmed);
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
            builder.Entity<FlatImage>().HasKey(i => new { i.FlatId, i.Image } );
            builder.Entity<RoomImage>().HasKey(i => new { i.RoomId, i.Image });
            builder.Entity<StudentInRoom>().HasKey(i => new { i.StuedntId, i.RoomId });
            //builder.Entity<Booking>().HasKey(b => new { b.Checkin, b.StudentId });
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.
        //        UseSqlServer(@"Data Source=LAPTOP-JFGR6E1R\SQLEXPRESS;Initial Catalog=SAKAN_DB;Integrated Security=True");
        //    //dbms - name server -db - autha
        //}
    }
}
