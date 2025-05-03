using AdminDashBoard.Models;
using AutoMapper;
using Domain.Models;


namespace Talabat.DashBoard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap();
        }
    }
}
