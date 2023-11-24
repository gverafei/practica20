using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smvcfei.Models;
using System.Diagnostics;

namespace smvcfei.Controllers
{
    public class HomeController : Controller
    {
        // Aqui si puede ingresar el usuario
        public IActionResult Index()
        {
            return View();
        }

        // Con el atributo Authorize no permite ingresar a esta acción a usuarios no autenticados
        // Puede especificar también acceso solo a ciertos Roles de usuario y hacer cosas como esta:
        //[Authorize(Roles = "Administrador, Editor")]
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        // Solo pueden entrar usuarios con rol "Administrador"
        [Authorize(Roles = "Administrador")]
        public IActionResult SoloParaAdministradores()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}