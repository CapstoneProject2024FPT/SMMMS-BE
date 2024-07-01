using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.News
{
    public class UpdateNewsRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? NewsContent { get; set; }

        public string? Cover { get; set; }

        public NewsStatus? Status { get; set; }

        public Guid? NewsCategoryId { get; set; }

    }
}
