using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Repository;
using Repository.Models;
using AutoMapper;
using System.Linq;
using Service.DTO;

namespace Service
{
    public class AddressService : BaseService<Address, AddressDto>
    {

        public AddressService(UnitOfWork unitOfWork): base(unitOfWork)
        {
            var configFromDto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<AddressElementDto, AddressElement>();
            });
            MapperFromDto = configFromDto.CreateMapper();

            var configToDto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<AddressElement, AddressElementDto>();
            });
            MapperToDto = configToDto.CreateMapper();
        }

        public override AddressDto GetById(Guid id)
        {
            Address address = _unitOfWork.AddressRepository.GetById(id);
            if (address == null) 
                throw new Exception("Адрес не найден");
            address.Country = _unitOfWork.AddressElementRepository.GetById(address.CountryId);
            address.Region = address.RegionId != null ? _unitOfWork.AddressElementRepository.GetById((Guid)address.RegionId) : null;
            address.City = _unitOfWork.AddressElementRepository.GetById(address.CityId);
            address.Street = address.StreetId != null ? _unitOfWork.AddressElementRepository.GetById((Guid)address.StreetId) : null;
            return ToDto(address);
        }

        public override List<AddressDto> Get(Expression<Func<Address, bool>> filter = null)
        {
            var addresses = (List<Address>)_unitOfWork.AddressRepository.Get(filter);
            return addresses.Select(a => ToDto(a)).ToList();
        }


        public override void Add(AddressDto addressDto)
        {
            Address address = FromDto(addressDto);
            _unitOfWork.AddressRepository.Insert(address);
            _unitOfWork.Save();

        }

        public override void Add(Address address)
        {
            _unitOfWork.AddressRepository.Insert(address);
            _unitOfWork.Save();

        }

        public override void Update(AddressDto addressDto)
        {
            if (addressDto.Id == null) 
                throw new Exception("Для изменения данных адреса необходимо указать его ID");
            Address oldAddress = _unitOfWork.AddressRepository.GetById((Guid)addressDto.Id);
            if (oldAddress == null) 
                throw new Exception("Адрес не найден");


            Address newAddress = FromDto(addressDto);
            newAddress.Id = oldAddress.Id;
            // если адресный элемент по сути не изменился, оставляем старый
            // TO DO: проверка на то, существует ли в БД адресный элемент с таким значением
            // TO DO: обилие проверок намекает на проблемы в архитектуре DTO и/или в работе с DTO
            bool countryNotChanged = newAddress.Country.Value.Equals(oldAddress.Country.Value);
            newAddress.Country = countryNotChanged ? oldAddress.Country : newAddress.Country;
            newAddress.Country.Id = newAddress.Country.Id == Guid.Empty ? Guid.NewGuid() : newAddress.Country.Id;
            newAddress.CountryId = countryNotChanged ? oldAddress.CountryId : newAddress.Country.Id;

            bool regionNotChanged = oldAddress.Region != null && newAddress.Region.Value.Equals(oldAddress.Region.Value);
            newAddress.Region = regionNotChanged ? oldAddress.Region : newAddress.Region;
            newAddress.Region.Id = newAddress.Region.Id == Guid.Empty ? Guid.NewGuid() : newAddress.Region.Id;
            newAddress.RegionId = regionNotChanged ? oldAddress.RegionId : newAddress.Region.Id;

            bool cityNotChanged = newAddress.City.Value.Equals(oldAddress.City.Value);
            newAddress.City = cityNotChanged ? oldAddress.City : newAddress.City;
            newAddress.City.Id = newAddress.City.Id == Guid.Empty ? Guid.NewGuid() : newAddress.City.Id;
            newAddress.CityId = cityNotChanged ? oldAddress.CityId : newAddress.City.Id;

            bool streetNotChanged = oldAddress.Street != null && newAddress.Street.Value.Equals(oldAddress.Street.Value);
            newAddress.Street = streetNotChanged ? oldAddress.Street : newAddress.Street;
            newAddress.Street.Id = newAddress.Street.Id == Guid.Empty ? Guid.NewGuid() : newAddress.Street.Id;
            newAddress.StreetId = streetNotChanged ? oldAddress.StreetId : newAddress.Street.Id;


            _unitOfWork.AddressRepository.Update(oldAddress, newAddress);
            _unitOfWork.Save();
        }

        public  void Delete(AddressDto addressDto)
        {
            if (addressDto.Id == null)
                throw new Exception("Для удаления адреса необходимо указать его ID");
            Delete((Guid)addressDto.Id);
        }

        public override void Delete(Guid id)
        {
            Address address = _unitOfWork.AddressRepository.GetById(id);
            if (address == null)
                throw new Exception("Адрес не найден");
            _unitOfWork.AddressRepository.Delete(address);
        }

        // TODO: проверить маппинг вложенных объектов
        // мб нужно донастроить маппинг вложенных сущностей?
        public override AddressDto ToDto(Address address)
        {
            return MapperToDto.Map<AddressDto>(address);
        }

        /// <summary>
        /// Get Address data model from DTO with new or existing address elements
        /// </summary>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        public override Address FromDto(AddressDto addressDto)
        {
            // TODO: проверить маппинг
            // должен правильно создавать адресные элементы!
            return MapperFromDto.Map<Address>(addressDto);
        }

    }
}
