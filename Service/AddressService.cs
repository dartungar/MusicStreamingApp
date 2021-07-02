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
        }

        public override AddressDto GetById(Guid id)
        {
            Address address = _unitOfWork.AddressRepository.GetById(id);
            if (address == null) 
                throw new Exception("Адрес не найден");
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
            _unitOfWork.AddressRepository.Update(oldAddress, FromDto(addressDto));
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
