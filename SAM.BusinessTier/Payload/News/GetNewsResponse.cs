using Azure.Core.Pipeline;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.News
{
    public class GetNewsResponse
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? NewsContent { get; set; }

        public string? Cover { get; set; }

        public NewsStatus? Status { get; set; }

        public NewsTypes? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public NewsCategoryResponse? NewsCategory { get; set; }

        public AccountResponse? Account { get; set; }

        public List<NewsImageResponse> ImgList { get; set; } = new List<NewsImageResponse>();
    }
    public class NewsCategoryResponse
    {
        public Guid? NewsCategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
    public class NewsImageResponse
    {
        public Guid? Id { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? CreateDate { get; set; }

    }
    public class AccountResponse
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public RoleEnum? Role { get; set; }
    }

}
