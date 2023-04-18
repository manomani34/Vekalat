using AutoMapper;
using Vekalat.Application.Common.Helpers;
using Vekalat.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Vekalat.Application.Features.EquipmentFeature;
using static Vekalat.Application.Features.StudioFeature;

namespace Vekalat.Application.Features
{
    public class BasketFeature
    {
        #region CQRS

        #region Create Basket
        public class CreateBasketCommand : IRequest
        {
            public int UserId { get; set; }
            public BasketItemType ItemType { get; set; }
            public int ItemId { get; set; }
            public ISession Session { get; set; }
        }

        public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IStudioRepository _studioRepository;
            private readonly IMapper _mapper;
            public CreateBasketCommandHandler(IEquipmentRepository equipmentRepository, IStudioRepository studioRepository, IMapper mapper)
            {
                _equipmentRepository = equipmentRepository;
                _studioRepository = studioRepository;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
            {
                var basketId = $"BK-{request.UserId}";
                if (SessionHelper.GetObjectFromJson<Basket>(request.Session, basketId) == null)
                {
                    var basket = new Basket
                    {
                        BasketId = basketId,
                        UserId = request.UserId,
                        BasketItems = new List<BasketItem>()
                    };

                    if (request.ItemType == BasketItemType.Equipment)
                    {
                        var equipment = await _equipmentRepository.GetById(request.ItemId);
                        var mappedEquipment = _mapper.Map<EquipmentForBasketDto>(equipment);
                        basket.BasketItems.Add(
                            new BasketItem
                            {
                                BasketItemType = BasketItemType.Equipment,
                                ItemId = request.ItemId,
                                Equipment = mappedEquipment,
                                Quantity = 1,
                                Studio = null
                            }
                            );
                    }

                    if (request.ItemType == BasketItemType.Studio)
                    {
                        var studio = await _studioRepository.GetById(request.ItemId);
                        var mappedStudio = _mapper.Map<StudioForBasketDto>(studio);
                        basket.BasketItems.Add(
                            new BasketItem
                            {
                                BasketItemType = BasketItemType.Studio,
                                ItemId = request.ItemId,
                                Equipment = null,
                                Quantity = 1,
                                Studio = mappedStudio
                            }
                            );
                    }
                    //if (product.DiscountId != null)
                    //{
                    //    product.DiscountFk = await _discountRepository.GetDiscountByEquipmentId(product.Id);
                    //}



                    basket = CalculateTotalAmount(basket);
                    SessionHelper.SetObjectAsJson(request.Session, basketId, basket);
                }
                else
                {
                    var basket = SessionHelper.GetObjectFromJson<Basket>(request.Session, basketId);
                    int index = IsExist(basketId, request.ItemId, request.Session);
                    if (index != -1)
                    {
                        basket.BasketItems[index].Quantity++;
                    }
                    else
                    {
                        if (request.ItemType == BasketItemType.Equipment)
                        {
                            var equipment = await _equipmentRepository.GetById(request.ItemId);
                            var mappedEquipment = _mapper.Map<EquipmentForBasketDto>(equipment);
                            basket.BasketItems.Add(
                                new BasketItem
                                {
                                    BasketItemType = BasketItemType.Equipment,
                                    ItemId = request.ItemId,
                                    Equipment = mappedEquipment,
                                    Quantity = 1,
                                    Studio = null
                                }
                                );
                        }

                        if (request.ItemType == BasketItemType.Studio)
                        {
                            var studio = await _studioRepository.GetById(request.ItemId);
                            var mappedStudio = _mapper.Map<StudioForBasketDto>(studio);
                            basket.BasketItems.Add(
                                new BasketItem
                                {
                                    BasketItemType = BasketItemType.Studio,
                                    ItemId = request.ItemId,
                                    Equipment = null,
                                    Quantity = 1,
                                    Studio = mappedStudio
                                }
                                );
                        }
                        //if (product.DiscountId != null)
                        //{
                        //    product.DiscountFk = await _discountRepository.GetDiscountByEquipmentId(product.Id);
                        //}
                    }
                    basket = CalculateTotalAmount(basket);
                    SessionHelper.SetObjectAsJson(request.Session, basketId, basket);
                }
                return Unit.Value;
            }
        }

        #endregion

        //#region Applay Voucher
        //public class ApplayVoucherCommand : IRequest
        //{
        //    public string BasketId { get; set; }
        //    public string Code { get; set; }
        //    public ISession Session { get; set; }
        //}

        //public class ApplayVoucherCommandHandler : IRequestHandler<ApplayVoucherCommand>
        //{
        //    private readonly IVoucherRepository _voucherRepository;

        //    public ApplayVoucherCommandHandler(IVoucherRepository voucherRepository)
        //    {
        //        _voucherRepository = voucherRepository;
        //    }

        //    public async Task<Unit> Handle(ApplayVoucherCommand request, CancellationToken cancellationToken)
        //    {
        //        var basket = SessionHelper.GetObjectFromJson<Basket>(request.Session, request.BasketId);
        //        if (basket != null)
        //        {
        //            var voucher = await _voucherRepository.GetVoucherByCode(request.Code);
        //            if (voucher == null) throw new WebAppException(HttpStatusCode.NotFound, new KeyValuePair<string, string>(nameof(Messages.EntityNotFound), Messages.EntityNotFound));

        //            basket.Voucher = voucher.Code;
        //            basket.VoucherAmount = voucher.Amount;

        //            basket = CalculateTotalAmount(basket);
        //            SessionHelper.SetObjectAsJson(request.Session, request.BasketId, basket);
        //        }
        //        return Unit.Value;
        //    }
        //}

        //#endregion


        #region Edit Basket

        public class EditBasketCommand : IRequest
        {
            public Basket Basket { get; set; }
            public ISession Session { get; set; }
        }

        public class EditBasketCommandHandler : IRequestHandler<EditBasketCommand>
        {

            public async Task<Unit> Handle(EditBasketCommand request, CancellationToken cancellationToken)
            {
                var basketId = $"BK-{request.Basket.UserId}";
                var basket = SessionHelper.GetObjectFromJson<Basket>(request.Session, basketId);

                foreach (var item in basket.BasketItems)
                {

                    if (item.BasketItemType == BasketItemType.Equipment)
                    {
                        var equipment = request.Basket.BasketItems.SingleOrDefault(c => c.ItemId == item.Equipment.Id);
                        if (equipment == null) continue;
                        item.Quantity = equipment.Quantity;
                    }

                    if (item.BasketItemType == BasketItemType.Studio)
                    {
                        var studio = request.Basket.BasketItems.SingleOrDefault(c => c.ItemId == item.Studio.Id);
                        if (studio == null) continue;
                        item.Quantity = studio.Quantity;
                    }
                    
                }

                //basket.PaymentType = request.Basket.PaymentType;
                basket = CalculateTotalAmount(basket);
                SessionHelper.SetObjectAsJson(request.Session, basketId, basket);
                await Task.CompletedTask;
                return Unit.Value;
            }


        }

        #endregion

        #region Delete Basket

        public class DeleteBasketCommand : IRequest
        {
            public int UserId { get; set; }
            public BasketItemType ItemType { get; set; }
            public int ItemId { get; set; }
            public ISession Session { get; set; }
        }

        public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
        {

            public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
            {
                var basketId = $"BK-{request.UserId}";

                var basket = SessionHelper.GetObjectFromJson<Basket>(request.Session, basketId);
                int index = IsExist(basketId, request.ItemId, request.Session);
                basket.BasketItems.RemoveAt(index);
                basket = CalculateTotalAmount(basket);
                if (basket.BasketItems.Count == 0)
                    SessionHelper.RemoveObject(request.Session, basketId);
                else
                    SessionHelper.SetObjectAsJson(request.Session, basketId, basket);
                await Task.CompletedTask;
                return Unit.Value;
            }
        }

        #endregion

        #region Clear Basket

        public class ClearBasketCommand : IRequest
        {
            public int UserId { get; set; }
            public ISession Session { get; set; }
        }

        public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand>
        {
            public async Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
            {
                var basketId = $"BK-{request.UserId}";
                SessionHelper.RemoveObject(request.Session, basketId);
                await Task.CompletedTask;
                return Unit.Value;
            }
        }

        #endregion


        #region Get Basket Detail

        public class GetBasketDetailQuery : IRequest<Basket>
        {
            public int UserId { get; set; }
            public ISession Session { get; set; }
        }

        public class GetBasketDetailQueryHandler : IRequestHandler<GetBasketDetailQuery, Basket>
        {
            public async Task<Basket> Handle(GetBasketDetailQuery request, CancellationToken cancellationToken)
            {
                var basketId = $"BK-{request.UserId}";

                return await Task.FromResult(SessionHelper.GetObjectFromJson<Basket>(request.Session, basketId));
            }
        }

        #endregion

        public static int IsExist(string basketId, int itemId, ISession session)
        {
            var cart = SessionHelper.GetObjectFromJson<Basket>(session, basketId);
            for (int i = 0; i < cart.BasketItems.Count; i++)
            {
                if (cart.BasketItems[i].ItemId.Equals(itemId))
                {
                    return i;
                }
            }
            return -1;
        }

        public static Basket CalculateTotalAmount(Basket basket)
        {
            foreach (var item in basket.BasketItems)
            {
                if (item.BasketItemType == BasketItemType.Equipment)
                {
                    item.PriceWithDiscount = item.Equipment.Price;
                }
                if (item.BasketItemType == BasketItemType.Studio)
                {
                    item.PriceWithDiscount = item.Studio.Price;
                }
                //if (item.Equipment.DiscountFk != null)
                //{
                //    item.Discount = item.Equipment.DiscountFk.PercentValue;
                //    item.PriceWithDiscount = item.Equipment.Price
                //        .ToCalculateDiscount(item.Equipment.DiscountFk.PercentValue);
                //}
            }
            basket.TotalAmount = basket.BasketItems.Sum(c => c.Quantity * c.PriceWithDiscount);
            basket.TotalDiscount = basket.BasketItems.Sum(c => c.Discount);
            basket.TotalAmount -= basket.TotalDiscount;
            basket.TotalAmount -= basket.VoucherAmount;
            return basket;
        }


        #endregion


        #region Serialization

        public class BasketItem
        {
            public EquipmentForBasketDto Equipment { get; set; }
            public StudioForBasketDto Studio { get; set; }
            public BasketItemType BasketItemType { get; set; }
            public int ItemId { get; set; }
            public int Quantity { get; set; } = 0;
            public decimal PriceWithDiscount { get; set; } = 0.0M;
            public decimal Discount { get; set; } = 0.0M;
        }

        public class Basket
        {
            public string BasketId { get; set; } = string.Empty;
            public int UserId { get; set; }
            public int PaymentType { get; set; } = 1;
            public decimal TotalDiscount { get; set; } = 0.0M;
            public string Voucher { get; set; } = string.Empty;
            public decimal VoucherAmount { get; set; } = 0.0M;
            public decimal TotalAmount { get; set; } = 0.0M;
            public List<BasketItem> BasketItems { get; set; }
        }

        #endregion



    }
}
