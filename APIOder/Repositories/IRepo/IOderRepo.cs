using APIOder.Models;
using APIOder.ViewModel.OderViewModel;

namespace APIOder.Repositories.IRepo
{
    public interface IOderRepo
    {
        Task<string> Create(CreateOder create);
        Task<List<Oder>> GetAll();
        Task<Oder> GetById(Guid Id);
        Task<List<Oder>> GetByIdKhachHang(Guid Id);
        Task UpdateTotalPrice(Guid IdOder);
        Task<ViewOder> GetByIdOder(Guid Id);
    }
}
