using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.FFModel.Account
{
    public class UserBetModel
    {
        /// <summary>
        /// 第三方唯一充值订单号, 可为空
        /// </summary>
        [Description("第三方唯一充值订单号, 可为空")]
        public string OrderId { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>
        [Description("商户Id")]
        public string MerchantId { get; set; }

        /// <summary>
        /// 会员用户名
        /// </summary>
        [Description("会员用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 签名字符串, 按顺序(下注单号&商户Id&会员用户名)MD5加密串
        /// </summary>
        [Description("签名字符串, 按顺序(下注单号&商户Id&会员用户名)MD5加密串")]
        public string SignKey { get; set; }

        /// <summary>
        /// 一注彩票的ID
        /// </summary>
        [Description("下注单ID")]
        public string SsId { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        [Description("彩种ID")]
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法ID
        /// </summary>
        [Description("玩法ID")]
        public int PlayId { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [Description("投注金额")]
        public decimal Price { get; set; }

        /// <summary>
        /// 投注金额单位
        /// </summary>
        [Description("投注金额单位")]
        public string PriceName { get; set; }

        /// <summary>
        /// 投注次数
        /// </summary>
        [Description("投注次数")]
        public int Times { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public decimal Num { get; set; }

        /// <summary>
        /// 单奖获奖金额
        /// </summary>
        [Description("单奖获奖金额")]
        public decimal SingelBouns { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [Description("倍数")]
        public int Point { get; set; }

        /// <summary>
        /// 下注号码
        /// </summary>
        [Description("下注号码")]
        public string Balls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string StrPos { get; set; }

        /// <summary>
        /// 玩法名称
        /// </summary>
        [Description("玩法名称")]
        public string PlayName { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Description("总金额")]
        public decimal Alltotal { get; set; }
    }
}
