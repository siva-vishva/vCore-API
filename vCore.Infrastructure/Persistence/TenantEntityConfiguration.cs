using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vCore.Domain.Tenants;

namespace vCore.Infrastructure.Persistence;

public sealed class TenantEntityConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenant", "tenant");

        builder.HasKey(x => x.TenantId);

        builder.Property(x => x.TenantId)
            .HasColumnName("TenantId")
            .IsRequired();

        builder.Property(x => x.TenantCode)
            .HasColumnName("TenantCode")
            .HasMaxLength(Tenant.MaxTenantCodeLength)
            .IsRequired();

        builder.HasIndex(x => x.TenantCode)
            .IsUnique();

        builder.Property(x => x.TenantName)
            .HasColumnName("TenantName")
            .HasMaxLength(Tenant.MaxTenantNameLength)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.TenantType)
            .HasColumnName("TenantType")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.TimeZone)
            .HasColumnName("TimeZone")
            .HasMaxLength(Tenant.MaxTimeZoneLength)
            .IsRequired();

        builder.Property(x => x.Locale)
            .HasColumnName("Locale")
            .HasMaxLength(Tenant.MaxLocaleLength)
            .IsRequired();

        builder.Property(x => x.CountryCode)
            .HasColumnName("CountryCode")
            .HasColumnType("char(2)")
            .IsRequired();

        builder.Property(x => x.ContactEmail)
            .HasColumnName("ContactEmail")
            .HasMaxLength(Tenant.MaxContactEmailLength)
            .IsRequired();

        builder.Property(x => x.ContactPhone)
            .HasColumnName("ContactPhone")
            .HasMaxLength(Tenant.MaxContactPhoneLength)
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime2(3)")
            .HasDefaultValueSql("sysutcdatetime()")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .IsRequired(false);

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("datetime2(3)")
            .IsRequired(false);

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy")
            .IsRequired(false);

        builder.Property(x => x.RowVersion)
            .HasColumnName("RowVersion")
            .IsRowVersion();
    }
}
