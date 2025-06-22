using Microsoft.EntityFrameworkCore;
using SupportSystem.Domain.Entities;
using SupportSystemApp.Domain.Entities;

namespace SupportSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<TicketMessage> TicketMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(x => x.Ticket)
                .WithMany()
                .HasForeignKey(x => x.TicketId);

            modelBuilder.Entity<TicketMessage>()
                .HasOne(x => x.SenderUser)
                .WithMany()
                .HasForeignKey(x => x.SenderUserId);
        }
    }
}
