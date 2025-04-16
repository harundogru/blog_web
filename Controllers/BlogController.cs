using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog_Web.Data;
using Blog_Web.Models;
using Blog_Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Blog_Web.Controllers;

public class BlogController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public BlogController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    
    [AllowAnonymous]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var blogsQuery = _context.Blogs
            .Include(b => b.Category)
            .Include(b => b.User)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            blogsQuery = blogsQuery.Where(b => b.CategoryId == categoryId.Value);
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.SelectedCategory = categoryId;

        var blogs = await blogsQuery.ToListAsync();
        return View(blogs);
    }

    // GET: Blog/Create
    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(BlogCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }

        string? imagePath = null;

        if (model.Image != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            imagePath = "/uploads/" + fileName;
        }

        var user = await _userManager.GetUserAsync(User);

        var blog = new Blog
        {
            Title = model.Title,
            Content = model.Content,
            CategoryId = model.CategoryId,
            UserId = user.Id,
            PublishedDate = DateTime.UtcNow,
            ImagePath = imagePath
        };

        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }           

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var blog = await _context.Blogs
            .Include(b => b.Category)
            .Include(b => b.User)
            .Include(b => b.Comments)                 // ⬅️ Yorumları da dahil et
            .ThenInclude(c => c.User)             // ⬅️ Yorumu yapan kullanıcıyı da çek
            .FirstOrDefaultAsync(b => b.Id == id);

        if (blog == null) return NotFound();

        return View(blog);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound();

        var viewModel = new BlogEditViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            CategoryId = blog.CategoryId
        };

        ViewBag.Categories = _context.Categories.ToList();
        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(BlogEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }

        var blog = await _context.Blogs.FindAsync(model.Id);
        if (blog == null) return NotFound();

        blog.Title = model.Title;
        blog.Content = model.Content;
        blog.CategoryId = model.CategoryId;
        blog.PublishedDate = DateTime.UtcNow;
        
        if (model.Image != null)
        {

            // Yeni görselin adını belirle
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            // Eski görseli sil
            if (!string.IsNullOrEmpty(blog.ImagePath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Yeni görseli yükle
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            // Yeni yolu yaz
            blog.ImagePath = "/uploads/" + fileName;
        }
        
        _context.Update(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound();

        return View(blog);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Blog blog)
    {
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
