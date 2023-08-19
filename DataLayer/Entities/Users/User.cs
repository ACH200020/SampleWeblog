using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities.Posts;

namespace DataLayer.Entities.Users
{
    public class User : BaseEntity
    {
        public int UserRoleId { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageAvatarName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        #region Relation
        [ForeignKey("UserRoleId")]
        public UserRole UserRole { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<UserToken> UserToken { get; set; }

        #endregion
    }


  
    
}
