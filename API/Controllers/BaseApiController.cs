using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //base controller extended by all others controllers, sets the root of the website
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}