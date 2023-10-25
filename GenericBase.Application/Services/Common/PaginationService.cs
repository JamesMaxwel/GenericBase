using GenericBase.Application.Interfaces.Common;
using GenericBase.Application.Services.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GenericBase.Application.Services.Common
{
    public class PaginationService : IPaginatonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaginationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IList<T>> ToPageAsync<T>(IQueryable<T> items, int pageNumber, int pageSize)
        {
            int totalItems = await items.CountAsync();
            var pagenationMetaData = new PagenationMetaData()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                HasPrevious = pageNumber > 0
            };
            pagenationMetaData.HasNext = pagenationMetaData.CurrentPage < pagenationMetaData.TotalPages;
            string json = JsonConvert.SerializeObject(pagenationMetaData);
            _httpContextAccessor.HttpContext!.Response.Headers.Add("X-Pagination", json);
            return await items.Skip(pageNumber * pageSize - pageSize).Take(pageSize).ToListAsync();
        }
    }
}
