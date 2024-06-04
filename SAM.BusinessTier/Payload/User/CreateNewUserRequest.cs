using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class CreateNewUserRequest
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Status { get; set; }

        public string? Email { get; set; }
        public List<AccountRank> ProductList { get; set; } = new List<AccountRank>();

        public Guid? Rank { get; set; }

        public double? Amount { get; set; }

        public int? YearsOfExperience { get; set; }

    }
    public class AccountRank
    {
        public Guid RankId { get; set; }
        public string Name {  get; set; }
        public int Range { get; set; }
    }

}
