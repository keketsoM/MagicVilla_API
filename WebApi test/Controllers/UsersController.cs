using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Xml.Xsl;
using WebApi_test.Model;
using WebApi_test.Model.Dto;
using WebApi_test.Repository.IRepository;

namespace WebApi_test.Controllers
{

    [ApiVersionNeutral]
    [Route("api/v{version:apiVersion}/UsersAuth")]

    [ApiController]

    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private APIResponse _response;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new APIResponse();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var tokenDto = await _userRepository.Login(model);
            if (tokenDto == null && string.IsNullOrEmpty(tokenDto.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = tokenDto;
            return Ok(_response);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(model);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while Registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDto tokenDto)
        {
            if (ModelState.IsValid)
            {
                var tokenDtoResponse = await _userRepository.RefreshAccessToken(tokenDto);
                if (tokenDtoResponse == null || string.IsNullOrEmpty(tokenDtoResponse.AccessToken))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Token invaild");
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = tokenDtoResponse;
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid token Input");
                return BadRequest(_response);
            }

        }

        [HttpPost("Revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDto tokenDto)
        {

            if (ModelState.IsValid)
            {
                await _userRepository.RevokeRefreshToken(tokenDto);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            _response.IsSuccess = false;
            _response.Result = "Invalid Token";
            return BadRequest(_response);
        }
    }
}
