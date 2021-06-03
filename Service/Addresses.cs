using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    class Addresses
    {

        // address methods
        // TODO: overloads for various address compositions (e.g with region)
        public Address AddAddress(string country, string city, int house)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement countryElement = AddAddressElement(country, "Country");
            AddressElement cityElement = AddAddressElement(city, "City");
            if (countryElement == null)
                throw new Exception($"Ошибка при создании адреса: страна '{country}'");
            if (cityElement == null)
                throw new Exception($"Ошибка при создании адреса: населенный пункт '{city}'");

            Address existingAddress = db.Addresses.FirstOrDefault(
                a => a.CountryId == countryElement.Id &&
                a.CityId == cityElement.Id &&
                a.House == house.ToString());

            if (existingAddress != null)
                return existingAddress;

            Address address = new Address
            {
                Id = Guid.NewGuid(),
                CountryId = countryElement.Id,
                CityId = cityElement.Id,
                House = house.ToString()
            };
            db.Addresses.Add(address);
            db.SaveChanges();
            return address;
        }

        public void RemoveAddress(Address address)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Addresses.Remove(address);
            db.SaveChanges();
        }

        public void RemoveAddress(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Address address = db.Addresses.Find(id);
            if (address != null) db.Addresses.Remove(address);
            db.SaveChanges();
        }

        public void UpdateAddress(Address address)
        {
            using ApplicationContext db = new ApplicationContext();
            // нужно ли повторно искать пользователя? по идее, новый контекст => да
            // возможно, стоит использовать паттерн Unit Of Work? когда изучу его
            Address foundAddress = db.Addresses.Find(address.Id);
            if (foundAddress != null) foundAddress = address;
            db.SaveChanges();
        }

        public AddressElement AddAddressElement(string value, string type) // TODO: addressElementType enum
        {
            using ApplicationContext db = new ApplicationContext();
            List<AddressElementType> addressElementTypes = db.AddressElementTypes.ToList();
            AddressElementType elementType = addressElementTypes.FirstOrDefault(et => et.Name == type);
            if (elementType == null)
                throw new Exception($"Неправильный тип адресного элемента: {type}");

            AddressElement existingElement =
                db.AddressElements.FirstOrDefault(
                el => el.Value == value
                && el.AddressElementTypeId == elementType.Id);

            if (existingElement != null) return existingElement;

            AddressElement el = new AddressElement
            {
                Id = Guid.NewGuid(),
                Value = value,
                AddressElementTypeId = elementType.Id
            };
            db.AddressElements.Add(el);
            db.SaveChanges();
            return el;
        }

        public void UpdateAddressElement(AddressElement addressElement)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement foundAddressElement = db.AddressElements.Find(addressElement.Id);
            if (foundAddressElement != null) foundAddressElement = addressElement;
            db.SaveChanges();
        }

        public void RemoveAddressElement(AddressElement addressElement)
        {
            using ApplicationContext db = new ApplicationContext();
            db.AddressElements.Remove(addressElement);
            db.SaveChanges();
        }

        public void RemoveAddressElement(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement addressElement = db.AddressElements.Find(id);
            if (addressElement != null) db.AddressElements.Remove(addressElement);
            db.SaveChanges();
        }
    }
}
