using AutoMapper;
using userAddress = Domain.Models.Identity;
using orderAddress = Domain.Models.OrderModels.Address;
using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrderModels;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {



            CreateMap<Order,OrderResultDto>()
                .ForMember(d => d.PaymentStatus, O => O.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.Total, O => O.MapFrom(s => s.SubTotal + s.DeliveryMethod.Cost));
            CreateMap<DeliveryMethod, DeliveryMethodDto>();
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(s => s.ProductInfo.ProductId))
                .ForMember(d => d.PrductName, O => O.MapFrom(s => s.ProductInfo.ProductName))
                .ForMember(d => d.PictureUrl,O => O.MapFrom(s => s.ProductInfo.PictureUrl));
            

            CreateMap<orderAddress, AddressDto>().ReverseMap();
            CreateMap<userAddress.Address, AddressDto>().ReverseMap();
            

        }
    }
}
