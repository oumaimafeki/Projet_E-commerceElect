﻿using E_commerceElect.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceElect.Controllers
{
	public class AccountController : Controller
	{// GET: AccountController
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser>
		signInManager;
		public AccountController(UserManager<IdentityUser>
		userManager, SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)

		{
			if (ModelState.IsValid)
			{

				var user = new IdentityUser

				{
					UserName = model.Email,
					Email = model.Email
				};
				var result = await userManager.CreateAsync(user, model.Password);
				

				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("index", "home");
				}

				// If there are any errors, add them to the ModelState object
				// which will be displayed by the validation summary tag helper
				foreach (var error in result.Errors)

				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}
        [HttpPost]

        public async Task<IActionResult> Logout()

        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,
                model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
