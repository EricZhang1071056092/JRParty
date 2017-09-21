using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using System.Text.RegularExpressions;
using System.ServiceModel.Activation;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Party : IParty
    {
        static Timer timerOne = new Timer();
        static object lockNum = new object();
        static object lockNum2 = new object();
        static int minutes = 0;

        #region 任务发布

        /*-----------------新增任务提醒---------*/
        public CommonOutput addPlanAlarm(string id, string alarmTime)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(alarmTime))
            {
                try
                {
                    var x = ds.PAR_Activity.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        x.alarmTime = alarmTime;
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.success = false;
                        returnData.message = "无该记录";
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

        /*-----------------查看任务提醒---------*/
        public CommonOutputT<string> getPlanAlarm(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.PAR_Activity.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        returnData.data = x.alarmTime;
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.data = "";
                        returnData.success = false;
                        returnData.message = "无该记录";
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

        /*-----------------一级查看任务（执行中的任务）---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> getPlanList(int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            int num = (from a in ds.SYS_District
                       where a.districtLevel == 3
                       select a.id).Count();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                            }
                        }

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = ""
                                                         };

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = ""
                                        };
                                    break;
                            }
                        }
                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where a.districtID == "01" && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = ""
                                                         };
                        var temp = x.ToList();
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Remove(i);
                            }
                        }
                        var t = temp.ToArray();
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = t.ToArray<PAR_ActivityList>().Length;
                        returnData.rows = (t.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------二级查看任务（执行中的任务）---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> get2PlanList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            var subDistrict = (from a in ds.SYS_District
                               where a.districtLevel == 3 && a.attachTo == districtID
                               select a.id).ToArray();
            int num = subDistrict.Length;
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""

                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" && subDistrict.Contains(b.districtID) select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }
                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && subDistrict.Contains(b.districtID) && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };
                        IQueryable<PAR_ActivityList> y = from a in ds.PAR_Activity
                                                         join b in ds.PAR_ActivityPerform on a.id equals b.ActivityID into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && subDistrict.Contains(b2.districtID) && b2.status == "2"
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && subDistrict.Contains(b.districtID) && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };

                        var temp = (x.Except(y)).ToList<PAR_ActivityList>();
                        var t = from a in temp
                                orderby a.month ascending
                                select a;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = t.ToArray<PAR_ActivityList>().Length;
                        returnData.rows = (t.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------三级查看任务（执行中的任务）---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> get3PlanList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };

                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                            flag = (a.districtID == "01") ? "市级" : "镇级",
                                            isZhen = ""
                                        };
                                    break;
                            }
                        }
                        var temp = x;
                        foreach (var i in x)
                        {
                            if (i.percentage == "100%")
                            {
                                temp.Except(x);
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_ActivityList>().Length;
                        x = temp.Skip<PAR_ActivityList>(offset);
                        x = temp.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1)
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };
                        IQueryable<PAR_ActivityList> y = from a in ds.PAR_Activity
                                                         join b in ds.PAR_ActivityPerform on a.id equals b.ActivityID into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && b2.districtID == districtID && b2.status == "2"
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() * 100).ToString() + "%",
                                                             flag = (a.districtID == "01") ? "市级" : "镇级",
                                                             isZhen = ""
                                                         };


                        var temp = (x.Except(y)).ToList<PAR_ActivityList>();
                        var t = from a in temp
                                orderby a.month ascending
                                select a;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = t.ToArray<PAR_ActivityList>().Length;
                        returnData.rows = (t.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------新增任务---------*/
        public CommonOutputT<string> addPlan(string month, string content, string title, string districtID, string type)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            try
            {
                string[] checkDisIds = new string[2] { "01", "0112" };
                bool check = !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(type)
                    && !string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(districtID);
                if (check)
                {
                    var thisGuy = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
                    if (thisGuy != null)
                    {
                        if (Array.IndexOf(checkDisIds, districtID) != -1)
                        {
                            var x = new PAR_Activity();
                            x.id = Guid.NewGuid().ToString();
                            x.context = content;
                            x.month = month;
                            x.type = type;
                            x.title = title;
                            x.status = "1";
                            x.districtID = districtID;
                            x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            ds.PAR_Activity.InsertOnSubmit(x);
                            ds.SubmitChanges();
                            returnData.data = x.id;
                            returnData.success = true;
                            returnData.message = "success";
                        }
                        else
                        {
                            returnData.message = "Error:该区域不允许发布任务！";
                        }
                    }
                    else
                    {
                        returnData.message = "Error:非法区域id！";
                    }
                }
                else
                {
                    returnData.message = "Error:请补全请求信息！";
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
            }
            ds.Dispose();
            return returnData;
        }

        /*-----------------编辑任务---------*/
        public CommonOutput editPlan(string id, string month, string content, string title, string districtID, string type)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(type))
            {
                try
                {
                    var x = ds.PAR_Activity.SingleOrDefault(d => d.id == id);
                    x.context = content;
                    x.month = month;
                    x.type = type;
                    x.title = title;
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

        /*-----------------取消任务(删除数据库记录)---------*/
        public CommonOutput deletePlan(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.PAR_Activity.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.PAR_Activity.DeleteOnSubmit(x);
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

        /*-----------------取消任务---------*/
        //public CommonOutput deletePlan(string id)
        //{
        //    JRPartyDataContext ds = new JRPartyDataContext();
        //    CommonOutput returnData = new CommonOutput();
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        try
        //        {
        //            var x = ds.PAR_Activity.SingleOrDefault(d => d.id == id);
        //            if (x != null)
        //            {
        //                x.status = "0";
        //                ds.SubmitChanges();
        //                returnData.success = true;
        //                returnData.message = "success";
        //                ds.Dispose(); return returnData;
        //            }
        //            else
        //            {
        //                returnData.success = false;
        //                returnData.message = "信息有误！";
        //                ds.Dispose(); return returnData;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            returnData.success = false;
        //            returnData.message = "网络错误！";
        //            ds.Dispose(); return returnData;
        //        }
        //    }
        //    else
        //    {
        //        returnData.success = false;
        //        returnData.message = "请确认输入了所有必要信息";
        //        ds.Dispose(); return returnData;
        //    }
        //}

        /*-----------------新增任务附带文件---------*/
        public CommonOutput AddActivity(string activityID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(activityID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = activityID;
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

        /*-----------------删除任务附件---------*/
        public CommonOutput deleteActivityFile(string id)
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

        #region 任务管理

        /*-----------------查看单条任务---------*/
        public CommonOutputT<PAR_ActivityOne> getActivityDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<PAR_ActivityOne> returnData = new CommonOutputT<PAR_ActivityOne>();
            try
            {
                var x = (from a in ds.PAR_Activity

                         where a.id == id
                         select new PAR_ActivityOne
                         {
                             id = a.id,
                             title = a.title,
                             content = a.context,
                             month = a.month,
                             type = a.type,
                             distritID = a.districtID,
                             file = (from b in ds.PAR_ActivityReleaseFile where a.id == b.activityID select b).ToArray()
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

        /*-----------------查询任务(一级单位)---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> getActivityList(string districtID, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.districtLevel == 3
                            select a.id).ToArray();
            int num = (from a in ds.SYS_District
                       where a.districtLevel == 3
                       select a.id).Count();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                            }
                        }

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_ActivityList>().Length;
                        x = x.Skip<PAR_ActivityList>(offset);
                        x = x.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where a.districtID == "01" && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                                         };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_ActivityList>().Length;
                        x = x.Skip<PAR_ActivityList>(offset);
                        x = x.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.type ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.context ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.month ascending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.type descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.context descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where a.districtID == "01" && a.status == "1"
                                        orderby a.month descending
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                            flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_ActivityList>().Length;
                        x = x.Skip<PAR_ActivityList>(offset);
                        x = x.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                         where a.districtID == "01" && a.status == "1"
                                                         orderby a.month ascending
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             title = a.title,
                                                             type = a.type,
                                                             content = a.context,
                                                             percentage = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.status == "2" select b.districtID).Count() * 100 / num).ToString() + "%",
                                                             flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id select b.districtID).Count() == num)) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已完成")))
                                                         };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_ActivityList>().Length;
                        x = x.Skip<PAR_ActivityList>(offset);
                        x = x.Take<PAR_ActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }

            }
        }

        /*-----------------查询任务(一级单位跟踪)---------*/
        public CommonOutPutT_S<PAR_SubActivityList[]> getSubActivityList(string id, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_S<PAR_SubActivityList[]> returnData = new CommonOutPutT_S<PAR_SubActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.districtLevel == 3
                            select a.id).ToArray();
            var activity = from a in ds.SYS_District
                           where a.districtLevel == 3
                           select new PAR_SubActivityList
                           {
                               id = a.id,
                               districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                               SubdistrictName = a.districtName,
                               record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                               subDistrictNum = 0,
                               TVPicture = "",
                               PhonePicture = "",
                               percentage = "已完成",
                               flag0 = "0",
                               flag1 = "0"
                           };
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "districtName":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                            }
                        }

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = from a in ds.SYS_District
                                                            join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                            from b2 in b1.DefaultIfEmpty()
                                                            join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                                            from c2 in c1.DefaultIfEmpty()
                                                            join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                                            from d2 in d1.DefaultIfEmpty()
                                                            where b2.ActivityID == id && a.districtLevel == 3
                                                            orderby a.districtName ascending
                                                            select new PAR_SubActivityList
                                                            {
                                                                id = a.id,
                                                                districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                                                SubdistrictName = a.districtName,
                                                                record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                                                subDistrictNum = 0,
                                                                TVPicture = d2.ImageURL,
                                                                PhonePicture = c2.ImageUrl,
                                                                percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                                            };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.PAR_ActivityPicture on b2.ActivityID equals c.activityID into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        join d in ds.PAR_Picture_Infro on b2.ActivityID equals d.StudyContent into d1
                                        from d2 in d1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                            subDistrictNum = 0,
                                            TVPicture = d2.ImageURL,
                                            PhonePicture = c2.ImageUrl,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id && d.status == "2") == null ? "未完成" : "已完成"
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = from a in ds.SYS_District
                                                            join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                            from b2 in b1.DefaultIfEmpty()
                                                            where b2.ActivityID == id && a.districtLevel == 3 && b2.status == "2"
                                                            select new PAR_SubActivityList
                                                            {
                                                                id = a.id,
                                                                districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                                                SubdistrictName = a.districtName,
                                                                record = (from d in ds.SYS_District where d.attachTo == a.attachTo select d.id).Count().ToString(),
                                                                subDistrictNum = 0,
                                                                TVPicture = "",
                                                                PhonePicture = "",
                                                                percentage = "已完成",
                                                                flag0 = "0",
                                                                flag1 = "0"
                                                            };
                        //IQueryable<PAR_SubActivityList> y = null;
                        var temp = activity.ToArray().ToList();
                        for (int i = 0; i < temp.ToArray().Length; i++)
                        {
                            if (x.Contains(temp[i]))
                            {
                                var thisSubDistrict = ds.SYS_District.SingleOrDefault(d => d.districtName == temp[i].districtName);
                                temp[i].record = ((from a in ds.SYS_District
                                                   join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                   from b2 in b1.DefaultIfEmpty()
                                                   where b2.ActivityID == id && a.attachTo == thisSubDistrict.id && b2.status == "2"
                                                   select a.id).Count() * 100 / Convert.ToInt32(temp[i].record)).ToString() + "%";
                                temp[i].percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == temp[i].id && d.ActivityID == id && d.status == "1") == null ? ((ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == temp[i].id && d.ActivityID == id && d.status == "2") == null) ? "审核不通过" : "已完成") : "待审核";
                                var self = from d in ds.PAR_Picture_Infro join f in ds.SYS_District on d.PartyBranch equals f.districtName into f1 from f2 in f1.DefaultIfEmpty() where f2.id == temp[i].id && d.StudyContent == id orderby d.CreateTime descending select d.ImageURL;
                                if (self.ToArray().Length == 0)
                                { }
                                else
                                {
                                    temp[i].TVPicture = self.ToArray()[0];
                                    temp[i].flag1 = "1";
                                }
                                var pic = (from c in ds.PAR_ActivityPicture
                                           join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                           from d2 in d1.DefaultIfEmpty()
                                           join e in ds.POP_Basic on d2.userId equals e.id into e1
                                           from e2 in e1.DefaultIfEmpty()
                                           where d2.snId == id && e2.districtID == temp[i].id
                                           select c.ImageUrl).ToArray();
                                //var wu = from d in ds.PAR_ActivityFeedback join f in ds.SYS_User on d.userId equals f.id into f1 from f2 in f1.DefaultIfEmpty() join g in ds.SYS_District on f2.district equals g.id into g1 from g2 in g1.DefaultIfEmpty() join h in ds.PAR_ActivityPicture on d.id equals h.activityID into h1 from h2 in h1.DefaultIfEmpty() where g2.id == temp[i].id select h2.ImageUrl;
                                if (pic.Length == 0)
                                { }
                                else
                                {
                                    temp[i].PhonePicture = pic[0];
                                    temp[i].flag0 = "1";
                                }
                            }
                            else
                            {
                                temp[i].percentage = "未完成";
                                temp[i].record = "0%";
                            }
                        }
                        for (int j = 0; j < temp.ToArray().Length; j++)
                        {
                            for (int k = 0; k < temp.ToArray().Length; k++)
                            {
                                if (temp[j].record != "0%" && temp[j].districtName == temp[k].districtName)
                                {
                                    temp[k].record = temp[j].record;
                                }
                            }
                        }
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.percentage == "未完成")
                            {
                                IncompleteNum += 1;
                            }
                            else if (i.percentage == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                expireNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_SubActivityList>().Length;
                        var t = temp.Skip(offset);
                        var m = t.Take(limit);
                        returnData.rows = m.ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------查看任务详情---------*/
        public CommonOutPutT_M<PAR_Picture_Infro[]> getActivityPicture(string districtID, string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_Picture_Infro[]> returnData = new CommonOutPutT_M<PAR_Picture_Infro[]>();
            try
            {
                IQueryable<PAR_Picture_Infro> x = from a in ds.PAR_Picture_Infro
                                                  join b in ds.SYS_District on a.PartyBranch equals b.districtName into b1
                                                  from b2 in b1.DefaultIfEmpty()
                                                  where b2.id == districtID && a.StudyContent == id
                                                  orderby a.CreateTime descending
                                                  select a;
                var t = x.ToArray();
                PAR_Picture_Infro[] w = new PAR_Picture_Infro[6];
                if (t.Length >= 6)
                {
                    w[0] = t[0];
                    w[1] = t[1];
                    w[2] = t[2];
                    w[3] = t[3];
                    w[4] = t[4];
                    w[5] = t[t.Length - 1];


                    returnData.success = true;
                    returnData.message = "Success";
                    returnData.rows = w;
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.success = true;
                    returnData.message = "Success";
                    returnData.rows = x.ToArray();
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------查询任务(二级单位)---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> get2ActivityList(string districtID, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.attachTo == districtID
                            select a.id).ToArray();
            int num = (from a in ds.SYS_District
                       where a.attachTo == districtID
                       select a.id).Count();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> y = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    y = from a in ds.PAR_Activity
                                        orderby a.type ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    y = from a in ds.PAR_Activity
                                        orderby a.context ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    y = from a in ds.PAR_Activity
                                        orderby a.type descending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    y = from a in ds.PAR_Activity
                                        orderby a.context descending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = y.ToArray<PAR_ActivityList>().Length;
                        y = y.Skip<PAR_ActivityList>(offset);
                        y = y.Take<PAR_ActivityList>(limit);
                        returnData.rows = y.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> y = from a in ds.PAR_Activity
                                                         orderby a.month ascending
                                                         where (a.districtID == "01" || a.districtID == districtID) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             type = a.type,
                                                             content = a.context,
                                                             title = a.title,
                                                             percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                                             flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                                             isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                                         };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = y.ToArray<PAR_ActivityList>().Length;
                        y = y.Skip<PAR_ActivityList>(offset);
                        y = y.Take<PAR_ActivityList>(limit);
                        returnData.rows = y.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> y = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    y = from a in ds.PAR_Activity
                                        orderby a.type ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    y = from a in ds.PAR_Activity
                                        orderby a.context ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    y = from a in ds.PAR_Activity
                                        orderby a.type descending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    y = from a in ds.PAR_Activity
                                        orderby a.context descending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    y = from a in ds.PAR_Activity
                                        orderby a.month ascending
                                        where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                        select new PAR_ActivityList
                                        {
                                            id = a.id,
                                            month = a.month,
                                            type = a.type,
                                            content = a.context,
                                            title = a.title,
                                            percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                            flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = y.ToArray<PAR_ActivityList>().Length;
                        y = y.Skip<PAR_ActivityList>(offset);
                        y = y.Take<PAR_ActivityList>(limit);
                        returnData.rows = y.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {
                    try
                    {
                        IQueryable<PAR_ActivityList> y = from a in ds.PAR_Activity
                                                         orderby a.month ascending
                                                         where (a.districtID == "01" || a.districtID == districtID) && a.status == "1"
                                                         select new PAR_ActivityList
                                                         {
                                                             id = a.id,
                                                             month = a.month,
                                                             type = a.type,
                                                             content = a.context,
                                                             title = a.title,
                                                             percentage = (((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id && b.status == "2" && c2.id.Substring(0, 4) == districtID select b.districtID).Count()) * 100 / num).ToString() + "%",
                                                             flag = ((from b in ds.PAR_ActivityPerform join c in ds.SYS_District on b.districtID equals c.id into c1 from c2 in c1.DefaultIfEmpty() where b.ActivityID == a.id &&b.status == "2"&& c2.id.Substring(0, 4) == districtID select b.districtID).Count() == num) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期"))),
                                                             isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                                         };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = y.ToArray<PAR_ActivityList>().Length;
                        y = y.Skip<PAR_ActivityList>(offset);
                        y = y.Take<PAR_ActivityList>(limit);
                        returnData.rows = y.ToArray<PAR_ActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------查询任务(二级单位跟踪)---------*/
        public CommonOutPutT_S<PAR_SubActivityList[]> get2SubActivityList(string id, string districtID, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_S<PAR_SubActivityList[]> returnData = new CommonOutPutT_S<PAR_SubActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.attachTo == districtID
                            select a.id).ToArray();
            var activity = from a in ds.SYS_District
                           where a.attachTo == districtID
                           select new PAR_SubActivityList
                           {
                               id = a.id,
                               SubdistrictName = a.districtName,
                               districtName = "",
                               record = "",
                               PhonePicture = "",
                               TVPicture = "",
                               subDistrictNum = 0,
                               percentage = "已完成",
                               flag0 = "0",
                               flag1 = "0"
                           };
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "districtName":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                            }
                        }

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = from a in ds.SYS_District
                                                            join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                            from b2 in b1.DefaultIfEmpty()
                                                            where b2.ActivityID == id && a.districtLevel == 3
                                                            orderby a.districtName ascending
                                                            select new PAR_SubActivityList
                                                            {
                                                                id = a.id,
                                                                districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                                                SubdistrictName = a.districtName,
                                                                percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                                            };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                                default:
                                    x = from a in ds.SYS_District
                                        join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where b2.ActivityID == id && a.districtLevel == 3
                                        orderby a.districtName ascending
                                        select new PAR_SubActivityList
                                        {
                                            id = a.id,
                                            districtName = ds.SYS_District.SingleOrDefault(d => d.id == a.attachTo).districtName,
                                            SubdistrictName = a.districtName,
                                            percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == a.id) == null ? "0" : "1"
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = x.ToArray<PAR_SubActivityList>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {
                    try
                    {
                        IQueryable<PAR_SubActivityList> x = from a in ds.SYS_District
                                                            join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                            from b2 in b1.DefaultIfEmpty()
                                                            where b2.ActivityID == id && a.attachTo == districtID
                                                            select new PAR_SubActivityList
                                                            {
                                                                id = a.id,
                                                                SubdistrictName = a.districtName,
                                                                districtName = "",
                                                                record = "",
                                                                subDistrictNum = 0,
                                                                PhonePicture = "",
                                                                TVPicture = "",
                                                                percentage = "已完成",
                                                                flag0 = "0",
                                                                flag1 = "0"
                                                            };
                        //IQueryable<PAR_SubActivityList> y = null;
                        var temp = activity.ToArray().ToList();
                        for (int i = 0; i < temp.ToArray().Length; i++)
                        {
                            if (x.Contains(temp[i]))
                            {
                                temp[i].percentage = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == temp[i].id && d.ActivityID == id && d.status == "1") == null ? ((ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID == temp[i].id && d.ActivityID == id && d.status == "2") == null) ? "审核不通过" : "已完成") : "待审核";
                                var pic = (from c in ds.PAR_ActivityPicture
                                           join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                           from d2 in d1.DefaultIfEmpty()
                                           join e in ds.POP_Basic on d2.userId equals e.id into e1
                                           from e2 in e1.DefaultIfEmpty()
                                           where d2.snId == id && e2.districtID == temp[i].id
                                           select c.ImageUrl).ToArray();
                                var TVpic = (from f in ds.PAR_Picture_Infro
                                             where f.StudyContent == id && f.PartyBranch == temp[i].SubdistrictName
                                             orderby f.CreateTime descending
                                             select f.ImageURL).ToArray();
                                if (pic.Length > 0)
                                {
                                    temp[i].PhonePicture = pic[0];
                                    temp[i].flag0 = "1";
                                }
                                if (TVpic.Length > 0)
                                {
                                    temp[i].TVPicture = TVpic[0];
                                    temp[i].flag1 = "1";
                                }

                            }
                            else
                            {
                                temp[i].percentage = "未完成";
                            }
                        }
                        //var t = (from a in temp where a.percentage == "已完成" select a.id).Count();
                        //var m = (from b in ds.PAR_Activity where Convert.ToDateTime(b.month) < DateTime.Now select b.id).Count() - t;
                        //var n = (from c in ds.PAR_Activity where )
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.percentage == "未完成")
                            {
                                IncompleteNum += 1;
                            }
                            else if (i.percentage == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                expireNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        var wo = temp.ToArray<PAR_SubActivityList>();
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_SubActivityList>().Length;
                        x = x.Skip<PAR_SubActivityList>(offset);
                        x = x.Take<PAR_SubActivityList>(limit);
                        returnData.rows = wo.ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------查询任务(三级单位)---------*/
        public CommonOutPutT_Three<PAR_Activity3List[]> get3ActivityList(string districtID, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_Three<PAR_Activity3List[]> returnData = new CommonOutPutT_Three<PAR_Activity3List[]>();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_Activity3List> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        orderby a.title ascending
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.type descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.context descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                        orderby a.month descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }
                        var temp = x.ToArray();
                        for (int j = 0; j < temp.Length; j++)
                        {
                            var pic = (from c in ds.PAR_ActivityPicture
                                       join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                       from d2 in d1.DefaultIfEmpty()
                                       join e in ds.POP_Basic on d2.userId equals e.id into e1
                                       from e2 in e1.DefaultIfEmpty()
                                       where d2.snId == temp[j].id && e2.districtID == districtID
                                       select c.ImageUrl).ToArray();
                            var TVpic = (from f in ds.PAR_Picture_Infro
                                         join g in ds.SYS_District on f.PartyBranch equals g.districtName into g1
                                         from g2 in g1.DefaultIfEmpty()
                                         where f.StudyContent == temp[j].id && g2.id == districtID
                                         orderby f.CreateTime descending
                                         select f.ImageURL).ToArray();
                            if (pic.Length > 0)
                            {
                                temp[j].PhonePicture = pic[0];
                                temp[j].flag0 = "1";
                            }
                            if (TVpic.Length > 0)
                            {
                                temp[j].TVPicture = TVpic[0];
                                temp[j].flag1 = "1";
                            }

                        }
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.flag == "已过期")
                            {
                                expireNum += 1;
                            }
                            else if (i.flag == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                IncompleteNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_Activity3List>().Length;
                        returnData.rows = (temp.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_Activity3List> x = from a in ds.PAR_Activity
                                                          where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1" && (a.month.Contains(search) || a.context.Contains(search) || a.type.Contains(search) || a.title.Contains(search))
                                                          orderby a.month ascending
                                                          select new PAR_Activity3List
                                                          {
                                                              id = a.id,
                                                              month = a.month,
                                                              title = a.title,
                                                              type = a.type,
                                                              content = a.context,
                                                              percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                                              flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                                              PhonePicture = "",
                                                              TVPicture = "",
                                                              flag0 = "0",
                                                              flag1 = "0",
                                                              isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                                          };
                        var temp = x.ToArray();
                        for (int j = 0; j < temp.Length; j++)
                        {
                            var pic = (from c in ds.PAR_ActivityPicture
                                       join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                       from d2 in d1.DefaultIfEmpty()
                                       join e in ds.POP_Basic on d2.userId equals e.id into e1
                                       from e2 in e1.DefaultIfEmpty()
                                       where d2.snId == temp[j].id && e2.districtID == districtID
                                       select c.ImageUrl).ToArray();
                            var TVpic = (from f in ds.PAR_Picture_Infro
                                         join g in ds.SYS_District on f.PartyBranch equals g.districtName into g1
                                         from g2 in g1.DefaultIfEmpty()
                                         where f.StudyContent == temp[j].id && g2.id == districtID
                                         orderby f.CreateTime descending
                                         select f.ImageURL).ToArray();
                            if (pic.Length > 0)
                            {
                                temp[j].PhonePicture = pic[0];
                                temp[j].flag0 = "1";
                            }
                            if (TVpic.Length > 0)
                            {
                                temp[j].TVPicture = TVpic[0];
                                temp[j].flag1 = "1";
                            }

                        }
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.flag == "已过期")
                            {
                                expireNum += 1;
                            }
                            else if (i.flag == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                IncompleteNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_Activity3List>().Length;
                        returnData.rows = (temp.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_Activity3List> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.type ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.context ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.month ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.month ascending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.type descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.context descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                case "month":
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.month descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_Activity
                                        where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                        orderby a.month descending
                                        select new PAR_Activity3List
                                        {
                                            id = a.id,
                                            month = a.month,
                                            title = a.title,
                                            type = a.type,
                                            content = a.context,
                                            percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                            flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                            PhonePicture = "",
                                            TVPicture = "",
                                            flag0 = "0",
                                            flag1 = "0",
                                            isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                        };
                                    break;
                            }
                        }
                        var temp = x.ToArray();
                        for (int j = 0; j < temp.Length; j++)
                        {
                            var pic = (from c in ds.PAR_ActivityPicture
                                       join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                       from d2 in d1.DefaultIfEmpty()
                                       join e in ds.POP_Basic on d2.userId equals e.id into e1
                                       from e2 in e1.DefaultIfEmpty()
                                       where d2.snId == temp[j].id && e2.districtID == districtID
                                       select c.ImageUrl).ToArray();
                            var TVpic = (from f in ds.PAR_Picture_Infro
                                         join g in ds.SYS_District on f.PartyBranch equals g.districtName into g1
                                         from g2 in g1.DefaultIfEmpty()
                                         where f.StudyContent == temp[j].id && g2.id == districtID
                                         orderby f.CreateTime descending
                                         select f.ImageURL).ToArray();
                            if (pic.Length > 0)
                            {
                                temp[j].PhonePicture = pic[0];
                                temp[j].flag0 = "1";
                            }
                            if (TVpic.Length > 0)
                            {
                                temp[j].TVPicture = TVpic[0];
                                temp[j].flag1 = "1";
                            }

                        }
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.flag == "已过期")
                            {
                                expireNum += 1;
                            }
                            else if (i.flag == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                IncompleteNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_Activity3List>().Length;
                        returnData.rows = (temp.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_Activity3List> x = from a in ds.PAR_Activity
                                                          where (a.districtID == "01" || a.districtID == districtID.Substring(0, 4)) && a.status == "1"
                                                          orderby a.month ascending, a.month ascending
                                                          select new PAR_Activity3List
                                                          {
                                                              id = a.id,
                                                              month = a.month,
                                                              title = a.title,
                                                              type = a.type,
                                                              content = a.context,
                                                              percentage = (from c in ds.PAR_ActivityReleaseFile where c.activityID == a.id select c.id).Count() > 0 ? "1" : "0",
                                                              flag = ((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "1" select b.districtID).Count() == 1) ? "待审核" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "2" select b.districtID).Count() == 1) ? "已完成" : (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID && b.status == "3" select b.districtID).Count() == 1) ? "审核未通过" : (((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now.AddDays(7)) ? "进行中" : "已过期")))))),
                                                              PhonePicture = "",
                                                              TVPicture = "",
                                                              flag0 = "0",
                                                              flag1 = "0",
                                                              isZhen = (a.districtID == "01") ? "市级" : "镇级"
                                                          };
                        var temp = x.ToArray();
                        for (int j = 0; j < temp.Length; j++)
                        {
                            var pic = (from c in ds.PAR_ActivityPicture
                                       join d in ds.PAR_ActivityFeedback on c.activityID equals d.id into d1
                                       from d2 in d1.DefaultIfEmpty()
                                       join e in ds.POP_Basic on d2.userId equals e.id into e1
                                       from e2 in e1.DefaultIfEmpty()
                                       where d2.snId == temp[j].id && e2.districtID == districtID
                                       select c.ImageUrl).ToArray();
                            var TVpic = (from f in ds.PAR_Picture_Infro
                                         join g in ds.SYS_District on f.PartyBranch equals g.districtName into g1
                                         from g2 in g1.DefaultIfEmpty()
                                         where f.StudyContent == temp[j].id && g2.id == districtID
                                         orderby f.CreateTime descending
                                         select f.ImageURL).ToArray();
                            if (pic.Length > 0)
                            {
                                temp[j].PhonePicture = pic[0];
                                temp[j].flag0 = "1";
                            }
                            if (TVpic.Length > 0)
                            {
                                temp[j].TVPicture = TVpic[0];
                                temp[j].flag1 = "1";
                            }

                        }
                        int IncompleteNum = 0;
                        int expireNum = 0;
                        int completeNum = 0;
                        //var wo = temp.ToArray<PAR_SubActivityList>();
                        foreach (var i in temp)
                        {
                            if (i.flag == "已过期")
                            {
                                expireNum += 1;
                            }
                            else if (i.flag == "已完成")
                            {
                                completeNum += 1;
                            }
                            else
                            {
                                IncompleteNum += 1;
                            }
                        }
                        returnData.completeNum = completeNum;
                        returnData.IncompleteNum = IncompleteNum;
                        returnData.expireNum = expireNum;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = temp.ToArray<PAR_Activity3List>().Length;
                        returnData.rows = (temp.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
        }

        /*-----------------查看管理员发布图片---------*/
        public CommonOutPutT_M<ActivityPicture[]> getPictureByActivity(string districtID, string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<ActivityPicture[]> returnData = new CommonOutPutT_M<ActivityPicture[]>();
            try
            {
                IQueryable<ActivityPicture> x = from a in ds.PAR_ActivityFeedback
                                                join d in ds.POP_Basic on a.userId equals d.id into d1
                                                from d2 in d1.DefaultIfEmpty()
                                                join e in ds.SYS_District on d2.districtID equals e.id into e1
                                                from e2 in e1.DefaultIfEmpty()
                                                orderby a.time descending
                                                where a.snId == id && a.flag == "1" && d2.districtID == districtID
                                                select new ActivityPicture
                                                {
                                                    id = a.id,
                                                    userName = d2.name,
                                                    content = a.context,
                                                    time = a.time,
                                                    URL = (from b in ds.PAR_ActivityPicture
                                                           where a.id == b.activityID
                                                           select b.ImageUrl).ToArray()
                                                };
                returnData.success = true;
                returnData.message = "Success";
                returnData.rows = x.ToArray<ActivityPicture>();
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------待审核任务列表---------*/
        public CommonOutPutT_M<PAR_CheckActivityList[]> getCheckActivityList(string districtID, int offset, int limit, string order, string search, string sort)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_CheckActivityList[]> returnData = new CommonOutPutT_M<PAR_CheckActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.attachTo == districtID
                            select a.id).ToArray();
            try
            {
                IQueryable<PAR_CheckActivityList> x = from a in ds.PAR_ActivityPerform
                                                      join b in ds.PAR_Activity on a.ActivityID equals b.id into b1
                                                      from b2 in b1.DefaultIfEmpty()
                                                      join c in ds.SYS_District on a.districtID equals c.id into c1
                                                      from c2 in c1.DefaultIfEmpty()
                                                      where a.status == "1" && district.Contains(a.districtID) && b2.status == "1"
                                                      select new PAR_CheckActivityList
                                                      {
                                                          id = a.id,
                                                          title = b2.title,
                                                          type = b2.type,
                                                          content = b2.context,
                                                          month = b2.month,
                                                          percentage = c2.id,
                                                          flag = c2.districtName,
                                                          TVPicture = b2.id,
                                                          PhonePicture = b2.id,
                                                          picture = a.ActivityID,
                                                          isZhen = (b2.districtID == "01") ? "市级" : "镇级"
                                                      };
                var temp = x.ToArray();
                for (int i = 0; i < x.ToArray().Length; i++)
                {
                    var y = (from e in ds.PAR_Picture_Infro
                             where e.PartyBranch == temp[i].flag && e.StudyContent == temp[i].TVPicture
                             orderby e.CreateTime descending
                             select e.ImageURL).ToArray();
                    if (y.Length > 0)
                    {
                        temp[i].TVPicture = y[0];
                    }
                    else {
                        temp[i].TVPicture = "";
                    }
                    var z = (from f in ds.PAR_ActivityPicture
                             join g in ds.PAR_ActivityFeedback on f.activityID equals g.id into g1
                             from g2 in g1.DefaultIfEmpty()
                             join h in ds.POP_Basic on g2.userId equals h.id into h1
                             from h2 in h1.DefaultIfEmpty()
                             orderby g2.time descending
                             where g2.snId == temp[i].picture && h2.districtID == temp[i].percentage
                             select f.ImageUrl).ToArray();
                    if (z.Length > 0)
                    {
                        temp[i].picture = z[0];

                    }
                    else
                    {
                        temp[i].picture = "";
                    }
                }
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = temp.ToArray<PAR_CheckActivityList>().Length;
                //temp = temp.ToList().Skip<PAR_CheckActivityList>(offset);
                //temp = temp.Take<PAR_CheckActivityList>(limit);
                returnData.rows = temp.ToArray();
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------任务审核---------*/
        public CommonOutput checkActivity(string id, string districtID, string IsOK)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(IsOK))
            {
                try
                {
                    var thisActivity = ds.PAR_ActivityPerform.SingleOrDefault(d => d.id == id);
                    if (thisActivity == null)
                    {
                        returnData.success = false;
                        returnData.message = "不存在该任务！";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        if (IsOK == "true")
                        {
                            thisActivity.status = "2";
                            ds.SubmitChanges();
                            returnData.success = true;
                            returnData.message = "success";
                            ds.Dispose(); return returnData;
                        }
                        else
                        {
                            thisActivity.status = "3";
                            ds.SubmitChanges();
                            returnData.success = true;
                            returnData.message = "success";
                            ds.Dispose(); return returnData;
                        }
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

        /*-----------------任务附件下载---------*/
        public CommonOutPutT_M<string[]> getActivityFile(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<string[]> returnData = new CommonOutPutT_M<string[]>();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = (from a in ds.PAR_ActivityReleaseFile
                                where a.activityID == id
                                select a.Url).ToArray();
                    returnData.rows = file;
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


        #endregion

        #region 短信通知（茅山党建）

        /*-----------------查看通知---------*/
        public CommonOutPutT_M<PAR_MsgPush[]> getSubPlanList(int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_MsgPush[]> returnData = new CommonOutPutT_M<PAR_MsgPush[]>();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_MsgPush> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "title":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.title ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.context ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.releaseTime ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.releaseTime ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "title":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.title descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.context descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.releaseTime descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_MS_msgPush
                                        where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                        orderby a.releaseTime descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_MsgPush>().Length;
                        x = x.Skip<PAR_MsgPush>(offset);
                        x = x.Take<PAR_MsgPush>(limit);
                        returnData.rows = x.ToArray<PAR_MsgPush>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<PAR_MsgPush> x = from a in ds.PAR_MS_msgPush
                                                    where a.releaseTime.Contains(search) || a.context.Contains(search) || a.title.Contains(search)
                                                    orderby a.releaseTime descending
                                                    select new PAR_MsgPush
                                                    {
                                                        id = a.id,
                                                        title = a.title,
                                                        content = a.context,
                                                        releaseTime = a.releaseTime
                                                    };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_MsgPush>().Length;
                        x = x.Skip<PAR_MsgPush>(offset);
                        x = x.Take<PAR_MsgPush>(limit);
                        returnData.rows = x.ToArray<PAR_MsgPush>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_MsgPush> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "title":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.title ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.context ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.releaseTime ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.releaseTime ascending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "title":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.title descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "context":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.context descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.releaseTime descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.PAR_MS_msgPush
                                        orderby a.releaseTime descending
                                        select new PAR_MsgPush
                                        {
                                            id = a.id,
                                            title = a.title,
                                            content = a.context,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_MsgPush>().Length;
                        x = x.Skip<PAR_MsgPush>(offset);
                        x = x.Take<PAR_MsgPush>(limit);
                        returnData.rows = x.ToArray<PAR_MsgPush>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<PAR_MsgPush> x = from a in ds.PAR_MS_msgPush
                                                    orderby a.releaseTime descending
                                                    select new PAR_MsgPush
                                                    {
                                                        id = a.id,
                                                        title = a.title,
                                                        content = a.context,
                                                        releaseTime = a.releaseTime
                                                    };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_MsgPush>().Length;
                        returnData.rows = (x.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------新增通知---------*/
        public CommonOutputT<string> addSubPlan(string title, string content)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                try
                {
                    var x = new PAR_MS_msgPush();
                    x.id = Guid.NewGuid().ToString();
                    x.context = content;
                    x.title = title;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    ds.PAR_MS_msgPush.InsertOnSubmit(x);
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

        /*-----------------编辑通知---------*/
        public CommonOutput editSubPlan(string id, string title, string content)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                try
                {
                    var x = ds.PAR_MS_msgPush.SingleOrDefault(d => d.id == id);
                    x.context = content;
                    x.title = title;
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

        /*-----------------取消通知(删除数据库记录)---------*/
        public CommonOutput deleteSubPlan(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.PAR_MS_msgPush.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.PAR_MS_msgPush.DeleteOnSubmit(x);
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

        /*-----------------查看推送对象列表---------*/
        public CommonOutPutT_M<List_MsgPush[]> getObjectList(string id, string districtID, int offset, int limit)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_MsgPush[]> returnData = new CommonOutPutT_M<List_MsgPush[]>();
            try
            {
                IQueryable<List_MsgPush> x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            join c in ds.PAR_MS_msgPush on id equals c.id into c1
                                            from c2 in c1.DefaultIfEmpty()
                                            where b2.attachTo == districtID
                                            select new List_MsgPush
                                            {
                                                id = a.id,
                                                name = a.name,
                                                phone = a.phone,
                                                districtName = b2.districtName,
                                                IsPush = (from d in ds.PAR_MS_List where d.MS_msgPushID == id && d.districtID == b2.id select d.id).Count()>0?"已推送":"未推送"
                                            };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray<List_MsgPush>().Length;
                returnData.rows = (x.Skip(offset)).Take(limit).ToArray();
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        ///*-----------------推送---------*/
        //public CommonOutputT<string> sendMessageToSubAdmin(string districtID, string content)
        //{
        //    JRPartyDataContext ds = new JRPartyDataContext();
        //    CommonOutputT<string> returnData = new CommonOutputT<string>();
        //    if (!string.IsNullOrEmpty(districtID) && !string.IsNullOrEmpty(content))
        //    {
        //        districtID.Split(',');
        //        var thisDistrict = ds.SYS_User.SingleOrDefault(d => d.district == districtID);
        //        if (thisDistrict == null)
        //        {
        //            returnData.success = false;
        //            returnData.message = "该用户不存在！";
        //            ds.Dispose(); return returnData;
        //        }
        //        else
        //        {
        //            try
        //            {
        //                var x = new PAR_MS_msgPush();
        //                var time = DateTime.Now.ToString("yyyy年MM月dd日");
        //                x.id = Guid.NewGuid().ToString();
        //                x.context = content;
        //                x.title = title;
        //                x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //                ds.PAR_MS_msgPush.InsertOnSubmit(x);
        //                ds.SubmitChanges();
        //                IAppConfig appConfig = new MessageConfig("14708", "ea48d9dca5d53195a7943c583f77a4fc");
        //                MessageXSend messageXSend = new MessageXSend(appConfig);
        //                messageXSend.AddTo(phone);
        //                messageXSend.SetProject("V9OVb2");
        //                messageXSend.AddVar("code", content);
        //                messageXSend.AddVar("time", time);
        //                string returnMessage = string.Empty;

        //                returnData.success = true;
        //                returnData.message = "success";
        //                ds.Dispose(); return returnData;
        //            }
        //            catch (Exception ex)
        //            {
        //                returnData.success = false;
        //                returnData.message = "网络错误！";
        //                ds.Dispose(); return returnData;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        returnData.success = false;
        //        returnData.message = "请确认输入了所有必要信息";
        //        ds.Dispose(); return returnData;
        //    }
        //}
        #endregion

        #region 考核汇总

        /*-----------------查看所有任务---------*/
        public CommonOutPutT_M<PAR_Activity[]> getActivity(string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_Activity[]> returnData = new CommonOutPutT_M<PAR_Activity[]>();
            try
            {
                var x =  from a in ds.PAR_Activity
                         where a.status == "1" && (a.districtID == "01"||a.districtID == districtID)
                         orderby a.month descending
                         select a;
                returnData.success = true;
                returnData.message = "Success";
                returnData.rows = x.ToArray();
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------任务完成一览表---------*/
        public CommonOutPutT_MT<string[]> getStat(string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_MT<string[]> returnData = new CommonOutPutT_MT<string[]>();
            try
            {
                var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
                if (thisDistrict.districtLevel == 1)
                {
                    var activity = (from a in ds.PAR_Activity
                                    where a.status == "1" && a.districtID == districtID
                                    orderby a.month ascending
                                    select new E_AcIdAndNa
                                    {
                                        id = a.id,
                                        name = a.type
                                    }).ToArray();
                    //if (thisDistrict.districtLevel == 1)

                    var districts = (from a in ds.SYS_District
                                     where a.districtLevel == 3
                                     select new string[2] { a.id, a.districtName }).ToArray();
                    var numbers = new string[districts.Length]; //不定长

                    for (int i = 0; i < districts.Length; i++)
                    {
                        string[] t = new string[50]; //不定长
                        t[0] = districts[i][0];
                        t[1] = districts[i][1];
                        for (int j = 0; j < activity.Length; j++)
                        {
                            t[j + 2] = (from a in ds.PAR_ActivityPerform
                                        join b in ds.PAR_Activity on a.ActivityID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.SYS_District on a.districtID equals c.id into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        where c2.districtName == districts[i][1] && b2.type == activity[j].name && a.status == "2"
                                        select a).Count() == 1 ? "已完成" : "未完成";
                        }
                        string temp = "";
                        for (int m = 0; m < activity.Length+1; m++)
                        {
                            temp = temp + t[m] + ",";
                        }
                        temp = temp + t[activity.Length + 1];
                        numbers[i] = temp;
                    }
                    returnData.type = activity;
                    returnData.rows = numbers;
                    returnData.success = true;
                    returnData.message = "Success";
                }
                else
                {
                    var activity = (from a in ds.PAR_Activity
                                    where a.status == "1" && (a.districtID == "01" || a.districtID == districtID)
                                    orderby a.month ascending
                                    select new E_AcIdAndNa
                                    {
                                        id = a.id,
                                        name = a.type
                                    }).ToArray();

                    var districts = (from a in ds.SYS_District
                                     where a.districtLevel == 3 && a.attachTo == districtID
                                     select new string[2] { a.id, a.districtName }).ToArray();
                    var numbers = new string[districts.Length]; //不定长

                    for (int i = 0; i < districts.Length; i++)
                    {
                        string[] t = new string[50]; //不定长
                        t[0] = districts[i][0];
                        t[1] = districts[i][1];
                        for (int j = 0; j < activity.Length; j++)
                        {
                            t[j + 2] = (from a in ds.PAR_ActivityPerform
                                        join b in ds.PAR_Activity on a.ActivityID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        join c in ds.SYS_District on a.districtID equals c.id into c1
                                        from c2 in c1.DefaultIfEmpty()
                                        where c2.districtName == districts[i][1] && b2.type == activity[j].name && a.status == "2"&&b2.status=="1"
                                        select a).Count() == 1 ? "已完成" : "未完成";
                        }
                        string temp = "";
                        for (int m = 0; m < activity.Length + 1; m++)
                        {
                            temp = temp + t[m] + ",";
                        }
                        temp = temp + t[activity.Length + 1];
                        numbers[i] = temp;
                    }
                    returnData.type = activity;
                    returnData.rows = numbers;
                    returnData.success = true;
                    returnData.message = "Success";
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex.Message;
                
            }
            ds.Dispose();
            return returnData;
        }

        /*-----------------统计单条任务完成情况---------*/
        public Stat_Activityone1 getStatPie(string districtID,string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            Stat_Activityone1 returnData = new Stat_Activityone1();
            var level = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
            if (level.districtLevel == 1)
            {
                try
                {

                    var districts = (from a in ds.SYS_District where a.districtLevel == 3 select a.districtName).ToList();
                    var  x = (from a in ds.SYS_District
                                                      join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                                                      from b2 in b1.DefaultIfEmpty()
                                                      where  b2.ActivityID == id&& b2.status == "2"
                                                      select a.districtName).ToList();
                    returnData.completeName = x.ToArray();
                    foreach(var i in x)
                    {
                        districts.Remove(i);
                    }
                    returnData.incompleteName = districts.ToArray();
                    returnData.complete = x.ToArray().Length;
                    returnData.incomplete = districts.ToArray().Length;
                    returnData.success = true;
                    returnData.message = "Success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "数据异常";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                try
                {

                    var districts = (from a in ds.SYS_District where a.districtLevel == 3 && a.attachTo == districtID select a.districtName).ToList();
                    var x = (from a in ds.SYS_District
                             join b in ds.PAR_ActivityPerform on a.id equals b.districtID into b1
                             from b2 in b1.DefaultIfEmpty()
                             where a.attachTo == districtID && b2.ActivityID == id && b2.status == "2"
                             select a.districtName).ToList();
                    returnData.completeName = x.ToArray();
                    foreach (var i in x)
                    {
                        districts.Remove(i);
                    }
                    returnData.incompleteName = districts.ToArray();
                    returnData.complete = x.ToArray().Length;
                    returnData.incomplete = districts.ToArray().Length;
                    returnData.success = true;
                    returnData.message = "Success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "数据异常";
                    ds.Dispose(); return returnData;
                }
            }
        }

        #endregion

        #region 预警系统

        /*--------------开始线程------------*/
        public void start2()
        {
            timerOne.Interval = 1000 * 60;  //设定定时器的间隔为1s
            timerOne.Elapsed += new ElapsedEventHandler(timerOne_Elapsed);
            timerOne.Enabled = true;
        }

        /*--------------业务处理------------*/
        private static void timerOne_Elapsed(object source, ElapsedEventArgs e)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            minutes++;
            if (minutes >= 20)
            {
                // 分钟数清零，准备下次查询
                minutes = 0;
                //在此执行你的查询
                if (DateTime.Now.Hour >= 9)
                {
                    var activity = (from a in ds.PAR_Activity
                                    join c in ds.SYS_User on 1 equals 1 into c1
                                    from c2 in c1.DefaultIfEmpty()
                                    join d in ds.SYS_District on c2.district equals d.id into d1
                                    from d2 in d1.DefaultIfEmpty()
                                    where  Convert.ToDateTime(a.month) > DateTime.Now.AddDays(-1) && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)&&a.status == "1"&& a.districtID == "01"
                                    select new PAR_Message
                                    {
                                        districtName = d2.districtName,
                                        content = a.title + a.type,
                                        phone = c2.phone
                                    }).ToArray();
                    var complete = (from c in ds.PAR_ActivityPerform
                                    join d in ds.PAR_Activity on c.ActivityID equals d.id into d1
                                    from d2 in d1.DefaultIfEmpty()
                                    join g in ds.SYS_User on c.districtID equals g.district into g1
                                    from g2 in g1.DefaultIfEmpty()
                                    join h in ds.SYS_District on g2.district equals h.id into h1
                                    from h2 in h1.DefaultIfEmpty()
                                    where c.status == "2" && d2.status == "1" && d2.districtID == "01" && g2.roleID == "1" && g2.enable == "1"&&d2.alarmTime != DateTime.Now.ToString("yyyy-MM-dd")
                                    select new PAR_Message
                                    {
                                        districtName = h2.districtName,
                                        content = d2.title + d2.type,
                                        phone = g2.phone
                                    }).ToArray();
                    var alarm = (from a in ds.PAR_Activity
                                join c in ds.SYS_User on 1 equals 1 into c1
                                from c2 in c1.DefaultIfEmpty()
                                join d in ds.SYS_District on c2.district equals d.id into d1
                                from d2 in d1.DefaultIfEmpty()
                                where a.alarmTime == DateTime.Now.ToString("yyyy-MM-dd") && a.status == "1" && d2.districtLevel == 3 && c2.enable == "1" && a.districtID == "01"
                                 select new PAR_Message
                                {
                                    districtName = d2.districtName,
                                    content = a.title + a.type,
                                    phone = c2.phone
                                }).ToArray();
                    var t = activity.Except(complete);
                    t = t.Concat(alarm);
                    var temp = t.ToArray().ToList();
                    for(int i=0;i<t.ToArray().Length;i++)
                    {
                        var thisDistrict = ds.PAR_SendMsg.SingleOrDefault(d=>d.phone== temp[i].phone&&d.districtName== temp[i].districtName&&d.context== temp[i].content);
                        bool y = IsMobilePhone(temp[i].phone);
                        if (thisDistrict == null&&y)
                        {
                            sendMessageToAdmin(temp[i].phone, temp[i].districtName, temp[i].content);
                            PAR_SendMsg x = new PAR_SendMsg();
                            x.id = Guid.NewGuid().ToString();
                            x.districtName = temp[i].districtName;
                            x.context = temp[i].content;
                            x.phone = temp[i].phone;
                            ds.PAR_SendMsg.InsertOnSubmit(x);
                            ds.SubmitChanges();
                            
                        }
                    }
                    ds.Dispose();
                }
            }
        }

        /*--------------接入第三方短信服务------------*/
        public static void sendMessageToAdmin(string phone, string districtName, string activity)
        {
            try
            {

                IAppConfig appConfig = new MessageConfig("14708", "ea48d9dca5d53195a7943c583f77a4fc");
                MessageXSend messageXSend = new MessageXSend(appConfig);
                messageXSend.AddTo(phone);
                messageXSend.SetProject("oFsjV2");
                messageXSend.AddVar("code", districtName);
                messageXSend.AddVar("time", activity);
                string returnMessage = string.Empty;
                
                if (messageXSend.XSend(out returnMessage) == false)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>  
        /// 判断输入的字符串是否是一个合法的手机号  
        /// </summary>  
        /// <param name="input"></param>  
        /// <returns></returns>  
        public static bool IsMobilePhone(string input)
        {
            return Regex.IsMatch(input, @"^[1]+[3,5,7,8]+\d{9}");
        }
        #endregion

        #region 其他

        //------获取服务器当前时间------
        public CommonOutputT<string> getDateTime()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> result = new CommonOutputT<string>();
            try
            {
                result.success = true;
                result.message = "success";
                result.data = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }

        #endregion

    }
}
