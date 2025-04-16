using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog_Web.Data;
using Blog_Web.Models;
using Blog_Web.ViewModels;
using Microsoft.AspNetCore.Identity;


public class CommentController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CommentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Blog", new { id = model.BlogId });
        }

        var user = await _userManager.GetUserAsync(User);

        var comment = new Comment
        {
            Content = model.Content,
            BlogId = model.BlogId,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Blog", new { id = model.BlogId });
    }
}