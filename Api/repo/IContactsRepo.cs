using Contacts.Api.Entities;
using System.Collections.Generic;

namespace Contacts.Api.repo
{
    public interface IContactsRepo
    {
        ContactItem GetItem(int id);
        IEnumerable<ContactItem> GetItems();
        ContactItem CreateItem(ContactItem item);
        void UpdateItem(ContactItem item);
        void DeleteItem(int id);
    }
}