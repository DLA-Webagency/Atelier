using System.Linq.Expressions;

namespace Tennis.Repository
{
    public interface IReadJson<T> 
    {
        Task<IEnumerable<T>?> GetAllPlayers();
        Task<T?> Get(int id);
        Task<(string Country, double IMC, double Mediane)> GetStatistic();
    }
}
