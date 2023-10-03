using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplicacion.Repository;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuario
{
    public UsuarioRepository(JWT_Context context) : base(context)
    {
    }
}