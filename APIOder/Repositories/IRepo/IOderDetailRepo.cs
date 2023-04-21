using APIOder.Models;
using APIOder.ViewModel.OderDetailViewModel;

namespace APIOder.Repositories.IRepo
{
    public interface IOderDetailRepo
    {
        Task<List<OderDetail>> GetByIdOder(Guid Id);
        Task<int> Create(CreateOderDetail create);
    }
}
