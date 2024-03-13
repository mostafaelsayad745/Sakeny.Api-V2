using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sakeny.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sakeny.Models
{
    public class PostFeedbackForCreationDto
    {
        
       

        public DateTime? PostFeedDate { get; set; }

      
        public TimeSpan? PostFeedTime { get; set; }

       
        public string? PostFeedText { get; set; }



    }
}