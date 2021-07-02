using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Repository;
using Repository.Models;
using Service.DTO;

namespace Service
{
    public class UserService : BaseService<User, UserDto>
    {

        public UserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
            // переопределяем маппинг
            // обеспечиваем поддержку маппинга для вложенных объектов
            var configFromDto = new MapperConfiguration(cfg =>
            {
                // игнорируем пароль, т.к при регистрации будем хэшировать отдельно
                cfg.CreateMap<UserDto, User>().ForMember(user => user.PasswordHash, opt => opt.Ignore());
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<AddressElementDto, AddressElement>();
            });
            MapperFromDto = configFromDto.CreateMapper();

            var configToDto = new MapperConfiguration(cfg =>
            {
                // игнорируем пароль
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<AddressElement, AddressElementDto>();
            });
            MapperToDto = configToDto.CreateMapper();
        }

        public override UserDto GetById(Guid id)
        {
            User user = _unitOfWork.UserRepository.GetById(id);
            if (user == null) throw new Exception("Пользователь не найден");
            return ToDto(user);
        }

        public override List<UserDto> Get(Expression<Func<User, bool>> filter = null)
        {
            var users = (List<User>)_unitOfWork.UserRepository.Get(filter);
            return users.Select(a => ToDto(a)).ToList();
        }

        public override void Add(UserDto userDto)
        {
            User user = FromDto(userDto);
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
        }

        public override void Add(User user)
        {
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
        }

        public override void Update(UserDto userDto)
        {
            if (userDto.Id == null)
                throw new Exception("Для изменения данных пользователя необходимо указать его ID");

            User oldUser = _unitOfWork.UserRepository.GetById((Guid)userDto.Id);
            if (oldUser == null)
                throw new Exception("Пользователь не найден");

            User newUser = FromDto(userDto);

            _unitOfWork.UserRepository.Update(oldUser, newUser);
            _unitOfWork.Save();
        }

        public void Delete(UserDto userDto)
        {
            if (userDto.Id != null)
                Delete((Guid)userDto.Id);
            throw new Exception("Для удаления пользователя необходимо указать его ID");
        }

        public override void Delete(Guid id)
        {
            User user = _unitOfWork.UserRepository.GetById(id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            _unitOfWork.UserRepository.Delete(user);
            _unitOfWork.Save();
        }

        public override User FromDto(UserDto userDto)
        {
            try
            {
                User user = MapperFromDto.Map<User>(userDto);
                user.Id = userDto.Id ?? Guid.NewGuid();

                // TODO: отрефакторить многочисленные однотипные проверки и манипуляции

                if (userDto.Address.Id == null)
                {
                    Guid newAddressId = Guid.NewGuid();
                    user.AddressId = newAddressId;
                    user.Address.Id = newAddressId;
                }

                if (userDto.Address.Country.Id == null)
                {
                    user.Address.Country.Id = Guid.NewGuid();
                    user.Address.CountryId = user.Address.Country.Id;
                    user.Address.Country.AddressElementType = _unitOfWork.AddressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "Country").FirstOrDefault();
                    user.Address.Country.AddressElementTypeId = user.Address.Country.AddressElementType.Id;
                }

                if (userDto.Address.Region.Id == null)
                {
                    user.Address.Region.Id = Guid.NewGuid();
                    user.Address.RegionId = user.Address.Region.Id;
                    user.Address.Region.AddressElementType = _unitOfWork.AddressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "Region").FirstOrDefault();
                    user.Address.Region.AddressElementTypeId = user.Address.Region.AddressElementType.Id;
                }

                if (userDto.Address.City.Id == null)
                {
                    user.Address.City.Id = Guid.NewGuid();
                    user.Address.CityId = user.Address.City.Id;
                    user.Address.City.AddressElementType = _unitOfWork.AddressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "City").FirstOrDefault();
                    user.Address.City.AddressElementTypeId = user.Address.City.AddressElementType.Id;
                }

                if (userDto.Address.Street.Id == null)
                {
                    user.Address.Street.Id = Guid.NewGuid();
                    user.Address.StreetId = user.Address.Street.Id;
                    user.Address.Street.AddressElementType = _unitOfWork.AddressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "Street").FirstOrDefault();
                    user.Address.Street.AddressElementTypeId = user.Address.Street.AddressElementType.Id;
                }

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
