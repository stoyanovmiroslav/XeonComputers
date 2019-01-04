using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<XeonUser> _userManager;
        private readonly SignInManager<XeonUser> _signInManager;
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<XeonUser> userManager,
            SignInManager<XeonUser> signInManager,
            IEmailSender emailSender,
            IUsersService userService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userService = userService;
            _mapper = mapper;
        }

        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
            [EmailAddress]
            [Display(Name = "Имейл")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Полето \"{0}\" e задължително.")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Име")]
            public string Name { get; set; }

            [Display(Name = "ЕИК")]
            public string UniqueIdentifier { get; set; }

            [Display(Name = "Управител")]
            public string Manager { get; set; }
            
            [Display(Name = "Град")]
            public string AddressCityName { get; set; }

            [Display(Name = "Адрес")]
            public string AddressStreet { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = this.User.Identity.Name;

            var company = _userService.GetUserCompanyByUsername(Username);
            Input = _mapper.Map<InputModel>(company);

            if (Input == null)
            {
                Input = new InputModel();
            }

            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;
            Input.Email = email;
            Input.PhoneNumber = phoneNumber;

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                _userService.EditFirstName(user, Input.FirstName);
            }

            if (Input.LastName != user.LastName)
            {
                _userService.EditLastName(user, Input.LastName);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Потребителският ви профил бе актуализиран";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Потвърдете имейла си",
                $"Моля потвърдете вашия акаунт, като <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>кликнете тук</a>.");

            StatusMessage = "Имейлът за потвърждение е изпратен. Моля, проверете електронната си поща.";
            return RedirectToPage();
        }
    }
}