using System.ComponentModel.DataAnnotations;

namespace ChatApp.ViewModel
{
    public class RegisterUserViewModel
    {
        [Required]
        public string UserName { get; set; }

     
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name ="Comfirm Password")]
        public string ConfirmPass { get; set; }
    }
}
