using System;
using System.Collections.Generic;
using Repository;
using Repository.Models;
using Repository.DTO;
using AutoMapper;
using System.Linq;

namespace Service
{
    public class AddressService : BaseService<Address, AddressDto>
    {

        public AddressService(UnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public override AddressDto GetById(Guid id)
        {
            Address address = _unitOfWork.AddressRepository.GetById(id);
            if (address == null) throw new Exception("Адрес не найден");
            return ToDto(address);
        }

        public override List<AddressDto> Get()
        {
            var addresses = (List<Address>)_unitOfWork.AddressRepository.Get();
            return addresses.Select(a => ToDto(a)).ToList();
        }


        public override void Add(AddressDto addressDto)
        {
            Address address = FromDto(addressDto);
            _unitOfWork.AddressRepository.Insert(address);
            _unitOfWork.Save();

        }

        public override void Update(AddressDto addressDto)
        {
            if (addressDto.Id == Guid.Empty) 
                throw new Exception("Для изменения данных адреса необходимо указать его ID");
            if (_unitOfWork.AddressRepository.GetById(addressDto.Id) == null) 
                throw new Exception("Адрес не найден");
            _unitOfWork.AddressRepository.Update(FromDto(addressDto));
            _unitOfWork.Save();
        }

        public  void Delete(AddressDto addressDto)
        {
            if (addressDto.Id == Guid.Empty)
                throw new Exception("Для удаления адреса необходимо указать его ID");
            Delete(addressDto.Id);
        }

        public override void Delete(Guid id)
        {
            Address address = _unitOfWork.AddressRepository.GetById(id);
            if (address == null)
                throw new Exception("Адрес не найден");
            _unitOfWork.AddressRepository.Delete(address);
        }

        // TODO: нормальный маппинг
        public override AddressDto ToDto(Address address)
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
        public override Address FromDto(AddressDto addressDto)
        {
            // TODO: реализовать маппинг с AutoMapper (уже реализовал маппер, осталось проверить)
            // выглядит сложно, возможно есть ошибка проектирования?
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

        protected AddressElement AddressElementFromDto(AddressElementDto addressElementDto)
        {
            if (addressElementDto.Id != Guid.Empty)
            {
                AddressElement addressElement = _unitOfWork.AddressElementRepository.GetById(addressElementDto.Id);
                if (addressElement != null) return addressElement;
                throw new Exception("Адресный элемент не найден");
            }
            return new AddressElement { Value = addressElementDto.Value };
        }
    }
}
