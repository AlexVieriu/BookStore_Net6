using AutoMapper;
using BookStore.UI.Server.Services;

namespace BookStore.UI.Server.MappingUI;
public class MapUIConfig : Profile
{
    public MapUIConfig()
    {
        CreateMap<AuthorReadDto,AuthorUpdateDto>().ReverseMap();
        CreateMap<BookReadDto,BookUpdateDto>().ReverseMap();
        CreateMap<Response<BookUpdateDto>, Response<BookReadDto>>().ReverseMap();
    }
}
