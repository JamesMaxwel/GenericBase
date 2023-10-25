namespace GenericBase.Application.Services.Common.Utils
{
    public class PagenationParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize)
                ? MaxPageSize : (value <= 0) ? _pageSize : value;
        }
    }
}
