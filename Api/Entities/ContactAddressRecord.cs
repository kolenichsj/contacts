namespace Contacts.Api.Entities
{
    public record ContactAddressRecord
    {
        public string street { get; init; }
        public string city { get; init; }
        public string state { get; init; }
        public string zip { get; init; }
    }
}