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
    public class System : ISystem
    {
        /*-------------登录------------*/
        public LoginAccess<PAR_WarningPara[]> accountLogin(string username, string password)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            LoginAccess<PAR_WarningPara[]> returnData = new LoginAccess<PAR_WarningPara[]>();
            SYS_LoginNote x = new SYS_LoginNote();
            x.id = Guid.NewGuid().ToString();
            x.createTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
            {
                try
                {
                    var thisGuy = ds.SYS_User.SingleOrDefault(d => d.userName == username && d.enable == "1");
                    if (thisGuy != null)
                    {
                        if (thisGuy.password == password)
                        {
                            if (thisGuy.enable == "1" || thisGuy.enable == "true")
                            {
                                x.userName = username;
                                x.name = thisGuy.name;
                                x.action = "登录成功";
                                thisGuy.lastTime = x.createTime;
                                ds.SYS_LoginNote.InsertOnSubmit(x);
                                ds.SubmitChanges();
                                returnData.success = true;
                                returnData.message = "success";
                                returnData.userID = thisGuy.id;
                                returnData.districtName = ds.SYS_District.SingleOrDefault(d => d.id == thisGuy.district).districtName;
                                var districtLevel = ds.SYS_District.SingleOrDefault(d => d.id == thisGuy.district);
                                if (districtLevel.districtLevel == 3)
                                {
                                    var y = from a in ds.PAR_Activity
                                            join b in ds.PAR_ActivityPerform on a.id equals b.ActivityID into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            orderby a.month ascending
                                            where a.status == "1" && b2.status == "2"&& a.alarmTime != DateTime.Now.ToString("yyyy-MM-dd") && districtLevel.id == b2.districtID && Convert.ToDateTime(a.month) > DateTime.Now && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)
                                            select new PAR_WarningPara
                                            {
                                                time = a.month,
                                                title = a.title,
                                                type = a.type
                                            };
                                    var z = from a in ds.PAR_Activity
                                            orderby a.month ascending
                                            where a.status == "1" && a.alarmTime != DateTime.Now.ToString("yyyy-MM-dd") && Convert.ToDateTime(a.month) > DateTime.Now && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)
                                            select new PAR_WarningPara
                                            {
                                                time = a.month,
                                                title = a.title,
                                                type = a.type
                                            };
                                    var alarm = from a in ds.PAR_Activity
                                                orderby a.month ascending
                                                where a.status == "1" && a.alarmTime == DateTime.Now.ToString("yyyy-MM-dd")
                                                select new PAR_WarningPara
                                                {
                                                    time = a.month,
                                                    title = a.title,
                                                    type = a.type
                                                };
                                    z = z.Except(y);
                                    z = z.Concat(alarm);
                                    returnData.rows = z.ToArray();
                                }
                                returnData.districtID = thisGuy.district;
                                ds.Dispose(); return returnData;
                            }
                            else
                            {
                                x.userName = username;
                                x.action = "账号被禁用，请联系管理员";
                                ds.SYS_LoginNote.InsertOnSubmit(x);
                                ds.SubmitChanges();
                                returnData.success = false;
                                returnData.message = "登录失败：账号被禁用，请联系管理员！";
                                ds.Dispose(); return returnData;
                            }
                        }
                        else
                        {
                            x.userName = username;
                            x.action = "密码错误";
                            ds.SYS_LoginNote.InsertOnSubmit(x);
                            ds.SubmitChanges();
                            returnData.success = false;
                            returnData.message = "登录失败：密码错误！";
                            ds.Dispose(); return returnData;
                        }
                    }
                    else
                    {
                        x.userName = username;
                        x.action = "查无此人";
                        ds.SYS_LoginNote.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.success = false;
                        returnData.message = "登录失败：查无此人！";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception ex)
                {
                    x.userName = username;
                    x.action = ex.ToString();
                    ds.SYS_LoginNote.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.success = false;
                    returnData.message = "登录失败：" + ex.Message;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                x.userName = username;
                x.action = "信息不完整！";
                ds.SYS_LoginNote.InsertOnSubmit(x);
                ds.SubmitChanges();
                returnData.success = false;
                returnData.message = "登录失败：请输入完整的信息！";
                ds.Dispose(); return returnData;
            }
        }


        /*-----------------查询组织树---------*/
        public CommonOutPutT_M<PAR_Organ<string[]>[]> getOrganTree()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_Organ<string[]>[]> returnData = new CommonOutPutT_M<PAR_Organ<string[]>[]>();
            try
            {
                var x = from a in ds.SYS_District
                        where a.districtLevel == 2
                        orderby a.id descending
                        select new PAR_Organ<string[]>
                        {
                            id = a.id,
                            name = a.districtName,
                            SubOrgan = (from b in ds.SYS_District
                                        where b.districtLevel == 3 && b.attachTo == a.id
                                        orderby b.id descending
                                        select b.districtName).ToArray()
                        };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray().Length;
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

        /*-----------------查看组织详情---------*/
        public CommonOutPutT_M<PAR_Organ<string[]>[]> getOrganDetail()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_Organ<string[]>[]> returnData = new CommonOutPutT_M<PAR_Organ<string[]>[]>();
            try
            {
                var x = from a in ds.SYS_District
                        where a.districtLevel == 2
                        orderby a.id descending
                        select new PAR_Organ<string[]>
                        {
                            id = a.id,
                            name = a.districtName,
                            SubOrgan = (from b in ds.SYS_District
                                        where b.districtLevel == 3 && b.attachTo == a.id
                                        orderby b.id descending
                                        select b.districtName).ToArray()
                        };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray().Length;
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

        /*-----------------新增简介---------*/
        public CommonOutput addBriefIntriduction(string districtID, string description)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(districtID) && !string.IsNullOrEmpty(description))
            {

                try
                {
                    var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
                    if (thisDistrict != null)
                    {
                        thisDistrict.description = description;
                        ds.SubmitChanges();
                        returnData.message = "success";
                        returnData.success = true;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.message = "区域不存在！";
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



        #region 用户管理

        /*-----------------查询区域---------*/
        public CommonOutPutT_M<App_Organs[]> getOrgan(string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<App_Organs[]> returnData = new CommonOutPutT_M<App_Organs[]>();
            var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
            //搜索是否为空
            if (!String.IsNullOrEmpty(districtID))
            {
                try
                {
                    if (thisDistrict.districtLevel == 2)
                    {
                        IQueryable<App_Organs> x = from a in ds.SYS_District
                                                   where a.id == districtID
                                                   select new App_Organs
                                                   {
                                                       id = a.id,
                                                       name = a.districtName
                                                   };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray().Length;
                        returnData.rows = x.ToArray();
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        IQueryable<App_Organs> x = from a in ds.SYS_District
                                                   orderby a.id descending
                                                   where a.id == districtID || a.attachTo == districtID
                                                   select new App_Organs
                                                   {
                                                       id = a.id,
                                                       name = a.districtName
                                                   };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray().Length;
                        returnData.rows = x.ToArray();
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "提示:" + ex;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.success = false;
                returnData.message = "提示:请输入有效地址！";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------查询区域（本级以及下级）---------*/
        public CommonOutPutT_M<App_Organs[]> getOrgan2(string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<App_Organs[]> returnData = new CommonOutPutT_M<App_Organs[]>();
            var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
            //搜索是否为空
            if (!String.IsNullOrEmpty(districtID))
            {
                try
                {
                        IQueryable<App_Organs> x = from a in ds.SYS_District
                                                   orderby a.id descending
                                                   where a.id == districtID || a.attachTo == districtID
                                                   select new App_Organs
                                                   {
                                                       id = a.id,
                                                       name = a.districtName
                                                   };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray().Length;
                        returnData.rows = x.ToArray();
                        ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "提示:" + ex;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.success = false;
                returnData.message = "提示:请输入有效地址！";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------新增用户---------*/
        public CommonOutput addUser(string name, string phone, string password, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            string temp = "Eric";
            if (!string.IsNullOrEmpty(districtID) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
            {

                try
                {
                    var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);

                    int i = Convert.ToInt32(thisDistrict.subDistrictNum) + 1;
                    var t = "0"; temp = thisDistrict.subDistrictNum;
                    switch (i)
                    {
                        case 1: t = "01"; break;
                        case 2: t = "02"; break;
                        case 3: t = "03"; break;
                        case 4: t = "04"; break;
                        case 5: t = "05"; break;
                        case 6: t = "06"; break;
                        case 7: t = "07"; break;
                        case 8: t = "08"; break;
                        case 9: t = "09"; break;
                        default: t = i.ToString(); break;

                    }
                    SYS_District x = new SYS_District();
                    x.id = String.Format("{0}{1}", districtID, t);
                    x.attachTo = districtID;
                    x.districtName = name;
                    x.subDistrictNum = "0";
                    x.districtLevel = thisDistrict.districtLevel + 1;


                    var thisClient = ds.SYS_User.SingleOrDefault(d => d.userName == phone && d.enable == "1");
                    if (thisClient == null)
                    {
                        SYS_User y = new SYS_User();
                        y.id = Guid.NewGuid().ToString();
                        y.phone = phone;
                        y.enable = "1";
                        y.password = password;
                        y.name = name;
                        y.userName = name;
                        y.roleID = x.districtLevel == 2? "镇级管理员": "村级管理员";
                        y.district = x.id;
                        POP_Basic z = new POP_Basic();
                        z.id = y.id;
                        z.name = name;
                        z.phone = phone;
                        z.password = password;
                        z.districtID = x.id;
                        z.status = 1;
                        ds.SYS_District.InsertOnSubmit(x);
                        ds.SYS_User.InsertOnSubmit(y);
                        ds.POP_Basic.InsertOnSubmit(z);
                        thisDistrict.subDistrictNum = (Convert.ToInt32(thisDistrict.subDistrictNum) + 1).ToString();
                        ds.SubmitChanges();
                        returnData.message = "success";
                        returnData.success = true;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.message = "账号已存在！";
                        returnData.success = false;
                        ds.Dispose(); return returnData;

                    }
                }
                catch (Exception ex)
                {
                    returnData.message = temp+"Error:" +ex;
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

        /*-----------------编辑用户(注：修改用户即修改区域ID，此时应该把与待修改的ID相关联数据全部改成新的ID)---------*/
        public CommonOutput editUser(string id, string name, string phone, string password, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
            {

                try
                {
                    var thisClient = ds.SYS_User.SingleOrDefault(d => d.id == id);
                    var thisPop = ds.POP_Basic.SingleOrDefault(d => d.id == id);
                    //检测区域变化
                    var thisDistrict = ds.SYS_District.SingleOrDefault(d=>d.id == thisClient.district);
                    if (thisDistrict.attachTo == districtID)              //区域等级不变,只需改变名字和其他参数
                    {
                        thisDistrict.districtName = name;
                        if (thisClient != null)
                        {
                            thisClient.name = name;
                            thisClient.userName = name;
                            thisClient.password = password;
                            thisClient.phone = phone;
                            thisClient.district = thisClient.district;
                            thisClient.roleID = thisDistrict.districtLevel == 2 ? "镇级管理员" : "村级管理员";
                            thisPop.name = name;
                            thisPop.phone = phone;
                            thisPop.password = password;
                            thisPop.districtID = thisClient.district;
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
                    else                                                 //区域等级改变，需要修改原所属区域组织树，并修改当前所属组织
                    {
                        var district = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
                        int i = Convert.ToInt32(district.subDistrictNum) + 1;
                        var t = "0";
                        switch (i)
                        {
                            case 1: t = "01"; break;
                            case 2: t = "02"; break;
                            case 3: t = "03"; break;
                            case 4: t = "04"; break;
                            case 5: t = "05"; break;
                            case 6: t = "06"; break;
                            case 7: t = "07"; break;
                            case 8: t = "08"; break;
                            case 9: t = "09"; break;
                            default: t = i.ToString(); break;

                        }
                        SYS_District x = new SYS_District();
                        x.id = String.Format("{0}{1}", districtID, t);
                        x.attachTo = districtID;
                        x.districtName = name;
                        x.subDistrictNum = "0";
                        x.districtLevel = district.districtLevel + 1;
                        if (thisClient != null)
                        {
                            //修改任务完成列表区域
                            var thisPartyActivity = from a in ds.PAR_ActivityPerform
                                                    where a.districtID == thisClient.district
                                                    select a;
                            foreach (var j in thisPartyActivity)
                            {
                                j.districtID = x.id;
                            }
                            //修改电视任务完成图片列表区域
                            var thisPartyActivityPicture = from a in ds.PAR_Picture_Infro
                                                    where a.PartyBranch == thisDistrict.districtName
                                                    select a;
                            foreach (var w in thisPartyActivityPicture)
                            {
                                w.PartyBranch = name;
                            }
                            //修改电视监控所属区域
                            var thisBox = from a in ds.PAR_Picutre
                                          where a.districtID == thisClient.district
                                          select a;
                            foreach (var m in thisBox)
                            {
                                m.districtID = x.id;
                                m.name = name;
                            }
                            //修改基本组织所属区域
                            var thisTis = from a in ds.TIS_basic
                                          where a.districtID == thisClient.district
                                          select a;
                            foreach (var n in thisTis)
                            {
                                n.districtID = x.id;
                            }
                            //修改基本阵地所属区域
                            var thisPOS = from a in ds.POS_basic
                                          where a.districtID == thisClient.district
                                          select a;
                            foreach (var b in thisPOS)
                            {
                                b.districtID = x.id;
                            }
                            //修改基本制度所属区域
                            var thisINF = from a in ds.INF_information
                                          where a.districtID == thisClient.district
                                          select a;
                            foreach (var c in thisINF)
                            {
                                c.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisBrand = from a in ds.PAR_Brand
                                            where a.districtID == thisClient.district
                                          select a;
                            foreach (var m in thisBrand)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisPAR_MS_List = from a in ds.PAR_MS_List
                                                  where a.districtID == thisClient.district
                                            select a;
                            foreach (var m in thisPAR_MS_List)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisLeadingShip = from a in ds.POP_LeadingShip
                                                  where a.districtID == thisClient.district
                                            select a;
                            foreach (var m in thisLeadingShip)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisReserveCadre = from a in ds.POP_ReserveCadre
                                                   where a.districtID == thisClient.district
                                                  select a;
                            foreach (var m in thisReserveCadre)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisVolunteer = from a in ds.POP_Volunteer
                                                where a.districtID == thisClient.district
                                                  select a;
                            foreach (var m in thisVolunteer)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisConference = from a in ds.TEA_Conference
                                                 where a.districtID == thisClient.district
                                                select a;
                            foreach (var m in thisConference)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisVolunteerActivity = from a in ds.TEA_VolunteerActivity
                                                        where a.districtID == thisClient.district
                                                select a;
                            foreach (var m in thisVolunteerActivity)
                            {
                                m.districtID = x.id;
                            }
                            //修改电视监控所属区域
                            var thisCheckLeadingShip = from a in ds.POP_CheckLeadingShip
                                                where a.districtID == thisClient.district
                                                select a;
                            foreach (var m in thisCheckLeadingShip)
                            {
                                m.districtID = x.id;
                            }
                            thisClient.name = name;
                            thisClient.userName = name;
                            thisClient.password = password;
                            thisClient.phone = phone;
                            thisClient.district = x.id;
                            thisClient.roleID = thisDistrict.districtLevel == 2 ? "镇级管理员" : "村级管理员";
                            thisPop.name = name;
                            thisPop.phone = phone;
                            thisPop.password = password;
                            thisPop.districtID = x.id;
                            district.subDistrictNum = (Convert.ToInt32(district.subDistrictNum) + 1).ToString();
                            ds.SYS_District.DeleteOnSubmit(thisDistrict);
                            ds.SYS_District.InsertOnSubmit(x);
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

        /*-----------------查询用户列表---------*/
        public CommonOutPutT_M<SYS_UserList[]> getUserList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<SYS_UserList[]> returnData = new CommonOutPutT_M<SYS_UserList[]>();
            var thisDistrict = ds.SYS_District.SingleOrDefault(d => d.id == districtID);
            if(thisDistrict.districtLevel == 1)
            {
                //搜索是否为空
                if (!String.IsNullOrEmpty(search))
                {
                    //排序字段是否为空
                    if (!String.IsNullOrEmpty(sort))
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = null;
                            if (order == "asc" || order == "")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.userName ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.lastTime ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
                            else if (order == "desc")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.userName descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.lastTime descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
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
                    else
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = from a in ds.SYS_User
                                                         join b in ds.SYS_District on a.district equals b.id into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where  a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                                         orderby a.name descending
                                                         select new SYS_UserList
                                                         {
                                                             id = a.id,
                                                             district = a.district,
                                                             enable = a.enable,
                                                             lastTime = a.lastTime,
                                                             name = a.name,
                                                             password = a.password,
                                                             portrait = a.portrait,
                                                             roleID = a.roleID,
                                                             userName = a.userName,
                                                             department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                             phone = a.phone,
                                                             attachDistrict = b2.attachTo
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
                            returnData.message = "Error:" + ex.Message;
                            ds.Dispose(); return returnData;
                        }
                    }
                }
                else
                {
                    //排序字段是否为空
                    if (!String.IsNullOrEmpty(sort))
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = null;
                            if (order == "asc" || order == "")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.userName ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.lastTime ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
                            else if (order == "desc")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.userName descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.lastTime descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where  a.enable == "1"
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
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
                    else
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = from a in ds.SYS_User
                                                         join b in ds.SYS_District on a.district equals b.id into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where  a.enable == "1"
                                                         orderby a.name descending
                                                         select new SYS_UserList
                                                         {
                                                             id = a.id,
                                                             district = a.district,
                                                             enable = a.enable,
                                                             lastTime = a.lastTime,
                                                             name = a.name,
                                                             password = a.password,
                                                             portrait = a.portrait,
                                                             roleID = a.roleID,
                                                             userName = a.userName,
                                                             department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                             phone = a.phone,
                                                             attachDistrict = b2.attachTo
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
                            returnData.message = "Error:" + ex.Message;
                            ds.Dispose(); return returnData;
                        }
                    }
                }
            }

            else {
                //搜索是否为空
                if (!String.IsNullOrEmpty(search))
                {
                    //排序字段是否为空
                    if (!String.IsNullOrEmpty(sort))
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = null;
                            if (order == "asc" || order == "")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.userName ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.lastTime ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
                            else if (order == "desc")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.userName descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.lastTime descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
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
                    else
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = from a in ds.SYS_User
                                                         join b in ds.SYS_District on a.district equals b.id into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1" && (a.name.Contains(search) || a.userName.Contains(search) || a.lastTime.Contains(search))
                                                         orderby a.name descending
                                                         select new SYS_UserList
                                                         {
                                                             id = a.id,
                                                             district = a.district,
                                                             enable = a.enable,
                                                             lastTime = a.lastTime,
                                                             name = a.name,
                                                             password = a.password,
                                                             portrait = a.portrait,
                                                             roleID = a.roleID,
                                                             userName = a.userName,
                                                             department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                             phone = a.phone,
                                                             attachDistrict = b2.attachTo
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
                            returnData.message = "Error:" + ex.Message;
                            ds.Dispose(); return returnData;
                        }
                    }
                }
                else
                {
                    //排序字段是否为空
                    if (!String.IsNullOrEmpty(sort))
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = null;
                            if (order == "asc" || order == "")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.userName ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.lastTime ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.name ascending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
                            else if (order == "desc")
                            {
                                switch (sort)
                                {
                                    case "name":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "userName":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.userName descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    case "lastTime":
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.lastTime descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                    default:
                                        x = from a in ds.SYS_User
                                            join b in ds.SYS_District on a.district equals b.id into b1
                                            from b2 in b1.DefaultIfEmpty()
                                            where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                            orderby a.name descending
                                            select new SYS_UserList
                                            {
                                                id = a.id,
                                                district = a.district,
                                                enable = a.enable,
                                                lastTime = a.lastTime,
                                                name = a.name,
                                                password = a.password,
                                                portrait = a.portrait,
                                                roleID = a.roleID,
                                                userName = a.userName,
                                                department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                phone = a.phone,
                                                attachDistrict = b2.attachTo
                                            };
                                        break;
                                }
                            }
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
                    else
                    {
                        try
                        {
                            IQueryable<SYS_UserList> x = from a in ds.SYS_User
                                                         join b in ds.SYS_District on a.district equals b.id into b1
                                                         from b2 in b1.DefaultIfEmpty()
                                                         where (b2.attachTo == districtID || b2.id == districtID) && a.enable == "1"
                                                         orderby a.name descending
                                                         select new SYS_UserList
                                                         {
                                                             id = a.id,
                                                             district = a.district,
                                                             enable = a.enable,
                                                             lastTime = a.lastTime,
                                                             name = a.name,
                                                             password = a.password,
                                                             portrait = a.portrait,
                                                             roleID = a.roleID,
                                                             userName = a.userName,
                                                             department = ds.SYS_District.SingleOrDefault(d => d.id == b2.attachTo).districtName,
                                                             phone = a.phone,
                                                             attachDistrict = b2.attachTo
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
                            returnData.message = "Error:" + ex.Message;
                            ds.Dispose(); return returnData;
                        }
                    }
                }
            }
        }

        /*-----------------登出用户---------*/
        public CommonOutput deleteUser(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            var x = ds.SYS_User.SingleOrDefault(d => d.id == id);
            if (x.id == "01")
            {
                returnData.success = false;
                returnData.message = "超级管理员账号，请勿删除！";
                ds.Dispose(); return returnData;
            }
            else
            {
                var y = ds.POP_Basic.SingleOrDefault(d => d.id == id);
                var z = ds.SYS_District.SingleOrDefault(d => d.id == x.district);
                //var supDistrict = ds.SYS_District.SingleOrDefault(d => d.id == z.attachTo);
                //supDistrict.subDistrictNum = (Convert.ToInt32(supDistrict.subDistrictNum) - 1).ToString();
                ds.SYS_District.DeleteOnSubmit(z);
                ds.SYS_User.DeleteOnSubmit(x);
                ds.POP_Basic.DeleteOnSubmit(y);
                ds.SubmitChanges();
                returnData.success = true;
                returnData.message = "Success";
                ds.Dispose(); return returnData;
            }
        }

        #endregion


        #region 党员管理

        /*-----------------新增党员---------*/
        public CommonOutput addPolitic(string name, string IDCard, string phone, string sex, string nation, string workPlace, string educationDegree, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(districtID))
            {

                try
                {
                    var thisClient = ds.POP_Basic.SingleOrDefault(d => d.name == name && d.IDCard == IDCard);
                    if (thisClient == null)
                    {
                        POP_Basic x = new POP_Basic();
                        x.id = Guid.NewGuid().ToString();
                        x.name = name;
                        x.IDCard = IDCard;
                        x.phone = phone;
                        x.sex = sex;
                        x.nation = nation;
                        x.workPlace = workPlace;
                        x.educationDegree = educationDegree;
                        x.districtID = districtID;
                        x.status = 1;
                        ds.POP_Basic.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.message = "success";
                        returnData.success = true;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.message = "账号已存在！";
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

        /*-----------------编辑党员---------*/
        public CommonOutput editPolitic(string id, string name, string IDCard, string phone, string sex, string nation, string workPlace, string educationDegree, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
            {

                try
                {
                    var thisPop = ds.POP_Basic.SingleOrDefault(d => d.id == id);
                    if (thisPop != null)
                    {
                        thisPop.name = name;
                        thisPop.IDCard = IDCard;
                        thisPop.nation = nation;
                        thisPop.sex = sex;
                        thisPop.phone = phone;
                        thisPop.workPlace = workPlace;
                        thisPop.educationDegree = educationDegree;
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

        /*-----------------查询党员列表---------*/
        public CommonOutPutT_M<POP_Basic[]> getPoliticList(string districtID, int offset, int limit, string order, string search)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<POP_Basic[]> returnData = new CommonOutPutT_M<POP_Basic[]>();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                try
                {
                    IQueryable<POP_Basic> x = from a in ds.POP_Basic
                                              where a.districtID == districtID && a.status == 1 && a.name.Contains(search)
                                              orderby a.id descending
                                              select a;
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
            else
            {
                try
                {
                    IQueryable<POP_Basic> x = from a in ds.POP_Basic
                                              where a.districtID == districtID && a.status == 1
                                              orderby a.id descending
                                              select a;
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
        }

        /*-----------------登出党员---------*/
        public CommonOutput deletePolitic(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            var y = ds.POP_Basic.SingleOrDefault(d => d.id == id);
            if (y != null)
            {
                y.status = 0;
                ds.SubmitChanges();
            }
            returnData.success = true;
            returnData.message = "Success";
            ds.Dispose(); return returnData;
        }

        /*-----------------查看党员上传活动---------*/
        public CommonOutPutT_M<ActivityPicture[]> getActivityByPolitic(string districtID, string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<ActivityPicture[]> returnData = new CommonOutPutT_M<ActivityPicture[]>();
            try
            {
                var x = from a in ds.PAR_Activity
                        join c in ds.PAR_ActivityFeedback on a.id equals c.snId into c1
                        from c2 in c1.DefaultIfEmpty()
                        join d in ds.POP_Basic on c2.userId equals d.id into d1
                        from d2 in d1.DefaultIfEmpty()
                        join e in ds.SYS_District on d2.districtID equals e.id into e1
                        from e2 in e1.DefaultIfEmpty()
                        orderby c2.time descending
                        where a.id == id && e2.id == districtID && c2.flag == "0"
                        select new ActivityPicture
                        {
                            id = c2.id,
                            userName = d2.name,
                            content = c2.context,
                            time = c2.time,
                            URL = (from b in ds.PAR_ActivityPicture
                                   where c2.id == b.activityID
                                   select b.ImageUrl).ToArray()
                        };
                returnData.success = true;
                returnData.message = "Success";
                returnData.total = x.ToArray().Length;
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

        #endregion
        
        #region 监控管理

        /*-----------------列表查看监控---------*/
        public CommonOutPutT_M<PAR_Picutre[]> getCameraList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<PAR_Picutre[]> returnData = new CommonOutPutT_M<PAR_Picutre[]>();
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<PAR_Picutre> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "name":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.name ascending
                                        select a;
                                    break;
                                case "number":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.number ascending
                                        select a;
                                    break;
                                default:
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.name ascending
                                        select a;
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "name":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.name descending
                                        select a;
                                    break;
                                case "number":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.number descending
                                        select a;
                                    break;
                                default:
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                        orderby a.name descending
                                        select a;
                                    break;
                            }
                        }

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_Picutre>().Length;
                        x = x.Skip<PAR_Picutre>(offset);
                        x = x.Take<PAR_Picutre>(limit);
                        returnData.rows = x.ToArray<PAR_Picutre>();
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
                        IQueryable<PAR_Picutre> x = from a in ds.PAR_Picutre
                                                    join b in ds.SYS_District on a.districtID equals b.id into b1
                                                    from c in b1.DefaultIfEmpty()
                                                    where a.status != "0" && (a.name.Contains(search) || a.number.Contains(search) || a.IP.Contains(search) || a.districtID.Contains(search))
                                                    orderby a.name descending
                                                    select a;

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_Picutre>().Length;
                        x = x.Skip<PAR_Picutre>(offset);
                        x = x.Take<PAR_Picutre>(limit);
                        returnData.rows = x.ToArray<PAR_Picutre>();
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
                        IQueryable<PAR_Picutre> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "name":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.name ascending
                                        select a;
                                    break;
                                case "number":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.number ascending
                                        select a;
                                    break;
                                default:
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.name ascending
                                        select a;
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "name":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.name descending
                                        select a;
                                    break;
                                case "number":
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.number descending
                                        select a;
                                    break;
                                default:
                                    x = from a in ds.PAR_Picutre
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from c in b1.DefaultIfEmpty()
                                        where a.status != "0"
                                        orderby a.name descending
                                        select a;
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_Picutre>().Length;
                        x = x.Skip<PAR_Picutre>(offset);
                        x = x.Take<PAR_Picutre>(limit);
                        returnData.rows = x.ToArray<PAR_Picutre>();
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
                        IQueryable<PAR_Picutre> x = x = from a in ds.PAR_Picutre
                                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                                        from c in b1.DefaultIfEmpty()
                                                        where a.status != "0"
                                                        orderby a.name descending
                                                        select a;
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<PAR_Picutre>().Length;
                        x = x.Skip<PAR_Picutre>(offset);
                        x = x.Take<PAR_Picutre>(limit);
                        returnData.rows = x.ToArray<PAR_Picutre>();
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
        /*-----------------新增监控---------*/
        public CommonOutput addCamera(string name, string districtID, string IP, string number, string time)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(IP))
            {
                try
                {
                    var x = new PAR_Picutre();
                    x.id = Guid.NewGuid().ToString();
                    x.name = name;
                    x.districtID = districtID;
                    x.IP = IP;
                    x.number = number;
                    x.time = time;
                    x.status = "1";
                    ds.PAR_Picutre.InsertOnSubmit(x);
                    ds.SubmitChanges();
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
        /*-----------------编辑监控---------*/
        public CommonOutput editCamera(string id, string districtID, string name, string IP,  string number, string time)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(IP) && !string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.PAR_Picutre.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        x.name = name;
                        x.IP = IP;
                        x.number = number;
                        x.districtID = districtID;
                        x.time = time;
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
        /*-----------------删除监控---------*/
        public CommonOutput deleteCamera(string id,string password)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id)|| !string.IsNullOrEmpty(password))
            {
                try
                {
                    var isAdmin = ds.SYS_User.SingleOrDefault(d => d.id == "1" && d.password == password);
                    if (isAdmin == null)
                    {
                        returnData.success = false;
                        returnData.message = "密码验证错误！";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        var x = ds.PAR_Picutre.SingleOrDefault(d => d.id == id);
                        if (x != null)
                        {
                            ds.PAR_Picutre.DeleteOnSubmit(x);
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
        /*-----------------截图参数设置---------*/
        public CommonOutput editPictureCatchInfo(string time)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(time))
            {
                try
                {
                    var t = new PAR_Time();
                    t.id = Guid.NewGuid().ToString();
                    t.nowTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    t.time = time;
                    t.stauts = 1;
                    ds.PAR_Time.InsertOnSubmit(t);
                    ds.SubmitChanges();
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
        /*-----------------查看监控截图时间---------*/
        public CommonOutputAppT<string> getPictureCatchInfo()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<string> returnData = new CommonOutputAppT<string>();
            try
            {
                var x = (from a in ds.PAR_Time
                         where a.stauts == 1
                         orderby a.nowTime descending
                         select a.time).ToArray();
                if (x.Length == 0)
                {
                    returnData.IsOk = 1;
                    returnData.Msg = "Success";
                    returnData.data = "0";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 1;
                    returnData.Msg = "Success";
                    returnData.data = x[0];
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 1;
                returnData.Msg = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        #endregion

    }
}
