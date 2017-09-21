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
    public interface IDynamic
    {
        #region 登录注册

        /*-------------APP推送信息------------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "sendMessage?phone={phone}&flag={flag}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_Code> sendMessage(string phone, string flag);

        /*-------------组织列表------------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appGetOrganization?", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_Organs[]> appGetOrganization();

        /*-------------APP注册------------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "userRegist?phone={phone}&password={password}&orgination={orgination}&name={name}&IDCard={IDCard}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_Login> userRegist(string phone, string password, string orgination, string name, string IDCard);
        
        /*-----------------APP登录---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appLogin?Account={Account}&PassWord={PassWord}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_Login> appLogin(string Account, string PassWord);
        

        /*-----------------APP获取轮播信息---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appGetInformation?", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_Information[]> appGetInformation();

        /*-----------------APP获取新闻列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appGetInformationList?PageIndex={PageIndex}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_InformationList[]> appGetInformationList(string PageIndex);

        /*-----------------APP获取即将过期活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appGetWarningPara?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<PAR_WarningPara[]> appGetWarningPara(string id);
        #endregion

        #region 任务管理

        /*-----------------APP获取任务列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appGetActivityList?userId={userId}&PageIndex={PageIndex}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_ActivityList[]> appGetActivityList(string userId, string PageIndex);

        /*-----------------APP任务签到---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appActivitySign?userId={userId}&snId={snId}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputApp appActivitySign(string userId, string snId);

        /*-----------------APP任务上报---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appAddActivity?userId={userId}&snId={snId}&content={content}&flag={flag}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<string> appAddActivity(string userId, string snId, string content, string flag);


        /*-----------------任务材料补充上报（超管权限）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "replenishActivity?districtID={districtID}&snId={snId}&content={content}&flag={flag}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<string> replenishActivity(string districtID, string snId, string content, string flag);
        /*-----------------任务提醒---------*/

        /*-----------------任务执行---------*/

        #endregion

        #region 个人中心

        /*-----------------版本更新---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appAddVersion?versionNum={versionNum}&flag={flag}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputApp appAddVersion(string versionNum, string flag);

        /*-----------------版本信息---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getVersion?", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<string> getVersion();

        /*-----------------软件反馈---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "appAddFeedback?clientID={clientID}&districtID={districtID}&feedbackContent={feedbackContent}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputApp appAddFeedback(string clientID, string districtID, string feedbackContent);

        /*-------------保存头像------------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "savePortrait?clientID={clientID}&ImageUrl={ImageUrl}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<string> savePortrait(string clientID, string ImageUrl);

        /*-----------------我参与的任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "myActivityList?userId={userId}&PageIndex={PageIndex}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputAppT<App_ActivityList[]> myActivityList(string userId, string PageIndex);
        #endregion
        
    }
}
