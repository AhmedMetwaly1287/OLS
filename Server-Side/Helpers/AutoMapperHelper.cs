using AutoMapper;
using OLS.DTO;
using OLS.Models;

namespace OLS.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<RegisterDTO, User>().ReverseMap(); 
            CreateMap<LoginDTO, User>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<AddBookDTO, Book>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<AdminBorrowedBookDTO, BorrowedBook>().ReverseMap();
            CreateMap<UserBorrowedBookDTO, BorrowedBook>().ReverseMap();
            CreateMap<RequestDTO, BorrowedBook>().ReverseMap();
            CreateMap<AdminArchiveDTO, Archive>().ReverseMap();
            CreateMap<UserArchiveDTO, Archive>().ReverseMap();
            CreateMap<AddArchiveDTO, Archive>().ReverseMap();
        }
    }
}
