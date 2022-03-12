namespace BookStore.UI.Client.MappingUI;
public class MapUIConfig : Profile
{
    public MapUIConfig()
    {
        CreateMap<AuthorReadDto,AuthorUpdateDto>().ReverseMap();
        CreateMap<BookReadDto,BookUpdateDto>().ReverseMap();
        CreateMap<Response<BookUpdateDto>, Response<BookReadDto>>().ReverseMap();
    }
}
