
namespace Auction.Domain.Abstractions
{
    public interface IConverter<T,T2> where T : class where T2 : class
    {
        Task<T2> Convert(T obj);
    }      

}

