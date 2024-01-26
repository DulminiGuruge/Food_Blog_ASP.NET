using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Identity;
using BlogApp.Enum;
using X.PagedList;
using BlogApp.Services.ViewModels;
using System.Reflection.Metadata;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        private readonly BlogSearchService _blogSearchService;

        public PostsController(ApplicationDbContext context, ISlugService slugService, IImageService imageService, UserManager<BlogUser> userManager, BlogSearchService blogSearchService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
            _blogSearchService = blogSearchService;
        }

        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 5;

            var posts = _blogSearchService.Search(searchTerm);

           
            return View(await posts.ToPagedListAsync(pageNumber,pageSize));



        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {

            
            var applicationDbContext = _context.Posts.Include(p => p.Blog).Include(p => p.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> BlogPostIndex(int?id, int? page)
        {
            if(id is null)
            {
                return NotFound();
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;

            //var posts = _context.Posts.Where(p => p.BlogId == id).ToList();
            var posts =await _context.Posts
                .Where(p => p.BlogId == id && p.ReadyStaus == Enum.ReadyStatus.ProductionReady)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(posts);
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .Include(p=>p.Tags)
                .Include(p=>p.Comments)
                .ThenInclude(c=>c.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var dataVM = new PostDetailViewModel()
            {

                Post = post,
                Tags = _context.Tags.Select(t => t.Text.ToLower()).Distinct().ToList()
                

            };

            ViewData["HeaderImage"] = _imageService.DecodeImage(post.ImageData, post.ContentType);
            ViewData["MainText"] = post.Title;
            ViewData["SubText"] = post.Abstract;


            return View(dataVM);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "ID", "Description");
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,ReadyStaus,Image")] Post post,List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                
                post.Created = DateTime.Now.ToUniversalTime();

                var authorId = _userManager.GetUserId(User);
                post.BlogUserId = authorId;

                post.ImageData = await _imageService.EncodeImageAsync(post.Image);
                post.ContentType = _imageService.ContentType(post.Image);


         



                var slug = _slugService.UrlFrienly(post.Title);
                //if (!_slugService.IsUnique(slug))
                //{
                //    ModelState.AddModelError("Title", "title you rpovided gives an error as it results in a duplicate slug.");
                //    ViewData["TagValue"] = string.Join(",", tagValues);
                //    return View(post);
                //}

                //create a variable to store whether an error has occured

                var validationError = false;


                //post.Slug = slug;

                if (string.IsNullOrEmpty(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("", "The tile you provided cannot be used as it result in empty slug");
                    //ViewData["TagValues"] = string.Join(",", tagValues);
                   // return View(post);

                }

                //Detect incoming duplicate slugs

                if (!_slugService.IsUnique(slug))
                {
                    validationError = true;
                    ModelState.AddModelError("Title", "The tile you provided cannot be used as it is giving a duplicate slug");
                   // ViewData["TagValues"] = string.Join(",", tagValues);
                   // return View(post);
                }


                if (validationError)
                {
                    ViewData["TagValues"] = string.Join(",", tagValues);
                     return View(post);
                }


                post.Slug = slug;




                _context.Add(post);
                await _context.SaveChangesAsync();

                foreach (var tagText in tagValues)
                {
                    _context.Add(new Tag()
                    {
                        PostId = post.Id,
                        BlogUserId = authorId,
                        Text = tagText


                    });
                }

                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "ID", "Name", post.BlogId);
           // ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", post.BlogUserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p=>p.Tags).FirstOrDefaultAsync(p=>p.Id==id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "ID", "Name", post.BlogId);
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", post.BlogUserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,ReadyStaus")] Post post,IFormFile newImage,List<string> tagValues)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //the original post
                    var newPost = await _context.Posts.Include(p=>p.Tags).FirstOrDefaultAsync(p=>p.Id==post.Id);

                    newPost.Updated = DateTime.Now.ToUniversalTime();
                    newPost.Title = post.Title;
                    newPost.Abstract = post.Abstract;
                    newPost.Content = post.Content;
                    newPost.ReadyStaus = post.ReadyStaus;


                    var newSlug = _slugService.UrlFrienly(post.Title);
                    if(newSlug != newPost.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            newPost.Title = post.Title;
                            newPost.Slug = newSlug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "This title cannot be used as it results in duplicate slug.");
                            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
                            return View(post);
                        }
                    }




                    if(newImage is not null)
                    {
                        post.ImageData = await _imageService.EncodeImageAsync(newImage);
                        post.ContentType = _imageService.ContentType(newImage);
                    }

                    //Remove all Tags previosly associated with this post
                    _context.Tags.RemoveRange(newPost.Tags);

                    //add in the new tags from the edit form
                    foreach(var tagText in tagValues)
                    {
                        _context.Add(new Tag()
                        {

                            PostId=post.Id,
                            BlogUserId=newPost.BlogUserId,
                            Text = tagText

                        });
                    }



                   // _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "ID", "Description", post.BlogId);
            //ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", post.BlogUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .Include(p => p.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
