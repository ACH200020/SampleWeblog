using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities.Users;

namespace DataLayer.Entities.Posts
{
    public class Post : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }  
        public string Description { get; set; }
        public string ImagePost { get; set; }
        public string Slug { get; set; }

        #region Relation
        [ForeignKey("UserId")]
        public User User { get; set; }
        #endregion
    }
}
