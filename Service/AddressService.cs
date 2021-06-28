using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Repository.DTO;

namespace Service
{
    class AddressService
    {
        private UnitOfWork unitOfWork;

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


        // TODO: реализовать добавление AddressElements в новый Address
        // 
        public void AddAddress(AddressDto addressData)
        {
            Address address = unitOfWork.AddressRepository.Get(
                address => address.Country.Value == addressData.Country &&
                address.Region.Value == addressData.Region &&
                address.City.Value == addressData.City &&
                address.Street.Value == addressData.Street &&
                address.House == addressData.House
                ).FirstOrDefault();
            if (address == null)
                // TODO
        }

        public void UpdateAddress(AddressDto addressData)
        {
            Address address;
            if (addressData.Id != Guid.Empty)
            {
                address = unitOfWork.AddressRepository.GetById(addressData.Id);
            } else
            {
                address = unitOfWork.AddressRepository.Get(
                address => address.Country.Value == addressData.Country &&
                address.Region.Value == addressData.Region &&
                address.City.Value == addressData.City &&
                address.Street.Value == addressData.Street &&
                address.House == addressData.House
                ).FirstOrDefault();
            }

            if (address != null) throw new Exception("Адрес не найден");
            unitOfWork.AddressRepository.Update(address);
        }

        private AddressDto ToDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                Country = address.Country.Value,
                Region = address.Region.Value,
                City = address.City.Value,
                Street = address.Street.Value,
                House = address.House
            };
        }
    }
}
