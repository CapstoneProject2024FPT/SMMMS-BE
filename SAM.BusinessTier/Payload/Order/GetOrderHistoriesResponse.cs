using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class GetOrderHistoriesResponse
    {
        public Guid Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? UserId { get; set; }
        public string ? Username { get; set;}
    }

}

