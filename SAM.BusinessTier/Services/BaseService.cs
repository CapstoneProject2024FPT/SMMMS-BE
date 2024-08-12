using AutoMapper;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SAM.BusinessTier.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<SamDevContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        protected BaseService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<T> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUsernameFromJwt()
        {
            string username = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return username;
        }

        protected string GetRoleFromJwt()
        {
            string role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            return role;
        }
    }
}

