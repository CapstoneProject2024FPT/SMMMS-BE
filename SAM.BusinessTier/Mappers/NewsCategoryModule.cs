using AutoMapper;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.NewsCategory;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class NewsCategoryModule : Profile
    {
        public NewsCategoryModule() {
            CreateMap<NewsCategory, GetNewsCategoriesResponse>();
            CreateMap<CreateNewNewsCategoryRequest, NewsCategory>();

        }
    }
}
