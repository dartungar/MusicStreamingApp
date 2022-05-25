using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain;
using Domain.Models;
using DAL.EF;
using AutoMapper;
using System.Linq;
using Service.DTO;

namespace Service
{
    public class AddressService : BaseService<Address, AddressDto>
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly IGenericRepository<AddressElement> _addressElementRepository;
        private readonly IGenericRepository<AddressElementType> _addressElementTypeRepository;

        public AddressService(
            IUnitOfWork unitOfWork, 
            IGenericRepository<Address> addressRepo,
            IGenericRepository<AddressElement> addressElementRepo,
            IGenericRepository<AddressElementType> addressElementTypeRepo
            ) : base(unitOfWork)
        {
            // init repositories
            _addressRepository = addressRepo;
            _addressElementRepository = addressElementRepo;
            _addressElementTypeRepository = addressElementTypeRepo;

            // configure custom mapping
            var configFromDto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressDto, Address>()
                    .ForMember(address => address.Id, opts => opts.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));
                cfg.CreateMap<AddressElementDto, AddressElement>()
                    .ForMember(addressElement => addressElement.Id, opts => opts.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));
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
            Address address = _addressRepository.GetById(id);
            if (address == null) 
                throw new Exception("Адрес не найден");
            address.Country = _addressElementRepository.GetById(address.CountryId);
            address.Region = address.RegionId != null ? _addressElementRepository.GetById((Guid)address.RegionId) : null;
            address.City = _addressElementRepository.GetById(address.CityId);
            address.Street = address.StreetId != null ? _addressElementRepository.GetById((Guid)address.StreetId) : null;
            return ToDto(address);
        }

        public override List<AddressDto> Get(Expression<Func<Address, bool>> filter = null)
        {
            var addresses = (List<Address>)_addressRepository.Get(filter);
            return addresses.Select(a => ToDto(a)).ToList();
        }


        public override void Add(AddressDto addressDto)
        {
            Address address = FromDto(addressDto);
            _addressRepository.Insert(address);
            _unitOfWork.Save();

        }

        public override void Add(Address address)
        {
            _addressRepository.Insert(address);
            _unitOfWork.Save();

        }

        public override void Update(AddressDto addressDto)
        {
            if (addressDto.Id == Guid.Empty) 
                throw new Exception("Для изменения данных адреса необходимо указать его ID");
            Address oldAddress = _addressRepository.GetById((Guid)addressDto.Id);
            if (oldAddress == null)
                throw new Exception("Адрес не найден");

            oldAddress.Country = _addressElementRepository.GetById(oldAddress.CountryId);
            oldAddress.Region = oldAddress.RegionId != null ? _addressElementRepository.GetById((Guid)oldAddress.RegionId) : null;
            oldAddress.City = _addressElementRepository.GetById(oldAddress.CityId);
            // TO DO: почему-то не находит улицу в случае юзера TEST
            oldAddress.Street = oldAddress.Street != null ? _addressElementRepository.GetById((Guid)oldAddress.StreetId) : null;

            Address newAddress = FromDto(addressDto);
            newAddress.Id = oldAddress.Id;
            // если адресный элемент по сути не изменился, оставляем старый
            // TO DO: проверка на то, существует ли в БД адресный элемент с таким значением
            // TO DO: обилие проверок намекает на проблемы в архитектуре DTO и/или в работе с DTO

            if (!oldAddress.Country.Value.Equals(newAddress.Country.Value))
            {
                newAddress.Country.AddressElementType = oldAddress.Country.AddressElementType ??
                    _addressElementTypeRepository.Get(aet => aet.Name.Equals("Country")).FirstOrDefault();
                newAddress.Country.AddressElementTypeId = newAddress.Country.AddressElementType.Id;
                _addressElementRepository.Insert(newAddress.Country);
            }
            else newAddress.Country = oldAddress.Country;
            newAddress.CountryId = newAddress.Country.Id;

            if ((oldAddress.Region == null || !oldAddress.Region.Value.Equals(newAddress.Region.Value)))
            {
                if (newAddress.Region.Value != null)
                {
                    newAddress.Region.AddressElementType = oldAddress.Region?.AddressElementType ??
                        _addressElementTypeRepository.Get(aet => aet.Name.Equals("Region")).FirstOrDefault();
                    newAddress.Region.AddressElementTypeId = newAddress.Region.AddressElementType.Id;
                    _addressElementRepository.Insert(newAddress.Region);
                }

            }
            else newAddress.Region = oldAddress.Region;
            newAddress.RegionId = newAddress.Region?.Id;

            if (!oldAddress.City.Value.Equals(newAddress.City.Value))
            {
                newAddress.City.AddressElementType = oldAddress.City.AddressElementType ??
                    _addressElementTypeRepository.Get(aet => aet.Name.Equals("City")).FirstOrDefault();
                newAddress.City.AddressElementTypeId = newAddress.City.AddressElementType.Id;
                _addressElementRepository.Insert(newAddress.City);
            }
            else newAddress.City = oldAddress.City;
            newAddress.CityId = newAddress.City.Id;

            if (oldAddress.Street == null || !oldAddress.Street.Value.Equals(newAddress.Street.Value))
            {
                if (newAddress.Street.Value != null)
                {
                    newAddress.Street.AddressElementType = oldAddress.Street?.AddressElementType ??
                        _addressElementTypeRepository.Get(aet => aet.Name.Equals("Street")).FirstOrDefault();
                    newAddress.Street.AddressElementTypeId = newAddress.Street.AddressElementType.Id;
                    _addressElementRepository.Insert(newAddress.Street);

                }
            }
            else newAddress.Street = oldAddress.Street;
            newAddress.StreetId = newAddress.Street?.Id;

            _addressRepository.Update(oldAddress, newAddress);
            
            _unitOfWork.Save();
        }

        public  void Delete(AddressDto addressDto)
        {
            if (addressDto.Id == Guid.Empty)
                throw new Exception("Для удаления адреса необходимо указать его ID");
            Delete((Guid)addressDto.Id);
        }

        public override void Delete(Guid id)
        {
            Address address = _addressRepository.GetById(id);
            if (address == null)
                throw new Exception("Адрес не найден");
            _addressRepository.Delete(address);
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
