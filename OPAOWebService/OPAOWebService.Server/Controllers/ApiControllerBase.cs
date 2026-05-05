using Microsoft.AspNetCore.Mvc;

namespace OPAOWebService.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Define it ONCE here
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
