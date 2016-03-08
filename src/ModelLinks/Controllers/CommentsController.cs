using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ModelLinks.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace ModelLinks.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comment.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Comment comment = await _context.Comment.SingleAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User");
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Make sure to set the comment to user that crated it
                string id = User.GetUserId();
                if(id != null) {
                    // Only update and save if there is a current user
                    comment.ApplicationUserId = id;
                    _context.Comment.Add(comment);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", comment.ApplicationUserId);
            return View(comment);
        }

        // POST: Comments/Add
        [HttpPost]
        public async Task<IActionResult> Add(Post post)
        {
            Comment comment = new Comment();
            if (ModelState.IsValid)
            {
                // Make sure to set the comment to user that crated it
                string id = User.GetUserId();
                if (id != null)
                {
                    // Only update and save if there is a current user
                    comment.Body = post.Body;
                    comment.Subject = post.Subject;
                    comment.PostId = post.PostId;
                    comment.ApplicationUserId = id;
                    _context.Comment.Add(comment);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Details", "Posts", new { id = post.PostId });
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", comment.ApplicationUserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Comment comment = await _context.Comment.SingleAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", comment.ApplicationUserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
                Post post = await _context.Post.SingleAsync(s => s.PostId == comment.PostId);
                return RedirectToAction("Details", "Posts", new { id = post.PostId });
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "User", comment.ApplicationUserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Comment comment = await _context.Comment.SingleAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await _context.Comment.SingleAsync(m => m.CommentId == id);
            Post post = await _context.Post.SingleAsync(s => s.PostId == comment.PostId);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = post.PostId });
        }
    }
}
