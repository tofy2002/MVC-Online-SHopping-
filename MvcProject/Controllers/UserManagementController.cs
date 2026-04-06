using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProject.Models.ViewModel;
using OnlineShopping;
using System.Reflection.Metadata.Ecma335;

namespace MvcProject.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class UserManagementController : Controller
    {
        UserManager<Customer> userManager;
        RoleManager<IdentityRole> roleManager;
        public UserManagementController(UserManager<Customer> _userManager,RoleManager<IdentityRole> _roleManager)
        {
           userManager= _userManager;
            roleManager= _roleManager;

        }
        public async Task<IActionResult> Index()
        {
            var myuser =  userManager.GetUserId(User);
            var allusers = userManager.Users.Where(c=>myuser != c.Id).ToList();
            var vm = new List<UserManagementVM>();
            foreach (var user in allusers) {
                var role = await userManager.GetRolesAsync(user);
                UserManagementVM um = new UserManagementVM
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CurrentRole = role.FirstOrDefault()
                };
                vm.Add(um);
            }
        
            var allroles =roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(allroles, "Name", "Name");
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userid,string role)
        { 
            var user=await userManager.FindByIdAsync(userid);
            if (user == null)
                return NotFound();
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user,roles);
            await userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Index");
        }
    }
}
