using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    public class AuthorForCreationDto
    {
        [Required(ErrorMessage = "必須提供姓名")]
        [MaxLength(20, ErrorMessage = "姓名的最大長度為20個字符")]
        public string? Name { get; set; }

        public int Age { get; set; }

        [EmailAddress(ErrorMessage = "郵箱的格式不正確")]
        public string? Email { get; set; }
    }
}
