using Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        => builder.ToTable("UserClaims", SchemaNames.Identity);
}
