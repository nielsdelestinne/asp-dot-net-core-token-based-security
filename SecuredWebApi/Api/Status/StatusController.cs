using Microsoft.AspNetCore.Mvc;

namespace SecuredWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Up and running!");
        }

    }
}
