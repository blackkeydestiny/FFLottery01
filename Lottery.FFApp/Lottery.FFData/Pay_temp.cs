//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lottery.FFData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pay_temp
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string PayCode { get; set; }
        public string PayRequestId { get; set; }
        public Nullable<decimal> PayAmount { get; set; }
        public string PaySTime { get; set; }
        public string IpsRequestId { get; set; }
        public string IpsCompleteSTime { get; set; }
        public string PayState { get; set; }
        public string PayBankInfo { get; set; }
    }
}