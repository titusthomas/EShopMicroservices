using Discount.Grpc.Data;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbcontext,ILogger<DiscountService> logger):DiscountProtoService.DiscountProtoServiceBase
    {
        public override async  Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invlid Request"));
            }
            await dbcontext.AddAsync<Coupon>(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation($"Discount coupon created {coupon.ProductName}");
            return coupon.Adapt<CouponModel>();            
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Discount with productname:{request.ProductName} not found"));
            }

            dbcontext.Coupons.Remove(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation($"Successfully deleted discount coupon {coupon.ProductName}");
            return new DeleteDiscountResponse(){ Success=true};
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
            {
                coupon=new Coupon() { ProductName="No Discount",Amount=0,Description="No discount"};
            }
            logger.LogInformation($"Discount retrieved for Product:{coupon.ProductName} Amount:{coupon.Amount}");
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invlid Request"));
            }
            dbcontext.Update(coupon);
            await dbcontext.SaveChangesAsync();
            logger.LogInformation($"Discount coupon updated {coupon.ProductName}");
            return coupon.Adapt<CouponModel>();
        }
    }
}
