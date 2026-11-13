namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasConversion(customerId => customerId.Value, dbId => CustomerId.Of(dbId));
            // hasConversion => first lambda responsible for conversion before storing in database(from CustomerId to GUID),
            // second lambda responsible for conversion after retrieving from database(from GUID to CustomerId)

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Email).HasMaxLength(100);

            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
