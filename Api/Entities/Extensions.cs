using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;

namespace Contacts.Api.Entities
{
    public static class Extensions
    {
        public static ContactListDTO AsListDTO(this ContactItem item)
        {
            return new ContactListDTO()
            {
                name = item.name.ToRecord(),
                phone = item.phone.Where(s => s.type == PhoneType.home).First().number
            };
        }

        public static IEnumerable<int> getHomePhones(this ContactPhone[] phoneNumbers)
        {
            List<int> indexes = new();
            for (int xCount = 0; xCount <= phoneNumbers.GetUpperBound(0); xCount++)
            {
                if (phoneNumbers[xCount].type == PhoneType.home)
                {
                    indexes.Add(xCount);
                }
            }

            return indexes;
        }

        public static ContactListDTO AsListDTO(this ContactItem item, int phoneIndex)
        {
            return new ContactListDTO()
            {
                name = item.name.ToRecord(),
                phone = item.phone[phoneIndex].number
            };
        }

        public static ContactPhone FromRecord(this ContactPhoneRecord item)
        {
            return new ContactPhone { number = item.number, type = (PhoneType)Enum.Parse(typeof(PhoneType), item.type) };
        }

        public static ContactPhoneRecord ToRecord(this ContactPhone item)
        {
            return new ContactPhoneRecord { number = item.number, type = Enum.GetName(typeof(PhoneType), item.type) };
        }

        public static ContactRecord ToRecord(this ContactItem item)
        {
            return (item is null) ? null : 
            new ContactRecord()
            {
                id = item.id,
                name = item.name.ToRecord(),
                address = item.address.ToRecord(),
                phone = item.phone.Select(s => s.ToRecord()).ToArray(),
                email = item.email
            };
        }

        public static ContactAddressRecord ToRecord(this ContactAddress item)
        {
            return new ContactAddressRecord()
            {
                street = item.street,
                city = item.city,
                state = item.state,
                zip = item.zip
            };
        }

        public static ContactAddress FromRecord(this ContactAddressRecord item)
        {
            return new ContactAddress()
            {
                street = item.street,
                city = item.city,
                state = item.state,
                zip = item.zip
            };
        }

        public static ContactNameRecord ToRecord(this ContactName item)
        {
            return new ContactNameRecord()
            {
                first = item.first,
                middle = item.middle,
                last = item.last
            };
        }

        public static ContactName FromRecord(this ContactNameRecord item)
        {
            return new ContactName()
            {
                first = item.first,
                middle = item.middle,
                last = item.last
            };
        }
    }
}