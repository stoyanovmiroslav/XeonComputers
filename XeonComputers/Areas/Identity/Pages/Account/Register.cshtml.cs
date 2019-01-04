using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using XeonComputers.Common;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.ShoppingCart;

namespace XeonComputers.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<XeonUser> _signInManager;
        private readonly UserManager<XeonUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUsersService _userService;
        private readonly IShoppingCartsService _shoppingCartService;

        public RegisterModel(
            UserManager<XeonUser> userManager,
            SignInManager<XeonUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUsersService userService,
            IShoppingCartsService shoppingCartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userService = userService;
            _shoppingCartService = shoppingCartService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Повтори парола")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Име")]
            public string FirsName { get; set; }

            [Required]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }
        }

        public async void OnGet(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Home/Error");
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new XeonUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirsName,
                    LastName = Input.LastName,
                    ShoppingCart = new ShoppingCart()
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var cart = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
                    if (cart != null)
                    {
                        foreach (var product in cart)
                        {
                            _shoppingCartService.AddProductInShoppingCart(product.Id, Input.Email, product.Quantity);
                        }

                        HttpContext.Session.Remove(GlobalConstants.SESSION_SHOPPING_CART_KEY);
                    }

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
