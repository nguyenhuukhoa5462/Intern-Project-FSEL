using APIOder.DatabaseContext;
using APIOder.Models;
using APIOder.Repositories.IRepo;
using APIOder.ViewModel.OderDetailViewModel;
using Microsoft.EntityFrameworkCore;

namespace APIOder.Repositories.Repo
{
    public class OderDetailRepo : IOderDetailRepo
    {
        private readonly OderDBContext _context;
        private readonly IOderRepo _oderRepo;


        public OderDetailRepo(OderDBContext context, IOderRepo oderRepo)
        {
            _context = context;
            _oderRepo = oderRepo;
        }
        public async Task<int> Create(CreateOderDetail create)
        {
            try
            {
                var oder = await _context.Oders.FindAsync(create.OderId);
                if (oder == null) return 0;
                var checkoderdetail = await _context.OderDetails.FirstOrDefaultAsync(p => p.OderId == create.OderId && p.ProductName == create.ProductName && p.UnitPrice == create.UnitPrice);
                if (checkoderdetail != null)
                {
                    checkoderdetail.Quantity += create.Quantity;
                    _context.OderDetails.Update(checkoderdetail);
                    await _context.SaveChangesAsync();
                    await _oderRepo.UpdateTotalPrice(create.OderId);
                    return 1;
                }

                OderDetail oderDetail = new OderDetail()
                {
                    Id = Guid.NewGuid(),
                    OderId = create.OderId,
                    ProductName = create.ProductName,
                    Quantity = create.Quantity,
                    UnitPrice = create.UnitPrice,
                };
                await _context.OderDetails.AddAsync(oderDetail);
                await _context.SaveChangesAsync();
                await _oderRepo.UpdateTotalPrice(create.OderId);
                return 2;
            }
            catch (Exception ex)
            {
                return 3;
            }
        }

        public async Task<List<OderDetail>> GetByIdOder(Guid Id)
        {
            return await _context.OderDetails.Where(p => p.OderId == Id).ToListAsync();
        }
    }
}
