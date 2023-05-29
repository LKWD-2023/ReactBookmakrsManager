using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ReactBookmarkManager.Data
{
    public class BookmarksRepository
    {
        private readonly string _connectionString;

        public BookmarksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Bookmark bookmark)
        {
            using var context = new BookmarkManagerContext(_connectionString);
            context.Bookmarks.Add(bookmark);
            context.SaveChanges();
        }

        public List<Bookmark> GetForUser(int userId)
        {
            using var context = new BookmarkManagerContext(_connectionString);
            return context.Bookmarks.Where(b => b.UserId == userId).ToList();
        }

        public bool UserOwnsBookmark(int userId, int bookmarkId)
        {
            using var context = new BookmarkManagerContext(_connectionString);
            return context.Bookmarks.Any(b => b.UserId == userId && b.Id == bookmarkId);
        }

        public void UpdateBookmark(string title, int bookmarkId)
        {
            using var context = new BookmarkManagerContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE Bookmarks SET Title = {title} WHERE Id = {bookmarkId}");
        }

        public void DeleteBookmark(int bookmarkId)
        {
            using var context = new BookmarkManagerContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Bookmarks WHERE Id = {bookmarkId}");
        }

        public List<TopBookmark> GetTopBookmarkUrls()
        {
            using var context = new BookmarkManagerContext(_connectionString);
            return context.Bookmarks.GroupBy(b => b.Url)
                .OrderByDescending(b => b.Count()).Take(5)
                .Select(i => new TopBookmark
                {
                    Url = i.Key,
                    Count = i.Count()
                })
                .ToList();
        }
        
        public List<TopBookmark> GetTopBookmarkUrls_TheSimplestWay()
        {
            using var context = new BookmarkManagerContext(_connectionString);
            var bookmarks = context.Bookmarks.ToList();
            var list = new List<TopBookmark>();
            foreach (var bookmark in bookmarks)
            {
                var topBookmark = list.FirstOrDefault(b => b.Url == bookmark.Url);
                if (topBookmark == null)
                {
                    topBookmark = new TopBookmark { Url = bookmark.Url };
                    list.Add(topBookmark);
                }

                topBookmark.Count++;
            }

            return list.OrderByDescending(b => b.Count).Take(5).ToList();
        }

        public List<TopBookmark> GetTopBookmarkUrls_TheLongWay()
        {
            using var context = new BookmarkManagerContext(_connectionString);
            var bookmarks = context.Bookmarks.ToList();
            var dictionary = new Dictionary<string, int>();
            foreach (var bookmark in bookmarks)
            {
                if (dictionary.ContainsKey(bookmark.Url))
                {
                    dictionary[bookmark.Url]++;
                }
                else
                {
                    dictionary[bookmark.Url] = 1;
                }
            }


            return dictionary.OrderByDescending(k => k.Value).Take(5).Select(kvp => new TopBookmark
            {
                Url = kvp.Key,
                Count = kvp.Value
            }).ToList();
        }

        public List<TopBookmark> GetTopBookmarkUrls_DbQueryWay()
        {
            var query = @"SELECT TOP 5 Url, Count(*) AS 'Count' from Bookmarks
                        GROUP BY Url
                        ORDER BY Count(*) DESC";
            
            using var context = new BookmarkManagerContext(_connectionString);
            return context.TopBookmarks.FromSqlRaw(query).ToList();
        }
    }
}
