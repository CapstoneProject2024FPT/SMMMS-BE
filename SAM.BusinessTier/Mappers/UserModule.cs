using AutoMapper;
using SAM.BusinessTier.Payload.User;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class UserModule : Profile
    {
        public UserModule()
        {
            CreateMap<User, GetUsersResponse>();
            CreateMap<CreateNewUserRequest, User>();
            //CreateMap<Account, GetUsersResponse>()
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
