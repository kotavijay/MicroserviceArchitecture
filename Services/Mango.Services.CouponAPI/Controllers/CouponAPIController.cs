using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public CouponAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto Get()
        {
            try
            {
                var coupons = _dbContext.Coupons.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto GetCouponById(int id)
        {
            try
            {
                var coupon = _dbContext.Coupons.FirstOrDefault(c => c.CouponId == id);

                if (coupon == null)
                    _responseDto.IsSuccess = false;

                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet]
        [Route("{couponCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto GetCouponByCode(string couponCode)
        {
            try
            {
                var coupon = _dbContext.Coupons.FirstOrDefault(c => c.CouponCode.Equals(couponCode));

                if (coupon == null)
                    _responseDto.IsSuccess = false;

                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto CreateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);

                _dbContext.Coupons.Add(coupon);
                _dbContext.SaveChanges();

                CouponDto updatedCoupon = _mapper.Map<CouponDto>(coupon);

                _responseDto.Result = updatedCoupon;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);

                _dbContext.Coupons.Update(coupon);
                _dbContext.SaveChanges();

                CouponDto updatedCoupon = _mapper.Map<CouponDto>(coupon);

                _responseDto.Result = updatedCoupon;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                Coupon coupon = _dbContext.Coupons.FirstOrDefault(c => c.CouponId == id);

                if (coupon == null)
                {
                    _responseDto.IsSuccess = false;
                    return _responseDto;
                }

                _dbContext.Coupons.Remove(coupon);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

    }
}
