using Microsoft.AspNetCore.Mvc;
using Server.WebAPI.Filter;

namespace Server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [APIResultFilter]
    public class BaseController : ControllerBase
    {
    }
}
