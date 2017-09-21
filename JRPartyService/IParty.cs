using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JRPartyData;
using JRPartyService.DataContracts;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IParty”。
    [ServiceContract]
    public interface IParty
    {
        #region 任务发布 

        /*-----------------新增任务提醒---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addPlanAlarm?id={id}&alarmTime={alarmTime}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput addPlanAlarm(string id, string alarmTime);
        /*-----------------查看任务提醒---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPlanAlarm?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> getPlanAlarm(string id);

        /*-----------------一级查看任务（执行中的任务）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPlanList?offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> getPlanList(int offset, int limit, string order, string search, string sort);
        /*-----------------二级查看任务（执行中的任务）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get2PlanList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> get2PlanList(string districtID,int offset, int limit, string order, string search, string sort);
        /*-----------------三级查看任务（执行中的任务）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get3PlanList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> get3PlanList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------新增任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addPlan?month={month}&content={content}&title={title}&districtID={districtID}&type={type}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addPlan(string month, string content, string title, string districtID, string type);
        /*-----------------编辑任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editPlan?id={id}&month={month}&content={content}&title={title}&districtID={districtID}&type={type}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editPlan(string id, string month, string content, string title, string districtID, string type);
        /*-----------------取消任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deletePlan?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deletePlan(string id);

        /*-----------------新增任务附带文件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddActivity?activityID={activityID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddActivity(string activityID, string Url);

        /*-----------------删除任务附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteActivityFile?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteActivityFile(string id);
        #endregion

        #region 任务管理
        /*-----------------查看单条任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivityDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<PAR_ActivityOne> getActivityDetail(string id);

        /*-----------------查询任务(一级单位)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivityList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> getActivityList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查询任务(一级单位跟踪)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getSubActivityList?id={id}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_S<PAR_SubActivityList[]> getSubActivityList(string id, int offset, int limit, string order, string search, string sort);
        /*-----------------查看任务详情---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivityPicture?districtID={districtID}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_Picture_Infro[]> getActivityPicture(string districtID, string id);
        /*-----------------查询任务(二级单位)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get2ActivityList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> get2ActivityList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查询任务(二级单位跟踪)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get2SubActivityList?id={id}&districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_S<PAR_SubActivityList[]> get2SubActivityList(string id, string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查询任务(三级单位)---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "get3ActivityList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_Three<PAR_Activity3List[]> get3ActivityList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看管理员发布图片---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPictureByActivity?districtID={districtID}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<ActivityPicture[]> getPictureByActivity(string districtID, string id);

        /*-----------------待审核任务列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getCheckActivityList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_CheckActivityList[]> getCheckActivityList(string districtID, int offset, int limit, string order, string search, string sort);

        /*-----------------任务审核 status=2---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "checkActivity?id={id}&districtID={districtID}&IsOK={IsOK}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput checkActivity(string id, string districtID, string IsOK);

        /*-----------------任务附件下载---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivityFile?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<string[]> getActivityFile(string id);

        #endregion

        #region 任务子系统（茅山党建）

        /*-----------------查看通知---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getSubPlanList?offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_MsgPush[]> getSubPlanList(int offset, int limit, string order, string search, string sort);
        /*-----------------新增任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addSubPlan?title={title}&content={content}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addSubPlan(string title, string content);
        /*-----------------编辑任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editSubPlan?id={id}&title={title}&content={content}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editSubPlan(string id, string title, string content);
        /*-----------------取消任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteSubPlan?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteSubPlan(string id);
        /*-----------------查看推送对象列表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getObjectList?id={id}&districtID={districtID}&offset={offset}&limit={limit}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_MsgPush[]> getObjectList(string id, string districtID, int offset, int limit);

        #endregion

        #region 考核汇总

        /*-----------------查看所有任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getActivity?districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_Activity[]> getActivity(string districtID);

        /*-----------------任务完成一览表---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getStat?districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_MT<string[]> getStat(string districtID);

        /*-----------------统计查看---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getStatPie?districtID={districtID}&id={id}", ResponseFormat = WebMessageFormat.Json)]
        Stat_Activityone1 getStatPie(string districtID, string id);


        #endregion

        #region 预警系统

        /*-----------------开始线程---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "start2?", ResponseFormat = WebMessageFormat.Json)]
        void start2();
        #endregion

        #region 其他

        //------获取服务器当前时间------
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getDateTime", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> getDateTime();

        #endregion

    }
}
