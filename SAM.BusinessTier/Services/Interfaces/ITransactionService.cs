using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ICollection<GetTransactionResponse>> GetTransactionList(TransactionFilter filter);
        Task<GetTransactionResponse> GetTransactionById(Guid id);
    }
}
