namespace sakeny.Models.PostDtos
{
    
        public class PostForSerchDto
        {
            public string? PostTitle { get; set; }
            public string? PostAddress { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
            public decimal? MinArea { get; set; }
            public decimal? MaxArea { get; set; }
            public int? MinRooms { get; set; }
            public int? MaxRooms { get; set; }
            public int? MinBathrooms { get; set; }
            public int? MaxBathrooms { get; set; }
            //public int? MinGarages { get; set; }
            //public int? MaxGarages { get; set; }
            public int? MinFloors { get; set; }
            public int? MaxFloors { get; set; }
        public bool? PostLookSea { get; set; }

        public bool? PostPetsAllow { get; set; }


        //public int? MinYearBuilt { get; set; }
        //public int? MaxYearBuilt { get; set; }
        //public decimal? MinRating { get; set; }
        //public decimal? MaxRating { get; set; }
    }




}
