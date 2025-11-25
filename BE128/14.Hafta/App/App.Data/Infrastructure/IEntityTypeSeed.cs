using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Infrastructure
{
    public interface IEntityTypeSeed<T> where T : class
    {
        void SeedData(EntityTypeBuilder<T> builder);
    }
}