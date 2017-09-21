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
    public interface ITeam
    {
        #region 发展党员（会议）

        /*-----------------查看会议---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTeamList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Conference[]> getTeamList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条会议---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTeamDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<CON_Detail> getTeamDetail(string id);
        /*-----------------查看单条会议附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTDFiles?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string[]> getTDFiles(string id);
        /*-----------------新增会议---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addTeam?title={title}&description={description}&participation={participation}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addTeam(string title, string description, string participation, string districtID);
        /*-----------------编辑会议---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editTeam?id={id}&title={title}&description={description}&participation={participation}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editTeam(string id, string title, string description, string participation, string districtID);
        /*-----------------取消会议---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteTeam?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteTeam(string id);
        /*-----------------新增会议附带文件（同任务图片一个表）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddTeamPicture?TeamID={TeamID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddTeamPicture(string TeamID, string Url);
        /*-----------------删除会议附带文件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteTeamPicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteTeamPicture(string id);
        #endregion

        #region 党员志愿者

        /*-----------------查看党员志愿者---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getVolunteerList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<TIS_LeadingShip[]> getVolunteerList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条党员志愿者---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getVolunteerDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<VOL_Detail> getVolunteerDetail(string id);
        /*-----------------新增党员志愿者---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addVolunteer?name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addVolunteer(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------编辑党员志愿者---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editVolunteer?id={id}&name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editVolunteer(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------取消党员志愿者---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteVolunteer?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteVolunteer(string id);
        /*-----------------新增党员志愿者头像---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddVolunteerPicture?VolunteerID={VolunteerID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddVolunteerPicture(string VolunteerID, string Url);
        /*-----------------删除党员志愿者头像---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteVolunteerPicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteVolunteerPicture(string id);
        #endregion

        #region 志愿者活动

        /*-----------------查看志愿者活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getVolunteerActivityList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_VolunteerActivity[]> getVolunteerActivityList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条志愿者活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getVolunteerActivityDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<VOLACT_Detail> getVolunteerActivityDetail(string id);
        /*-----------------新增志愿者活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addVolunteerActivity?description={description}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addVolunteerActivity(string description, string districtID);
        /*-----------------编辑志愿者活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editVolunteerActivity?id={id}&description={description}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editVolunteerActivity(string id, string description, string districtID);
        /*-----------------取消志愿者活动---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteVolunteerActivity?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteVolunteerActivity(string id);
        /*-----------------新增志愿者活动图片---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddVolunteerActivityPicture?VolunteerID={VolunteerID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddVolunteerActivityPicture(string VolunteerID, string Url);
        /*-----------------删除志愿者活动图片---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteVolunteerActivityPicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteVolunteerActivityPicture(string id);
        #endregion

        #region 后备干部队伍

        /*-----------------查看后备干部---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getReserveCadreList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<TIS_LeadingShip[]> getReserveCadreList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------查看单条后备干部---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getReserveCadreDetail?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<VOL_Detail> getReserveCadreDetail(string id);
        /*-----------------新增后备干部---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addReserveCadre?name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addReserveCadre(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------编辑后备干部---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editReserveCadre?id={id}&name={name}&IDCard={IDCard}&sex={sex}&nation={nation}&birthDay={birthDay}&JionTime={JionTime}&workTime={workTime}&duty={duty}&education={education}&TrainingTitle={TrainingTitle}&type={type}&phone={phone}&financialType={financialType}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editReserveCadre(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID);
        /*-----------------取消后备干部---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteReserveCadre?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteReserveCadre(string id);
        /*-----------------新增后备干部头像---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddReserveCadrePicture?VolunteerID={VolunteerID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddReserveCadrePicture(string VolunteerID, string Url);
        /*-----------------删除后备干部头像---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteReserveCadrePicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteReserveCadrePicture(string id);
        #endregion
    }
}
