using GTI.API.Models;
 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GTIMVC.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        private readonly string apiUrl = "http://localhost:55812/api/cliente";
        public ActionResult Home()
        {

            return View();
        }
        public async Task<IActionResult> Index()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(apiResponse);
                }
            }
            return (IActionResult)View(listaClientes);
        }

        [HttpPost]
        public async Task<IActionResult> GetCliente(int id)
        {
            Cliente cliente = new Cliente();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                }
            }
            return (IActionResult)View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente(Cliente cliente)
        {
            Cliente cli = new Cliente();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(cliente),
                                                  Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(apiUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cli = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                }
            }
            return (IActionResult)View(cli);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCliente(int id)
        {
            Cliente cliente = new Cliente();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                }
            }
            return (IActionResult)View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCliente(Cliente  cliente)
        {
            Cliente cli = new Cliente();

            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(cli.Id.ToString()), "Id");
                content.Add(new StringContent(cli.Nome), "Nome");
                content.Add(new DateTime(cli.DataNascimento), "DataNascimento");
                content.Add(new StringContent(cliente.Cpf), "Cpf");

                using (var response = await httpClient.PutAsync(apiUrl, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Sucesso";
                    cli = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                }
            }
            return (IActionResult)View(cli);
        }
    }
}