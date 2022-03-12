namespace BookStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _conf;
    private readonly UserManager<ApiUser> _userManager;

    public UserController(IMapper mapper,
                          ILogger<UserController> logger,
                          IConfiguration conf,
                          UserManager<ApiUser> userManager)
    {
        _mapper = mapper;
        _logger = logger;
        _conf = conf;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser userRegister)
    {
        try
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var user = _mapper.Map<ApiUser>(userRegister);
            var isCreated = await _userManager.CreateAsync(user, userRegister.Password);
            if (isCreated.Succeeded == false)
            {
                foreach (var error in isCreated.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRoleAsync(user, userRegister.Role);

            return Ok("User Created");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(Register)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUser userLogin)
    {
        try
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(userLogin.UserName);
            var passValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);

            if (user == null || passValid == false)
                return Unauthorized(userLogin);

            string tokenString = await GenerateJwt(user);

            var response = new UserResponse()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = tokenString
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error at {nameof(Login)} - {ErrorMsg.Error500(ex)}");
            return StatusCode(500, ErrorMsg.Error500(ex));
        }
    }

    private async Task<string> GenerateJwt(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userClaims = await _userManager.GetClaimsAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();


        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())            
        }
        .Union(userClaims)
        .Union(roleClaims);
                
        var token = new JwtSecurityToken(
            issuer: _conf["JWT:Issuer"],
            audience: _conf["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
            );

        var tokenHandle = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenHandle;
    }
}
