using Microsoft.EntityFrameworkCore;

namespace PokemonApp.Models.PokemonDto
{
    public record QueryForPaginatedReesult(uint PageNumber = 1, uint PageSize = 10);
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public uint PageNumber { get; set; }
        public uint TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;


        public PagedResult(List<T> items, int count, uint pageNumber, uint pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (uint)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }


        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, uint pageNumber = 1, uint pageSize = 10)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(((int)pageNumber - 1) * (int)pageSize)
                .Take((int)pageSize)
                .ToListAsync();

            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
