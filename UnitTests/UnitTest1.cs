using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Contacts.Api.Controllers;
using Contacts.Api.repo;
using Contacts.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using LiteDB;

namespace UnitTests
{
    [SingleThreaded]
    [TestFixture]
    public class Tests
    {
        private ContactController contactsController;
        private string connnectionstring = "contactitems-unittests.db";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.contactsController = new(new LightDBRepo(this.connnectionstring));
        }

        [Test, Order(0)]
        public void TestGetContactsInit()
        {
            var actionContacts = contactsController.GetContacts();
            Assert.IsInstanceOf(typeof(OkObjectResult), actionContacts.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(200, ((OkObjectResult)actionContacts.Result).StatusCode); // cast to get status code
            IEnumerable<ContactRecord> myrecs = ((IEnumerable<ContactRecord>)((OkObjectResult)actionContacts.Result).Value);
            Assert.False(myrecs.Any());
        }

        [Test, Order(1), TestCaseSource("testContacts")]
        public void TestCreateContact(CreateContactDTO testDTO)
        {
            var actionCreate = contactsController.CreateContact(testDTO);
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), actionCreate.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(201, ((CreatedAtActionResult)actionCreate.Result).StatusCode);
            ContactRecord resultsDTO = (ContactRecord)((CreatedAtActionResult)actionCreate.Result).Value;
            Assert.AreEqual(testDTO.address, resultsDTO.address);
            Assert.AreEqual(testDTO.email, resultsDTO.email);
            Assert.AreEqual(testDTO.name, resultsDTO.name);
            Assert.AreEqual(testDTO.phone, resultsDTO.phone);
        }

        [Test, Order(2)]
        public void TestGetContactsAfterInsert()
        {
            var actionContacts = contactsController.GetContacts();
            Assert.IsInstanceOf(typeof(OkObjectResult), actionContacts.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(200, ((OkObjectResult)actionContacts.Result).StatusCode);
            IEnumerable<ContactRecord> myrecs = ((IEnumerable<ContactRecord>)((OkObjectResult)actionContacts.Result).Value);
            Assert.True(myrecs.Any());
        }

        [Test, Order(3)]
        [TestCase(-1)]
        [TestCase(1)]
        public void TestGetContact(int id)
        {
            var actionGet = contactsController.GetContact(id);
            if (id < 0)
            {
                Assert.IsInstanceOf(typeof(NotFoundResult), actionGet.Result); // using this assert to check type before potential invalid cast
                Assert.AreEqual(404, ((NotFoundResult)actionGet.Result).StatusCode);
            }
            else
            {
                Assert.IsInstanceOf(typeof(OkObjectResult), actionGet.Result); // using this assert to check type before potential invalid cast
                Assert.AreEqual(200, ((OkObjectResult)actionGet.Result).StatusCode);
                Assert.AreEqual(id, ((ContactRecord)((OkObjectResult)actionGet.Result).Value).id);
            }
        }

        [Test, Order(4)]
        [TestCase(1)]
        public void TestUpdateContact(int id)
        {
            var actionGet = contactsController.GetContact(id);
            ContactRecord thatRecord = ((ContactRecord)((OkObjectResult)actionGet.Result).Value);
            Assert.AreNotEqual("item.email" + id.ToString() + "@example.com", thatRecord.email);

            CreateContactDTO item = ((CreateContactDTO)(testContacts[id - 1]));
            UpdateContactDTO contactRecord = new()
            {
                name = item.name with { },
                address = item.address with { },
                phone = item.phone.Select(s => s with { }).ToArray(),
                email = "item.email" + id.ToString() + "@example.com"
            };

            var actionUpdate = contactsController.UpdateContact(id, contactRecord);
            Assert.IsInstanceOf(typeof(NoContentResult), actionUpdate); // using this assert to check type before potential invalid cast
            Assert.AreEqual(204, ((NoContentResult)actionUpdate).StatusCode);

            actionGet = contactsController.GetContact(id);
            Assert.IsInstanceOf(typeof(OkObjectResult), actionGet.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(200, ((OkObjectResult)actionGet.Result).StatusCode);
            thatRecord = ((ContactRecord)((OkObjectResult)actionGet.Result).Value);
            Assert.AreEqual("item.email" + id.ToString() + "@example.com", thatRecord.email);
        }

        [Test, Order(6)]
        [TestCase(1)]
        public void TestDeleteContact(int id)
        {
            var actionGet = contactsController.GetContact(id);
            Assert.NotNull(((OkObjectResult)actionGet.Result).Value);

            var actionDelete = contactsController.DeleteContact(id);
            Assert.IsInstanceOf(typeof(NoContentResult), actionDelete); // using this assert to check type before potential invalid cast
            Assert.AreEqual(204, ((NoContentResult)actionDelete).StatusCode);

            actionGet = contactsController.GetContact(id);
            Assert.IsInstanceOf(typeof(NotFoundResult), actionGet.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(404, ((NotFoundResult)actionGet.Result).StatusCode);
        }

        [Test, Order(5)]
        public void TestGetCallList()
        {
            var callListAction = contactsController.GetCallList();
            Assert.IsInstanceOf(typeof(OkObjectResult), callListAction.Result); // using this assert to check type before potential invalid cast
            Assert.AreEqual(200, ((OkObjectResult)callListAction.Result).StatusCode);
            IEnumerable<ContactListDTO> callList = ((IEnumerable<ContactListDTO>)((OkObjectResult)callListAction.Result).Value);
            HashSet<ContactListDTO> callLstSet = new HashSet<ContactListDTO>(callList);

            HashSet<ContactListDTO> testLstSet = new();

            foreach (CreateContactDTO curContact in testContacts)
            {
                foreach (ContactPhoneRecord curPhone in curContact.phone)
                {
                    if (curPhone.type == "home")
                    {
                        testLstSet.Add(
                            new ContactListDTO()
                            {
                                name = curContact.name with { },
                                phone = curPhone.number
                            });
                    }
                }
            }

            // the two set checks passing prove the sets are identical
            Assert.True(callLstSet.IsSubsetOf(testLstSet));
            Assert.False(callLstSet.IsProperSubsetOf(testLstSet));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            LiteDatabase contactdb = new LiteDatabase(connnectionstring);
            var contCol = contactdb.GetCollection<ContactItem>("contact");
            contCol.DeleteAll();
        }

        static object[] testContacts =
            new object[] {
                new CreateContactDTO() { email="harold.gilkey@yahoo.com", name = new ContactNameRecord(){ first="Harold", middle= "Francis", last= "Gilkey" }, address = new ContactAddressRecord(){ street= "8360 High Autumn Row", state= "Delaware", city= "Cannon", zip= "19797" }, phone = new ContactPhoneRecord[] { new(){number= "302-611-9148",type= "home"}, new(){number= "302-532-9427",type= "mobile"} } },
                new CreateContactDTO() { name = new() { first = "Not", middle = "Frito", last = "Sure" }, address = new() { street = "123 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "home" } }, email = "harold@example.com" },
                new CreateContactDTO() { name = new() { first = "Bob", middle = "Jo", last = "Jones" }, address = new() { street = "321 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "mobile" } }, email = "harold1@example.com" },
                new CreateContactDTO() { name = new() { first = "Not", middle = "Frito", last = "Sure" }, address = new() { street = "123 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "home" } }, email = "harold@example.com" },
                new CreateContactDTO() { name = new() { first = "Bob", middle = "Jo", last = "Jones" }, address = new() { street = "321 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "mobile" } }, email = "harold1@example.com" },
                new CreateContactDTO() { name = new() { first = "Robert", middle = "Jo", last = "Burns" }, address = new() { street = "333 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "mobile" } }, email = "harold2@example.com" },
                new CreateContactDTO() { name = new() { first = "Third", middle = "Namey", last = "McThirderson" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "home" } }, email = "harold3@example.com" },
                new CreateContactDTO() { name = new() { first = "Teejay", middle = "Ahrjay", last = "Backslashinfourth" }, address = new() { street = "555 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "home" } }, email = "harold4@example.com" },
                new CreateContactDTO() { name = new() { first = "Albert", middle = "Ramey", last = "McThirderson" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-9148", type = "home" }, new() { number = "302-211-3148", type = "work" } }, email = "harold7@example.com" },
                new CreateContactDTO() { name = new() { first = "Chip", middle = "Sam", last = "Smithy" }, address = new() { street = "444 Pine St", city = "Richmond", state = "VA", zip = "12345" }, phone = new ContactPhoneRecord[] { new() { number = "302-611-3147", type = "home" }, new() { number = "302-211-3149", type = "home" } }, email = "harold7@example.com" }
            };
    }
}