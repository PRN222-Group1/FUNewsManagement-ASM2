using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessObjects.Entities;
using BusinessObjects.Enums;

namespace DataAccessLayer.Configs
{
    public class SystemAccountConfiguration : IEntityTypeConfiguration<SystemAccount>
    {
        public void Configure(EntityTypeBuilder<SystemAccount> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("SystemAccount");

            builder.Property(e => e.Id).HasColumnName("AccountID");
            builder.Property(e => e.AccountEmail).HasMaxLength(70);
            builder.Property(e => e.AccountName).HasMaxLength(100);
            builder.Property(e => e.AccountPassword).HasMaxLength(70);

            builder.Property(c => c.Gender)
                .HasConversion(
                    s => s.ToString(),
                    s => (Gender)Enum.Parse(typeof(Gender), s))
                .HasColumnType("varchar(20)");

            builder.Property(e => e.ImageUrl).HasMaxLength(500);
        }
    }
}
