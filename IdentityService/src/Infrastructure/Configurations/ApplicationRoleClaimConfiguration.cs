using Infrastructure.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        => builder.ToTable("RoleClaims", SchemaNames.Identity);
}
