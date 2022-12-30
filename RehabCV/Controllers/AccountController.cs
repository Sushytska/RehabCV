using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RehabCV.Configurations;
using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IOptions<ApplicationConfiguration> _optionsApplicationConfiguration;
        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IEmailService emailService,
                                 IOptions<ApplicationConfiguration> o)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _optionsApplicationConfiguration = o;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                    return View(model);              
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Ви не підтвердили свою електронну пошту!");
                    return View(model);
                }

                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Notice()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userWithSameEmail = model.Email == null ? await _userManager.FindByNameAsync(model.Login)
                                                        : await _userManager.FindByEmailAsync(model.Email);

                if (userWithSameEmail == null)
                {
                    var user = new User
                    {
                        UserName = model.Email ?? model.Login,
                        FirstName = model.FirstNameOfUser,
                        MiddleName = model.MiddleNameOfUser,
                        LastName = model.LastNameOfUser,
                        Email = model.Email,
                        Phone = model.PhoneNumber
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Parent");

                        if (model.Email != null)
                        {
                            await _signInManager.SignInAsync(user, false);

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            var emailOfCenter = _optionsApplicationConfiguration.Value.EmailOfCenter;
                            var passwordOfEmail = _optionsApplicationConfiguration.Value.PasswordOfEmail;

                            var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                "Account",
                                new { userId = user.Id, code },
                                protocol: HttpContext.Request.Scheme);
                            await _emailService.SendEmailAsync(model.Email, "Confirm your account",
                                $"Підтвердіть реєстрацію, перейшовши за посиланням: <a href='{callbackUrl}'>link</a>", emailOfCenter, passwordOfEmail);

                            return RedirectToAction("Notice");
                        }
                        else
                        {
                            user.EmailConfirmed = true;

                            await _userManager.UpdateAsync(user);

                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Користувач з даною електронною поштою/логіном вже існує");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error");
        }
    }
}
