using SAM.BusinessTier.Payload.Machinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Favorite
{
    public class GetFavoriteResponse
    {
        public Guid? AccountId { get; set; }
        public string? Name { get; set; }
        public List<MachineryFavoriteResponse>? Machinery { get; set; } = new List<MachineryFavoriteResponse>();

    }
    public class MachineryFavoriteResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public double? SellingPrice { get; set; }
        public BrandResponse? Brand { get; set; }
        public List<MachineryImagesResponse>? Image { get; set; } = new List<MachineryImagesResponse>();

    }
}
