using APIOder.DatabaseContext;
using APIOder.Models;
using APIOder.Repositories.IRepo;
using APIOder.ViewModel.OderViewModel;
using Microsoft.EntityFrameworkCore;

namespace APIOder.Repositories.Repo
{
    public class OderRepo : IOderRepo
    {
        private readonly OderDBContext _context;

        public OderRepo(OderDBContext context)
        {
            _context = context;
        }

        public async Task<List<Oder>> GetAll()
        {
            return await _context.Oders.ToListAsync();
        }
        public async Task<Oder> GetById(Guid Id)
        {
            var oder = await _context.Oders.FindAsync(Id);
            return oder;
        }

        public async Task<ViewOder> GetByIdOder(Guid Id)
        {
            var oder = await _context.Oders.FindAsync(Id);
            if (oder == null) return null;
            var listoderdetail = await _context.OderDetails.Where(p => p.OderId == Id).ToListAsync();

            ViewOder view = new ViewOder();
            ViewModelOder oderobj = new ViewModelOder();

            List<ViewModelOderDetail> lstOderDetail = new List<ViewModelOderDetail>();
            oderobj.Id = oder.Id;
            oderobj.IdCustomer = oder.IdCustomer;
            oderobj.OderDate = oder.OderDate;
            oderobj.TotalPrice = oder.TotalPrice;
            view.OderObj = oderobj;
            if(listoderdetail.Count == 0)
            {
                view.OderObj.ListOderDetail = null;
            }
            else
            {
                foreach (var item in listoderdetail)
                {
                    ViewModelOderDetail detail = new ViewModelOderDetail();
                    detail.Id = item.Id;
                    detail.OderId = item.OderId;
                    detail.Quantity = item.Quantity;
                    detail.UnitPrice = item.UnitPrice;
                    detail.ProductName = item.ProductName;
                    lstOderDetail.Add(detail);
                };
                view.OderObj.ListOderDetail = lstOderDetail;
            }
            
            return view;
        }
        public async Task<List<Oder>> GetByIdKhachHang(Guid Id)
        {
            return await _context.Oders.Where(p => p.IdCustomer == Id).ToListAsync();
        }
        public async Task<string> Create(CreateOder create)
        {
            try
            {
                Oder oder = new Oder()
                {
                    Id = Guid.NewGuid(),
                    IdCustomer = create.IdCustomer,
                    OderDate = DateTime.Now,
                    TotalPrice = 0,
                };
                await _context.Oders.AddAsync(oder);
                await _context.SaveChangesAsync();
                try
                {
                    foreach (var item in create.OderDetail)
                    {
                        OderDetail oderDetail = new OderDetail()
                        {
                            Id = Guid.NewGuid(),
                            OderId = oder.Id,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                        };
                        await _context.OderDetails.AddAsync(oderDetail);
                        await _context.SaveChangesAsync();
                    }
                    await UpdateTotalPrice(oder.Id);
                    return oder.Id.ToString();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task UpdateTotalPrice(Guid IdOder)
        {
            var listOderDeTail = await _context.OderDetails.Where(p => p.OderId == IdOder).ToListAsync();
            decimal total = 0;
            foreach (var item in listOderDeTail)
            {
                total += item.UnitPrice * item.Quantity;
            }
            var oder = await GetById(IdOder);
            oder.TotalPrice = total;
            _context.Oders.Update(oder);
            await _context.SaveChangesAsync();
        }
    }
}
