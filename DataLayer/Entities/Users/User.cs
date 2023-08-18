using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities.Posts;

namespace DataLayer.Entities.Users
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public UserRole UserRole { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageAvatarName { get; set; }
        public string Password { get; set; }

        #region Relation

        public ICollection<Post> Post { get; set; }
        public ICollection<UserToken> UserToken { get; set; }

        #endregion
    }


    public enum UserRole
    {
        PanelAdmin,
        EditProfile,
        ChangePassword,
        Writer,
        Downloader,
        User
    }
    
}
