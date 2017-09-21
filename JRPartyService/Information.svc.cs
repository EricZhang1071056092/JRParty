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
    public class Information : IInformation
    {

        #region 通知公告

        /*-----------------新增通知公告---------*/
        public CommonOutput addInformation(string title, string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(districtID))
            {

                try
                {
                    var thisClient = ds.INF_information.SingleOrDefault(d => d.title == title && d.description == description);
                    if (thisClient == null)
                    {
                        INF_information x = new INF_information();
                        x.id = Guid.NewGuid().ToString();
                        x.title = title;
                        x.description = description;
                        x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        x.districtID = districtID;
                        ds.INF_information.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.message = "success";
                        returnData.success = true;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.message = "请勿重复上传！";
                        returnData.success = false;
                        ds.Dispose(); return returnData;

                    }
                }
                catch (Exception)
                {
                    returnData.message = "Error";
                    returnData.success = false;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.message = "数据不全！";
                returnData.success = false;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------编辑通知公告---------*/
        public CommonOutput editInformation(string id, string title, string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(title))
            {

                try
                {
                    var thisPop = ds.INF_information.SingleOrDefault(d => d.id == id);
                    if (thisPop != null)
                    {
                        thisPop.title = title;
                        thisPop.description = description;
                        thisPop.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        thisPop.districtID = districtID;
                        thisPop.districtID = districtID;
                        ds.SubmitChanges();
                        returnData.message = "success";
                        returnData.success = true;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.message = "不存在该用户！";
                        returnData.success = false;
                        ds.Dispose(); return returnData;

                    }
                }
                catch (Exception ex)
                {
                    returnData.message = "Error：" + ex.Message;
                    returnData.success = false;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.message = "输入信息不全！";
                returnData.success = false;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------查询通知公告列表---------*/
        public CommonOutPutT_M<List_information[]> getInformationList(string districtID, int offset, int limit)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_information[]> returnData = new CommonOutPutT_M<List_information[]>();
            try
            {
                IQueryable<List_information> x = from a in ds.INF_information
                                                 join b in ds.SYS_District on a.districtID equals b.id into b1
                                                 from b2 in b1.DefaultIfEmpty()
                                          orderby a.releaseTime descending
                                          select new List_information
                                          {
                                              id = a.id,
                                              districtName = b2.districtName,
                                              description = a.description,
                                              releaseTime = a.releaseTime,
                                              title = a.title
                                          };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray().Length;
                x = x.Skip(offset);
                x = x.Take(limit);
                returnData.rows = x.ToArray();
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "提示:" + ex;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------登出通知公告---------*/
        public CommonOutput deleteInformation(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            var y = ds.INF_information.SingleOrDefault(d => d.id == id);
            if (y != null)
            {
                ds.INF_information.DeleteOnSubmit(y);
                ds.SubmitChanges();
            }
            returnData.success = true;
            returnData.message = "Success";
            ds.Dispose(); return returnData;
        }

        /*-----------------发起公告推送---------*/
        public CommonOutput infPushIn(string districtId, string title, string objs)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput result = new CommonOutput();
            try
            {
                bool check = !string.IsNullOrEmpty(districtId) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(objs);
                if (check)
                {
                    var thisGuy = ds.SYS_District.SingleOrDefault(d => d.id == districtId);
                    if (thisGuy != null)
                    {
                        string[] objs1 = objs.Split(',');
                        string objs2 = "";
                        foreach (var item in objs1)
                        {
                            var obj = ds.SYS_District.SingleOrDefault(d => d.id == item);
                            if (obj != null) objs2 += ",'" + item + "'";
                        }
                        if (objs2.Length > 0)
                        {
                            objs2 = objs2.Substring(1);
                            INF_Push x = new INF_Push();
                            x.id = Guid.NewGuid().ToString();
                            x.authorId = districtId;
                            x.title = title;
                            x.objs = objs2;
                            x.status = "";
                            x.creatTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            ds.INF_Push.InsertOnSubmit(x);
                            ds.SubmitChanges();
                            result.success = true;
                            result.message = "success";
                        }
                        else
                        {
                            result.message = "Error:无效目标id！";
                        }
                    }
                    else
                    {
                        result.message = "Error:无效区域id！";
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

        /*-----------------推送目标列表---------*/
        public CommonOutputT<PushObject> getObjList()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<PushObject> result = new CommonOutputT<PushObject>();
            try
            {
                var x = (from c in ds.SYS_District
                         where c.id.Length == 2
                         select new PushObject
                         {
                             authorityID = c.id,
                             moduleName = c.districtName,
                             subAuthority = (from t in ds.SYS_District
                                             where t.attachTo == c.id && t.id.Length == 4
                                             select new PushObject
                                             {
                                                 authorityID = t.id,
                                                 moduleName = t.districtName,
                                                 subAuthority = (from v in ds.SYS_District
                                                                 where v.attachTo == t.id && v.id.Length == 6
                                                                 select new PushObject
                                                                 {
                                                                     authorityID = v.id,
                                                                     moduleName = v.districtName,
                                                                     subAuthority=null
                                                                 }).ToArray()
                                             }).ToArray()
                         }).FirstOrDefault();
                result.success = true;
                result.message = "success";
                result.data = x;
            }
            catch(Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }

        /*-----------------推送消息列表---------*/
        public CommonOutPutT_M<InfPush[]> getPushList(string districtId, int offset, int limit, string order, string sort, string search)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<InfPush[]> result = new CommonOutPutT_M<InfPush[]>();
            try
            {
                if (!string.IsNullOrEmpty(districtId))
                {
                    var thisGuy = ds.SYS_District.SingleOrDefault(d => d.id == districtId);
                    if (thisGuy != null)
                    {
                        if (limit == 0) limit = 10;
                        if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                        if (String.IsNullOrEmpty(search)) search = "";

                        var x = from p in ds.INF_Push
                                where (p.objs.Contains("'" + districtId + "'") || p.authorId == districtId)
                                && p.title.Contains(search)
                                select new InfPush
                                {
                                    id = p.id,
                                    //town = p.authorId.Length < 6 ? ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName : ds.SYS_District.SingleOrDefault(d => d.attachTo == p.authorId).districtName,
                                    //village = p.authorId.Length == 6 ? ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName : null,
                                    districtName = ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName,
                                    creatTime = p.creatTime,
                                    objects = p.objs,
                                    title = p.title
                                };
                        switch (sort)
                        {
                            //case "town":
                            //    x = x.OrderBy(c => c.town);
                            //    break;
                            //case "village":
                            //    x = x.OrderBy(c => c.village);
                            //    break;
                            case "districtName":
                                x = x.OrderBy(c => c.districtName);
                                break;
                            case "creatTime":
                                x = x.OrderBy(c => c.creatTime);
                                break;
                            case "objects":
                                x = x.OrderBy(c => c.objects);
                                break;
                            case "title":
                                x = x.OrderBy(c => c.title);
                                break;
                            default:
                                x = x.OrderBy(c => c.creatTime);
                                break;
                        }
                        result.total = x.ToArray().Length;
                        x = x.Skip(offset);
                        x = x.Take(limit);
                        var y = x.ToArray();
                        foreach (var item in y)
                        {
                            item.objects = item.objects.Replace("'", "");
                            var objArr = item.objects.Split(',');
                            string objs = "";
                            foreach (var item2 in objArr)
                            {
                                var objCheck = ds.SYS_District.SingleOrDefault(d => d.id == item2);
                                if (objCheck != null) objs += "、" + objCheck.districtName;
                            }
                            item.objects = objs.Substring(1);
                        }
                        result.rows = y.ToArray();
                        if (order == "desc") result.rows = result.rows.Reverse().ToArray();
                        result.success = true;
                        result.message = "success";
                    }
                    else
                    {
                        result.message = "Error:无效区域id！";
                        result.total = 1;
                    }
                }
                else
                {
                    result.message = "Error:请补全请求信息！";
                    result.total = 1;
                }
            }
            catch(Exception ex)
            {
                result.message = "Error:" + ex;
                result.total = 1;
            }
            ds.Dispose();
            return result;
        }

        /*-----------------接收公告推送---------*/
        public CommonOutputT<InfPush[]> infPushCheck(string districtId)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<InfPush[]> result = new CommonOutputT<InfPush[]>();
            try
            {
                if (!string.IsNullOrEmpty(districtId))
                {
                    var thisGuy = ds.SYS_District.SingleOrDefault(d => d.id == districtId);
                    if (thisGuy != null)
                    {
                        var x = (from p in ds.INF_Push
                                 where p.objs.Contains("'" + districtId + "'") && !p.status.Contains("'" + districtId + "'")
                                 select new InfPush
                                 {
                                     id = p.id,
                                     //town = p.authorId.Length < 6 ? ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName : ds.SYS_District.SingleOrDefault(d => d.attachTo == p.authorId).districtName,
                                     //village = p.authorId.Length == 6 ? ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName : null,
                                     districtName = ds.SYS_District.SingleOrDefault(d => d.id == p.authorId).districtName,
                                     creatTime = p.creatTime,
                                     title = p.title
                                 }).ToArray();
                        if (x.Length > 0)
                        {
                            foreach (var item in x)
                            {
                                var PushS = ds.INF_Push.SingleOrDefault(d => d.id == item.id);
                                PushS.status += PushS.status.Length > 0 ? ",'" + districtId + "'" : "'" + districtId + "'";
                            }
                            ds.SubmitChanges();
                            result.success = true;
                            result.message = "success";
                            result.data = x;
                        }
                        else
                        {
                            result.message = "Error:暂无推送公告！";
                        }
                    }
                    else
                    {
                        result.message = "Error:无效区域id！";
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

        #endregion
    }
}
