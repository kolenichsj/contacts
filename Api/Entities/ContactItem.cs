using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public class ContactItem
    {
        [KeyAttribute]
        public int id { get; set; }
        public ContactName name { get; set; }
        public ContactAddress address { get; set; }
        public ContactPhone[] phone { get; set; }

        [EmailAddress]
        public string email { get; set; }
    }
}