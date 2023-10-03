using API.DTOS;
using API.Services;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
    public class PaisController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PaisController(IUnitOfWork unitOfWork, IMapper mapper,IUserService userService) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public async Task<ActionResult<IEnumerable<PaisDto>>> GetAllPaises(string Token)
        {
            if(!_userService.ValidarTokenUsuario(Token)) return BadRequest("Token Invalido");

            IEnumerable<Pais> paises = await  _unitOfWork.Paises.GetAll();

            return Ok(_mapper.Map<IEnumerable<PaisDto>>(paises));
        }
        
    }
