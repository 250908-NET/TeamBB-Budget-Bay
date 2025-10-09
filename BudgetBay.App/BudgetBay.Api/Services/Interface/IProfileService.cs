using BudgetBay.DTOs;
using BudgetBay.Models;

namespace BudgetBay.Services
{
    public interface IProfileService
    {
        //Upload Profile Picture
        Task<string> UploadProfilePictureAsync(UploadProfilePictureDto dto);
    }
}