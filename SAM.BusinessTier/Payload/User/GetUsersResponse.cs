using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class GetUsersResponse
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public RoleEnum? Role { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public UserStatus? Status { get; set; }

        public string? Email { get; set; }
        
        public double? Amount { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? Image { get; set; }

        public RankResponse? Rank { get; set; }

        //public CertificationResponse? Certification { get; set; }



    }
    public class RankResponse
    {
        public string? Name { get; set; }

        public int? Range { get; set; }
    }
    public class CertificationResponse
    {
        public string? CertificationLink { get; set; }

        public DateTime? DateObtained { get; set; }

    }
}
