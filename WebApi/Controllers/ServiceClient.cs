namespace WebApi.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("[controller]")]
    public class ServiceClient : ControllerBase
    {
        private readonly IDadJokesService _dadJokesService;

        public ServiceClient(IDadJokesService dadJokesService) => _dadJokesService = dadJokesService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Gets a dad joke", Description = "This endpoint creates the api following the recommended service client approach")]
        [Route("GetDadJoke")]
        public async Task<ActionResult<string>> GetDadJoke()
        {
            string dadJoke;

            try
            {
                dadJoke = await _dadJokesService.GetDadJoke();
            }
            catch (Exception)
            {
                return Problem("Unable to return a dad joke");
            }
            return Ok(dadJoke);
        }
    }
}