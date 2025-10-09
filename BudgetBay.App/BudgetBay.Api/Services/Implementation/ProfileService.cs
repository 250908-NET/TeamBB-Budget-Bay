using BudgetBay.DTOs;
using BudgetBay.Models;
using BudgetBay.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BudgetBay.Services
{
    public class ProfileService : IProfileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _context;

        public ProfileService(Cloudinary cloudinary, AppDbContext context)
        {
            _cloudinary = cloudinary;
            _context = context;
        }

        public async Task<string> UploadProfilePictureAsync(UploadProfilePictureDto dto)
        {
            //validate input
            if (dto.File == null || dto.File.Length == 0)
            {
                throw new ArgumentException("File is null or empty");
            }

            //upload to cloudinary
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.File.FileName, dto.File.OpenReadStream()),
                Folder = "BudgetBay/Profiles",
                PublicId = $"user_{dto.UserId}_{Guid.NewGuid()}", //unique name
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to upload to Cloudinary");
            }

            //update user record with new profile URL
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.ProfilePictureUrl = uploadResult.SecureUrl.ToString();
            
            await _context.SaveChangesAsync();

            return uploadResult.SecureUrl.ToString();
        }
    }
}