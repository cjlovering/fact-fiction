namespace FactOrFictionTextHandling.MLClient
{
    using System.Threading.Tasks;

    public interface IMLClient<T> where T: IMLResult
    {
        Task<T> Query(string textEntry);
    }
}
