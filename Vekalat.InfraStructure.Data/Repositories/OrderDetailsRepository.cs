using Vekalat.Core.Entities;
using static Vekalat.Application.Features.OrderDetailFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly VekalatDataContext _context;
        public OrderDetailRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }

        //public IQueryable<OrderDetailsFeature.OrderDetailsDto> GetOrderDetailsList(int orderID)
        //{
        //    var result = _context.OrderDetails.AsNoTracking().Where(o => o.OrderID == orderID).AsQueryable();
        //    return result.OrderByDescending(c => c.OrderID).Select(c => new OrderDetailsFeature.OrderDetailsDto()
        //    {
        //         BookInfo = c.BookInfo,
        //         OrderID = c.OrderID,
        //         UnitPrice = c.UnitPrice,
        //         Radif = c.Radif,
        //         OrderDate = c.OrderDate,   
        //         Product = c.Product,
        //         ProductID = c.ProductID,
        //         Quantity = c.Quantity,
        //         Order = c.Order,
        //    });
        //}
    }
}
