using System.ComponentModel.DataAnnotations.Schema;

namespace sakeny.Models.PicturesDtos
{
    public class PicturesForReturnDto
    {
        public int PostPicId { get; set; }


        public string? PictureString { get; set; }
    }
}
