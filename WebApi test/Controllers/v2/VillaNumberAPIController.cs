using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Model.Dto;
using WebApi_test.Repository.IRepository;

namespace WebApi_test.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        public VillaNumberAPIController(IVillaNumberRepository villaNumberRepository, IMapper mapper, IVillaRepository villaRepository)
        {
            _response = new();
            _villaNumberRepository = villaNumberRepository;
            _mapper = mapper;
            _villaRepository = villaRepository;

        }

        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {

            return new string[] { "keketso", "Moloi" };
        }




    }
}
