using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ModelLinks.Models;
using System.Security.Claims;
using System;
using Microsoft.AspNet.Http;
using System.Collections.Generic;

namespace ModelLinks.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Post.Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Post.SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var comments = _context.Comment.Where<Comment>(c => c.PostId == id);
            ViewBag.Comments = comments;
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                // Make sure to set the comment to user that crated it
                string id = User.GetUserId();
                if (id != null)
                {
                    post.ApplicationUserId = id;
                    _context.Post.Add(post);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", post.ApplicationUserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Post.SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", post.ApplicationUserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", post.ApplicationUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Post post = await _context.Post.SingleAsync(m => m.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Post post = await _context.Post.SingleAsync(m => m.PostId == id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
