namespace WebApi.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService) => _usersService = usersService;

        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Returns every user from the database")]
        [Route("Get", Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IList<User>>> Get()
        {
            IList<User>? users;
            try
            {
                users = await _usersService.Get();
                if (users == null || users?.Count == 0)

                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok(users);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a user by a user id", Description = "Returns a single user matching the id from the database")]
        [Route("GetById", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<User>> GetById([FromQuery, Required] Guid userId)
        {
            User? user;
            try
            {
                user = await _usersService.Get(userId)!;
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok(user);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets users by name", Description = "Returns a collection of any users matching the name")]
        [Route("GetByName", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IList<User>>> GetByName([FromQuery, Required] string forename, [FromQuery, Required] string surname)
        {
            IList<User>? users;
            try
            {
                users = await _usersService.Get(forename, surname)!;
                if (users == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            return Ok(users);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Adds a new user", Description = "Creates a new user in the database")]
        [Route("AddUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<User>> Post([FromBody] UserDto user)
        {
            ValidationContext validationContext = new(user);
            List<ValidationResult> validationResults = new();
            User? createdUser; 

            if (!Validator.TryValidateObject(user, validationContext, validationResults))
            {
                return BadRequest(validationResults);
            }
            try
            {
                createdUser = await _usersService.Insert(user);
            }
            catch (Exception e)
            {
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction(nameof(GetById), new { userId = createdUser?.Id }, createdUser);
        }
    }
}