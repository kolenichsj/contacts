using Contacts.Api.Entities;
using Contacts.Api.repo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contacts.Api.Controllers
{
    [ApiController]
    [Route("contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactsRepo repo;

        public ContactController(IContactsRepo repo)
        {
            this.repo = repo;
        }

        // GET /contacts
        [HttpGet]
        public ActionResult<IEnumerable<ContactRecord>> GetContacts()
        {
            try
            {
                var retItems = this.repo.GetItems().Select(s => s.ToRecord());
                return Ok(retItems);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET /contacts/{id}
        [HttpGet("{id}")]
        public ActionResult<ContactRecord> GetContact(int id)
        {
            try
            {
                var item = this.repo.GetItem(id);
                return item is null ? NotFound() : Ok(item.ToRecord());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST /contacts
        [HttpPost]
        public ActionResult<ContactRecord> CreateContact(CreateContactDTO contactRecord)
        {
            try
            {
                ContactItem newContact = new()
                {
                    id = 0,
                    name = contactRecord.name.FromRecord(),
                    address = contactRecord.address.FromRecord(),
                    phone = contactRecord.phone.Select(s => s.FromRecord()).ToArray(),
                    email = contactRecord.email
                };

                newContact = repo.CreateItem(newContact);
                return CreatedAtAction(nameof(GetContact), new { id = newContact.id }, newContact.ToRecord());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // PUT /contacts/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, UpdateContactDTO contactRecord)
        {
            try
            {
                var existingItem = repo.GetItem(id);

                if (existingItem is null)
                {
                    return NotFound();
                }

                existingItem.name = contactRecord.name.FromRecord();
                existingItem.address = contactRecord.address.FromRecord();
                existingItem.phone = contactRecord.phone.Select(s => s.FromRecord()).ToArray();
                existingItem.email = contactRecord.email;

                repo.UpdateItem(existingItem);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            try
            {
                var existingItem = repo.GetItem(id);

                if (existingItem is null)
                {
                    return NotFound();
                }

                repo.DeleteItem(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET /contacts/call-list
        [HttpGet("call-list")]
        public ActionResult<IEnumerable<ContactListDTO>> GetCallList()
        {
            try
            {
                var curItems = this.repo.GetItems(); // all items
                var curfilter = new List<ContactListDTO>();

                //this accounts for users with multiple home phone numbers (as unlikely as that is these days)
                foreach (var curritem in curItems)
                {
                    var homeIndexes = curritem.phone.getHomePhones();
                    foreach (int index in homeIndexes)
                    {
                        curfilter.Add(curritem.AsListDTO(index));
                    }
                }

                var sortedfilter = curfilter
                    .OrderBy(contact => contact.name.last)
                    .ThenBy(contact => contact.name.first);
                return Ok(sortedfilter);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}