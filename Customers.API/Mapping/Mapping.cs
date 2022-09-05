using AutoMapper;
using Customers.API.Entities;
using Customers.API.Models;

namespace Customers.API.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Customer, CustomerVM>().ReverseMap();
            CreateMap<Customer, CustomerRequest>().ReverseMap();
        }
    }
}
