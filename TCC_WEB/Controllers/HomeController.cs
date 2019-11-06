using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TCC_WEB.Data;
using TCC_WEB.Models;
using TCC_WEB.ViewModel;

namespace TCC_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Usuario> _gerenciadorUsuarios;
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly RoleManager<NivelAcesso> _roleManager;
        private readonly ApplicationDbContext _contexto;

        public HomeController(UserManager<Usuario> gerenciadorUsuarios, SignInManager<Usuario> gerenciadorLogin, RoleManager<NivelAcesso> roleManager, ApplicationDbContext contexto)
        {
            _roleManager = roleManager;
            _gerenciadorUsuarios = gerenciadorUsuarios;
            _gerenciadorLogin = gerenciadorLogin;
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "Administrador")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        //[Authorize]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registro(RegistroViewModel registro)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    UserName = registro.NomeUsuario,
                    Nome = registro.Nome,
                    SobreNome = registro.SobreNome,
                    Idade = registro.Idade,
                    Email = registro.Email
                };

                var usuarioCriado = await _gerenciadorUsuarios.CreateAsync(usuario, registro.Senha);

                if (usuarioCriado.Succeeded)
                {
                    if (_gerenciadorLogin.IsSignedIn(User) && User.IsInRole("Administrador"))
                    {
                        return RedirectToAction("Registro", "Home");
                    }
                    await _gerenciadorLogin.SignInAsync(usuario, false);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registro);
        }

        public async Task<IActionResult> LogOut()
        {
            await _gerenciadorLogin.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _gerenciadorUsuarios.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _gerenciadorLogin.PasswordSignInAsync(user,
                    loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Usuário/Senha inválidos ou não localizados");
            return View(loginVM);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrador")]
        public IActionResult NovoNivelAcesso()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> NovoNivelAcesso(NivelAcesso nivelAcesso)
        {
            if (ModelState.IsValid)
            {
                //nivelAcesso.Name = nivelAcesso.Nome;
                bool roleExiste = await _roleManager.RoleExistsAsync(nivelAcesso.Name);

                if (!roleExiste)
                {
                    await _roleManager.CreateAsync(nivelAcesso);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(nivelAcesso);
        }

        [Authorize]
        [Authorize(Roles = "Administrador")]
        public IActionResult MostrarUsers()
        {
            var users = _gerenciadorUsuarios.Users;
            // ViewData["UsuarioId"] = await _contexto.Usuarios.ToListAsync();
            return View(users);
        }

        //[Authorize]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AssociarUsuario()
        {
            ViewData["UsuarioId"] = new SelectList(await _contexto.Usuarios.ToListAsync(), "Id", "UserName");
            ViewData["NivelAcessoId"] = new SelectList(await _contexto.NiveisAcessos.ToListAsync(), "Name", "Name");

            return View();
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AssociarUsuario(UsuarioRoles usuarioRoles)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _gerenciadorUsuarios.FindByIdAsync(usuarioRoles.UsuarioId);
                //aqui teria que impedir para nao adicionar mais roles em um usuario
                await _gerenciadorUsuarios.AddToRoleAsync(usuario, usuarioRoles.NivelAcessoId);

                return RedirectToAction("Index", "Home");
            }

            return View(usuarioRoles);
        }

        public IActionResult Bootstrap()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Denied()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _gerenciadorUsuarios.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _gerenciadorUsuarios.GetClaimsAsync(user);
            var userRoles = await _gerenciadorUsuarios.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _gerenciadorUsuarios.FindByIdAsync(model.Id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await _gerenciadorUsuarios.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("MostrarUsers");
                }

                foreach(var error in result.Errors)//faz um loop nos erros e é mostrado all sumury na view page
                {
                    ModelState.AddModelError("",error.Description);
                }

                return View(model);
            }
        }

        [Authorize]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _gerenciadorUsuarios.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _gerenciadorUsuarios.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("MostrarUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

                return View("MostrarUsers");
            }
        }

        //nao usado
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("MostrarUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("MostrarUsers");
            }
        }
    }
}
