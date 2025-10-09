using BudgetBay.DTOs;
using BudgetBay.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureDto dto)
        {
            try
            {
                var imageUrl = await _profileService.UploadProfilePictureAsync(dto);
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}