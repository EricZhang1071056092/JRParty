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
    public interface IBrand
    {
        #region 党建品牌 
        /*-----------------单条党建品牌---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getsingleBrand?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<BrandDetail> getsingleBrand(string id);
        /*-----------------单条党建品牌附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getSBFiles?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string[]> getSBFiles(string id);
        /*-----------------查看党建品牌---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getBrandList?districtID={districtID}&offset={offset}&limit={limit}&order={order}&search={search}&sort={sort}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutPutT_M<List_Brand[]> getBrandList(string districtID, int offset, int limit, string order, string search, string sort);
        /*-----------------新增党建品牌---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "addBrand?title={title}&description={description}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutputT<string> addBrand(string title, string description, string districtID);
        /*-----------------编辑党建品牌---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "editBrand?id={id}&title={title}&description={description}&districtID={districtID}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput editBrand(string id, string title, string description, string districtID);
        /*-----------------取消党建品牌---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteBrand?id={id}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteBrand(string id);
        /*-----------------新增党建品牌附带文件（同任务图片一个表）---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AddBrandPicture?BrandID={BrandID}&Url={Url}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput AddBrandPicture(string BrandID, string Url);

        /*-----------------删除党建品牌附件---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "deleteBrandPicture?id={id}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput deleteBrandPicture(string id);
        /*-----------------党建品牌评级---------*/
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "rateBrand?id={id}&rate={rate}", ResponseFormat = WebMessageFormat.Json)]
        CommonOutput rateBrand(string id, string rate);
        #endregion
    }
}
