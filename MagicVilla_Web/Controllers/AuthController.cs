using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Schema;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {

            _authService = authService;
            _tokenProvider = tokenProvider;

        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO requestDTO = new LoginRequestDTO();
            return View(requestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO requestDTO)
        {

            var response = await _authService.LoginAsync<APIResponse>(requestDTO);
            if (response != null && response.IsSuccess)
            {
                TokenDto model = JsonConvert.DeserializeObject<TokenDto>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.AccessToken);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                _tokenProvider.SetToken(model);

                //HttpContext.Session.SetString(SD.AccessToken, model.AccessToken);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Custom", response.ErrorMessages.FirstOrDefault());
                return View(requestDTO);
            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            var role = new List<SelectListItem> {
             new SelectListItem{ Value = SD.Customer, Text = SD.Customer},
             new SelectListItem{ Value = SD.Admin, Text = SD.Admin},
            };
            ViewBag.Roles = role;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            if (registerationRequestDTO.Role == null)
            {
                registerationRequestDTO.Role = SD.Customer;
            }
            var Result = await _authService.RegisterAsync<APIResponse>(registerationRequestDTO);
            if (Result != null && Result.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }
            var role = new List<SelectListItem> {
             new SelectListItem{ Value = SD.Customer, Text = SD.Customer},
             new SelectListItem{ Value = SD.Admin, Text = SD.Admin},
            };
            ViewBag.Roles = role;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            var token = _tokenProvider.GetToken();
            await _authService.LogoutAsync<APIResponse>(token);
            _tokenProvider.RemoveToken();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
