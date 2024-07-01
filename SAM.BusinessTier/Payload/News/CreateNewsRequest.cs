using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.News
{
    public class CreateNewsRequest
    {

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? NewsContent { get; set; }

        public string? Cover { get; set; }

        public Guid? MachineryId { get; set; }

        public Guid? AccountId { get; set; }

        public List<CreateNewImage> Image { get; set; } = new List<CreateNewImage>();

        
    }
    public class CreateNewImage
    {
        public string? ImageURL { get; set; }

    }
}
