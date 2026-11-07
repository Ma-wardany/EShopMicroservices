
using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService (DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
                coupon = new Coupon { ProductName = request.ProductName, Description = "No Discount", Amount = 0 };

            logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;

        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if(coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data."));
            

            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if(coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data."));

            var existingCoupon = await dbContext.Coupons.FindAsync(coupon.Id);
            if (existingCoupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon for {coupon.ProductName} not found."));

            dbContext.Coupons.Update(existingCoupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount updated for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();


        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon for {request.ProductName} not found."));

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
