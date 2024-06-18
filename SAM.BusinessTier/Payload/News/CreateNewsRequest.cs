using SAM.BusinessTier.Enums;
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

        public NewsStatus? Status { get; set; }

        public List<NewsMachineryResponse> MachineryList { get; set; } = new List<NewsMachineryResponse>();

        public List<CreateNewImage> ImgList { get; set; } = new List<CreateNewImage>();

        public Guid? AccountId { get; set; }
    }
    public class NewsMachineryResponse
    {
        public Guid? MachineryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class CreateNewImage
    {
        public string? ImageURL { get; set; }

    }
}
