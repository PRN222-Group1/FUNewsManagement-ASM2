using BusinessObjects.Entities;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Data
{
    public static class FuNewsManagementContextSeed
    {
        public static async Task SeedAsync(FuNewsManagementContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            // Seed categories
            if (!context.Categories.Any())
            {
                var catData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/categories.json");

                var categories = JsonSerializer.Deserialize<List<Category>>(catData);

                if (categories == null) return;

                context.Categories.AddRange(categories);

                await context.SaveChangesAsync();
            }

            // Seed system account
            if (!context.SystemAccounts.Any())
            {
                var accData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/systemAccounts.json");

                var accs = JsonSerializer.Deserialize<List<SystemAccount>>(accData, options);

                if (accs == null) return;

                context.SystemAccounts.AddRange(accs);

                await context.SaveChangesAsync();
            }

            // Seed news articles
            if (!context.NewsArticles.Any())
            {
                var newsData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/newsArticles.json");

                var news = JsonSerializer.Deserialize<List<NewsArticle>>(newsData);

                if (news == null) return;

                context.NewsArticles.AddRange(news);

                await context.SaveChangesAsync();
            }

            // Seed tags
            if (!context.Tags.Any())
            {
                var tagData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/tags.json");

                var tags = JsonSerializer.Deserialize<List<Tag>>(tagData);

                if (tags == null) return;

                context.Tags.AddRange(tags);

                await context.SaveChangesAsync();
            }

            // Seed news tags
            if (!context.NewsTags.Any())
            {
                var newsData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/newsTags.json");

                var newsTag = JsonSerializer.Deserialize<List<NewsTag>>(newsData);

                if (newsTag == null) return;

                context.NewsTags.AddRange(newsTag);

                await context.SaveChangesAsync();
            }
        }
    }
}
