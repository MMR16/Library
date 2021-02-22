using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Library.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        //Role Manger
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDBContext Db;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext _Db,
        IWebHostEnvironment _webHostEnvironment
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            Db = _Db;
            webHostEnvironment = _webHostEnvironment;
        }

        //Bind Property To Bind data 2 Ways Between View & Model
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public string UserImage { get; set; }

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
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //adding more to Register Model
            [Required(ErrorMessage = "Please Enter Your First Name")]
            [StringLength(15, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2~15 Letters")]
            [Display(Name = "First Name")]
            public string UserFName { get; set; }

            [Display(Name = "Last Name")]
            [StringLength(15, MinimumLength = 2, ErrorMessage = "Last Name Must Be Between 2~15 Letters")]
            public string UserLName { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Required(ErrorMessage = "Please Enter Mobile Number")]
            [Display(Name = "Mobile Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "User Name")]
            [StringLength(10, MinimumLength = 2, ErrorMessage = "User Name Must Be Between 2~10 Letters")]
            public string UserName { get; set; }

            [DisplayName("Image")]
            public string UserImage { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            //Creating Roles For First Time
            //Check If Not Exist In Database
            if (!await _roleManager.RoleExistsAsync(WC.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(WC.AdminRole));
                await _roleManager.CreateAsync(new IdentityRole(WC.CustomerRole));
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //Changing IdentityUser With AppUser To Get New Properties We Add
                //add to container in startUp First
                var user = new AppUser();
                //setting Properties
                user.UserName = Input.UserName;
                user.Email = Input.Email;
                user.PhoneNumber = Input.PhoneNumber;
                user.UserFName = Input.UserFName;
                user.UserLName = Input.UserLName;

                //Image File
                #region Image File Upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count < 1)
                {
                    ViewData["ImageError"] = "Please User Upload Image ";
                    return Page();
                }
                if (files.Count is not 1)
                {
                    ViewData["ImageError"] = "Please Upload one Image only don't think you'r so smart";
                    return Page();
                }
                string extention = Path.GetExtension(files[0].FileName);
                if (Path.HasExtension(extention))
                {
                    //for vaildate image by the type "image"
                    //better than extentions beccause i onky know about 5 extention of images
                    string image_type = string.Join("", files[0].ContentType.Take(5)).ToLower();
                    if (image_type == "image")
                    {
                        string webrootbath = webHostEnvironment.WebRootPath;
                        string upload = webrootbath + WC.UserImagePath;
                        string filename = Guid.NewGuid().ToString(); // to create guid image name to avoid duplication errors
                        using (var filestream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        //setting Image Name
                        user.UserImage = filename + extention;
                    }
                    else
                    {
                        ViewData["ImageError"] = "Please Upload Proper Image";
                        return Page();
                    }
                }
                else
                {

                    ViewData["ImageError"] = "Please Upload Proper Image";
                    return Page();
                }

                #endregion
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var UsersNo = Db.Users.Count();
                    //Register First User As Admin User
                    if (UsersNo < 2 && UsersNo > 0)
                    {
                        await _userManager.AddToRoleAsync(user, WC.AdminRole);
                    }
                    else if (User.IsInRole(WC.AdminRole))
                    {
                        await _userManager.AddToRoleAsync(user, WC.AdminRole);
                        return LocalRedirect(returnUrl);

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, WC.CustomerRole);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //Log In Automatically After Register if any User Register 
                        if (!User.IsInRole(WC.AdminRole ))
                        {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return LocalRedirect(returnUrl);
                        }
                    }
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
