using System.Net.Http.Headers;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("[controller]")]
    public class HeadersController : ControllerBase
    {
        private readonly IDummyService _dummyService;

        public HeadersController(IDummyService dummyService) => _dummyService = dummyService;

        [HttpGet]
        [Route("GetOutboundHeaders")]
        [SwaggerOperation(Summary = "Gets all the default outbound headers", Description = "This endpoint is to show the propagated header appearing on the outbound request without needing to be configured in service directly")]
        #pragma warning disable IDE0060 // Remove unused parameter
        public async Task<ActionResult<HttpRequestHeaders?>> GetOutboundHeaders([FromHeader, Required] string propagatedHeader, [FromHeader, Required] string notPropagatedHeader)
        #pragma warning restore IDE0060 // Remove unused parameter
        {
            return await _dummyService.GetOutboundHeaders();
        }

        [HttpGet]
        [Route("GetInboundHeaders")]
        [SwaggerOperation(Summary = "Reads the incoming headers")]
        public IHeaderDictionary GetInboundHeaders()
        {
            return HttpContext.Request.Headers;
        }
    }
}