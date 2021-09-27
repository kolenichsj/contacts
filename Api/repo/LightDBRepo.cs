using LiteDB;
using System.Collections.Generic;
using Contacts.Api.Entities;

namespace Contacts.Api.repo
{
    public class LightDBRepo : IContactsRepo
    {
        private readonly ILiteCollection<ContactItem> contCol;

        public LightDBRepo() : this("contactitems.db") { }

        public LightDBRepo(string connnectionstring) : this(connnectionstring, "contact") { }

        public LightDBRepo(string connnectionstring, string collectionName)
        {
            LiteDatabase contactdb = new LiteDatabase(connnectionstring);
            this.contCol = contactdb.GetCollection<ContactItem>(collectionName);
            this.contCol.EnsureIndex(x=> x.id,true);
        }

        public ContactItem CreateItem(ContactItem item)
        {
            var bsonItem = this.contCol.Insert(item);
            item.id = BsonMapper.Global.Deserialize<int>(bsonItem);
            return item;
        }

        public void DeleteItem(int id)
        {
            if (!this.contCol.Delete(new BsonValue(id)))
            {
                throw new System.Exception("Item not deleted");
            }
        }

        public ContactItem GetItem(int id)
        {
            return this.contCol.FindOne(x => x.id == id);
        }

        public IEnumerable<ContactItem> GetItems()
        {
            return this.contCol.FindAll();
        }

        public void UpdateItem(ContactItem item)
        {
            if (!this.contCol.Update(item))
            {
                throw new KeyNotFoundException("ID " + item.id.ToString());
            }
        }
    }
}