namespace OrderWebsite.Application.Abstractions.IServices
{
    public interface IConverterService<Object1, Object2> where Object1 : class where Object2 : class
    {
        Task<Object2> ConvertAsync(Object1 obj);
    }

}

