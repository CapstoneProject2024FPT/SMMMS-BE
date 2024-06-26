using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Brand
{
    public class GetBrandResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? URLImage { get; set; }
    }
}
