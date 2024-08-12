using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Transaction;
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
    public class TransactionService : BaseService<TransactionService>, ITransactionService

    {
        public TransactionService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<TransactionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<GetTransactionResponse> GetTransactionById(Guid id)
        {
            var transaction = await _unitOfWork.GetRepository<TransactionPayment>()
                .SingleOrDefaultAsync(
                    selector: transaction => new GetTransactionResponse
                    {
                        Id = transaction.Id,
                        Status = transaction.Status != null ? EnumUtil.ParseEnum<PaymentStatus>(transaction.Status) : (PaymentStatus?)null,
                        Description = transaction.Description,
                        InvoiceId = transaction.InvoiceId,
                        TotalAmount = transaction.TotalAmount,
                        CreatedAt = transaction.CreatedAt,
                        PayType = transaction.PayType != null ? EnumUtil.ParseEnum<PaymentType>(transaction.PayType) : (PaymentType?)null,
                        TransactionJson = transaction.TransactionJson,
                        PaymentId = transaction.PaymentId,
                        OrderId = transaction.Payment.OrderId,
                        AccountId = transaction.Payment != null ? transaction.Payment.Order.AccountId : (Guid?)null
                    },
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.Payment))
                ?? throw new BadHttpRequestException(MessageConstant.Transaction.NotFoundFailedMessage);

            return transaction;
        }


        public async Task<ICollection<GetTransactionResponse>> GetTransactionList(TransactionFilter filter)
        {
            var transactionList = await _unitOfWork.GetRepository<TransactionPayment>()
                .GetListAsync(
                    selector: transaction => new GetTransactionResponse
                    {
                        Id = transaction.Id,
                        Status = transaction.Status != null ? EnumUtil.ParseEnum<PaymentStatus>(transaction.Status) : (PaymentStatus?)null,
                        Description = transaction.Description,
                        InvoiceId = transaction.InvoiceId,
                        TotalAmount = transaction.TotalAmount,
                        CreatedAt = transaction.CreatedAt,
                        PayType = transaction.PayType != null ? EnumUtil.ParseEnum<PaymentType>(transaction.PayType) : (PaymentType?)null,
                        TransactionJson = transaction.TransactionJson,
                        PaymentId = transaction.PaymentId,
                        OrderId = transaction.Payment.OrderId,
                        AccountId = transaction.Payment != null ? transaction.Payment.Order.AccountId : (Guid?)null
                    },
                    filter: filter,
                    orderBy: x => x.OrderByDescending(x => x.CreatedAt), // Adjust as per your sorting requirements
                    include: x => x.Include(x => x.Payment))
                ?? throw new BadHttpRequestException(MessageConstant.Transaction.NotFoundFailedMessage);

            return transactionList;
        }

    }
}
