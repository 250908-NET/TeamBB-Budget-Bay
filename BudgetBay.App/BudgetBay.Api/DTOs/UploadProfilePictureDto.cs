namespace BudgetBay.DTOs
{
    //DTO for uploading profile pictures
    public class UploadProfilePictureDto
    {
        public IFormFile File { get; set; } = null!;
        public int UserId { get; set; }
    }
}