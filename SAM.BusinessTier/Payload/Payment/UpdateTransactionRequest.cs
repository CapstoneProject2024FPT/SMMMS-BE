using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SAM.BusinessTier.Payload.Payment
{
    public class UpdateTransactionRequest
    {
        public TransactionStatus? Status { get; set; }
    }
}
