using Microsoft.AspNetCore.Mvc;
using MVCBlog.Data;
using MVCBlog.Models.ViewModels;
using MVCBlog.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCBlog.Models.ViewModels;

namespace MVCBlog.Controllers
{
    public class AdminController : Controller
    {
        private MyBlogDbContext _myBlogDbContext;
        public AdminController(MyBlogDbContext myBlogDbContext) {
            _myBlogDbContext = myBlogDbContext;
        }
        public async Task<IActionResult> Index()
        {
            if (_myBlogDbContext.BlogPosts == null)
            {
                return Problem("Entity set 'MyBlogDbContext.Posts'  is null.");
            }

            IQueryable<string> postQuery = from m in _myBlogDbContext.BlogPosts
                                          orderby m.Heading
                                          select m.Heading;
            var posts = await _myBlogDbContext.BlogPosts.ToListAsync(); // Use ToListAsync()
            if (_myBlogDbContext.Tags == null)
            {
                return Problem("Entity set 'MyBlogDbContext.Tags'  is null.");
            }

            IQueryable<string> tagQuery = from m in _myBlogDbContext.Tags
                                            orderby m.Name
                                            select m.Name;
            var tags = await _myBlogDbContext.Tags.ToListAsync(); // Use ToListAsync()
            var viewModel = new DataViewModel
            {
                Posts = posts,
                Tags = tags
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }
        [HttpPost]
        [ActionName ("AddTag")]
        public IActionResult AddTag(AddTagRequest addTagRequest) 
        {
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            _myBlogDbContext.Tags.Add(tag);
            _myBlogDbContext.SaveChanges();
            return RedirectToAction("AddTag");
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        [ActionName("AddPost")]
        public IActionResult AddPost(AddPostRequest addPostRequest)
        {
            //Mapping AddPostRequest to Post domain model
            var post = new BlogPost
            {
                Author = addPostRequest.Author,
                Content = addPostRequest.Content,
                Heading = addPostRequest.Heading,
                PageTitle = addPostRequest.PageTitle,
                ShortDescription = addPostRequest.ShortDescription,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                UrlHandle = addPostRequest.UrlHandle,
                PublishedDate = DateTime.Now,
                Visible = true
            };
            _myBlogDbContext.BlogPosts.Add(post);
            _myBlogDbContext.SaveChanges();
            return RedirectToAction("AddPost");
        }
        public async Task<IActionResult> DeleteTag(Guid? id)
        {
            if (id == null || _myBlogDbContext.Tags == null)
            {
                return NotFound();
            }
            var tag = await _myBlogDbContext.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost, ActionName("DeleteTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTagConfirmed(Guid id)
        {
            if (_myBlogDbContext.Tags == null)
            {
                return Problem("Entity set 'MyBlogDbContext.Tags'  is null.");
            }
            var tag = await _myBlogDbContext.Tags.FindAsync(id);
            if (tag != null)
            {
                _myBlogDbContext.Tags.Remove(tag);
            }
            await _myBlogDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeletePost(Guid? id)
        {
            if (id == null || _myBlogDbContext.BlogPosts == null)
            {
                return NotFound();
            }
            var post = await _myBlogDbContext.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostConfirmed(Guid id)
        {
            if (_myBlogDbContext.BlogPosts == null)
            {
                return Problem("Entity set 'MyBlogDbContext.Posts'  is null.");
            }
            var post = await _myBlogDbContext.BlogPosts.FindAsync(id);
            if (post != null)
            {
                _myBlogDbContext.BlogPosts.Remove(post);
            }
            await _myBlogDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
