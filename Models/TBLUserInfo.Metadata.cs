using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserSignupLogin.Models
{
    // Kết hợp với class TBLUserInfo gốc (file auto-generated)
    [MetadataType(typeof(TBLUserInfoMetadata))]
    public partial class TBLUserInfo
    {
        // ✅ Thuộc tính thêm mới: không lưu trong DB
        [NotMapped]
        [DataType(DataType.Password)]
        public string RePasswordUs { get; set; }
    }

    // ✅ Metadata cho các thuộc tính trong DB
    public class TBLUserInfoMetadata
    {
        [Key]
        public int IdUs { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string UsernameUs { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string PasswordUs { get; set; }
    }
}
