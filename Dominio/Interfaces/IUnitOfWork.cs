namespace Dominio.Interfaces;


public interface IUnitOfWork  
{
    IUsuario Usuarios {get;}
    IPais Paises {get;}


    Task<int> SaveChanges();

}