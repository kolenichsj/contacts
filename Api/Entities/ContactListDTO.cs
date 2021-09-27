namespace Contacts.Api.Entities
{
    public record ContactListDTO
    {
        public ContactNameRecord name { get; init; }
        public string phone { get; init; }
    };
}