﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PageProcessor;
using PageProcessor.ServiceFactory;
using PageProcessor.Storage;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ShowCastController : Controller
    {
        
        public ShowCastController()
        {
            StorageFactory = new DefaultServiceFactory();
        }

        public IServiceFactory<IStorage> StorageFactory { get; set; }
        // GET api/ShowCast/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const long id = default(long);
            var json = (await StorageFactory.Service.GetPageByPageId(id))?.Json;
            return JsonResult(json);
        }

        // GET api/ShowCast/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var json = (await StorageFactory.Service.GetPageByPageId(id))?.Json;
            return JsonResult(json);
        }

        private IActionResult JsonResult(string json)
        {
            return json == null?
                (IActionResult) StatusCode((int) HttpStatusCode.NotFound):
                Content(json, Constants.JsonContentType);

        }
    }
}
