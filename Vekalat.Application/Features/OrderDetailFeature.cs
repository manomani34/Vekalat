using Vekalat.Application.Common;
using Vekalat.Core.Errors;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Core.Entities;
using Application.Common.Dto.Paging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vekalat.Application.Features
{
    public class OrderDetailFeature
    {
     
                    
        #region Get OrderDetails ByOrderId

        public class GetOrderDetailsByOrderId : IRequest<OrderDetail>
        {
            public int OrderID { get; set; }

        }
        public class GetOrderDetailsByOrderIdHandler : IRequestHandler<GetOrderDetailsByOrderId, OrderDetail>
        {

            private readonly IOrderDetailRepository _repository;

            public GetOrderDetailsByOrderIdHandler(IOrderDetailRepository repository)
            {
                _repository = repository;
            }

            public async Task<OrderDetail> Handle(GetOrderDetailsByOrderId request, CancellationToken cancellationToken)
            {
                return await _repository.GetById(request.OrderID);
            }
        }

        #endregion


        #region Create OrderDetail

        public class CreateCommand : IRequest
        {
            public OrderDetail item { get; set; }

        }
        public class CreateCommandHandler : IRequestHandler<CreateCommand>
        {
            private readonly IOrderDetailRepository _repository;

            public CreateCommandHandler(IOrderDetailRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(CreateCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    await _repository.InsertNew(request.item);
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Edit OrderDetail

        public class EditCommand : IRequest
        {
            public OrderDetail item { get; set; }

        }
        public class EditCommandHandler : IRequestHandler<EditCommand>
        {
            private readonly IOrderDetailRepository _repository;


            public EditCommandHandler(IOrderDetailRepository repository)
            {
                _repository = repository;
            }
            public async Task<Unit> Handle(EditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.Update(request.item);
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Delete OrderDetail
        public class DeleteCommand : IRequest
        {
            public int Id { get; set; }
        }

        public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
        {
            private readonly IOrderDetailRepository _repository;

            public DeleteCommandHandler(IOrderDetailRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
            {

                var item = await _repository.GetById(request.Id);
                if (item == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>("Not Found", "یافت نشد"));
                try
                {
                    await _repository.Delete(request.Id);
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Error", "خطای ارتباط با سرور"));
                }
            }
        }
        #endregion

        #region Mapper And Repo

        public interface IOrderDetailRepository : IRepository<OrderDetail>
        {
            //IQueryable<OrderDetailsDto> GetOrderDetailsList(int orderID);
        }

        public class OrderDetailsProfile : Profile
        {

        }

        #endregion


        #region Serialization

        public class OrderDetailDto
        {
            public int OrderID { get; set; }
            [ForeignKey("OrderID")]
            public virtual Order Order { get; set; }
            //public int ProductID { get; set; }
            //[ForeignKey("ProductID")]
            //public virtual Product Product { get; set; }
            public double UnitPrice { get; set; }
            public short Quantity { get; set; }
            public string OrderDate { get; set; }
            public int Radif { get; set; }
        }



        #endregion


    }
}
