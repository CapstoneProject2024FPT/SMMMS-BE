using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class OrderFilter
    {
        public Guid Id { get; set; }

        public string? InvoiceCode { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string? Note { get; set; }

        public OrderStatus? status { get; set; }

        public Guid? AccountId { get; set; }

        //public Guid? PaymentId { get; set; }
    }
}
