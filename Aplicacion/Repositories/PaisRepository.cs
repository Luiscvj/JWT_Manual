using Dominio.Interfaces;
using Dominio.Entities;
namespace Aplicacion.Repository;

public class PaisRepository : GenericRepository<Pais>, IPais
{
    public PaisRepository(JWT_Context context) : base(context)
    {
    }
}