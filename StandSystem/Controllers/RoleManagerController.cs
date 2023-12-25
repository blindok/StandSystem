using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StandSystem.IdentityScheme;
using System.Data;

namespace StandSystem.Controllers;

[Authorize(Roles = "admin")]
public class RoleManagerController : Controller
{
    private readonly RoleManager<Role> _roleManager;

    public RoleManagerController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName)
    {
        if (roleName != null)
        {
            await _roleManager.CreateAsync(new Role() { Name = roleName, NormalizedName = roleName.ToUpper()});
        }
        return RedirectToAction("Index");
    }
}
