using Application.Common.Dto.Paging;
using AutoMapper;
using Vekalat.Application.Common;
using Vekalat.Application.Common.InfraServices;
using Vekalat.Application.Enums;
using Vekalat.Core.Entities;
using Vekalat.Core.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Vekalat.Application.Features
{
    public class OrderFeature
    {

        #region Get Order Query

        public class GetOrderQuery : IRequest<List<OrderDto>>
        {
            public int MemberId { get; set; }
            public OrderTypeEnum Type { get; set; }
            public string Mobil { get; set; }
            public int Id { get; set; }
        }
        public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, List<OrderDto>>
        {
            private readonly IOrderRepository _repository;
            private readonly IPagerService<OrderDto, OrderDto> _pager;

            public GetOrderQueryHandler(IOrderRepository repository)
            {
                _repository = repository;

            }
            public async Task<List<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                var item = await Task.FromResult(_repository.GetOrder(request.MemberId, (int)request.Type, request.Mobil, request.Id, cancellationToken));
                return item.ToList();
            }
        }
        #endregion

        #region Get Order Query By Paging
        public class GetOrderQueryByPaging : IRequest<PagingHandler<OrderDto>>
        {
            public int MemberId { get; set; }
            public int PageId { get; set; }
            public OrderTypeEnum Type { get; set; }
            public string Mobil { get; set; }
            public int Id { get; set; }
        }
        public class GetOrderQueryByPagingHandler : IRequestHandler<GetOrderQueryByPaging, PagingHandler<OrderDto>>
        {
            private readonly IOrderRepository _repository;
            private readonly IPagerService<OrderDto, OrderDto> _pager;

            public GetOrderQueryByPagingHandler(IOrderRepository repository, IPagerService<OrderDto, OrderDto> pager)
            {
                _repository = repository;
                _pager = pager;
            }
            public async Task<PagingHandler<OrderDto>> Handle(GetOrderQueryByPaging request, CancellationToken cancellationToken)
            {
                const int take = 10;
                var item = await Task.FromResult(_repository.GetOrder(request.MemberId, (int)request.Type, request.Mobil, request.Id, cancellationToken));
                var pager = _pager.PageBuilder(item.Count(), request.PageId, take);
                return await _pager.SetItems(item, pager);
            }
        }

        #endregion

        #region Create Order

        public class CreateCommand : IRequest
        {
            public Order item { get; set; }

        }
        public class CreateCommandHandler : IRequestHandler<CreateCommand>
        {
            private readonly IOrderRepository _repository;

            public CreateCommandHandler(IOrderRepository repository)
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

        #region Edit 

        public class EditCommand : IRequest
        {
            public Order item { get; set; }

        }
        public class EditCommandHandler : IRequestHandler<EditCommand>
        {
            private readonly IOrderRepository _repository;


            public EditCommandHandler(IOrderRepository repository)
            {
                _repository = repository;
            }
            public async Task<Unit> Handle(EditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _repository.Update(request.item);
                    await _repository.SaveChangesAsync();
                    return Unit.Value;
                }
                catch
                {
                    throw new WebAppException(HttpStatusCode.InternalServerError, new KeyValuePair<string, string>("Internal Server Error", "خطا رخ داده است"));
                }
            }
        }

        #endregion

        #region Delete Order
        public class DeleteCommand : IRequest
        {
            public int Id { get; set; }
        }

        public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
        {
            private readonly IOrderRepository _repository;

            public DeleteCommandHandler(IOrderRepository repository)
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
                    await _repository.SaveChangesAsync();
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
        public interface IOrderRepository : IRepository<Order>
        {
            public IQueryable<OrderDto> GetOrder(int memberId, int orderTypeId, string Mobil, int Id, CancellationToken cancellationToken);
        }

        public class OrderProfile : Profile
        {
            public OrderProfile()
            {
                CreateMap<OrderDto, Order>();
                CreateMap<OrderDto, Order>().ReverseMap();
            }
        }
        #endregion

        #region Serialization

        public class OrderDto
        {
            public int Id { get; set; }
            public int Memberid { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Mobil { get; set; }
            public DateTime OrderDate { get; set; }
            public int? OrderTypeid { get; set; }
            public double? SubTotal { get; set; }
            public double? Discount { get; set; }
            public double? Total { get; set; }
            public int? SendCost { get; set; }
            public int? SendType { get; set; }
            public string Authority { get; set; }
        }
        #endregion
    }
}

