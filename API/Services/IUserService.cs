using Dominio.Entities;

namespace API.Services;


public interface IUserService 
{
          Task<object> RegisterAsync(Usuario user);
         
          
          string getTokenLogin(string email, string password);
          string LoginByToken(string loginToken);
          bool SetPassword(string token, string newPassword,string oldPassword);
          bool ValidarTokenUsuario(string tokenUsuario);
          string GetEmailUsuarioFromToken(string token);
          bool Logout(string tokenUsuario);
}