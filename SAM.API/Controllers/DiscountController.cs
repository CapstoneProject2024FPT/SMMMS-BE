using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class DiscountController : BaseController<DiscountController>
    {
        private readonly IDiscountService _discountService;

        public DiscountController(ILogger<DiscountController> logger, IDiscountService discountService) : base(logger)
        {
            _discountService = discountService;
        }
    }
}
