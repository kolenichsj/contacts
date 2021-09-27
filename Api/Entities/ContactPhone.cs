namespace Contacts.Api.Entities
{
    public struct ContactPhone
    {
        public string number { get; set; }

        public PhoneType type { get; set; }
    }
}