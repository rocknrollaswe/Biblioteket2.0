using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Bibblan.Models
{
    public partial class BiblioteketContext : DbContext
    {
        public BiblioteketContext()
        {
        }

        public BiblioteketContext(DbContextOptions<BiblioteketContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookStockLoan> BookStockLoans { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Loanlog> Loanlogs { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockLoanLogBooksJoin> StockLoanLogBooksJoins { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserReport> UserReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings[1].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn)
                    .HasName("PK__Books__447D36EB3623D596");

                entity.Property(e => e.Isbn).HasColumnName("ISBN");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Ddk).HasColumnName("DDK");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Publisher)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Sab)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SAB");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__Category__531856C7");
            });

            modelBuilder.Entity<BookStockLoan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("BookStockLoan");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("author");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Comment)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Condition)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Ddk).HasColumnName("DDK");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Discarded).HasColumnName("discarded");

                entity.Property(e => e.Edition).HasColumnName("edition");

                entity.Property(e => e.Isbn).HasColumnName("isbn");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Publisher)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.Sab)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SAB");

                entity.Property(e => e.Stockid).HasColumnName("stockid");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).ValueGeneratedNever();

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Loanlog>(entity =>
            {
                entity.HasKey(e => e.StockId)
                    .HasName("PK__Loanlog__2C83A9E2838BB213");

                entity.ToTable("Loanlog");

                entity.Property(e => e.StockId)
                    .ValueGeneratedNever()
                    .HasColumnName("StockID");

                entity.Property(e => e.Loandate).HasColumnType("date");

                entity.Property(e => e.Returndate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Stock)
                    .WithOne(p => p.Loanlog)
                    .HasForeignKey<Loanlog>(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loanlog__StockID__5BAD9CC8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Loanlogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Loanlog__UserID__5AB9788F");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionId)
                    .ValueGeneratedNever()
                    .HasColumnName("PermissionID");

                entity.Property(e => e.Permissions)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Condition)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn).HasColumnName("ISBN");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Stock__ISBN__55F4C372");
            });

            modelBuilder.Entity<StockLoanLogBooksJoin>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("StockLoanLogBooksJoin");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("author");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Isbn).HasColumnName("isbn");

                entity.Property(e => e.Loandate)
                    .HasColumnType("date")
                    .HasColumnName("loandate");

                entity.Property(e => e.Returndate)
                    .HasColumnType("date")
                    .HasColumnName("returndate");

                entity.Property(e => e.Stockid).HasColumnName("stockid");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "uniqueEmail")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Debt).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.HasLoanCard).HasDefaultValueSql("((1))");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Socialsecuritynumber).IsRequired();

                entity.Property(e => e.UserComment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.PermissionsNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Permissions)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__Permissio__2BFE89A6");
            });

            modelBuilder.Entity<UserReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UserReport");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Returndate).HasColumnType("date");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
