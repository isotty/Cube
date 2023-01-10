using Microsoft.AspNetCore.Mvc;
using Cube.Models;
using Cube.Datas;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Cube.Controllers
{
    public class LoginController : Controller
    {
        private readonly CubeProjetIndiContext _employsRepository;
        public LoginController(CubeProjetIndiContext employsRepository)
        {
            _employsRepository = employsRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {   
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return  View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(Login lg)
        {
            var users = _employsRepository.Employs.Include(e => e.Cities).Include(e => e.Service);


            Employs userFound = users.FirstOrDefault(x => x.FirstName == lg.FirstName && lg.LastName == x.SurName && x.PassWord == lg.PassWord);
            if (userFound is not null) 
            
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, lg.FirstName),
                    new Claim("OtherProperties","")
                };
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                AuthenticationProperties properties = new AuthenticationProperties() 
                {
                    AllowRefresh = true,
                    IsPersistent = lg.RememberMe
                };

                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, properties);
                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "User Not Found" ;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            ViewData["Login"] = null;
            return RedirectToAction("Index", "Home");

        }
    }
}
