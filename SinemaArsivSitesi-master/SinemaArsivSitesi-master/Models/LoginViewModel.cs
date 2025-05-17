using System.ComponentModel.DataAnnotations;

namespace SinemaArsivSitesi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı veya e-posta gereklidir.")]
        [StringLength(100, ErrorMessage = "Kullanıcı adı veya e-posta en fazla 100 karakter olabilir.")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır.")]
        [StringLength(100, ErrorMessage = "Şifre en fazla 100 karakter olabilir.")]
        public string Password { get; set; }
    }
}
