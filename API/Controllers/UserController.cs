using API.DTOS;
using API.Services;
using Aplicacion.UnitOfWork;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers;


public class UserController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork unitOfWork, IUserService userService,IMapper mapper)
    {
        _unitOfWork =unitOfWork;
        _userService = userService;
        _mapper  = mapper;
    }


    [HttpPost("AddUser")]
    //[Authorize(Roles="")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> AddUser(RegisterDto registerDto )
    {
        Usuario user = _mapper.Map<Usuario>(registerDto);
        _userService.RegisterAsync(user);
     
       
        return Ok("Usuario creado con exito");
    }
    

    [HttpPost("getTokenLogin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetTokenLogin([FromForm] string email, [FromForm] string password)//  Este es el metodo que genera el token de parte del cliente
    {
        return  Ok(_userService.getTokenLogin(email, password)) ;
    }

    [HttpPost("loginByToken")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult LoginByToken([FromForm] string loginToken) //Metodo del login aca recibimos el token de la aplicaicon cliente
    {
        
        string token = _userService.LoginByToken(loginToken);

        switch (token)
        {
            case "-1": return BadRequest("Límite de tiempo excedido");
            case "-2": return BadRequest("Usuario o clave incorrectos");
            case "-3": return BadRequest("No se pudo hacer el login, revise los datos enviados");
            default: return Ok(token) ;
        }
    }

    [HttpPost("setPassword")] //Metodo para cambiar el password
    public ActionResult SetPassword([FromForm] string token, [FromForm] string newPassword,[FromForm] string oldPassword)
    {

        bool resultado = _userService.SetPassword(token, newPassword, oldPassword);
        if (resultado)
            return Ok("Cambio de contraseña exitoso");
        else
            return BadRequest("Hubo un error al intentar cambiar la contraseña");
    }


     [HttpPost("logout")]
    public ActionResult logout([FromForm] string token)
    {
        var result=_userService.Logout(token);
        if(result) return Ok("Sesion terminada");
        else return BadRequest("Error al finalizar la sesion");
    }



   
}