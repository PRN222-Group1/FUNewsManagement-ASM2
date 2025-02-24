using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessObjects.Entities;

namespace DataAccessLayer.Configs
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.Property(e => e.Id).HasColumnName("CategoryID");
            builder.Property(e => e.CategoryDescription).HasMaxLength(250);
            builder.Property(e => e.CategoryName).HasMaxLength(100);
            builder.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

            builder.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Category_Category");
        }
    }
}
