using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Activation;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Team : ITeam
    {

        #region 发展党员（会议）

        /*-----------------查看会议---------*/
        public CommonOutPutT_M<List_Conference[]> getTeamList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Conference[]> returnData = new CommonOutPutT_M<List_Conference[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);

            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };
                string[] conferences = new string[4] { "支委确定积极分子会议", "支委确定发展党员会议", "党员大会确定预备党员", "党员大会确定正式党员" };

                IQueryable<List_Conference> x = null;
                x = from a in ds.TEA_Conference
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where conferences.Contains(a.title) && subDistrict.Contains(a.districtID)
                    && (a.title.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                    select new List_Conference
                    {
                        id = a.id,
                        description = a.description,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        participation = a.participation,
                        releaseTime = a.releaseTime,
                        title = a.title,
                        imageURL = (from b in ds.PAR_ActivityReleaseFile
                                    where a.id == b.activityID && imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray()
                    };
                string[] y = (from a in ds.TEA_Conference
                              join b in ds.SYS_District on a.districtID equals b.id into b1
                              from b2 in b1.DefaultIfEmpty()
                              where conferences.Contains(a.title) && subDistrict.Contains(a.districtID)
                              && (a.title.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                              group b2 by b2.districtName into p
                              select p.Key).ToArray();
                List<List_Conference> xList = new List<List_Conference>();
                foreach (var item in y)
                {
                    string attachTo= (from a in ds.SYS_District
                                      where a.districtName == item
                                      select a.attachTo).FirstOrDefault();
                    string town = ds.SYS_District.SingleOrDefault(d => d.id == attachTo).districtName;
                    for (int i = 0; i < 4; i++)
                    {
                        var temp = new List_Conference();
                        temp.title = conferences[i];
                        temp.districtName = item;
                        temp.town = town;
                        foreach (var item2 in x)
                        {
                            if (item2.districtName == item && item2.title == conferences[i])
                            {
                                temp.id = item2.id;
                                temp.description = item2.description;
                                temp.participation = item2.participation;
                                temp.releaseTime = item2.releaseTime;
                                temp.imageURL = item2.imageURL;
                            }
                        }
                        xList.Add(temp);
                    }
                }
                IOrderedEnumerable<List_Conference> xList2=null;
                switch (sort)
                {
                    case "title":
                        xList2 = xList.OrderBy(c => c.title);
                        break;
                    case "description":
                        xList2 = xList.OrderBy(c => c.description);
                        break;
                    case "districtName":
                        xList2 = xList.OrderBy(c => c.districtName);
                        break;
                    case "releaseTime":
                        xList2 = xList.OrderBy(c => c.releaseTime);
                        break;
                    default:
                        xList2 = xList.OrderBy(c => c.districtName);
                        break;
                }        
                IEnumerable<List_Conference> xList3 = null;
                returnData.total = xList2.ToArray().Length;
                xList3 = xList2.Skip(offset);
                xList3 = xList3.Take(limit);
                returnData.rows = xList3.ToArray();
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

        /*-----------------查看单条会议---------*/
        public CommonOutputT<CON_Detail> getTeamDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<CON_Detail> returnData = new CommonOutputT<CON_Detail>();
            try
            {
                if(!string.IsNullOrEmpty(id)){
                    var x = (from a in ds.TEA_Conference
                             join b in ds.SYS_District on a.districtID equals b.id into b1
                             from b2 in b1.DefaultIfEmpty()
                             where a.id == id
                             select new CON_Detail
                             {
                                 id = a.id,
                                 districtName = b2.districtName,
                                 participation = a.participation,
                                 title = a.title,
                                 description = a.description,
                                 releaseTime = a.releaseTime,
                                 imageURL = (from b in ds.PAR_ActivityReleaseFile where a.id == b.activityID select b).ToArray()
                             }).ToArray();
                    returnData.success = true;
                    returnData.message = "Success";
                    returnData.data = x[0];
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

        /*-----------------查看单条会议附件---------*/
        public CommonOutputT<string[]> getTDFiles(string id)
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
            catch(Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }

        /*-----------------新增/更新会议---------*/
        public CommonOutputT<string> addTeam(string title, string description, string participation, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            try
            {
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(districtID))
                {
                    var thisGuy = ds.TEA_Conference.SingleOrDefault(d => d.title == title && d.districtID == districtID);
                    if (thisGuy != null)
                    {
                        thisGuy.description = description;
                        thisGuy.participation = participation;
                        thisGuy.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        returnData.data = thisGuy.id;
                    }
                    else
                    {
                        var x = new TEA_Conference();
                        x.id = Guid.NewGuid().ToString();
                        x.description = description;
                        x.title = title;
                        x.participation = participation;
                        x.districtID = districtID;
                        x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        ds.TEA_Conference.InsertOnSubmit(x);
                        returnData.data = x.id;
                    }
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    
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

        /*-----------------编辑会议---------*/
        public CommonOutput editTeam(string id, string title, string description, string participation, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = ds.TEA_Conference.SingleOrDefault(d => d.id == id);
                    x.description = description;
                    x.title = title;
                    x.participation = participation;
                    x.districtID = districtID;
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

        /*-----------------取消会议---------*/
        public CommonOutput deleteTeam(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.TEA_Conference.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.TEA_Conference.DeleteOnSubmit(x);
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

        /*-----------------新增会议附带文件（同任务图片一个表）---------*/
        public CommonOutput AddTeamPicture(string TeamID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            try
            {
                if (!string.IsNullOrEmpty(TeamID) || !string.IsNullOrEmpty(Url))
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = TeamID;
                    x.Url = Url;
                    ds.PAR_ActivityReleaseFile.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                }
                else
                {
                    returnData.message = "Error:请补全请求信息！";
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex.Message;
            }
            ds.Dispose();
            return returnData;
        }

        /*-----------------删除会议附带文件---------*/
        public CommonOutput deleteTeamPicture(string id)
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

        #region 党员志愿者
        /*-----------------查看党员志愿者---------*/
        public CommonOutPutT_M<TIS_LeadingShip[]> getVolunteerList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<TIS_LeadingShip[]> returnData = new CommonOutPutT_M<TIS_LeadingShip[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);

            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";

                IQueryable<TIS_LeadingShip> x = null;
                x = from a in ds.POP_Volunteer
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where subDistrict.Contains(a.districtID) && (a.name.Contains(search) || a.IDCard.Contains(search) || a.duty.Contains(search) || b2.districtName.Contains(search))
                    select new TIS_LeadingShip
                    {
                        id = a.id,
                        birthDay = a.birthDay,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        type = a.type,
                        duty = a.duty,
                        education = a.education,
                        financialType = a.financialType,
                        IDCard = a.IDCard,
                        JionTime = a.JionTime,
                        name = a.name,
                        nation = a.nation,
                        phone = a.phone,
                        sex = a.sex,
                        TrainingTitle = a.TrainingTitle,
                        workTime = a.workTime,
                        imageURL = (from b in ds.POP_Portrait
                                    where a.id == b.populationID
                                    select Tools.GetSERVERADDRESS()+ "/JRPartyService/Upload/Activity/" + b.imageURL).ToArray()
                    };
                switch (sort)
                {
                    case "town":
                        x = x.OrderBy(c => c.town);
                        break;
                    case "name":
                        x = x.OrderBy(c => c.name);
                        break;
                    case "IDCard":
                        x = x.OrderBy(c => c.IDCard);
                        break;
                    case "duty":
                        x = x.OrderBy(c => c.duty);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    default:
                        x = x.OrderBy(c => c.town);
                        break;
                }

                returnData.total = x.ToArray<TIS_LeadingShip>().Length;
                x = x.Skip<TIS_LeadingShip>(offset);
                x = x.Take<TIS_LeadingShip>(limit);
                returnData.rows = x.ToArray<TIS_LeadingShip>();
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
        /*-----------------新增党员志愿者---------*/
        public CommonOutputT<string> addVolunteer(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new POP_Volunteer();
                    x.id = Guid.NewGuid().ToString();
                    x.name = name;
                    x.IDCard = IDCard;
                    x.type = type;
                    x.sex = sex;
                    x.nation = nation;
                    x.birthDay = birthDay;
                    x.JionTime = JionTime;
                    x.workTime = workTime;
                    x.duty = duty;
                    x.education = education;
                    x.TrainingTitle = TrainingTitle;
                    x.phone = phone;
                    x.financialType = financialType;
                    x.districtID = districtID;
                    ds.POP_Volunteer.InsertOnSubmit(x);
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
        /*-----------------编辑党员志愿者---------*/
        public CommonOutput editVolunteer(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty))
            {
                try
                {
                    var x = ds.POP_Volunteer.SingleOrDefault(d => d.id == id);
                    x.name = name;
                    x.IDCard = IDCard;
                    x.type = type;
                    x.sex = sex;
                    x.nation = nation;
                    x.birthDay = birthDay;
                    x.JionTime = JionTime;
                    x.workTime = workTime;
                    x.duty = duty;
                    x.education = education;
                    x.TrainingTitle = TrainingTitle;
                    x.phone = phone;
                    x.financialType = financialType;
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
        /*-----------------取消党员志愿者---------*/
        public CommonOutput deleteVolunteer(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.POP_Volunteer.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.POP_Volunteer.DeleteOnSubmit(x);
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

        /*-----------------查看单条党员志愿者---------*/
        public CommonOutputT<VOL_Detail> getVolunteerDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<VOL_Detail> returnData = new CommonOutputT<VOL_Detail>();
            try
            {
                var x = (from a in ds.POP_Volunteer
                         join b in ds.SYS_District on a.districtID equals b.id into b1
                         from b2 in b1.DefaultIfEmpty()
                         where a.id == id
                         select new VOL_Detail
                         {
                             id = a.id,
                             districtName = b2.districtName,
                             birthDay = a.birthDay,
                             duty = a.duty,
                             education =a.education,
                             financialType =a.financialType,
                             IDCard =a.IDCard,
                             JionTime =a.JionTime,
                             name =a.name,
                             nation =a.nation,
                             phone= a.phone,
                             sex =a.sex,
                             TrainingTitle =a.TrainingTitle,
                             type =a.type,
                             workTime =a.workTime,
                             imageURL = (from b in ds.POP_Portrait where a.id == b.populationID select b).ToArray()
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

        /*-----------------新增党员志愿者头像---------*/
        public CommonOutput AddVolunteerPicture(string VolunteerID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(VolunteerID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new POP_Portrait();
                    x.id = Guid.NewGuid().ToString();
                    x.populationID = VolunteerID;
                    x.imageURL = Url;
                    ds.POP_Portrait.InsertOnSubmit(x);
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

        /*-----------------删除党员志愿者头像---------*/
        public CommonOutput deleteVolunteerPicture(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = ds.POP_Portrait.SingleOrDefault(d => d.id == id);
                    if (file != null)
                    {
                        ds.POP_Portrait.DeleteOnSubmit(file);
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

        #region 志愿者活动
        /*-----------------查看志愿者活动---------*/
        public CommonOutPutT_M<List_VolunteerActivity[]> getVolunteerActivityList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_VolunteerActivity[]> returnData = new CommonOutPutT_M<List_VolunteerActivity[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<List_VolunteerActivity> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "description":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.releaseTime ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TEA_VolunteerActivity
                                            join b in ds.SYS_District on a.districtID equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                            orderby b2.districtName ascending
                                            select new List_VolunteerActivity
                                            {
                                                id = a.id,
                                                description = a.description,
                                                districtName = b2.districtName,
                                                releaseTime = a.releaseTime
                                            };
                                    break;
                                default:
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "description":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.releaseTime descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_VolunteerActivity>().Length;
                        x = x.Skip<List_VolunteerActivity>(offset);
                        x = x.Take<List_VolunteerActivity>(limit);
                        returnData.rows = x.ToArray<List_VolunteerActivity>();
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
                        IQueryable<List_VolunteerActivity> x = from a in ds.TEA_VolunteerActivity
                                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                                        from b2 in b1.DefaultIfEmpty()
                                                        where subDistrict.Contains(a.districtID) && (a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                                                        orderby b2.districtName descending
                                                        select new List_VolunteerActivity
                                                        {
                                                            id = a.id,
                                                            description = a.description,
                                                            districtName = b2.districtName,
                                                            releaseTime = a.releaseTime
                                                        };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_VolunteerActivity>().Length;
                        x = x.Skip<List_VolunteerActivity>(offset);
                        x = x.Take<List_VolunteerActivity>(limit);
                        returnData.rows = x.ToArray<List_VolunteerActivity>();
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
                        IQueryable<List_VolunteerActivity> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "description":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) 
                                        orderby a.description ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) 
                                        orderby a.releaseTime ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) 
                                        orderby b2.districtName ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby b2.districtName ascending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "description":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.description descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "releaseTime":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.releaseTime descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby b2.districtName descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                                default:
                                    x = from a in ds.TEA_VolunteerActivity
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby b2.districtName descending
                                        select new List_VolunteerActivity
                                        {
                                            id = a.id,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            releaseTime = a.releaseTime
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_VolunteerActivity>().Length;
                        x = x.Skip<List_VolunteerActivity>(offset);
                        x = x.Take<List_VolunteerActivity>(limit);
                        returnData.rows = x.ToArray<List_VolunteerActivity>();
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
                        IQueryable<List_VolunteerActivity> x = from a in ds.TEA_VolunteerActivity
                                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                                        from b2 in b1.DefaultIfEmpty()
                                                        where subDistrict.Contains(a.districtID)
                                                        orderby b2.districtName descending
                                                        select new List_VolunteerActivity
                                                        {
                                                            id = a.id,
                                                            description = a.description,
                                                            districtName = b2.districtName,
                                                            releaseTime = a.releaseTime
                                                        };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_VolunteerActivity>().Length;
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
        /*-----------------新增志愿者活动---------*/
        public CommonOutputT<string> addVolunteerActivity(string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new TEA_VolunteerActivity();
                    x.id = Guid.NewGuid().ToString();
                    x.description = description;
                    x.districtID = districtID;
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    ds.TEA_VolunteerActivity.InsertOnSubmit(x);
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
        /*-----------------编辑志愿者活动---------*/
        public CommonOutput editVolunteerActivity(string id, string description, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = ds.TEA_VolunteerActivity.SingleOrDefault(d => d.id == id);
                    x.description = description;
                    x.districtID = districtID;
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
        /*-----------------取消志愿者活动---------*/
        public CommonOutput deleteVolunteerActivity(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.TEA_VolunteerActivity.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.TEA_VolunteerActivity.DeleteOnSubmit(x);
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

        /*-----------------查看单条志愿者活动---------*/
        public CommonOutputT<VOLACT_Detail> getVolunteerActivityDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<VOLACT_Detail> returnData = new CommonOutputT<VOLACT_Detail>();
            try
            {
                var x = (from a in ds.TEA_VolunteerActivity
                         join b in ds.SYS_District on a.districtID equals b.id into b1
                         from b2 in b1.DefaultIfEmpty()
                         where a.id == id
                         select new VOLACT_Detail
                         {
                             id = a.id,
                             districtName = b2.districtName,
                             description = a.description,
                             districtID = a.districtID,
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

        /*-----------------新增志愿者活动图片---------*/
        public CommonOutput AddVolunteerActivityPicture(string VolunteerID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(VolunteerID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = VolunteerID;
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
        /*-----------------删除志愿者活动图片---------*/
        public CommonOutput deleteVolunteerActivityPicture(string id)
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

        #region 后备干部队伍
        /*-----------------查看后备干部---------*/
        public CommonOutPutT_M<TIS_LeadingShip[]> getReserveCadreList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<TIS_LeadingShip[]> returnData = new CommonOutPutT_M<TIS_LeadingShip[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);

            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };

                IQueryable<TIS_LeadingShip> x = null;
                x = from a in ds.POP_ReserveCadre
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where subDistrict.Contains(a.districtID) && (a.name.Contains(search) || a.IDCard.Contains(search) || a.duty.Contains(search) || b2.districtName.Contains(search))
                    select new TIS_LeadingShip
                    {
                        id = a.id,
                        birthDay = a.birthDay,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        type = a.type,
                        duty = a.duty,
                        education = a.education,
                        financialType = a.financialType,
                        IDCard = a.IDCard,
                        JionTime = a.JionTime,
                        name = a.name,
                        nation = a.nation,
                        phone = a.phone,
                        sex = a.sex,
                        TrainingTitle = a.TrainingTitle,
                        workTime = a.workTime,
                        imageURL = (from b in ds.POP_Portrait
                                    where a.id == b.populationID && imgSuffix.Contains(b.imageURL.Substring(b.imageURL.Length-3,3))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.imageURL).ToArray()
                    };
                switch (sort)
                {
                    case "town":
                        x = x.OrderBy(c => c.town);
                        break;
                    case "name":
                        x = x.OrderBy(c => c.name);
                        break;
                    case "IDCard":
                        x = x.OrderBy(c => c.IDCard);
                        break;
                    case "duty":
                        x = x.OrderBy(c => c.duty);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    default:
                        x = x.OrderBy(c => c.town);
                        break;
                }
                returnData.total = x.ToArray<TIS_LeadingShip>().Length;
                x = x.Skip<TIS_LeadingShip>(offset);
                x = x.Take<TIS_LeadingShip>(limit);
                returnData.rows = x.ToArray<TIS_LeadingShip>();
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

        /*-----------------查看单条后备干部---------*/
        public CommonOutputT<VOL_Detail> getReserveCadreDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<VOL_Detail> returnData = new CommonOutputT<VOL_Detail>();
            try
            {
                var x = (from a in ds.POP_ReserveCadre
                         join b in ds.SYS_District on a.districtID equals b.id into b1
                         from b2 in b1.DefaultIfEmpty()
                         where a.id == id
                         select new VOL_Detail
                         {
                             id = a.id,
                             districtName = b2.districtName,
                             birthDay = a.birthDay,
                             duty = a.duty,
                             education = a.education,
                             financialType = a.financialType,
                             IDCard = a.IDCard,
                             JionTime = a.JionTime,
                             name = a.name,
                             nation = a.nation,
                             phone = a.phone,
                             sex = a.sex,
                             TrainingTitle = a.TrainingTitle,
                             type = a.type,
                             workTime = a.workTime,
                             imageURL = (from b in ds.POP_Portrait where a.id == b.populationID select b).ToArray()
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
        /*-----------------新增后备干部---------*/
        public CommonOutputT<string> addReserveCadre(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new POP_ReserveCadre();
                    x.id = Guid.NewGuid().ToString();
                    x.name = name;
                    x.IDCard = IDCard;
                    x.type = type;
                    x.sex = sex;
                    x.nation = nation;
                    x.birthDay = birthDay;
                    x.JionTime = JionTime;
                    x.workTime = workTime;
                    x.duty = duty;
                    x.education = education;
                    x.TrainingTitle = TrainingTitle;
                    x.phone = phone;
                    x.financialType = financialType;
                    x.districtID = districtID;
                    ds.POP_ReserveCadre.InsertOnSubmit(x);
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
        /*-----------------编辑后备干部---------*/
        public CommonOutput editReserveCadre(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty))
            {
                try
                {
                    var x = ds.POP_ReserveCadre.SingleOrDefault(d => d.id == id);
                    x.name = name;
                    x.IDCard = IDCard;
                    x.type = type;
                    x.sex = sex;
                    x.nation = nation;
                    x.birthDay = birthDay;
                    x.JionTime = JionTime;
                    x.workTime = workTime;
                    x.duty = duty;
                    x.education = education;
                    x.TrainingTitle = TrainingTitle;
                    x.phone = phone;
                    x.financialType = financialType;
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
        /*-----------------取消后备干部---------*/
        public CommonOutput deleteReserveCadre(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.POP_ReserveCadre.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.POP_ReserveCadre.DeleteOnSubmit(x);
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
        /*-----------------新增后备干部头像---------*/
        public CommonOutput AddReserveCadrePicture(string VolunteerID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(VolunteerID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new POP_Portrait();
                    x.id = Guid.NewGuid().ToString();
                    x.populationID = VolunteerID;
                    x.imageURL = Url;
                    ds.POP_Portrait.InsertOnSubmit(x);
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

        /*-----------------删除后备干部头像---------*/
        public CommonOutput deleteReserveCadrePicture(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = ds.POP_Portrait.SingleOrDefault(d => d.id == id);
                    if (file != null)
                    {
                        ds.POP_Portrait.DeleteOnSubmit(file);
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
