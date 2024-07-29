using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class CreateNewMachineryRequest
    {
        public string? Name { get; set; }

        public string? Model { get; set; }

        public string? Description { get; set; }

        public double? StockPrice { get; set; }

        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public Guid? BrandId { get; set; }

        public Guid? OriginId { get; set; }

        public int? TimeWarranty { get; set; }

        public List<MachineryImages>? Image { get; set; } = new List<MachineryImages>();

        public List<MachinerySpecifications>? SpecificationList { get; set; } = new List<MachinerySpecifications>();

        public Guid? CategoryId { get; set; }

        public List<Guid>? MachineComponentsId { get; set; }


    }
    public class MachinerySpecifications
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
    public class MachineryImages
    {
        public string? ImageURL { get; set; }

    }

}
