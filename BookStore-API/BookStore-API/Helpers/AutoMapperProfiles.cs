using AutoMapper;
using BookStore_DomainModels;
using BookStore_DTO;

namespace BookStore_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorCreateDTO>().ReverseMap();
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, BookCreateDTO>().ReverseMap();
            CreateMap<Book, BookUpdateDTO>().ReverseMap();
        }
    }
}