// using System.Collections.Generic;
// using Contacts.Api.Entities;
// using System.Linq;

// namespace Contacts.Api.repo
// {
//     public class ContactsRepo : IContactsRepo
//     {
//         private readonly List<ContactItem> contactitems = new()
//         {
//             new ContactItem { id = 0, name = new() { first = "Not", middle = "Frito", last = "Sure" }, address = new() { street = "123 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.home } }, email = "harold@example.com" },
//             new ContactItem { id = 1, name = new() { first = "Bob", middle = "Jo", last = "Jones" }, address = new() { street = "321 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.mobile } }, email = "harold1@example.com" },
//             new ContactItem { id = 2, name = new() { first = "Robert", middle = "Jo", last = "Burns" }, address = new() { street = "333 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.mobile } }, email = "harold2@example.com" },
//             new ContactItem { id = 3, name = new() { first = "Third", middle = "Namey", last = "McThirderson" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.home } }, email = "harold3@example.com" },
//             new ContactItem { id = 4, name = new() { first = "Teejay", middle = "Ahrjay", last = "Backslashinfourth" }, address = new() { street = "555 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.home } }, email = "harold4@example.com" },
//             new ContactItem { id = 5, name = new() { first = "Albert", middle = "Ramey", last = "McThirderson" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-9148", type = PhoneType.home }, new() { number = "302-211-3148", type = PhoneType.work } }, email = "harold7@example.com" },
//             new ContactItem { id = 5, name = new() { first = "Chip", middle = "Sam", last = "Smithy" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhone[] { new() { number = "302-611-3147", type = PhoneType.home }, new() { number = "302-211-3149", type = PhoneType.home } }, email = "harold7@example.com" }
//         };

//         public ContactItem CreateItem(ContactItem item)
//         {
//             var maxId= contactitems.Select(s => s.id).Max();
//             ContactItem newItem = new()
//             {
//                 id = maxId++,
//                 name = item.name,
//                 address = item.address,
//                 phone = item.phone,
//                 email = item.email
//             };

//             contactitems.Add(newItem);

//             return newItem;
//         }

//         public void DeleteItem(int id)
//         {
//             var index = contactitems.FindIndex(existingItem => existingItem.id == id);
//             contactitems.RemoveAt(index);            
//         }

//         public ContactItem GetItem(int id)
//         {
//             return contactitems[id];
//         }

//         public IEnumerable<ContactItem> GetItems()
//         {
//             return contactitems;
//         }

//         public void UpdateItem(ContactItem item)
//         {
//             var index = contactitems.FindIndex(existingItem => existingItem.id == item.id);
//             contactitems[index] = item;
//         }
//     }
// }