using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApiPod.Controllers
{
    
    public class StatusController : Controller
    {
        [HttpGet]
        [Route("api/status/health")]
        public IActionResult Health()
        {
            return Ok("pod is healthy"); 
        }


        [HttpGet]
        [Route("api/status/readiness")]
        public IActionResult Readiness()
        {
            var someFicticousDependency1IsReady = true;
            var someFicticousDependency2IsReady = true;

            var isOk = someFicticousDependency1IsReady && someFicticousDependency2IsReady;
            if(!isOk)
            {
                return StatusCode(
                    503,
                    "The systems that this POD relies or are not available"
                    );
            }

            return Ok("pod is ready to recieve the light");
        }

    }
}
