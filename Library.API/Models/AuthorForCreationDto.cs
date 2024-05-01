using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    public class AuthorForCreationDto
    {
        [Required(ErrorMessage = "必須提供姓名")]
        [MaxLength(20, ErrorMessage = "姓名的最大長度為20個字符")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "必須提供出生日期")]
        public DateTimeOffset BirthDate { get; set; }

        [Required(ErrorMessage = "必須提供出生地")]
        public string? BirthPlace { get; set; }

        [Required(ErrorMessage = "必須提供郵箱")]
        [EmailAddress(ErrorMessage = "郵箱的格式不正確")]
        public string? Email { get; set; }
    }
}
