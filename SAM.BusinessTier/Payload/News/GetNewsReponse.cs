using Azure.Core.Pipeline;
using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.News
{
    public class GetNewsReponse
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? NewsContent { get; set; }

        public string? Cover { get; set; }

        public NewsStatus? Status { get; set; }

        public DateTime? CreateDate { get; set; }

        public NewsMachineryResponse? Machinery { get; set; }

        public AccountResponse? AccountId { get; set; }

        public List<CreateNewImage> ImgList { get; set; } = new List<CreateNewImage>();
    }
    public class NewsMachineryResponse
    {
        public Guid? MachineryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class NewsImageResponse
    {
        public string? ImageURL { get; set; }

    }
    public class AccountResponse
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public RoleEnum? Role { get; set; }
    }

}
