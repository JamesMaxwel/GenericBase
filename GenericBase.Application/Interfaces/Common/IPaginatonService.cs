namespace GenericBase.Application.Interfaces.Common
{
    public interface IPaginatonService
    {
        Task<IList<T>> ToPageAsync<T>(IQueryable<T> items, int pageNumber, int pageSize);
    }
}
