using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelLinks.Models
{
    public class Post : IPost
    {
        public int PostId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ApplicationUserId { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ApplicationUser User { get; set; }
    }

    public interface IPost
    {
        int PostId { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string ApplicationUserId { get; set; }
        ICollection<Comment> Comments { get; set; }

        ApplicationUser User { get; set; }
    }
}
