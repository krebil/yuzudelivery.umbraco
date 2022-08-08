using System.ComponentModel.DataAnnotations;

namespace YuzuDelivery.Umbraco.Members
{
    public class ForgottenPasswordVm
    {
        [EmailAddress(ErrorMessage = "The email address must be valid")]
        [Required(ErrorMessage = "Email address is a required field")]
        public string Email { get; set; }
    }
}
