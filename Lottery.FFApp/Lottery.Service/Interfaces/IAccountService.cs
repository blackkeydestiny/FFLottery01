﻿using System;
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

namespace Lottery.Service
{
    public interface IAccountService
    {
        
        /// <summary>
        /// 会员充值
        /// </summary>
        /// <param name="model">充值信息</param>
        /// <returns>账户余额</returns>
        UserChargeResultModel Charge(UserChargeModel model);

        /// <summary>
        /// 会员取款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserWithdrawResultModel Withdraw(UserWithdrawModel model);

        /// <summary>
        /// 会员投注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserBetResultModel Betting(UserBetModel model);
    }
}