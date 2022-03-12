namespace BookStore.Api.Mapping;
public class MapConfig : Profile
{
    public MapConfig()
    {
        // User
        CreateMap<ApiUser,RegisterUser>().ReverseMap();

        // Authors
        CreateMap<Author, AuthorReadDto>().ReverseMap();
        CreateMap<Author, AuthorCreateDto>().ReverseMap();  
        CreateMap<Author, AuthorUpdateDto>().ReverseMap();

        // Books
        CreateMap<Book, BookCreateDto>().ReverseMap();
        CreateMap<Book, BookReadDto>()
            .ForMember(q=> q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();
        CreateMap<Book, BookUpdateDto>().ReverseMap();
    }
}
