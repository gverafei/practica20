using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using smvcfei.Models;

namespace smvcfei.Controllers
{
    // Observe como podemos colocar Authorize a nivel de controlador tambien
    // y afecta a todas las acciones del mismo
    [Authorize]
    public class CuentasController : Controller
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;

        public CuentasController(UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Este atributo permite que /Cuentas/Login si pueda ser accedido aunque no se este logueado
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool signInResult = false;
                    // Esta función verifica en la bd que el correo y contraseña sean válidos
                    var result = await _signInManager.PasswordSignInAsync(model.Correo, model.Password, isPersistent: false, lockoutOnFailure: false);
                    signInResult = result.Succeeded;    // Regresa true si es válido, false si no

                    if (signInResult)
                    {
                        // Usuario válido, lo envía al Home
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Correo", "Credenciales no válidas. Inténtelo nuevamente.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // Este atributo permite que /Cuentas/Registro si pueda ser accedido aunque no se este logueado
        [AllowAnonymous]
        public IActionResult Registro(bool creado = false)
        {
            ViewData["creado"] = creado;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroAsync(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Busca si el correo ya existe
                    var usuario = await _userManager.FindByEmailAsync(model.Correo);
                    if (usuario == null)
                    {
                        // Creamos un objeto para guardar en la bd
                        var usuarioToCreate = new CustomIdentityUser
                        {
                            UserName = model.Correo,
                            Email = model.Correo,
                            NormalizedEmail = model.Correo.ToUpper(),
                            Nombrecompleto = model.Nombre,
                            NormalizedUserName = model.Correo.ToUpper(),
                        };
                        // Se crea el usuario con el pwd ingresado
                        IdentityResult result = await _userManager.CreateAsync(usuarioToCreate, model.Password);
                        // En caso de éxito, se regresa al formulario para crear otro usuario
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(usuarioToCreate, "Administrador");
                            return RedirectToAction(nameof(Registro), new { creado = true });
                        }

                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(" ", errorList.Select(e => e.Description));
                        ModelState.AddModelError("Password", errors);
                    }
                    else
                    {
                        ModelState.AddModelError("Correo", $"El usuario {usuario.UserName} ya existe en el sistema.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewData["creado"] = false;
            return View();
        }

        public async Task<IActionResult> PerfilAsync()
        {
            // Busca si el correo ya existe
            var identityUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            UsuarioViewModel usuario = new()
            {
                Nombre = identityUser.Nombrecompleto,
                Correo = identityUser.Email
            };

            return View(usuario);
        }

        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            // Cierra la sesión
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                // Si hay una acción a donde regresar, se realiza un redirect
                return LocalRedirect(returnUrl);
            }
            else
            {
                // Sino, se redirige al inicio de sesión
                return RedirectToAction("Login");
            }
        }

        public IActionResult AccessDenied()
        {
            // Regresa un Vista con un mensaje de acceso denegado.
            return View();
        }
    }
}