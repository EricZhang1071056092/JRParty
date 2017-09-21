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
    public interface ISystem
    {

        /*--------------------登录------------------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "accountLogin?username={username}&password={password}", ResponseFormat = WebMessageFormat.Json)]
        LoginAccess<PAR_WarningPara[]> accountLogin(string username, string password);


        /*-----------------查询组织树---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOrganTree?", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_Organ<string[]>[]> getOrganTree();
        #region 用户管理

        /*-----------------查询用户列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getUserList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<SYS_UserList[]> getUserList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------新增用户---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addUser?name={name}&phone={phone}&password={password}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput addUser(string name, string phone, string password, string districtID);
        /*-----------------编辑用户---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editUser?id={id}&name={name}&phone={phone}&password={password}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editUser(string id, string name, string phone, string password, string districtID);
        /*-----------------查询区域---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOrgan?districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<App_Organs[]> getOrgan(string districtID);
        /*-----------------查询区域（本级以及下级）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getOrgan2?districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<App_Organs[]> getOrgan2(string districtID);
        /*-----------------登出用户---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteUser?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteUser(string id);

        #endregion

        #region 党员管理

        /*-----------------新增党员---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addPolitic?name={name}&IDCard={IDCard}&phone={phone}&sex={sex}&nation={nation}&workPlace={workPlace}&educationDegree={educationDegree}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput addPolitic(string name, string IDCard, string phone, string sex, string nation, string workPlace, string educationDegree, string districtID);
        /*-----------------编辑党员---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editPolitic?id={id}&name={name}&IDCard={IDCard}&phone={phone}&sex={sex}&nation={nation}&workPlace={workPlace}&educationDegree={educationDegree}&districtID={districtID}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editPolitic(string id, string name, string IDCard, string phone, string sex, string nation, string workPlace, string educationDegree, string districtID);
        /*-----------------查询党员列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPoliticList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<POP_Basic[]> getPoliticList(string districtID, int offset, int limit, string order, string search);
        /*-----------------登出党员---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deletePolitic?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deletePolitic(string id);
        /*-----------------查看党员上传活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivityByPolitic?districtID={districtID}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<ActivityPicture[]> getActivityByPolitic(string districtID, string id);

        #endregion


        #region 监控管理

        /*-----------------查看监控---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCameraList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_Picutre[]> getCameraList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------新增监控---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addCamera?name={name}&districtID={districtID}&IP={IP}&number={number}&time={time}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput addCamera(string name, string districtID, string IP,string number, string time);
        /*-----------------编辑监控---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editCamera?id={id}&districtID={districtID}&name={name}&IP={IP}&number={number}&time={time}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editCamera(string id, string districtID, string name, string IP, string number, string time);

        /*-----------------删除监控---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteCamera?id={id}&password={password}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteCamera(string id, string password);

        /*-----------------截图参数设置---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editPictureCatchInfo?time={time}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editPictureCatchInfo(string time);


        /*-----------------查看监控截图时间---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPictureCatchInfo?", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<string> getPictureCatchInfo();

        #endregion
    }
}
