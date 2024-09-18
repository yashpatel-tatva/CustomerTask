namespace CustomerTask.Services.IServices
{
    public interface ICustomerService
    {
        Task<T> GetAllAsync<T>(); 
        Task<T> GetAsync<T>(string acNo);
        Task<T> CreateAsync<T>(Customer dto);
        Task<T> UpdateAsync<T>(Customer dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
