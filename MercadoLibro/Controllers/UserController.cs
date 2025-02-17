using MercadoLibroDB;
using MercadoLibroDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLibro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(
        MercadoLibroContext context
    ) : Controller
    {
        readonly MercadoLibroContext _context = context;
        // GET: User

        [HttpPost("SingUp")]
        public async Task<ActionResult> SingUp(
            string name,
            string email,
            string password
        )
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                User user = new(name, email);

                try
                {
                    _context.User.Add(user);

                    _context.SaveChanges();

                    UserAuth userAuth = new(user.Id, password);

                    _context.UserAuth.Add(userAuth);

                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok(user);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return StatusCode(500, "Error al crear el usuario y su autenticación.");
                }
            }
        }
    }
}
