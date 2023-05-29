using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ReactBookmarkManager.Data
{
    public class BookmarkManagerContextFactory : IDesignTimeDbContextFactory<BookmarkManagerContext>
    {
        public BookmarkManagerContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}ReactBookmarkManager.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new BookmarkManagerContext(config.GetConnectionString("ConStr"));
        }
    }
}