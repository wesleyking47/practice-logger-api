using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Infrastructure.Persistence.Configurations
{
    public class PracticeSessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("practice_sessions");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Activity).HasMaxLength(100).IsRequired();

            builder.Property(t => t.Date).HasColumnType("date").IsRequired();

            builder.Property(t => t.Minutes).IsRequired();

            builder.Property(t => t.Notes).HasMaxLength(500).IsRequired(false);
        }
    }
}
