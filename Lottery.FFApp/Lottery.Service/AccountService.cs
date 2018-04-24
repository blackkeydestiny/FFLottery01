using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Lottery.FFModel;
using Lottery.Core;
using Lottery.FFData;
using System.Data;
using Lottery.Utils;
using Lottery.DAL;
using Lottery.FFModel.Account;
using Lottery.DAL.Flex;

namespace Lottery.Service
{
    public class AccountService : BaseService, IAccountService
    {
        /// <summary>
        /// 会员充值
        /// </summary>
        /// <param name="model">充值信息</param>
        /// <returns>账户余额</returns>
        public UserChargeResultModel Charge(UserChargeModel model)
        {
            using (var dbContext = new TicketEntities())
            {
                if (string.IsNullOrEmpty(model.MerchantId) && string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.SignKey))
                {
                    throw new InvalidOperationException("无效的用户登录信息");
                }

                if (model.Amount <= 0)
                {
                    throw new InvalidOperationException("无效的充值金额");                    
                }

                //1, 判断用户是否存在
                var merchantEntity = dbContext.N_Merchant.FirstOrDefault(it => (it.MerchantId.Equals(model.MerchantId, StringComparison.OrdinalIgnoreCase)));

                if (merchantEntity == null)
                {
                    Log.Error("商户不存在");
                    throw new InvalidOperationException("商户不存在");
                }

                if (string.IsNullOrEmpty(merchantEntity.Code))
                {
                    Log.Error("无效的商户信息");
                    throw new InvalidOperationException("无效的商户信息");
                }

                //2,验证用户
                var userEntity = dbContext.N_User.FirstOrDefault(it => it.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase));

                if (userEntity == null)
                {
                    Log.Error("用户不存在");
                    throw new InvalidOperationException("用户不存在");
                }

                //3, 验证加密串
                //按顺序(商户Id&会员用户名&商户安全码)MD5加密串
                var signKey = MD5Cryptology.GetMD5(string.Format("{0}&{1}&{2}&{3}&{4}", model.OrderId, model.MerchantId, model.UserName, model.Amount.ToString("f4"), merchantEntity.Code), "gb2312");
                if (string.Compare(signKey, model.SignKey, true) != 0)
                {
                    Log.Error("无效的商户安全码");
                    throw new InvalidOperationException("无效的商户安全码");
                }

                //4, 支付
                string orderId = SsId.Charge;
                UserChargeResultModel result = new UserChargeResultModel()
                {
                    SsId = orderId,
                    OrderId = model.OrderId,
                    MerchantId = model.MerchantId,
                    UserName = model.UserName,
                    Money = userEntity.Money ?? 0M + model.Amount,
                    BeforeMoney = userEntity.Money ?? 0M
                };

                int num = (new DAL.UserChargeDAL()).Save3(orderId, model.OrderId, userEntity.Id.ToString(), model.MerchantId, model.Amount);

                if (new DAL.Flex.UserChargeDAL().Update(orderId) == false)
                {
                    Log.ErrorFormat("充值失败，订单号: {0}", orderId);
                    throw new InvalidOperationException("充值失败");
                }
                else
                {
                    Log.InfoFormat("充值成功，订单号: {0}", orderId);
                }

                return result;
            }
        }

        /// <summary>
        /// 会员取现
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserWithdrawResultModel Withdraw(UserWithdrawModel model)
        {
            using (var dbContext = new TicketEntities())
            {
                if (string.IsNullOrEmpty(model.MerchantId) && string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.SignKey))
                {
                    throw new InvalidOperationException("无效的用户登录信息");
                }

                if (model.Amount <= 0)
                {
                    throw new InvalidOperationException("无效的取现金额");
                }

                //1, 判断用户是否存在
                var merchantEntity = dbContext.N_Merchant.FirstOrDefault(it => (it.MerchantId.Equals(model.MerchantId, StringComparison.OrdinalIgnoreCase)));

                if (merchantEntity == null)
                {
                    Log.Error("商户不存在");
                    throw new InvalidOperationException("商户不存在");
                }

                if (string.IsNullOrEmpty(merchantEntity.Code))
                {
                    Log.Error("无效的商户信息");
                    throw new InvalidOperationException("无效的商户信息");
                }

                //2,验证用户
                var userEntity = dbContext.N_User.FirstOrDefault(it => it.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase));

                if (userEntity == null)
                {
                    Log.Error("用户不存在");
                    throw new InvalidOperationException("用户不存在");
                }

                //3, 验证加密串
                //按顺序(商户Id&会员用户名&商户安全码)MD5加密串
                var signKey = MD5Cryptology.GetMD5(string.Format("{0}&{1}&{2}&{3}&{4}", model.OrderId, model.MerchantId, model.UserName, model.Amount.ToString("f4"), merchantEntity.Code), "gb2312");
                if (string.Compare(signKey, model.SignKey, true) != 0)
                {
                    Log.Error("无效的商户安全码");
                    throw new InvalidOperationException("无效的商户安全码");
                }

                //4, 取现
                string orderId = SsId.GetCash;
                UserWithdrawResultModel result = new UserWithdrawResultModel()
                {
                    SsId = orderId,
                    OrderId = model.OrderId,
                    MerchantId = model.MerchantId,
                    UserName = model.UserName,
                    Money = (userEntity.Money - model.Amount) ?? 0M,
                    BeforeMoney = userEntity.Money ?? 0M
                };

                // 获取user bank id
                var userBank = dbContext.N_UserBank.FirstOrDefault(it => it.PayAccount.Equals(userEntity.PayAccount, StringComparison.OrdinalIgnoreCase));
                if (userBank == null)
                {
                    Log.Error("无效的银行账号");
                    throw new InvalidOperationException("无效的银行账号");
                }

                int num = (new UserGetCashDAL()).Save3Withdraw(orderId, userEntity.Id.ToString(), model.OrderId, userBank.Id.ToString(), userEntity.PayBank, userEntity.PayAccount, userEntity.PayName, model.Amount);

                if (num == 0)
                {
                    Log.ErrorFormat("取现失败，订单号: {0}", orderId);
                    throw new InvalidOperationException("取现失败");
                }
                else
                {
                    new LogSysDAL().Save("会员管理", "Id为" + userEntity.Id.ToString() + "的会员申请提现！");
                    Log.InfoFormat("取现成功，订单号: {0}", orderId);
                }
                return result;
            }
        }

        /// <summary>
        /// 会员投注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserBetResultModel Betting(UserBetModel model)
        {
            using (var dbContext = new TicketEntities())
            {
                if (string.IsNullOrEmpty(model.MerchantId) && string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.SignKey))
                {
                    throw new InvalidOperationException("无效的用户登录信息");
                }

                //1, 判断商户是否存在
                var merchantEntity = dbContext.N_Merchant.FirstOrDefault(it => (it.MerchantId.Equals(model.MerchantId, StringComparison.OrdinalIgnoreCase)));

                if (merchantEntity == null)
                {
                    Log.Error("商户不存在");
                    throw new InvalidOperationException("商户不存在");
                }

                if (string.IsNullOrEmpty(merchantEntity.Code))
                {
                    Log.Error("无效的商户信息");
                    throw new InvalidOperationException("无效的商户信息");
                }

                //2,验证用户,此处通过userId查询
                var userEntity = dbContext.N_User.FirstOrDefault(it => it.Id.ToString().Equals(model.UserName, StringComparison.OrdinalIgnoreCase));

                if (userEntity == null)
                {
                    Log.Error("此商户下不存在此用户");
                    throw new InvalidOperationException("此商户下不存在此用户");
                }

                //3, 验证加密串
                //按顺序(商户Id&会员用户名&商户安全码)MD5加密串
                var signKey = MD5Cryptology.GetMD5(string.Format("{0}&{1}&{2}", model.MerchantId, userEntity.UserName, merchantEntity.Code), "gb2312");
                if (string.Compare(signKey, model.SignKey, true) != 0)
                {
                    Log.Error("无效的商户安全码");
                    throw new InvalidOperationException("无效的商户安全码");
                }

                string orderId = SsId.Bet;
                model.SsId = orderId;
                UserBetResultModel result = new UserBetResultModel()
                {
                    SsId = orderId,
                    OrderId = model.OrderId,
                    MerchantId = model.MerchantId,
                    UserName = userEntity.UserName,
                    Balls = model.Balls
                };

                // 获取玩法
                var playSmallTypeEntity = dbContext.Sys_PlaySmallType.FirstOrDefault(it => it.Id.ToString().Equals(model.PlayId.ToString(), StringComparison.OrdinalIgnoreCase));
                if (playSmallTypeEntity == null)
                {
                    Log.Error("无效的彩种玩法");
                    throw new InvalidOperationException("无效的彩种玩法");
                }

                MerchantUserBetDAL betDal = new MerchantUserBetDAL();
                int num2 = 0;
                DateTime STime = DateTime.Now;

                num2 = !model.StrPos.Equals("") ?
                    betDal.InsertBetPos(model, "Web端", STime, userEntity.Id.ToString(), playSmallTypeEntity.Title2) :
                    (playSmallTypeEntity.Title2.Equals("P_5ZH") || playSmallTypeEntity.Title2.Equals("P_4ZH_L") || (playSmallTypeEntity.Title2.Equals("P_4ZH_R") || playSmallTypeEntity.Title2.Equals("P_3ZH_L")) || (playSmallTypeEntity.Title2.Equals("P_3ZH_C") || playSmallTypeEntity.Title2.Equals("P_3ZH_R")) ?
                    betDal.InsertBetZH(model, "Web端", STime, userEntity.Id.ToString(), playSmallTypeEntity.Title2) :
                    betDal.InsertBet(model, "Web端", STime, userEntity.Id.ToString(), playSmallTypeEntity.Title2));

                string[] issueTimeAndSn = betDal.GetIssueTimeAndSN(model.LotteryId);
                if (issueTimeAndSn.Length <= 0)
                {
                    Log.ErrorFormat("无效的彩种");
                    throw new InvalidOperationException("无效的彩种");
                }
                string str1 = issueTimeAndSn[0];

                if (num2 > 0)
                {
                    Log.InfoFormat("第{0}期投注成功，请期待开奖！订单号: {1}", str1, orderId);
                }
                else
                {
                    Log.ErrorFormat("对不起,投注失败！");
                    throw new InvalidOperationException("对不起,投注失败！");
                }

                return result;
            }
        }
    }
}