using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocation.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IPAddressDTO, IPAddress>();
        }
    }
}
