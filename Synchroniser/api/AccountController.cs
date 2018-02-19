using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client;
using Client.Entities;

namespace Synchroniser.api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ITokenConsumer _crmClient;
        Account _account;

        public AccountController(ILogger<ContactController> logger, ITokenConsumer crmClient)
        {
            _logger = logger;
            _crmClient = crmClient;
            _account = new Account((CRMClient)_crmClient);
        }

        [HttpGet("myproxy")]
        public IActionResult Proxy()
        {
            string result = "{\"aps\":{\"alert\":\"blah\",\"sound\":\"ping.aiff\"}}";
            return Ok(result);
        }
    }
}
