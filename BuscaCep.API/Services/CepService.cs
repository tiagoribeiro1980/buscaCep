using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuscaCep.API.Domain;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Serialization.Json;

namespace BuscaCep.API.Services
{
    public interface ICepService
    {
        Task<IActionResult> GetCep(string cep);
    }
    
    public class CepService : ICepService
    {
        public async Task<IActionResult> GetCep(string cep)
        {
            try
            {
                if (!ValidaCepInformado(cep))
                    return new BadRequestObjectResult(new {message = "Informe apenas números no cep."});

                var responseCep = BuscaCep(cep);

                return await TrataRetorno(responseCep);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                    {message = "Ocorreu um erro ao pesquisar o cep. Erro:" + ex.Message});
            }
        }

        private bool ValidaCepInformado(string cep)
        {
            return cep.All(char.IsDigit);
        }
        
        private IRestResponse BuscaCep(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";

            var restClient = new RestClient(url);
            var restRequest = new RestRequest(Method.GET);
            return restClient.Execute(restRequest);
        }

        private async Task<IActionResult> TrataRetorno(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new
                    {message = "Houve um problema com a sua requisição: Verifique o Cep informado."});
            
            if (response.Content.Contains("erro"))
                return new NotFoundObjectResult(new {message = "Cep não localizado"}); 
                            
            var cepRetorno = new JsonDeserializer().Deserialize<CorreiosCep>(response);

            return new OkObjectResult(cepRetorno);
        }
    }
}