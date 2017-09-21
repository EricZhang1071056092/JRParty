using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JRPartyData;
//using cn.jpush.api;
//using cn.jpush.api.push.mode;
using JRPartyService.DataContracts;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IParty”。
    [ServiceContract]
    public interface IInformation
    {

        #region 通知公告

        /*-----------------新增通知公告---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addInformation?title={title}&description={description}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput addInformation(string title, string description, string districtID);
        /*-----------------编辑通知公告---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editInformation?id={id}&title={title}&description={description}&districtID={districtID}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editInformation(string id, string title, string description, string districtID);
        /*-----------------查询通知公告列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getInformationList?districtID={districtID}&offset={offset}&limit={limit}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_information[]> getInformationList(string districtID, int offset, int limit);
        /*-----------------登出通知公告---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteInformation?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteInformation(string id);

        /*-----------------发起公告推送---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "infPushIn?districtId={districtId}&title={title}&objs={objs}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput infPushIn(string districtId, string title, string objs);

        /*-----------------推送目标列表---------*/
        [WebInvoke(Method = "GET", UriTemplate = "getObjList", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<PushObject> getObjList();

        /*-----------------推送消息列表---------*/
        [WebInvoke(Method = "GET", UriTemplate = "getPushList?districtId={districtId}&offset={offset}&limit={limit}&order={order}&sort={sort}&search={search}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<InfPush[]> getPushList(string districtId, int offset, int limit, string order, string sort, string search);

        /*-----------------接收公告推送---------*/
        [WebInvoke(Method = "GET", UriTemplate = "infPushCheck?districtId={districtId}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<InfPush[]> infPushCheck(string districtId);


        #endregion

    }
}
