using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.News
{
    public class NewsFilter
    {
        public List<string>? Title { get; set; }

        public string? Description { get; set; }

        public List<string>? NewsContent { get; set; }

        public NewsStatus? Status { get; set; }

        public NewsTypes? Type { get; set; }

        public string? Cover { get; set; }

        public List<Guid>? NewsCategoryId { get; set; }

        public List<Guid>? AccountId { get; set; }
    }
}
