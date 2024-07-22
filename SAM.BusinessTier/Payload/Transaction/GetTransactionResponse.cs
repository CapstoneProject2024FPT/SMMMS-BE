using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Transaction
{
    public class GetTransactionResponse
    {
        public Guid Id { get; set; }

        public PaymentStatus? Status { get; set; }

        public string? Description { get; set; }

        public string? InvoiceId { get; set; }

        public double? TotalAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public PaymentType? PayType { get; set; }

        public string? TransactionJson { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? AccountId { get; set; }

    }
}

