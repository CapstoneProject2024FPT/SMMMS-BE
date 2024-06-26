﻿using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Payload.WarrantyDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IWarrantyDetailService
    {
        Task<Guid> CreateNewWarrantyDetail(CreateNewWarrantyDetailRequest createNewWarrantyDetailRequest);
        Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateWarrantyDetailRequest);
        Task<ICollection<GetWarrantyDetailResponse>> GetRankList(WarrantyDetailFilter filter);
        Task<GetBrandResponse> GetWarrantyDetailById(Guid id);
        Task<bool> RemoveWarrantyDetailStatus(Guid id);
    }
}
