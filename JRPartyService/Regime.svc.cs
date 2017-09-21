using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using System.ServiceModel.Activation;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Regime : IRegime
    {
        #region 基本制度

        /*-----------------查看基本制度---------*/
        public CommonOutPutT_M<RegimeList[]> getRegimeList(int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<RegimeList[]> returnData = new CommonOutPutT_M<RegimeList[]>();

            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };

                IQueryable<RegimeList> x = null;
                x = from a in ds.REG_Basic
                    where a.name.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search)
                    select new RegimeList
                    {
                        id = a.id,
                        name = a.name,
                        description = a.description,
                        releaseTime = a.releaseTime,
                        imageURL = (from b in ds.PAR_ActivityReleaseFile
                                    where a.id == b.activityID && imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray()
                    };
                switch (sort)
                {
                    case "name":
                        x = x.OrderBy(c => c.name);
                        break;
                    case "description":
                        x = x.OrderBy(c => c.description);
                        break;
                    case "releaseTime":
                        x = x.OrderBy(c => c.releaseTime);
                        break;
                    default:
                        x = x.OrderBy(c => c.releaseTime);
                        break;
                }

                returnData.total = x.ToArray<RegimeList>().Length;
                x = x.Skip<RegimeList>(offset);
                x = x.Take<RegimeList>(limit);
                returnData.rows = x.ToArray<RegimeList>();
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

        /*-----------------查看单条制度---------*/
        public CommonOutputT<REG_Detail> getRegimeDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<REG_Detail> returnData = new CommonOutputT<REG_Detail>();
            try
            {
                var x = (from a in ds.REG_Basic
                         where a.id == id
                         select new REG_Detail
                         {
                             id = a.id,
                             name = a.name,
                             description = a.description,
                             releaseTime = a.releaseTime,
                             imageURL = (from b in ds.PAR_ActivityReleaseFile where a.id == b.activityID select b).ToArray()
                         }).ToArray();
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

        /*-----------------查看单条制度附件---------*/
        public CommonOutputT<string[]> getRegimeFiles(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string[]> result = new CommonOutputT<string[]>();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };
                    var thisGuy = ds.REG_Basic.SingleOrDefault(d => d.id == id);
                    if (thisGuy != null)
                    {
                        result.data = (from b in ds.PAR_ActivityReleaseFile
                                       where b.activityID == id && !imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3))
                                       select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray();
                        result.success = true;
                        result.message = "success";
                    }
                    else
                    {
                        result.message = "Error:非法id！";
                    }
                }
                else
                {
                    result.message = "Error:请补全请求信息！";
                }
            }
            catch(Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }

        /*-----------------新增基本制度---------*/
        public CommonOutputT<string> addRegime(string name, string description)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                try
                {
                    var x = new REG_Basic();
                    x.id = Guid.NewGuid().ToString();
                    x.description = description;
                    x.name = name;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    ds.REG_Basic.InsertOnSubmit(x);
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

        /*-----------------编辑基本制度---------*/
        public CommonOutput editRegime(string id, string name, string description)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                try
                {
                    var x = ds.REG_Basic.SingleOrDefault(d => d.id == id);
                    x.description = description;
                    x.name = name;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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

        /*-----------------取消基本制度---------*/
        public CommonOutput deleteRegime(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.REG_Basic.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.REG_Basic.DeleteOnSubmit(x);
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

        /*-----------------新增基本制度附带文件（同任务图片一个表）---------*/
        public CommonOutput AddRegimePicture(string regimeID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(regimeID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = regimeID;
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

        /*-----------------删除基本制度附带文件---------*/
        public CommonOutput deleteRegimePicture(string id)
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

        #endregion

    }
}
