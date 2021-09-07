using System.Threading.Tasks;
using BuscaCep.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuscaCep.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }
        
        [HttpGet("{cep}")]
        public async Task<IActionResult> Get([FromRoute] string cep)
        {
            return await _cepService.GetCep(cep);
        }
    }
}