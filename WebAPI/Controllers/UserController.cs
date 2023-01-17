using System.Net;
using Application.Exceptions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ILogger<UserController> _logger;

		public UserController(IUserService userService, ILogger<UserController> logger)
		{
			_logger = logger;
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var entities = await _userService.GetAll();

				return Ok(entities);
			}
			catch (Exception err)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var entity = await _userService.GetById(id);

				return Ok(entity);
			}
			catch (EntityNotFoundException err)
			{
				return NotFound(err.Message);
			}
			catch (Exception err)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(User user)
		{
			try
			{
				var createdEntity = await _userService.Create(user);

				return Ok(createdEntity);
			}
			catch (EntityAlreadyExistException err)
			{
				return Conflict(err.Message);
			}
			catch (Exception err)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
			}
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _userService.Delete(id);

				return NoContent();
			}
			catch (EntityNotFoundException err)
			{
				return NotFound(err.Message);
			}
			catch (Exception err)
			{
				return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
			}
		}
	}
}