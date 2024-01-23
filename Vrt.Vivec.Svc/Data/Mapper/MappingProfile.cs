
using AutoMapper;
using Vrt.Vivec.Svc.Data.DTOs;
using Vrt.Vivec.Svc.Data.Response;

namespace Vrt.Vivec.Svc.Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewsDTO, News>();
            CreateMap<MensajesDTO, MessageDTO>();
            CreateMap<CategoryDTO, DTOs.CategoryDTO>();
            CreateMap<NewsDTO, NewsHtmlDTO>();
        }
    }
}
