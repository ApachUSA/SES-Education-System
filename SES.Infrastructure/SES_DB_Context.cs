using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.AbstractE;
using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Infrastructure
{
	public class SES_DB_Context : DbContext
	{
		public SES_DB_Context(DbContextOptions<SES_DB_Context> options) : base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<Abstracts> Abstracts { get; set; }

		public DbSet<Option> Options { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Test> Tests { get; set; }
		public DbSet<Test_History> Tests_Histories { get; set; }
		public DbSet<Test_Result> Test_Results { get; set; }

		public DbSet<City> Cities { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<Rang> Rangs { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Abstracts>(builder =>
			{
				builder.ToTable("Abstracts").HasKey(x => x.Abstract_ID);
				builder.Property(x => x.Abstract_ID).ValueGeneratedOnAdd();
			});


			modelBuilder.Entity<Option>(builder =>
			{
				builder.ToTable("Options").HasKey(x => x.Option_ID);
				builder.Property(x => x.Option_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.Question)
				.WithMany(x => x.Options)
				.HasForeignKey(x => x.Question_ID)
				.OnDelete(DeleteBehavior.Cascade);


			});

			modelBuilder.Entity<Question>(builder =>
			{
				builder.ToTable("Questions").HasKey(x => x.Question_ID);
				builder.Property(x => x.Question_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.Test)
				.WithMany(x => x.Questions)
				.HasForeignKey(x => x.Test_ID)
				.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Test>(builder =>
			{
				builder.ToTable("Tests").HasKey(x => x.Test_ID);
				builder.Property(x => x.Test_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.Position)
					.WithMany(x => x.Tests)
					.HasForeignKey(x => x.Position_ID)
				.OnDelete(DeleteBehavior.NoAction);


			});

			modelBuilder.Entity<Test_History>(builder =>
			{
				builder.ToTable("Tests_Histories").HasKey(x => x.Test_History_ID);
				builder.Property(x => x.Test_History_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.User)
					.WithMany(x => x.Test_Histories)
					.HasForeignKey(x => x.User_ID)
					.OnDelete(DeleteBehavior.Cascade);


				builder.HasOne(x => x.Test)
					.WithMany(x => x.Test_Histories)
					.HasForeignKey(x => x.Test_ID)
					.OnDelete(DeleteBehavior.Cascade);

			});

			modelBuilder.Entity<Test_Result>(builder =>
			{
				builder.ToTable("Test_Results").HasKey(x => x.Test_Result_ID);
				builder.Property(x => x.Test_Result_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.User)
					.WithMany(x => x.Test_Results)
					.HasForeignKey(x => x.User_ID)
					.OnDelete(DeleteBehavior.Cascade);

				builder.HasOne(x => x.Test)
					.WithMany(x => x.Test_Results)
					.HasForeignKey(x => x.Test_ID)
					.OnDelete(DeleteBehavior.Cascade);

			});

			modelBuilder.Entity<City>(builder =>
			{
				builder.ToTable("Cities").HasKey(x => x.City_ID);
				builder.Property(x => x.City_ID).ValueGeneratedOnAdd();

			});

			modelBuilder.Entity<Department>(builder =>
			{
				builder.ToTable("Departments").HasKey(x => x.Department_ID);
				builder.Property(x => x.Department_ID).ValueGeneratedOnAdd();

				builder.HasOne(x => x.City)
					.WithMany(x => x.Departments)
					.HasForeignKey(x => x.City_ID);

			});

			modelBuilder.Entity<Position>(builder =>
			{
				builder.ToTable("Positions").HasKey(x => x.Position_ID);
				builder.Property(x => x.Position_ID).ValueGeneratedOnAdd();

			});

			modelBuilder.Entity<Rang>(builder =>
			{
				builder.ToTable("Rangs").HasKey(x => x.Rang_ID);
				builder.Property(x => x.Rang_ID).ValueGeneratedOnAdd();

			});

			modelBuilder.Entity<User>(builder =>
			{
				builder.ToTable("Users").HasKey(x => x.User_ID);
				builder.Property(x => x.User_ID).ValueGeneratedOnAdd();
				builder.Property(x => x.Login).HasMaxLength(25);
				builder.Property(x => x.Password).HasMaxLength(10);

				builder.HasOne(x => x.Department)
					.WithMany(x => x.Users)
					.HasForeignKey(x => x.Department_ID);

				builder.HasOne(x => x.Position)
					.WithMany(x => x.Users)
					.HasForeignKey(x => x.Position_ID);

				builder.HasOne(x => x.Rang)
					.WithMany(x => x.Users)
					.HasForeignKey(x => x.Rang_ID);

			});

		}
	}
}
