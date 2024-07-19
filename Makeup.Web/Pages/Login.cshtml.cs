using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Makeup.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; } = default!;
        public string? ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await
               _signInManager.PasswordSignInAsync(Input?.Email!, Input?.Password!, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
                ModelState.AddModelError(string.Empty, "Неуспешен вход.");
            }

            return Page();
        }

    }
}
