using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class DistrictService : BaseService<DistrictService>, IDistrictService
    {
        public DistrictService(IUnitOfWork<SamContext> unitOfWork, ILogger<DistrictService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewDistrict(CreateNewDistrictRequest createNewDistrictRequest)
        {
            District district = await _unitOfWork.GetRepository<District>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewDistrictRequest.Name));
            if (district != null) throw new BadHttpRequestException(MessageConstant.District.DistrictExistedMessage);
            district = _mapper.Map<District>(createNewDistrictRequest);
            district.Id = Guid.NewGuid();
            district.Status = DistrictStatus.Active.GetDescriptionFromEnum();

            await _unitOfWork.GetRepository<District>().InsertAsync(district);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.District.CreateDistrictFailedMessage);
            return district.Id;
        }

        public async Task<GetDistrictResponse> GetDistrictById(Guid id)
        {
            var district = await _unitOfWork.GetRepository<District>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.City).
                                    Include(x => x.Wards))
                ?? throw new BadHttpRequestException(MessageConstant.District.NotFoundFailedMessage);

            var districtResponse = new GetDistrictResponse
            {
                Id = district.Id,
                Name = district.Name,
                Status = string.IsNullOrEmpty(district.Status) ? null : EnumUtil.ParseEnum<DistrictStatus>(district.Status),
                City = new CityResponse
                {
                    Id = district.City.Id,
                    Name = district.City.Name,
                    UnitId = district.City.UnitId,
                },
                Ward = district.Wards.Select(ward => new WardResponse
                {
                    Id = ward.Id,
                    Name = ward.Name,
                    UnitId = ward.UnitId,
                }).ToList(),
            };
            return districtResponse;
        }

        public async Task<ICollection<GetDistrictResponse>> GetDistrictList(DistrictFilter filter)
        {
            var districtList = await _unitOfWork.GetRepository<District>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Name),
                    include: x => x.Include(x => x.City).
                                    Include(x => x.Wards))
                ?? throw new BadHttpRequestException(MessageConstant.District.NotFoundFailedMessage);

            var districtResponses = districtList.Select(district => new GetDistrictResponse
            {
                Id = district.Id,
                Name = district.Name,
                Status = string.IsNullOrEmpty(district.Status) ? null : EnumUtil.ParseEnum<DistrictStatus>(district.Status),
                City = new CityResponse
                {
                    Id = district.City.Id,
                    Name = district.City.Name,
                    UnitId = district.City.UnitId,
                },
                Ward = district.Wards.Select(ward => new WardResponse
                {
                    Id = ward.Id,
                    Name = ward.Name,
                    UnitId = ward.UnitId,
                }).ToList(),
            }).ToList();

            return districtResponses;
        }

        public async Task<bool> RemoveDistrictStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.District.DistrictEmptyMessage);
            District district = await _unitOfWork.GetRepository<District>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);
            district.Status = DistrictStatus.InActive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<District>().UpdateAsync(district);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.District.DistrictEmptyMessage);

            var district = await _unitOfWork.GetRepository<District>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.District.DistrictExistedMessage);

            district.Name = string.IsNullOrEmpty(updateDistrictRequest.Name) ? district.Name : updateDistrictRequest.Name;
            district.Status = updateDistrictRequest.Status.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<District>().UpdateAsync(district);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
