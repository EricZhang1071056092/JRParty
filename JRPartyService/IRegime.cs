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
    public interface IRegime
    {
        #region 基本制度

        /*-----------------查看基本制度---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getRegimeList?offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<RegimeList[]> getRegimeList(int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getRegimeDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<REG_Detail> getRegimeDetail(string id);
        /*-----------------查看单条制度附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getRegimeFiles?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string[]> getRegimeFiles(string id);
        /*-----------------新增基本制度---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addRegime?name={name}&description={description}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addRegime(string name, string description);
        /*-----------------编辑基本制度---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editRegime?id={id}&name={name}&description={description}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editRegime(string id, string name, string description);
        /*-----------------取消基本制度---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteRegime?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteRegime(string id);
        /*-----------------新增基本制度附带文件（同任务图片一个表）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddRegimePicture?regimeID={regimeID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddRegimePicture(string regimeID, string Url);

        /*-----------------删除基本制度附带文件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteRegimePicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteRegimePicture(string id);
        #endregion
    }
}
