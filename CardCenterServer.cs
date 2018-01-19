using System;


using DigitalPlatform.Interfaces;
using DigitalPlatform.Text;

namespace CardChargesServerDemo
{
    public class CardCenterServer : MarshalByRefObject, ICardCenter
    {
        /// <summary>
        /// 从卡中心扣款
        /// </summary>
        /// <param name="strRecord">账户标识，一般为读者 barcode </param>
        /// <param name="strPriceString">扣款金额字符串,单位：元。一般为类似“CNY12.00”这样的形式</param>
        /// <param name="strPassword">账户密码</param>
        /// <param name="strRest">扣款后的余额，单位：元，格式为 货币符号+金额数字部分，形如“CNY100.00”标识人民币 100元</param>
        /// <param name="strError">错误信息</param>
        /// <returns>
        /// <para>-2  密码不正确</para>
        /// <para>-1  出错(调用出错等特殊原因)</para>
        /// <para>0   扣款不成功(因为余额不足等普通原因)。注意 strError 中应当返回不成功的原因</para>
        /// <para>1   扣款成功</para>
        /// </returns>
        public int Deduct(string strRecord,
            string strPriceString,
            string strPassword,
            out string strRest,
            out string strError)
        {
            strRest = "";
            strError = "";

            if (strRecord.Trim().Substring(0, 1) == "<")
            {
                strError = "参数'strRecord'值不正确，不是正确的卡号";
                return -1;
            }

            // 第一个流程 扣款金额字符串 为空，需返回 账户余额
            if (String.IsNullOrEmpty(strPriceString) == true)
            {
                strRest = "";
                return 1;
            }

            // 处理带前缀的价格字符串
            string strPrefix = ""; // 金额字符串前缀
            string strValue = ""; // 金额字符串数字部分
            string strPostfix = ""; // 金额字符串猴子
            int nRet = PriceUtil.ParsePriceUnit(strPriceString,
                out strPrefix,
                out strValue,
                out strPostfix,
                out strError);
            if (nRet == -1)
                return -1;



            // 扣款成功，返回扣款后的余额
            strRest = strPrefix + "0.00";
            return 1;
        }




        // 以下两个接口与扣费无关，可不实现

        // 获得一条读者记录
        // parameters:
        //      strID   读者记录标识符号。用什么字段作为标识，Client和Server需要另行约定
        //      strRecord   读者XML记录
        //                  注：读者记录中的某些字段卡中心可能缺乏对应字段，那么需要在XML记录中填入 <元素名 dprms:missing />，这样不至于造成同步时图书馆读者库中的这些字段被清除。至于读者借阅信息等字段，则不必操心
        // return:
        //      -1  出错(调用出错等特殊情况)
        //      0   读者记录不存在(读者记录正常性不存在)
        //      1   成功返回读者记录
        public int GetPatronRecord(string strID, 
            out string strRecord, 
            out string strError)
        {
            throw new NotImplementedException();
        }


        // 获得若干读者记录
        // parameters:
        //      strPosition 第一次调用前，需要将此参数的值清为空
        //      records 读者XML记录字符串数组。注：读者记录中的某些字段卡中心可能缺乏对应字段，那么需要在XML记录中填入 <元素名 dprms:missing />，这样不至于造成同步时图书馆读者库中的这些字段被清除。至于读者借阅信息等字段，则不必操心
        // return:
        //      -1  出错
        //      0   正常获得一批记录，但是尚未获得全部
        //      1   正常获得最后一批记录
        public int GetPatronRecords(ref string strPosition, 
            out string[] records, 
            out string strError)
        {
            throw new NotImplementedException();
        }
    }
}
