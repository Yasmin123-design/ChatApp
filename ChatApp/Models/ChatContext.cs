using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models
{
    
    public class ChatContext : IdentityDbContext<ApplicationUser>
    {
        public ChatContext() { }
        public ChatContext(DbContextOptions db) : base(db) { }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-I7PU4G3;Database=ChatApp;Trusted_Connection=True;Encrypt=false");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
         .HasKey(m => m.Id);

            modelBuilder.Entity<Message>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
