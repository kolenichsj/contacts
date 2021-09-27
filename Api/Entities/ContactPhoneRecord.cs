using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public record ContactPhoneRecord
    {
        [Phone] 
        public string number { get; init; }
        
        [EnumDataType(typeof(PhoneType))] 
        public string type { get; init; }
    };
}