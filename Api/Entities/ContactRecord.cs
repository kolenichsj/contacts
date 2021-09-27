using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public record ContactRecord
    {
        [KeyAttribute]
        public int id { get; init; }
        public ContactNameRecord name { get; init; }
        public ContactAddressRecord address { get; init; }
        public ContactPhoneRecord[] phone { get; init; }
        public string email { get; init; }
    };
}