namespace Dominio.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; }   
    public DateTime FechaCreacion { get; set; }
    public string Password { get; set; }
    public DateTime FechaExpiracion { get; set; }
}