using Microsoft.AspNetCore.Identity;

namespace TiendaOnline.Models
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
