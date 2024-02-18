using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, DTOs.AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));

            CreateMap<CreateAuctionDto, Item>();

            // profile for publish auction
            CreateMap<AuctionDto, AuctionCreated>();
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(m => m.Item);
            CreateMap<Auction, Item>();
        }
    }
}
