using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactBookmarkManager.Data;
using ReactBookmarkManager.Web.ViewModels;

namespace ReactBookmarkManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly string _connectionString;

        public BookmarksController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("add")]
        public void Add(Bookmark bookmark)
        {
            var user = GetCurrentUser();
            bookmark.UserId = user.Id;
            var bookmarksRepo = new BookmarksRepository(_connectionString);
            bookmarksRepo.Add(bookmark);
        }

        [HttpGet]
        [Route("getmybookmarks")]
        public List<Bookmark> GetMyBookmarks()
        {
            var user = GetCurrentUser();
            var bookmarksRepo = new BookmarksRepository(_connectionString);
            return bookmarksRepo.GetForUser(user.Id);
        }

        [HttpPost]
        [Route("updatetitle")]
        public void UpdateTitle(UpdateTitleViewModel viewModel)
        {
            var user = GetCurrentUser();
            var bookmarksRepo = new BookmarksRepository(_connectionString);
            if (!bookmarksRepo.UserOwnsBookmark(user.Id, viewModel.BookmarkId))
            {
                return;
            }

            bookmarksRepo.UpdateBookmark(viewModel.Title, viewModel.BookmarkId);
        }

        [HttpPost]
        [Route("delete")]
        public void Delete(DeleteViewModel viewModel)
        {
            var user = GetCurrentUser();
            var bookmarksRepo = new BookmarksRepository(_connectionString);
            if (!bookmarksRepo.UserOwnsBookmark(user.Id, viewModel.BookmarkId))
            {
                return;
            }

            bookmarksRepo.DeleteBookmark(viewModel.BookmarkId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("topfive")]
        public List<TopBookmark> GetTopFiveBookmarks()
        {
            var bookmarksRepo = new BookmarksRepository(_connectionString);
            return bookmarksRepo.GetTopBookmarkUrls();
            //return bookmarksRepo.GetTopBookmarkUrls_TheLongWay();
            //return bookmarksRepo.GetTopBookmarkUrls_DbQueryWay();
        }

        private User GetCurrentUser()
        {
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetByEmail(User.Identity.Name);
            return user;
        }
    }
}