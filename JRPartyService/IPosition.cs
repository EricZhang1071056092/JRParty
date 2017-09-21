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
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ITissue”。
    [ServiceContract]
    public interface IPosition
    {

        #region 基本阵地 

        /*-----------------查看基本阵地（市、镇）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPositionList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Position[]> getPositionList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看基本阵地(村)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get3PositionList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Position[]> get3PositionList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条基本阵地---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPositionDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<POS_Detail> getPositionDetail(string id);
        /*-----------------新增基本阵地---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addPosition?type={type}&description={description}&area={area}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addPosition(string type, string description, string area, string districtID);
        /*-----------------编辑基本阵地---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editPosition?id={id}&type={type}&description={description}&area={area}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editPosition(string id, string type, string description, string area, string districtID);
        /*-----------------取消基本阵地---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deletePosition?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deletePosition(string id);
        /*-----------------新增基本阵地附带文件（同任务图片一个表）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddPositionPicture?positionID={positionID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddPositionPicture(string positionID, string Url);

        /*-----------------删除任务附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deletePositionPicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deletePositionPicture(string id);
        /*-----------------基本阵地审核---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "checkPosition?id={id}&districtID={districtID}&IsOK={IsOK}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput checkPosition(string id, string districtID, string IsOK);
        /*-----------------查看待审核基本阵地---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getcheckPositionList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Position[]> getcheckPositionList(string districtID, int offset, int limit, string order, string search, string sort);
        #endregion

    }
}
