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
    public class Television : ITelevision
    {

        /*-----------------查看支部信息---------*/
        public CommonOutputT<string> getInformation(string number)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();

            try
            {

                var thisTopBox = ds.PAR_Picutre.SingleOrDefault(d => d.number == number&&d.status =="1");
                if (thisTopBox == null)
                {
                    returnData.message = "尚未绑定支部！";
                    returnData.success = false;
                    return returnData;
                }
                else
                {
                    returnData.data = thisTopBox.name;
                    returnData.message = "success";
                    returnData.success = true;
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
                returnData.success = true;
                return returnData;
            }
        }

        /*-----------------抓图---------*/
        public CommonOutputT<string> getPicture(string number, string StudyContent)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();

            try
            {
                
                var thisTopBox = ds.PAR_Picutre.SingleOrDefault(d => d.number == number);
                if (thisTopBox == null)
                {
                    returnData.message = "fail";
                    returnData.success = false;
                    return returnData;
                }
                else
                {
                    var thisTime = (from a in ds.PAR_Executions
                                    where a.districtID == thisTopBox.districtID && a.releaseTime == DateTime.Now.ToString("yyyy/MM/dd")
                                    select a).ToArray();
                    var sk = (from a in ds.PAR_Executions
                              where a.districtID == thisTopBox.districtID && a.releaseTime == DateTime.Now.ToString("yyyy/MM/dd")&&a.activityID == StudyContent
                              select a).ToArray();
                    if (thisTime.Length < 3)
                    {
                        if (thisTime.Length == 0)
                        {
                            var times = new PAR_Executions();
                            times.id = Guid.NewGuid().ToString();
                            times.activityID = StudyContent;
                            times.districtID = thisTopBox.districtID;
                            times.releaseTime = DateTime.Now.ToString("yyyy/MM/dd");
                            var FlvImgSize = "240 * 180";
                            var Second = "1";
                            var thisTopBoxInfro = (from a in ds.PAR_Picture_Infro
                                                   join b in ds.PAR_Picutre on a.PartyBranch equals b.name into b1
                                                   from b2 in b1.DefaultIfEmpty()
                                                   orderby a.CreateTime descending
                                                   where a.StudyContent == StudyContent && b2.number == number
                                                   select a.ImageURL).ToArray();
                            ffmpegHelp.CatchImg(number, thisTopBox.IP, FlvImgSize, Second, thisTopBox.name, StudyContent);
                            if (thisTopBoxInfro.Length == 0)
                            { returnData.data = ""; }
                            else
                            { returnData.data = thisTopBoxInfro[0]; }
                            ds.PAR_Executions.InsertOnSubmit(times);
                            ds.SubmitChanges();
                            returnData.message = "success";
                            returnData.success = true;
                            ds.Dispose();
                            return returnData;
                        }
                        else
                        {
                            int count = 0;
                            foreach (var i in thisTime)
                            {
                                if (i.activityID == StudyContent)
                                {
                                    count = count + 1;
                                }
                            }
                            if (count == 0)
                            {
                                var times = new PAR_Executions();
                                times.id = Guid.NewGuid().ToString();
                                times.activityID = StudyContent;
                                times.districtID = thisTopBox.districtID;
                                times.releaseTime = DateTime.Now.ToString("yyyy/MM/dd");
                                var FlvImgSize = "240 * 180";
                                var Second = "1";
                                var thisTopBoxInfro = (from a in ds.PAR_Picture_Infro
                                                       join b in ds.PAR_Picutre on a.PartyBranch equals b.name into b1
                                                       from b2 in b1.DefaultIfEmpty()
                                                       orderby a.CreateTime descending
                                                       where a.StudyContent == StudyContent && b2.number == number
                                                       select a.ImageURL).ToArray();
                                ffmpegHelp.CatchImg(number, thisTopBox.IP, FlvImgSize, Second, thisTopBox.name, StudyContent);
                                if (thisTopBoxInfro.Length == 0)
                                { returnData.data = ""; }
                                else
                                { returnData.data = thisTopBoxInfro[0]; }
                                ds.PAR_Executions.InsertOnSubmit(times);
                                ds.SubmitChanges();
                                returnData.message = "success";
                                returnData.success = true;
                                ds.Dispose();
                                return returnData;
                            }
                            else
                            {

                                var FlvImgSize = "240 * 180";
                                var Second = "1";
                                var thisTopBoxInfro = (from a in ds.PAR_Picture_Infro
                                                       join b in ds.PAR_Picutre on a.PartyBranch equals b.name into b1
                                                       from b2 in b1.DefaultIfEmpty()
                                                       orderby a.CreateTime descending
                                                       where a.StudyContent == StudyContent && b2.number == number
                                                       select a.ImageURL).ToArray();
                                ffmpegHelp.CatchImg(number, thisTopBox.IP, FlvImgSize, Second, thisTopBox.name, StudyContent);
                                if (thisTopBoxInfro.Length == 0)
                                { returnData.data = ""; }
                                else
                                { returnData.data = thisTopBoxInfro[0]; }
                                ds.SubmitChanges();
                                returnData.message = "success";
                                returnData.success = true;
                                ds.Dispose();
                                return returnData;
                            }
                        }
                    }
                    else
                    {
                        if (sk.Length == 1)
                        {
                            var FlvImgSize = "240 * 180";
                            var Second = "1";
                            var thisTopBoxInfro = (from a in ds.PAR_Picture_Infro
                                                   join b in ds.PAR_Picutre on a.PartyBranch equals b.name into b1
                                                   from b2 in b1.DefaultIfEmpty()
                                                   orderby a.CreateTime descending
                                                   where a.StudyContent == StudyContent && b2.number == number
                                                   select a.ImageURL).ToArray();
                            ffmpegHelp.CatchImg(number, thisTopBox.IP, FlvImgSize, Second, thisTopBox.name, StudyContent);
                            if (thisTopBoxInfro.Length == 0)
                            { returnData.data = ""; }
                            else
                            { returnData.data = thisTopBoxInfro[0]; }
                            ds.SubmitChanges();
                            returnData.message = "success";
                            returnData.success = true;
                            ds.Dispose();
                            return returnData;
                        }
                        else
                        {
                            returnData.message = "每日执行不得超过三次！";
                            returnData.success = false;
                            ds.Dispose();
                            return returnData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
                returnData.success = true;
                ds.Dispose();
                return returnData;
            }
        }

        /*-----------------查看支部抓图---------*/
        public CommonOutputT<string> getPictureCapture(string districtName, string StudyContent)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();

            try
            {
                var thisTopBox = (from a in ds.PAR_Picture_Infro
                                  orderby Convert.ToDateTime(a.CreateTime) descending
                                  where a.PartyBranch == districtName && a.StudyContent == StudyContent
                                  select a.ImageURL).ToArray();
                if (thisTopBox.Length == 0)
                {
                    returnData.data = "";
                    returnData.message = "success";
                    returnData.success = true;
                    return returnData;
                }
                else
                {
                    returnData.data = thisTopBox[0];
                    returnData.message = "success";
                    returnData.success = true;
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
                returnData.success = true;
                return returnData;
            }
        }

        /*-----------------查看支部任务详情---------*/
        public CommonOutPutT_M<PAR_ActivityList[]> getTopBoxActivityList(string districtID)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityList[]> returnData = new CommonOutPutT_M<PAR_ActivityList[]>();
            var district = (from a in ds.SYS_District
                            where a.districtLevel == 3
                            select a.id).ToArray();
            int num = (from a in ds.SYS_District
                       where a.districtLevel == 3
                       select a.id).Count();
            try
            {
                IQueryable<PAR_ActivityList> x = from a in ds.PAR_Activity
                                                 where a.status == "1"
                                                 orderby a.title ascending, a.month ascending
                                                 select new PAR_ActivityList
                                                 {
                                                     id = a.id,
                                                     month = a.month,
                                                     title = a.title,
                                                     type = a.type,
                                                     content = a.context,
                                                     percentage = "",
                                                     flag = (((from b in ds.PAR_ActivityPerform where b.ActivityID == a.id && b.districtID == districtID select b.districtID).Count() == 1) || (from c in ds.PAR_ActivityFeedback join d in ds.POP_Basic on c.userId equals d.id into d1 from d2 in d1.DefaultIfEmpty() where c.snId == a.id && d2.districtID == districtID && c.flag == "1" select c.id).Count() >= 1) ? "已完成" : ((Convert.ToDateTime(a.month) > DateTime.Now && (Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) ? "即将过期" : ((Convert.ToDateTime(a.month) > DateTime.Now) ? "进行中" : "已过期")))
                                                 };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray<PAR_ActivityList>().Length;
                //x = x.Skip<PAR_ActivityList>(offset);
                //x = x.Take<PAR_ActivityList>(limit);
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

        /*----------------左侧栏查看执行情况---------*/
        public TV_BoxActivity<string[], string[], string[]> getTopBoxActivityListByDistrict(string districtName, string PageIndex)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            TV_BoxActivity<string[], string[], string[]> returnData = new TV_BoxActivity<string[], string[], string[]>();
            var district = (from a in ds.SYS_District
                            where a.districtName == districtName
                            select a).ToArray();
            try
            {
                var all = (from a in ds.PAR_Activity
                           where a.status == "1" && (a.districtID == "01"||a.districtID ==district[0].id)
                           select a.type).ToArray();
                returnData.complete = (from a in ds.PAR_ActivityPerform
                                       join b in ds.PAR_Activity on a.ActivityID equals b.id into b1
                                       from b2 in b1.DefaultIfEmpty()
                                       where b2.status == "1" && a.districtID == district[0].id && a.status == "2"
                                       select  b2.type).ToArray();

                returnData.expired = (from a in ds.PAR_Activity
                                      where a.status == "1" && (Convert.ToDateTime(a.month) <= DateTime.Now) && (a.districtID == "01" || a.districtID == district[0].id)
                                      select a.type).ToArray();
                returnData.expired = (returnData.expired.Except(returnData.complete)).ToArray();

                returnData.Incomplete = (all.Except(returnData.complete).Except(returnData.expired)).ToArray();
                var temp = (from a in ds.PAR_Activity
                            where a.status == "1" && (Convert.ToDateTime(a.month) > DateTime.Now && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) && (a.districtID == "01" || a.districtID == district[0].id)
                            select a.type).ToArray();
                var t = temp.Except(returnData.complete);
                var m = returnData.Incomplete.Except(t);
                var n = (from a in ds.PAR_Activity
                         where a.status == "1" && (Convert.ToDateTime(a.month) > DateTime.Now && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)) && (a.districtID == "01" || a.districtID == district[0].id)
                         select a.type + "(即将过期)").ToArray();
                returnData.Incomplete = (m.Concat(n)).ToArray();
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = 0;
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*----------------根据月份查看任务---------*/
        public CommonOutPutT_M<PAR_ActivityComplete[]> getTopBoxActivityListByMonth(string districtName, string month, string PageIndex)
        {

            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_ActivityComplete[]> returnData = new CommonOutPutT_M<PAR_ActivityComplete[]>();
            var district = (from a in ds.SYS_District
                               where a.districtName == districtName
                               select a.id).ToArray();
            try
            {
                var complete = (from a in ds.PAR_Activity
                                where a.status == "1" && a.month.Substring(5, 2) == month && (a.districtID == "01" || a.districtID == district[0])
                                select new PAR_ActivityComplete
                                {
                                    id = a.id,
                                    title = a.title,
                                    type = a.type,
                                    context = a.context,
                                    flag = ""
                                });
                var complete1 = (from a in ds.PAR_Activity
                                 join c in ds.PAR_ActivityPerform on a.id equals c.ActivityID into c1
                                 from c2 in c1.DefaultIfEmpty()
                                 where a.status == "1" && a.month.Substring(5, 2) == month && c2.status == "2"&&c2.districtID == district[0]
                                 select new PAR_ActivityComplete
                                 {
                                     id = a.id,
                                     title = a.title,
                                     type = a.type,
                                     context = a.context,
                                     flag = ""
                                 });
                var t = complete.Except(complete1);
                //var z = ((t).ToList().Skip((Convert.ToInt32(PageIndex) - 1) * 5));
                //z = z.Take(5);
                var thisTime =  (from a in ds.PAR_Executions
                                where a.districtID == district[0] && a.releaseTime == DateTime.Now.ToString("yyyy/MM/dd")
                                select a).ToArray();
                returnData.rows = (t.Skip((Convert.ToInt32(PageIndex) - 1) * 5)).Take(5).ToArray();
                returnData.success = true;
                returnData.message = thisTime.Length.ToString();
                returnData.total = Convert.ToInt32(Math.Ceiling(t.ToArray().Length / 5.0));
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------获取监控信息---------*/
        public CommonOutputT<TV_TopBox> getTopBoxInfro(string number)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<TV_TopBox> returnData = new CommonOutputT<TV_TopBox>();
            try
            {

                var y = (from b in ds.PAR_Time
                         orderby b.nowTime descending
                         select b.time).ToArray();
                var x = from a in ds.PAR_Picutre
                        where a.number == number
                        select new TV_TopBox
                        {
                            IP = a.IP,
                            time = y[0],
                        };
                returnData.data = x.ToArray<TV_TopBox>()[0];
                returnData.message = "success";
                returnData.success = true;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
                returnData.success = true;
                return returnData;
            }
        }

    }
}
