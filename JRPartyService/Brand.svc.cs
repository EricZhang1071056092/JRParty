using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using JRPartyService.Methods;
using System.ServiceModel.Activation;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Brand : IBrand
    {
        #region 党建品牌 
        /*-----------------单条党建品牌---------*/
        public CommonOutputT<BrandDetail> getsingleBrand(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<BrandDetail> returnData = new CommonOutputT<BrandDetail>();
            try
            {
                var   x = (from a in ds.PAR_Brand
                                           join b in ds.SYS_District on a.districtID equals b.id into b1
                                           from b2 in b1.DefaultIfEmpty()
                                           where a.id == id 
                                           select new BrandDetail
                                           {
                                               id = a.id,
                                               districtName = b2.districtName,
                                               districtID = b2.id,
                                               title = a.title,
                                               description = a.description,
                                               releaseTime = a.releaseTime,
                                               rate = a.rate,
                                               imageURL = (from b in ds.PAR_ActivityReleaseFile where a.id == b.activityID select b).ToArray()
                                           }).ToArray(); ;
                returnData.success = true;
                returnData.message = "Success";
                returnData.data = x[0];
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------单条党建品牌附件---------*/
        public CommonOutputT<string[]> getSBFiles(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string[]> result = new CommonOutputT<string[]>();
            try
            {
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };

                var x = (from a in ds.PAR_ActivityReleaseFile
                         where a.activityID == id && !imgSuffix.Contains(a.Url.Substring(a.Url.Length - 3, 3))
                         select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + a.Url).ToArray();
                result.success = true;
                result.message = "success";
                result.data = x;
            }
            catch (Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }
        /*-----------------查看党建品牌---------*/
        public CommonOutPutT_M<List_Brand[]> getBrandList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Brand[]> returnData = new CommonOutPutT_M<List_Brand[]>();
            var subDistrict = CommonMethod.getSubDistrict(districtID);

            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };

                IQueryable<List_Brand> x = null;
                x = from a in ds.PAR_Brand
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where subDistrict.Contains(a.districtID) && (a.title.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search) || a.rate.Contains(search) || b2.districtName.Contains(search))
                    select new List_Brand
                    {
                        id = a.id,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        title = a.title,
                        description = a.description,
                        releaseTime = a.releaseTime,
                        rate = a.rate,
                        imageURL = (from b in ds.PAR_ActivityReleaseFile
                                    where a.id == b.activityID && imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray()
                    };
                switch (sort)
                {
                    case "title":
                        x = x.OrderBy(c => c.title);
                        break;
                    case "description":
                        x = x.OrderBy(c => c.description);
                        break;
                    case "releaseTime":
                        x = x.OrderBy(c => c.releaseTime);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    case "rate":
                        x = x.OrderBy(c => c.rate);
                        break;
                    default:
                        x = x.OrderBy(c => c.town);
                        break;
                }
                
                returnData.total = x.ToArray<List_Brand>().Length;
                x = x.Skip<List_Brand>(offset);
                x = x.Take<List_Brand>(limit);
                returnData.rows = x.ToArray<List_Brand>();
                if (order == "desc") returnData.rows = returnData.rows.Reverse().ToArray();
                returnData.success = true;
                returnData.message = "Success";
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
            }
            ds.Dispose();
            return returnData;
        }
        /*-----------------新增党建品牌---------*/
        public CommonOutputT<string> addBrand(string title, string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new PAR_Brand();
                    x.id = Guid.NewGuid().ToString();
                    x.title = title;
                    x.description = description;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    x.districtID = districtID;
                    ds.PAR_Brand.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.data = x.id;
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------编辑党建品牌---------*/
        public CommonOutput editBrand(string id, string title, string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = ds.PAR_Brand.SingleOrDefault(d => d.id == id);
                    x.title = title;
                    x.description = description;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    x.districtID = districtID;
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------取消党建品牌---------*/
        public CommonOutput deleteBrand(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.PAR_Brand.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.PAR_Brand.DeleteOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.success = false;
                        returnData.message = "信息有误！";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------新增党建品牌附带文件（同任务图片一个表）---------*/
        public CommonOutput AddBrandPicture(string BrandID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(BrandID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = BrandID;
                    x.Url = Url;
                    ds.PAR_ActivityReleaseFile.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex.Message;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "Error:photoTakeID或imageURL无效！";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------删除党建品牌附带文件---------*/
        public CommonOutput deleteBrandPicture(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = ds.PAR_ActivityReleaseFile.SingleOrDefault(d => d.id == id);
                    if (file != null)
                    {
                        ds.PAR_ActivityReleaseFile.DeleteOnSubmit(file);
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.success = false;
                        returnData.message = "id错误";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------党建品牌评级---------*/
        public CommonOutput rateBrand(string id, string rate)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(rate))
            {
                try
                {
                    var x = ds.PAR_Brand.SingleOrDefault(d => d.id == id);
                    x.rate = rate;
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        #endregion

    }
}
