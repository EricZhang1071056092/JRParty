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
    public interface ITelevision
    {
        /*-----------------查看支部信息---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getInformation?number={number}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> getInformation(string number);
        /*-----------------抓图---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPicture?number={number}&StudyContent={StudyContent}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> getPicture(string number, string StudyContent);
        
        /*-----------------查看支部信息---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getPictureCapture?districtName={districtName}&StudyContent={StudyContent}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> getPictureCapture(string districtName, string StudyContent);

        /*-----------------查看支部任务详情---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTopBoxActivityList?districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityList[]> getTopBoxActivityList(string districtID);

        /*-----------------左侧栏查看执行情况---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTopBoxActivityListByDistrict?districtName={districtName}&PageIndex={PageIndex}", ResponseFormat = WebMessageFormat.Json)]
        TV_BoxActivity<string[], string[], string[]> getTopBoxActivityListByDistrict(string districtName, string PageIndex);

        /*-----------------根据月份查看任务---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTopBoxActivityListByMonth?districtName={districtName}&month={month}&PageIndex={PageIndex}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<PAR_ActivityComplete[]> getTopBoxActivityListByMonth(string districtName, string month, string PageIndex);

        /*-----------------获取监控信息---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getTopBoxInfro?number={number}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<TV_TopBox> getTopBoxInfro(string number);
    }
}
