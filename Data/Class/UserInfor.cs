using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace CodeLeapChallengeAPI_06022025.Data.Class
{
    /// <summary>
    /// User Information
    /// </summary>
    public class UserInfor
    {
        /// <summary>
        /// UserName
        /// </summary>
        [Key]
        public string UserName { get; set; }
        /// <summary>
        /// Password base64
        /// </summary>
        public required string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Sex
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        ///  Role -  0 Admin -  1 User
        /// </summary>
        public int? AccountType { get; set; }
    }
    /// <summary>
    /// Login Request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
