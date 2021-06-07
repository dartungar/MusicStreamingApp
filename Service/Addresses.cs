using System;
using System.Collections.Generic;
using System.Linq;
using Repository;
using Repository.Models;
using Repository.DTO;

namespace Service
{
    internal class Addresses
    {

        internal static Address AddOrGetAddress(string country, string city, string house)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement countryElement = AddOrGetAddressElement(country, "Country");
            AddressElement cityElement = AddOrGetAddressElement(city, "City");
            if (countryElement == null)
                throw new Exception($"Ошибка при создании или поиске страны: '{country}'");
            if (cityElement == null)
                throw new Exception($"Ошибка при создании или поиске населенного пункта '{city}'");

            Address existingAddress = db.Addresses.FirstOrDefault(
                a => a.CountryId == countryElement.Id &&
                a.CityId == cityElement.Id &&
                a.House == house.ToString());

            Address address;
            if (existingAddress != null)
            {
                address = existingAddress;
            } else
            {
                address = new Address
                {
                    Id = Guid.NewGuid(),
                    CountryId = countryElement.Id,
                    CityId = cityElement.Id,
                    House = house.ToString()
                };
                db.Addresses.Add(address);
                db.SaveChanges();
            }

            return address;
        }

        internal static Address AddOrGetAddress(string country, string region, string city, string street, string house)
        {
            using ApplicationContext db = new ApplicationContext();

            AddressElement countryElement = AddOrGetAddressElement(country, "Country");
            if (countryElement == null)
                throw new Exception($"Ошибка при создании или поиске страны: '{country}'");

            AddressElement regionElement = AddOrGetAddressElement(region, "Region");
            if (regionElement == null)
                throw new Exception($"Ошибка при создании или региона '{region}'");

            AddressElement cityElement = AddOrGetAddressElement(city, "City");
            if (cityElement == null)
                throw new Exception($"Ошибка при создании или поиске населенного пункта '{city}'");

            AddressElement streetElement = AddOrGetAddressElement(street, "Street");
            if (streetElement == null)
                throw new Exception($"Ошибка при создании или поиске улицы '{street}'");

            Address existingAddress = db.Addresses.FirstOrDefault(
                a => a.CountryId == countryElement.Id &&
                a.RegionId == regionElement.Id &&
                a.StreetId == streetElement.Id &&
                a.CityId == cityElement.Id &&
                a.House.Equals(house.ToString()));

            Address address;
            if (existingAddress != null)
            {
                address = existingAddress;
            }
            else
            {
                address = new Address
                {
                    Id = Guid.NewGuid(),
                    CountryId = countryElement.Id,
                    RegionId = regionElement.Id,
                    CityId = cityElement.Id,
                    StreetId = streetElement.Id,
                    House = house
                };
                db.Addresses.Add(address);
                db.SaveChanges();
            }

            return address;
        }

        public static void RemoveAddress(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            Address address = db.Addresses.Find(id);
            if (address != null) db.Addresses.Remove(address);
            db.SaveChanges();
        }

        internal static void UpdateAddress(Guid id, string country, string region, string city, string street, int house)
        {
            using ApplicationContext db = new ApplicationContext();
            Address foundAddress = db.Addresses.Find(id);
            foundAddress.Country = AddOrGetAddressElement(region, "Country");
            foundAddress.Region = AddOrGetAddressElement(region, "Region");
            foundAddress.City = AddOrGetAddressElement(region, "City");
            foundAddress.Street = AddOrGetAddressElement(region, "Street");
            db.SaveChanges();
        }

        internal static void UpdateAddress(Address address)
        {
            using ApplicationContext db = new ApplicationContext();
            // нужно ли повторно искать пользователя? по идее, новый контекст => да
            // возможно, стоит использовать паттерн Unit Of Work? когда изучу его
            Address foundAddress = db.Addresses.Find(address.Id);
            if (foundAddress != null) foundAddress = address;
            db.SaveChanges();
        }

        public static AddressElement AddOrGetAddressElement(string value, string type) // TODO: addressElementType enum
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

        public static void UpdateAddressElement(AddressElement addressElement)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement foundAddressElement = db.AddressElements.Find(addressElement.Id);
            if (foundAddressElement != null) foundAddressElement = addressElement;
            db.SaveChanges();
        }

        public static void RemoveAddressElement(AddressElement addressElement)
        {
            using ApplicationContext db = new ApplicationContext();
            db.AddressElements.Remove(addressElement);
            db.SaveChanges();
        }

        public static void RemoveAddressElement(Guid id)
        {
            using ApplicationContext db = new ApplicationContext();
            AddressElement addressElement = db.AddressElements.Find(id);
            if (addressElement != null) db.AddressElements.Remove(addressElement);
            db.SaveChanges();
        }
    }
}
