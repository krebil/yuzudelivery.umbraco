using System.ComponentModel.DataAnnotations;

namespace YuzuDelivery.Umbraco.Members
{
    public class CreateMemberVm
    {
        [Required(ErrorMessage = "Name is a requierd field")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "The email address must be valid")]
        [Required(ErrorMessage = "Email address is a requierd field")]
        public string Email { get; set; }
    }
}
