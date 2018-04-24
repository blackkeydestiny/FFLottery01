using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.FFModel.Account
{
    /// <summary>
    /// 会员下注结果
    /// </summary>
    [Description("会员下注结果")]
    public class UserBetResultModel
    {
        /// <summary>
        /// 第三方唯一下注单号, 可为空
        /// </summary>
        [Description("第三方唯一下注号, 可为空")]
        public string OrderId { get; set; }

        /// <summary>
        /// 非凡充值记录Id
        /// </summary>
        [Description("非凡下注Id")]
        public string SsId { get; set; }

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
        /// 下注号码
        /// </summary>
        [Description("下注号码")]
        public string Balls { get; set; }
    }
}
