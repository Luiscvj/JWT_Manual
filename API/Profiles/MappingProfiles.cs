using API.DTOS;
using AutoMapper;
using Dominio.Entities;

namespace API.Controllers.Profiles;

public class MappingProfiles :Profile
{
    public MappingProfiles()
    {
        CreateMap<Usuario,RegisterDto>().ReverseMap();
        CreateMap<Pais,PaisDto>().ReverseMap();
    }
}