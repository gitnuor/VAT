using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vms.api.Authentication;
using vms.entity.Dto.Customer;
using vms.entity.Dto.User;
using vms.entity.models;
using vms.service.Services.SecurityService;
using vms.utility.StaticData;
using vms.utility;
using vms.service.ServiceImplementations.SecurityService;

namespace vms.api.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : VmsApiBaseController
{
	private readonly IUserService _userService;
	private readonly IConfiguration _configuration;

	public UserController(VmsApiUtility.ControllerBaseParamModel baseModel, IUserService userService, IConfiguration configuration) : base(baseModel)
	{
		_userService = userService;
		_configuration = configuration;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
	{
		return Ok(await _userService.GetAllForApi(CurrentUser.ProtectedOrganizationId));
	}

	[HttpPost]
	public async Task Post([FromBody] UserPostDto userPostDto)
	{
		Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var key);
		try
		{
			var defaultPassword = _configuration.GetValue<string>("PrivateData:DefaultPassword");
			var encryptionKey = _configuration.GetValue<string>("PrivateData:EncryptionKey");
			var userPassword = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey);
			await _userService.InsertUserDataFromApi(userPostDto, key, userPassword);
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception);
			throw;
		}
	}
}