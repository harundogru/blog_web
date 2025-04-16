using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Blog_Web.Models;

public class AppUser : IdentityUser
{
    public List<Blog> Blogs { get; set; }
}