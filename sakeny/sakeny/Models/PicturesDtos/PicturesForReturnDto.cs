using System.ComponentModel.DataAnnotations.Schema;

namespace sakeny.Models.PicturesDtos
{
    public class PicturesForReturnDto
    {
        public decimal PostPicId { get; set; }

        
        public decimal? PostId { get; set; }

        
        public byte[]? Picture { get; set; }
    }
}
