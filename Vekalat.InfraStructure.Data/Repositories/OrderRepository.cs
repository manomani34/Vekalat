using Vekalat.Application.Features;
using Vekalat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using static Vekalat.Application.Features.OrderFeature;

namespace Vekalat.InfraStructure.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly VekalatDataContext _context;
        public OrderRepository(VekalatDataContext context) : base(context)
        {
            _context = context;
        }



        public IQueryable<OrderDto> GetOrder(int userId, int orderTypeId, string Mobil, int Id, CancellationToken cancellationToken)
        {
            var result = _context.Orders.AsNoTracking();

            if (Id != 0)
            {
                result.Where(a => a.Id == Id).AsQueryable();
            }

            if (userId != 0)
            {
                result.Where(a => a.UserId == userId).AsQueryable();
            }

            if (orderTypeId != 0)
            {
                result = result.Where(c => c.OrderTypeid == orderTypeId);
            }

            return result.OrderByDescending(c => c.Id).Select(c => new OrderFeature.OrderDto()
            {
                Id = c.Id,
                Authority = c.Authority,
                Discount = c.Discount,
                //Userid = c.Userid,
                OrderDate = c.OrderDate,
                OrderTypeid = c.OrderTypeid,
                SendCost = c.SendCost,
                SendType = c.SendType,
                SubTotal = c.SubTotal,
                Total = c.Total,
                //Mobil = c.User.Mobil,
                //FirstName = c.User.FirstName,
                //LastName = c.User.LastName,
            }); ;
        }

    }
}
