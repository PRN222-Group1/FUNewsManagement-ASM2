using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessObjects.Entities;

namespace DataAccessLayer.Configs
{
    public class NewsArticleConfiguration : IEntityTypeConfiguration<NewsArticle>
    {
        public void Configure(EntityTypeBuilder<NewsArticle> builder)
        {
            builder.ToTable("NewsArticle");

            builder.Property(e => e.Id).HasColumnName("NewsArticleID");
            builder.Property(e => e.CategoryId).HasColumnName("CategoryID");
            builder.Property(e => e.CreatedById).HasColumnName("CreatedByID");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.Headline).HasMaxLength(150);
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime");
            builder.Property(e => e.NewsContent).HasMaxLength(4000);
            builder.Property(e => e.NewsSource).HasMaxLength(400);
            builder.Property(e => e.NewsTitle).HasMaxLength(400);
            builder.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");
            builder.Property(e => e.ImageUrl).HasMaxLength(500);

            builder.HasOne(d => d.Category).WithMany(p => p.NewsArticles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_Category");

            builder.HasOne(d => d.CreatedBy).WithMany(p => p.NewsArticles)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_SystemAccount");

            builder.HasMany(m => m.Tags)
                .WithMany(t => t.NewsArticles)
                .UsingEntity<NewsTag>(
                    j => j
                        .HasOne(nt => nt.Tag)
                        .WithMany()
                        .HasForeignKey(nt => nt.TagId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_NewsTag_Tag"),
                    j => j
                        .HasOne(nt => nt.NewsArticle)
                        .WithMany()
                        .HasForeignKey(nt => nt.NewsArticleId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_NewsTag_NewsArticle"),
                    j =>
                    {
                        j.HasKey(nt => new { nt.NewsArticleId, nt.TagId });
                        j.Property(nt => nt.NewsArticleId).HasColumnName("NewsArticleID");
                        j.Property(nt => nt.TagId).HasColumnName("TagID");
                        j.ToTable("NewsTag");
                    });
        }
    }
}
