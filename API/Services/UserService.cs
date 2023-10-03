using System.Data;
using System.Data.SqlClient;
using API.Helpers;
using Dominio.Entities;
using Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT_Context _context;
    private readonly IUnitOfWork _unitOfWork;
    private  readonly IEncriptacion _encriptacion;
    public UserService(JWT_Context context, IUnitOfWork unitOfWork, IEncriptacion encriptacion)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _encriptacion = encriptacion;
    }



     
    public async Task<object> RegisterAsync(Usuario user)
    {
        var usuario = new Usuario
        {
            Email = user.Email,
            Password = _encriptacion.GetSHA256(user.Password),
            
        };

        Console.WriteLine(usuario);

        var usuarioExiste = _unitOfWork.Usuarios
                                    .Find(u => u.Email == user.Email)
                                    .FirstOrDefault();

        if (usuarioExiste == null)
        {

            try
            {
                _unitOfWork.Usuarios.Add(usuario);
                await _unitOfWork.SaveChanges();

                return new
                {
                    mensaje = "Se ha registrado exitosamente",
                    status = true
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new
                {
                    mensaje = message,
                    status = false
                };
            }

        }
        else
        {
            return new
            {
                mensaje = "Ya se encuentra registrado.",
                status = false
            };
        }
    }



      public string getTokenLogin(string email, string password)//Generamos el tokeen por parte del cliente , usamos dicha encriptacion y la cadena deseada
    {
        string fecha = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string tokenLogin = _encriptacion.AES256_Encriptar(_encriptacion.AES256_LOGIN_Key, fecha + '#' + email + '#' + _encriptacion.GetSHA256(password));
        return tokenLogin;
    }



        public string LoginByToken(string loginToken)
    {
        try
        {
            string tokenUsuario = "";

            string tokenDecoficado = _encriptacion.AES256_Desencriptar( _encriptacion.AES256_LOGIN_Key, loginToken);
            string fecha = tokenDecoficado.Split('#')[0];
            string email = tokenDecoficado.Split('#')[1];
            string password = tokenDecoficado.Split('#')[2];

            // Validar fecha
            DateTime fechaLogin = DateTime.ParseExact(fecha, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            if (DateTime.UtcNow.Subtract(fechaLogin).TotalSeconds >= 30)
            {
                return "-1";    // -1 = Límite de tiempo excedido
            }

            // Validar login
            var verificarUsuario = _unitOfWork.Usuarios.Find(p => p.Email == email && p.Password == password).First();

            if (verificarUsuario != null)
            {
                tokenUsuario = verificarUsuario.Email + "#" + DateTime.UtcNow.AddHours(18).ToString("yyyyMMddHHmmss");        // Email # FechaCaducidad -> Encriptar con AES
                tokenUsuario = _encriptacion.AES256_Encriptar(_encriptacion.AES256_USER_Key, tokenUsuario);//Este es el token que entregamos 
                verificarUsuario.FechaCreacion = DateTime.UtcNow;
                verificarUsuario.FechaExpiracion = DateTime.UtcNow.AddHours(18);
                _unitOfWork.SaveChanges();
                return tokenUsuario;
            }
            else
            {
                return "-2";    // -2 = Usuario o clave incorrectas
            }
        }
        catch (Exception)
        {
            return "-3";        // -3 = Error
        }
    }



    public bool SetPassword(string token, string newPassword,string oldPassword)
    {
        try
        {
            if (!ValidarTokenUsuario(token))
            {
                return false;
            }
            string emailUsuario = this.GetEmailUsuarioFromToken(token);

            string encryptedOldPassword = _encriptacion.GetSHA256(oldPassword);
            string encryptedNewPassword = _encriptacion.GetSHA256(newPassword);


            var verificarContraseña = _unitOfWork.Usuarios.Find(p => p.Email == emailUsuario && p.Password == encryptedOldPassword).First();
            // Obtener el resultado del SP
            if (verificarContraseña != null){
                verificarContraseña.Password = encryptedNewPassword;
                var save=_unitOfWork.SaveChanges();
                if(save.Result!=0) return true;
                else return false;
            }else{
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }


     public bool ValidarTokenUsuario(string tokenUsuario)//validamos en cada llamada ,Extraemos todas las partes y vemos que no haya caducado
    {
        try
        {
            tokenUsuario =_encriptacion.CorregirToken(tokenUsuario);
            string tokenDescodificado = _encriptacion.AES256_Desencriptar(_encriptacion.AES256_USER_Key, tokenUsuario);
            string emailUsuario = tokenDescodificado.Split('#')[0];
            string fecha = tokenDescodificado.Split('#')[1]; 
            var verificarEmail = _unitOfWork.Usuarios.Find(p => p.Email == emailUsuario).First();
            // Validar fecha
            DateTime fechaCaducidad = DateTime.ParseExact(fecha, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            if (DateTime.UtcNow > fechaCaducidad || DateTime.UtcNow > verificarEmail.FechaExpiracion )
                return false;
            else
                return true;
        }
        catch (Exception)
        {
            return false;
        }


    }

      public string GetEmailUsuarioFromToken(string token) //M extrae la parte del email del usarioa del token
    {
        token = _encriptacion.CorregirToken(token);
        string tokenDecodificado = _encriptacion.AES256_Desencriptar (_encriptacion.AES256_USER_Key, token);
        string emailUsuario = tokenDecodificado.Split('#')[0];
        return emailUsuario;
    }


  public bool Logout(string tokenUsuario)
  {
        if(ValidarTokenUsuario(tokenUsuario)){
            tokenUsuario = _encriptacion.CorregirToken(tokenUsuario);
            string tokenDescodificado =_encriptacion.AES256_Desencriptar (_encriptacion.AES256_USER_Key , tokenUsuario);
            string emailUsuario = tokenDescodificado.Split('#')[0];
            var verificarContraseña = _unitOfWork.Usuarios.Find(p => p.Email == emailUsuario).First();
            verificarContraseña.FechaExpiracion=DateTime.UtcNow;
            _unitOfWork.SaveChanges();
            return true;
        }
        return false;       
        
    }

    
}