using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //handles the erros, helps the display in nice format for user
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}