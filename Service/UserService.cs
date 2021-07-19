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
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<AddressElementType> _addressElementTypeRepository;
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = new GenericRepository<User>(unitOfWork.Context);
            
            // переопределяем маппинг
            // обеспечиваем поддержку маппинга для вложенных объектов
            var configFromDto = new MapperConfiguration(cfg =>
            {
                // игнорируем пароль, т.к при регистрации будем хэшировать отдельно
                cfg.CreateMap<UserDto, User>()
                    .ForMember(user => user.PasswordHash, opt => opt.Ignore())
                    // если передан пустой ID, то генерируем новый
                    .ForMember(user => user.Id, opts => opts.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<AddressElementDto, AddressElement>();
            });
            MapperFromDto = configFromDto.CreateMapper();

            var configToDto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<AddressElement, AddressElementDto>();
            });
            MapperToDto = configToDto.CreateMapper();
        }

        public override UserDto GetById(Guid id)
        {
            User user = _userRepository.GetById(id);
            if (user == null) throw new Exception("Пользователь не найден");
            return ToDto(user);
        }

        public override List<UserDto> Get(Expression<Func<User, bool>> filter = null)
        {
            var users = (List<User>)_userRepository.Get(filter);
            return users.Select(a => ToDto(a)).ToList();
        }

        public override void Add(UserDto userDto)
        {
            User user = FromDto(userDto);
            _userRepository.Insert(user);
            _unitOfWork.Save();
        }

        public override void Add(User user)
        {
            _userRepository.Insert(user);
            _unitOfWork.Save();
        }

        public override void Update(UserDto userDto)
        {
            if (userDto.Id == Guid.Empty)
                throw new Exception("Для изменения данных пользователя необходимо указать его ID");

            User oldUser = _userRepository.GetById((Guid)userDto.Id);
            if (oldUser == null)
                throw new Exception("Пользователь не найден");
            userDto.Address.Id = oldUser.AddressId;

            User newUser = FromDto(userDto);
            newUser.PasswordHash = oldUser.PasswordHash;

            AddressService addressService = new AddressService(_unitOfWork);
            addressService.Update(userDto.Address);

            _userRepository.Update(oldUser, newUser);
            
            _unitOfWork.Save();
        }


        public void Delete(UserDto userDto)
        {
            if (userDto.Id != Guid.Empty)
                Delete((Guid)userDto.Id);
            throw new Exception("Для удаления пользователя необходимо указать его ID");
        }

        public override void Delete(Guid id)
        {
            User user = _userRepository.GetById(id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            _userRepository.Delete(user);
            _unitOfWork.Save();
        }

        public override User FromDto(UserDto userDto)
        {
            try
            {
                User user = MapperFromDto.Map<User>(userDto);

                // TODO: отрефакторить многочисленные однотипные проверки и манипуляции

                if (userDto.Address.Id == Guid.Empty)
                {
                    Guid newAddressId = Guid.NewGuid();
                    user.AddressId = newAddressId;
                    user.Address.Id = newAddressId;
                }

                if (userDto.Address.Country.Id == Guid.Empty)
                {
                    user.Address.Country.Id = Guid.NewGuid();
                    user.Address.CountryId = user.Address.Country.Id;
                    user.Address.Country.AddressElementType = _addressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "Country").FirstOrDefault();
                    user.Address.Country.AddressElementTypeId = user.Address.Country.AddressElementType.Id;
                }

                if (userDto.Address.Region.Id == Guid.Empty)
                {
                    user.Address.Region.Id = Guid.NewGuid();
                    user.Address.RegionId = user.Address.Region.Id;
                    user.Address.Region.AddressElementType = _addressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "Region").FirstOrDefault();
                    user.Address.Region.AddressElementTypeId = user.Address.Region.AddressElementType.Id;
                }

                if (userDto.Address.City.Id == Guid.Empty)
                {
                    user.Address.City.Id = Guid.NewGuid();
                    user.Address.CityId = user.Address.City.Id;
                    user.Address.City.AddressElementType = _addressElementTypeRepository
                        .Get()
                        .Where(aet => aet.Name == "City").FirstOrDefault();
                    user.Address.City.AddressElementTypeId = user.Address.City.AddressElementType.Id;
                }

                if (userDto.Address.Street.Id == Guid.Empty)
                {
                    user.Address.Street.Id = Guid.NewGuid();
                    user.Address.StreetId = user.Address.Street.Id;
                    user.Address.Street.AddressElementType = _addressElementTypeRepository
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
