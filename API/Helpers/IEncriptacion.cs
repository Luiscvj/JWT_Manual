namespace API.Helpers;


public interface IEncriptacion 
{
     string GetSHA256(string str);
     string AES256_Encriptar(string key, string texto);
     string AES256_Desencriptar(string key, string textoCifrado);
     string CorregirToken(string token);
     string  AES256_LOGIN_Key  {get;}
     string AES256_USER_Key {get;}
}