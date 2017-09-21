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
    public interface ITissue
    {
        #region 组织机构

        /*-----------------查看组织---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTissueList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Tissue[]> getTissueList(string districtID, int offset, int limit, string order, string search, string sort);

        /*-----------------查看组织2---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTissueList2?districtId={districtId}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<TissueList2> getTissueList2(string districtId);

        /*-----------------新增组织---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addTissue?leaderName={leaderName}&populationNum={populationNum}&tissueName={tissueName}&description={description}&type={type}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addTissue(string leaderName, string populationNum, string tissueName, string description, string type, string districtID);
        /*-----------------编辑组织---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editTissue?id={id}&leaderName={leaderName}&populationNum={populationNum}&tissueName={tissueName}&description={description}&type={type}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editTissue(string id, string leaderName, string populationNum, string tissueName, string description, string type, string districtID);
        /*-----------------取消组织---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteTissue?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteTissue(string id);
        #endregion
        #region 领导班子
        /*-----------------查看领导班子---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getLeadershipList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}" +
            "&AFName={AFName}&AFNation={AFNation}&AFSex={AFSex}&AFDuty={AFDuty}&AFEducation={AFEducation}&AFBirthday={AFBirthday}&AFJionTime={AFJionTime}&AFWorkTime={AFWorkTime}&AFType={AFType}&AFFinancialType={AFFinancialType}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<TIS_LeadingShip[]> getLeadershipList(string districtID, int offset, int limit, string order, string search, string sort, string AFName, string AFNation, string AFSex, string AFDuty, string AFEducation, string AFBirthday, string AFJionTime, string AFWorkTime, string AFType, string AFFinancialType);
        /*-----------------新增领导班子---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addLeadership?name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addLeadership(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------编辑领导班子---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editLeadership?id={id}&name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editLeadership(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------取消领导班子---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteLeadership?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteLeadership(string id);
        /*-----------------待审核列表领导班子---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCheckLeadershipList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<TIS_LeadingShip[]> getCheckLeadershipList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------领导班子审核---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "checkActivity?id={id}&districtID={districtID}&IsOK={IsOK}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput checkActivity(string id, string districtID, string IsOK);
        #endregion
    }
}
