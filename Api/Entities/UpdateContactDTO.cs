using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public record UpdateContactDTO
    {
        [Required]
        public ContactNameRecord name { get; init; }

        [Required]
        public ContactAddressRecord address { get; init; }

        [Required]
        public ContactPhoneRecord[] phone { get; init; }

        [Required]
        [EmailAddress]
        public string email { get; init; }
    }
}