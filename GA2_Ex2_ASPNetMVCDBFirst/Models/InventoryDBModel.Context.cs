using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GA2_Ex2_ASPNetMVCDBFirst.Models
{
    public partial class InventoryDBEntities : DbContext
    {
        public InventoryDBEntities()
            : base("name=InventoryDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.Agent)
                .HasForeignKey(e => e.AgentID);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Item)
                .HasForeignKey(e => e.ItemID);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Order)
                .HasForeignKey(e => e.OrderID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserID);
        }

        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}