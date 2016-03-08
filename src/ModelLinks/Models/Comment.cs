using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelLinks.Models
{
    public class Comment : IComment
    {
        public int CommentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ApplicationUserId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public ApplicationUser User { get; set; }
    }

    public interface IComment
    {
        int CommentId { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string ApplicationUserId { get; set; }
        int PostId { get; set; }
        Post Post { get; set; }
        
        ApplicationUser User { get; set; }
    }
}
