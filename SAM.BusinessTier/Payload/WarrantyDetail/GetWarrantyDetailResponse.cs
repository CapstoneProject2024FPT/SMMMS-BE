﻿using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.WarrantyDetail
{
    public class GetWarrantyDetailResponse
    {
        public Guid? Id { get; set; }

        public WarrantyDetailType? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public WarrantyDetailStatus? Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }
        public Guid? WarrantyId { get; set; }

        public OrderUserResponse? Staff { get; set; }
        public List<ComponentChangeResponse>? ComponentChange { get; set; } = new List<ComponentChangeResponse>();
        public List<WarrantyDetailNoteResponse>? Note { get; set; } = new List<WarrantyDetailNoteResponse>();
    }
    public class ComponentChangeResponse
    {
        public string? Image {  get; set; }
        public DateTime? CreateDate { get; set;}
        public ComponentResponse? Component { get; set; }
        

    }



}
