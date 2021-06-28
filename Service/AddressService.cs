using System;
using Repository;
using Repository.Models;
using Repository.DTO;

namespace Service
{
    class AddressService
    {
        private readonly UnitOfWork unitOfWork;

        public AddressService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public AddressDto GetAddressById(Guid id)
        {
            Address address = unitOfWork.AddressRepository.GetById(id);
            if (address == null) throw new Exception("Адрес не найден");
            return ToDto(address);
        }


        public void AddAddress(AddressDto addressData)
        {
            Address address = FromDto(addressData);
            unitOfWork.AddressRepository.Insert(address);
            unitOfWork.Save();

        }

        public void UpdateAddress(AddressDto addressData)
        {
            Address address;
            if (addressData.Id == Guid.Empty) throw new Exception("Для изменения данных адреса необходимо указать его ID");
            if (unitOfWork.AddressRepository.GetById(addressData.Id) == null) throw new Exception("Адрес не найден");
            address = FromDto(addressData);
            unitOfWork.AddressRepository.Update(address);
            unitOfWork.Save();
        }

        private AddressDto ToDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                Country = new AddressElementDto { Id = address.Country.Id, Value = address.Country.Value},
                Region = new AddressElementDto { Id = address.Region.Id, Value = address.Region.Value },
                City = new AddressElementDto { Id = address.City.Id, Value = address.City.Value },
                Street = new AddressElementDto { Id = address.Street.Id, Value = address.Street.Value },
                House = address.House
            };
        }

        /// <summary>
        /// Get Address data model from DTO with new or existing address elements
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        private Address FromDto(AddressDto addressDto)
        {
            AddressElement country = AddressElementFromDto(addressDto.Country);
            AddressElement region = AddressElementFromDto(addressDto.Region);
            AddressElement city = AddressElementFromDto(addressDto.City);
            AddressElement street = AddressElementFromDto(addressDto.Street);

            Address address = new Address
            {
                Country = country,
                Region = region,
                City = city,
                Street = street,
                House = addressDto.House
            };
            if (addressDto.Id != Guid.Empty)
            {
                address.Id = addressDto.Id;
            }
            return address;
        }

        private AddressElement AddressElementFromDto(AddressElementDto addressElementDto)
        {
            if (addressElementDto.Id != Guid.Empty)
            {
                AddressElement addressElement = unitOfWork.AddressElementRepository.GetById(addressElementDto.Id);
                if (addressElement != null) return addressElement;
                throw new Exception("Адресный элемент не найден");
            }
            return new AddressElement { Value = addressElementDto.Value };
        }
    }
}
