﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Wards;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAM.BusinessTier.Payload.News;

namespace SAM.BusinessTier.Services.Implements
{
    public class AddressService : BaseService<AddressService>, IAddressService
    {
        public AddressService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<AddressService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewAddress(CreateNewAddressRequest createNewAddressRequest)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));

            if (account == null)
                throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
            var existingAddress = await _unitOfWork.GetRepository<Address>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewAddressRequest.Name) && x.AccountId == account.Id);

            if (existingAddress != null)
                throw new BadHttpRequestException(MessageConstant.Address.AddressExisteMessage);

            var address = _mapper.Map<Address>(createNewAddressRequest);
            address.Id = Guid.NewGuid();
            address.Status = AddressStatus.Active.GetDescriptionFromEnum();
            address.AccountId = account.Id;
            address.NamePersonal = createNewAddressRequest.NamePersonal;
            address.PhoneNumber = createNewAddressRequest?.PhoneNumber;
            await _unitOfWork.GetRepository<Address>().InsertAsync(address);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
                throw new BadHttpRequestException(MessageConstant.Address.CreateAddressFailedMessage);

            return address.Id;
        }

        public async Task<GetAddressResponse> GetAddressById(Guid id)
        {
            var address = await _unitOfWork.GetRepository<Address>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.City)
                                   .Include(x => x.District)
                                   .Include(x => x.Ward)
                                   .Include(x => x.Account))
                ?? throw new BadHttpRequestException(MessageConstant.Address.NotFoundFailedMessage);

            var addressResponse = new GetAddressResponse
            {
                Id = address.Id,
                Name = address.Name,
                Status = string.IsNullOrEmpty(address.Status) ? null : EnumUtil.ParseEnum<AddressStatus>(address.Status),
                Note = address.Note,
                NamePersonal = address?.NamePersonal,
                PhoneNumber = address?.PhoneNumber,
                City = new CityResponse
                    {
                        Id = address.City.Id,
                        Name = address.City.Name,
                        UnitId = address.City.UnitId
                    }
                 ,
                District = new DistrictResponse
                    {
                        Id = address.District.Id,
                        Name = address.District.Name,
                        UnitId = address.District.UnitId
                    }
                 ,
                Ward =  new WardResponse
                    {
                        Id = address.Ward.Id,
                        Name = address.Ward.Name,
                        UnitId = address.Ward.UnitId
                    }
                ,
                Account =  new AccountResponse
                    {
                        Id = address.Account.Id,
                        FullName = address.Account.FullName
                    }
                
            };

            return addressResponse;
        }

        public async Task<ICollection<GetAddressResponse>> GetAddressList(AddressFilter filter)
        {
            var addressList = await _unitOfWork.GetRepository<Address>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Name),
                    include: x => x.Include(x => x.City)
                                   .Include(x => x.District)
                                   .Include(x => x.Ward)
                                   .Include(x => x.Account))
                ?? throw new BadHttpRequestException(MessageConstant.Address.NotFoundFailedMessage);

            var addressResponses = addressList.Select(address => new GetAddressResponse
            {
                Id = address.Id,
                Name = address.Name,
                Status = string.IsNullOrEmpty(address.Status) ? null : EnumUtil.ParseEnum<AddressStatus>(address.Status),
                Note = address.Note,
                NamePersonal = address?.NamePersonal,
                PhoneNumber = address?.PhoneNumber,
                City = new CityResponse
                {
                    Id = address.City.Id,
                    Name = address.City.Name,
                    UnitId = address.City.UnitId
                }
                 ,
                District = new DistrictResponse
                {
                    Id = address.District.Id,
                    Name = address.District.Name,
                    UnitId = address.District.UnitId
                }
                 ,
                Ward = new WardResponse
                {
                    Id = address.Ward.Id,
                    Name = address.Ward.Name,
                    UnitId = address.Ward.UnitId
                }
                ,
                Account = new AccountResponse
                {
                    Id = address.Account.Id,
                    FullName = address.Account.FullName
                }
            }).ToList();

            return addressResponses;
        }

        public async Task<bool> RemoveAddressStatus(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.Address.AddressdEmptyMessage);

            var address = await _unitOfWork.GetRepository<Address>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Address.NotFoundFailedMessage);

            address.Status = AddressStatus.InActive.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Address>().UpdateAsync(address);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }

        public async Task<bool> UpdateAddress(Guid id, UpdateAddressRequest updateAddressRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.Address.AddressdEmptyMessage);

            var address = await _unitOfWork.GetRepository<Address>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Address.AddressExisteMessage);


            address.Name = string.IsNullOrEmpty(updateAddressRequest.Name) ? address.Name : updateAddressRequest.Name;
            address.Note = string.IsNullOrEmpty(updateAddressRequest.Note) ? address.Note : updateAddressRequest.Note;
            address.NamePersonal = string.IsNullOrEmpty(updateAddressRequest.NamePersonal) ? address.NamePersonal : updateAddressRequest.NamePersonal;
            address.PhoneNumber = string.IsNullOrEmpty(updateAddressRequest.PhoneNumber) ? address.PhoneNumber : updateAddressRequest.PhoneNumber;
            if (!updateAddressRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                address.Status = updateAddressRequest.Status.GetDescriptionFromEnum();
            }
            

            _unitOfWork.GetRepository<Address>().UpdateAsync(address);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;

            return isSuccess;
        }

    }
}
