using Aplicacion.Repository;
using Dominio.Entities;
using Dominio.Interfaces;

namespace Aplicacion.UnitOfWork;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly JWT_Context _context;


    private UsuarioRepository _usuario;
    private PaisRepository _pais;
    public UnitOfWork(JWT_Context context)
    {
        _context =context;
    }
    public IUsuario Usuarios
    {
        get
        {
            if (_usuario == null)
            {
                _usuario = new UsuarioRepository(_context);
            }
            return _usuario;
        }
    }

    public IPais Paises 
    {
        get
        {
            if(_pais == null)
            {
                _pais = new PaisRepository(_context);
            }
            return _pais;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async  Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }
}