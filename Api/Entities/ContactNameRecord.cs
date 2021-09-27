using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Entities
{
    public record ContactNameRecord
    {
        public string first { get; init; }
        public string middle { get; init; }
        public string last { get; init; }
    }
}