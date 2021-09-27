using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public struct ContactName
    {
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
    }
}