using GymBro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GymBro.DAL.Data
{
    /// <summary>
    /// Контекст Entity Framework для работы с базой данных
    /// Подход Code First
    /// </summary>
    public class GymBroContext : DbContext
    {
        public GymBroContext(DbContextOptions<GymBroContext> options)
            : base(options)
        {
        }

        
        // Таблица профилей пользователей
        public DbSet<UserProfile> UserProfiles { get; set; }

        // Таблица тренировочных программ
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

        // Таблица упражнений
        public DbSet<Exercise> Exercises { get; set; }

        // Таблица оборудования
        public DbSet<Equipment> Equipment { get; set; }

        // Таблица пользователей
        public DbSet<User> Users { get; set; }

        // Таблица ролей
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи один-ко-многим
            // Один UserProfile -> много TrainingProgram
            modelBuilder.Entity<TrainingProgram>()
                .HasOne(p => p.UserProfile)
                .WithMany(u => u.TrainingPrograms)
                .HasForeignKey(p => p.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка ограничений для UserProfile
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.FitnessLevel)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<UserProfile>()
                .Property(u => u.TrainingGoal)
                .IsRequired()
                .HasMaxLength(50);

            // Настройка ограничений для TrainingProgram
            modelBuilder.Entity<TrainingProgram>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TrainingProgram>()
                .Property(p => p.ProgramType)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.Description)
                      .HasMaxLength(500);
                entity.Property(e => e.TechniqueTips)
                      .HasMaxLength(500);
                entity.Property(e => e.CommonMistakes)
                      .HasMaxLength(500);
                entity.Property(e => e.ImageUrl)
                      .HasMaxLength(255);
                entity.Property(e => e.VideoUrl)
                      .HasMaxLength(255);
            });

             modelBuilder.Entity<Exercise>()
                    .HasOne(e => e.TrainingProgram)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(e => e.TrainingProgramId)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Login).IsUnique(); // Логин должен быть уникальным
                entity.Property(u => u.Login).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.FullName).HasMaxLength(100);

                // Связь один к одному с UserProfile
                entity.HasOne(u => u.UserProfile)
                      .WithOne() // или .WithOne(up => up.User), если у UserProfile есть навигация
                      .HasForeignKey<User>(u => u.UserProfileId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>()
                    .HasMany(u => u.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(j => j.ToTable("UserRoles"));
        }

    }
}