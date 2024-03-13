﻿namespace sakeny.Services
{
    public class PaginationMetadata
    {
        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            TotalPageCount = (int) Math.Ceiling(totalItemCount / (double)pageSize);
            PageSize = pageSize;
            CurrentPage = currentPage;
        }

        public int TotalItemCount { get; set; }

        public int TotalPageCount { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

    }
}
