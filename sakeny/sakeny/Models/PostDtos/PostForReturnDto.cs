﻿using sakeny.Entities;
using sakeny.Models.PicturesDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace sakeny.Models.PostDtos
{
    public class PostForReturnDto
    {
        public int PostId { get; set; }

        public DateTime? PostDate { get; set; }

        public TimeSpan? PostTime { get; set; }

        public string? PostCategory { get; set; } // what do you mean

        public string? PostTitle { get; set; }

        public string? PostBody { get; set; }

        public decimal? PostArea { get; set; }

        public int? PostKitchens { get; set; }

        public int? PostBedrooms { get; set; }

        public int? PostBathrooms { get; set; }

        public bool? PostLookSea { get; set; }

        public bool? PostPetsAllow { get; set; }

        public string? PostCurrency { get; set; }

        public decimal? PostPriceAi { get; set; }

        public decimal? PostPriceDisplay { get; set; }

        public string? PostPriceType { get; set; }

        public string? PostAddress { get; set; }

        public string? PostCity { get; set; }

        public string? PostState { get; set; }

        public int? PostFloor { get; set; }

        public string? PostLatitude { get; set; } // location

        public string? PostLongitude { get; set; } // 

        public bool? PostStatue { get; set; } // unavailable

        public decimal PostUserId { get; set; }

        public List<PicturesForReturnDto> PostPics { get; set; } = new List<PicturesForReturnDto>();



        //[InverseProperty("Post")]
        public virtual ICollection<PicturesForReturnDto> images { get; set; } = new List<PicturesForReturnDto>();

        public virtual ICollection<PostFeedbackForReturnDto> feedbacks { get; set; } = new List<PostFeedbackForReturnDto>();


    }
}
