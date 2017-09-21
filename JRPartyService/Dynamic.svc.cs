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
    public class Dynamic : IDynamic
    {

        #region 登录注册

        /*--------------APP推送验证码(flag= 0注册;flag =1修改密码)------------*/
        public CommonOutputAppT<App_Code> sendMessage(string phone, string flag)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_Code> returnData = new CommonOutputAppT<App_Code>();
            try
            {
                var x = ds.POP_Basic.SingleOrDefault(d => d.phone == phone);
                if (x != null && flag == "0")
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "您已注册！请直接登陆";
                    ds.Dispose(); return returnData;
                }
                else if (x != null && flag == "1")
                {
                    Random Random = new Random();
                    int Result = Random.Next(1000, 9999);
                    sendMessageToSubmail(phone, Result);
                    var code = new App_Code();
                    code.code = Result;
                    code.phone = phone;
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    returnData.data = code;
                    ds.Dispose(); return returnData;
                }
                else if (x == null && flag == "0")
                {
                    Random Random = new Random();
                    int Result = Random.Next(1000, 9999);
                    sendMessageToSubmail(phone, Result);
                    var code = new App_Code();
                    code.code = Result;
                    code.phone = phone;
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    returnData.data = code;
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "您尚未注册！";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*--------------接入第三方短信服务------------*/
        public void sendMessageToSubmail(string phone, int Result)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            try
            {

                IAppConfig appConfig = new MessageConfig("14708", "ea48d9dca5d53195a7943c583f77a4fc");
                MessageXSend messageXSend = new MessageXSend(appConfig);
                messageXSend.AddTo(phone);
                messageXSend.SetProject("n4XkQ3");
                messageXSend.AddVar("code", Result.ToString());
                string returnMessage = string.Empty;
                if (messageXSend.XSend(out returnMessage) == false)
                {
                    Console.WriteLine(returnMessage);
                }

            }
            catch (Exception)
            {

            }
        }

        /*--------------组织列表------------*/
        public CommonOutputAppT<App_Organs[]> appGetOrganization()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_Organs[]> returnData = new CommonOutputAppT<App_Organs[]>();
            try
            {
                var x = from a in ds.SYS_District
                        select new App_Organs
                        {
                            id = a.id,
                            name = a.districtName,
                        };
                returnData.data = x.ToArray<App_Organs>();
                returnData.IsOk = 1;
                returnData.Msg = "success";
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }

        /*--------------APP注册------------*/
        public CommonOutputAppT<App_Login> userRegist(string phone, string password, string orgination, string name, string IDCard)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_Login> returnData = new CommonOutputAppT<App_Login>();
            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(IDCard))
            {

                try
                {
                    var thisClient = ds.POP_Basic.SingleOrDefault(d => d.phone == phone);
                    if (thisClient == null)
                    {
                        POP_Basic y = new POP_Basic();
                        y.phone = phone;
                        y.id = Guid.NewGuid().ToString();
                        y.name = name;
                        y.IDCard = IDCard;
                        y.status = 1;
                        y.password = password;
                        y.districtID = orgination;
                        ds.POP_Basic.InsertOnSubmit(y);
                        ds.SubmitChanges();
                        returnData.IsOk = 1;
                        returnData.Msg = "success";
                        App_Login x = new App_Login();
                        x.userID = y.id;
                        x.name = y.name;
                        x.department = ds.SYS_District.SingleOrDefault(d => d.id == y.districtID).districtName;
                        x.districtID = y.districtID;
                        x.userImage = "";
                        returnData.data = x;
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.IsOk = 0;
                        returnData.Msg = "该用户已存在！";
                        ds.Dispose(); return returnData;

                    }
                }
                catch (Exception ex)
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error：" + ex.Message;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "输入信息不全！";
                ds.Dispose(); return returnData;
            }
        }

        /*--------------APP登陆------------*/
        public CommonOutputAppT<App_Login> appLogin(string Account, string PassWord)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_Login> returnData = new CommonOutputAppT<App_Login>();
            if (!string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(PassWord))
            {
                try
                {

                    var thisClient = ds.POP_Basic.SingleOrDefault(d => d.phone == Account);
                    if (thisClient != null)
                    {
                        if (thisClient.password == PassWord)
                        {
                            var x = from a in ds.POP_Basic
                                    join c in ds.SYS_District on a.districtID.Substring(0, 4) equals c.id into c1
                                    from c2 in c1.DefaultIfEmpty()
                                    where a.status == 1 && a.phone == Account && a.password == PassWord
                                    select new App_Login
                                    {
                                        userID = a.id,
                                        name = a.name,
                                        department = c2.districtName,
                                        userImage = a.portrait,
                                        districtID = a.districtID,
                                        flag = ds.SYS_User.SingleOrDefault(d => d.name == a.name && d.userName == a.phone) == null ? "0" : "1"
                                    };
                            var y = x.ToArray<App_Login>();
                            returnData.data = y[0];
                            returnData.IsOk = 1;
                            returnData.Msg = "success";
                            ds.Dispose(); return returnData;
                        }
                        else
                        {
                            returnData.IsOk = 0;
                            returnData.Msg = "密码错误！";
                            ds.Dispose(); return returnData;
                        }
                    }
                    else
                    {
                        returnData.IsOk = 0;
                        returnData.Msg = "查无此人！";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception ex)
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error：" + ex.Message;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "输入信息不全！";
                ds.Dispose(); return returnData;
            }
        }

        ///*--------------APP重置密码------------*/
        //public CommonOutputApp setPassword(string phone, string password)
        //{
        //    JRPartyDataContext ds = new JRPartyDataContext();
        //    CommonOutputApp returnData = new CommonOutputApp();
        //    if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(password))
        //    {

        //        try
        //        {
        //            var thisClient = ds.DYNC_Client.SingleOrDefault(d => d.phone == phone);
        //            thisClient.phone = phone;
        //            thisClient.password = password;
        //            ds.SubmitChanges();
        //            returnData.IsOk = 1;
        //            returnData.Msg = "success";
        //            ds.Dispose(); return returnData;
        //        }
        //        catch (Exception ex)
        //        {
        //            returnData.IsOk = 0;
        //            returnData.Msg = "Error：" + ex.Message;
        //            ds.Dispose(); return returnData;
        //        }

        //    }
        //    else
        //    {
        //        returnData.IsOk = 0;
        //        returnData.Msg = "输入信息不全！";
        //        ds.Dispose(); return returnData;
        //    }
        //}

        /*--------------APP获取轮播信息------------*/
        public CommonOutputAppT<App_Information[]> appGetInformation()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_Information[]> returnData = new CommonOutputAppT<App_Information[]>();
            try
            {
                var x = from a in ds.PAR_Information
                        where a.status != "0"
                        select new App_Information
                        {
                            title = a.title,
                            Detail = a.detail,
                            picture = a.ImageURL
                        };
                returnData.data = x.ToArray();
                returnData.IsOk = 1;
                returnData.Msg = "success";
                ds.Dispose(); return returnData;

            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        /*--------------APP获取即将过期活动------------*/
        public CommonOutputAppT<PAR_WarningPara[]> appGetWarningPara(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<PAR_WarningPara[]> returnData = new CommonOutputAppT<PAR_WarningPara[]>();
            if (!string.IsNullOrEmpty(id))
            {
               
                try
                {
                    var thisClient = ds.POP_Basic.SingleOrDefault(d => d.id == id);
                    var y = from a in ds.PAR_Activity
                            join b in ds.PAR_ActivityPerform on a.id equals b.ActivityID into b1
                            from b2 in b1.DefaultIfEmpty()
                            orderby a.month ascending
                            where a.status == "1" && b2.status == "2"&&thisClient.districtID==b2.districtID&& a.alarmTime != DateTime.Now.ToString("yyyy-MM-dd") && Convert.ToDateTime(a.month) > DateTime.Now && Convert.ToDateTime(a.month) < DateTime.Now.AddDays(7)
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
                    var x = from a in ds.PAR_Activity
                            where a.status =="1"&& a.alarmTime == DateTime.Now.ToString("yyyy-MM-dd")
                            orderby a.month ascending
                            select new PAR_WarningPara
                            {
                                time = a.month,
                                title = a.title,
                                type = a.type
                            };
                    z = z.Except(y);
                    x = x.Concat(z);
                    returnData.data = x.ToArray();
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;

                }
                catch (Exception ex)
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error：" + ex.Message;
                    ds.Dispose(); return returnData;
                }

            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "输入信息不全！";
                ds.Dispose(); return returnData;
            }
        }

        /*--------------APP获取新闻列表------------*/
        public CommonOutputAppT<App_InformationList[]> appGetInformationList(string PageIndex)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_InformationList[]> returnData = new CommonOutputAppT<App_InformationList[]>();
            try
            {
                var x = from a in ds.PAR_Information
                        where a.status != "0"
                        orderby Convert.ToDateTime(a.time) ascending
                        select new App_InformationList
                        {
                            title = a.title,
                            Detail = a.detail,
                            picture = a.ImageURL,
                            content = a.context,
                            time = a.time,
                            department = a.department
                        };
                x = x.Skip((Convert.ToInt32(PageIndex) - 1) * 10);
                x = x.Take(10);
                returnData.data = x.ToArray();
                returnData.IsOk = 1;
                returnData.Msg = "success";
                ds.Dispose(); return returnData;

            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        #endregion

        #region 任务管理

        /*--------------APP获取任务列表------------*/
        public CommonOutputAppT<App_ActivityList[]> appGetActivityList(string userId, string PageIndex)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_ActivityList[]> returnData = new CommonOutputAppT<App_ActivityList[]>();
            try
            {

                if (!string.IsNullOrEmpty(userId))
                {
                    var x = from b in ds.PAR_Activity
                            join  c in  ds.POP_Basic on userId equals c.id into c1 
                            from c2 in c1.DefaultIfEmpty()
                                                where b.status == "1" &&(b.districtID =="01"||b.districtID == c2.districtID)
                                                select new App_ActivityList
                                                {
                                                    id = b.id,
                                                    title = b.title,
                                                    content = b.context,
                                                    address = "",
                                                    status = b.status,
                                                    time = b.month,
                                                    type = b.type,
                                                    department = ""
                                                };
                   
                    x = x.Skip((Convert.ToInt32(PageIndex) - 1) * 10);
                    x = x.Take(10);
                    returnData.data = x.ToArray();
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "信息不全";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        /*--------------APP任务签到------------*/
        public CommonOutputApp appActivitySign(string userId, string snId)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputApp returnData = new CommonOutputApp();
            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(snId))
                {
                    var x = new PAR_ActivitySign();
                    x.id = Guid.NewGuid().ToString();
                    x.personID = userId;
                    x.time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    x.ActivityID = snId;
                    ds.PAR_ActivitySign.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "信息不全";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        /*--------------APP任务上报------------*/
        public CommonOutputAppT<string> appAddActivity(string userId, string snId, string content, string flag)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<string> returnData = new CommonOutputAppT<string>();
            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(snId))
                {
                    var x = new PAR_ActivityFeedback();
                    
                    var district = ds.POP_Basic.SingleOrDefault(d => d.id == userId);
                    var thisActivity = (from a in ds.PAR_ActivityPerform where a.ActivityID == snId && a.districtID == district.districtID select a).ToArray();
                    x.id = Guid.NewGuid().ToString();
                    x.userId = userId;
                    x.time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    x.snId = snId;
                    x.context = content;
                    x.flag = flag;
                    if (thisActivity.Length == 0)
                    {
                        var y = new PAR_ActivityPerform();
                        y.id = Guid.NewGuid().ToString();
                        y.ActivityID = snId;
                        y.districtID = district.districtID;
                        y.status = "1";
                        ds.PAR_ActivityPerform.InsertOnSubmit(y);
                    }
                    else if (thisActivity[0].status == "3")
                    {

                        thisActivity[0].status = "1";
                    }
                    else
                    { }
                    ds.PAR_ActivityFeedback.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.data = x.id;
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "信息不全";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }


        /*--------------APP任务上报图片-------------*/
        public CommonOutput appPhoto2PhotoTake(string activityID, string ImageUrl)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(activityID) || !string.IsNullOrEmpty(ImageUrl))
            {
                try
                {
                    var x = new PAR_ActivityPicture();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = activityID;
                    x.ImageUrl = ImageUrl;
                    ds.PAR_ActivityPicture.InsertOnSubmit(x);
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

        /*--------------任务材料补充上报（超管权限）------------*/
        public CommonOutputAppT<string> replenishActivity(string districtID, string snId, string content, string flag)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<string> returnData = new CommonOutputAppT<string>();
            try
            {
                if (!string.IsNullOrEmpty(districtID) && !string.IsNullOrEmpty(snId))
                {
                    var x = new PAR_ActivityFeedback();

                    var district = ds.POP_Basic.SingleOrDefault(d => d.districtID == districtID);
                    var thisActivity = (from a in ds.PAR_ActivityPerform where a.ActivityID == snId && a.districtID == districtID select a).ToArray();
                    var activity = ds.PAR_Activity.SingleOrDefault(d => d.id == snId);
                    x.id = Guid.NewGuid().ToString();
                    x.userId = district.id;
                    x.time = activity.month;
                    x.snId = snId;
                    x.context = content;
                    x.flag = flag;
                    if (thisActivity.Length == 0)
                    {
                        var y = new PAR_ActivityPerform();
                        y.id = Guid.NewGuid().ToString();
                        y.ActivityID = snId;
                        y.districtID = district.districtID;
                        y.status = "2";
                        ds.PAR_ActivityPerform.InsertOnSubmit(y);
                    }
                    ds.PAR_ActivityFeedback.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.data = x.id;
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "信息不全";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }


        /*-----------------任务查看---------*/

        /*-----------------任务执行---------*/

        #endregion
        
        #region 扶贫管理

        /*-----------------扶贫查看---------*/


        #endregion
        
        #region 个人中心

        /*-----------------版本更新---------*/
        public CommonOutputApp appAddVersion(string versionNum, string flag)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputApp returnData = new CommonOutputApp();
            if (!string.IsNullOrEmpty(versionNum))
            {
                if (!string.IsNullOrEmpty(flag))
                {
                    try
                    {
                        var x = new SYS_Version();
                        x.id = Guid.NewGuid().ToString();
                        x.versionNum = versionNum;
                        x.time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        x.flag = flag;
                        ds.SYS_Version.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.IsOk = 1;
                        returnData.Msg = "success";
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.IsOk = 0;
                        returnData.Msg = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error:未识别设备信息！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error:版本号获取失败！";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------版本信息---------*/
        public CommonOutputAppT<string> getVersion()
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<string> returnData = new CommonOutputAppT<string>();
            try
            {
                var x = (from a in ds.SYS_Version
                         orderby a.time descending
                         select a.versionNum).ToArray();
                returnData.data = x[0];
                returnData.IsOk = 1;
                returnData.Msg = "success";
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        /*-----------------软件反馈---------*/
        public CommonOutputApp appAddFeedback(string clientID, string districtID, string feedbackContent)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputApp returnData = new CommonOutputApp();
            if (!string.IsNullOrEmpty(clientID))
            {
                if (!string.IsNullOrEmpty(feedbackContent))
                {
                    try
                    {
                        var x = new PAR_Feedback();
                        x.id = Guid.NewGuid().ToString();
                        x.clientID = clientID;
                        x.createTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        x.districtID = districtID;
                        x.feedbackContent = feedbackContent;
                        x.status = 1;
                        ds.PAR_Feedback.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.IsOk = 1;
                        returnData.Msg = "success";
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.IsOk = 0;
                        returnData.Msg = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error:上传失败，没有信息输入！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error:您的登录信息已过期，请重新登陆！";
                ds.Dispose(); return returnData;
            }
        }

        /*---------------保存头像-------------*/
        public CommonOutputAppT<string> savePortrait(string clientID, string ImageUrl)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<string> returnData = new CommonOutputAppT<string>();
            if (!string.IsNullOrEmpty(clientID))
            {
                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    try
                    {
                        var thisClient = ds.POP_Basic.SingleOrDefault(d => d.id == clientID);
                        if (thisClient != null)
                        {
                            thisClient.portrait = Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Portrait/" + ImageUrl;
                            ds.SubmitChanges();
                            returnData.data = thisClient.portrait;
                            returnData.IsOk = 1;
                            returnData.Msg = "success";
                            ds.Dispose(); return returnData;
                        }
                        else
                        {
                            returnData.IsOk = 0;
                            returnData.Msg = "Error：用户信息不存在，请重登陆！";
                            ds.Dispose(); return returnData;
                        }
                    }
                    catch (Exception ex)
                    {
                        returnData.IsOk = 0;
                        returnData.Msg = "Error：" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "Error：输入信息不全！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：用户信息不存在，请重登陆！";
                ds.Dispose(); return returnData;
            }
        }

        /*--------------我参与的任务------------*/
        public CommonOutputAppT<App_ActivityList[]> myActivityList(string userId, string PageIndex)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputAppT<App_ActivityList[]> returnData = new CommonOutputAppT<App_ActivityList[]>();
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var x = from a in ds.PAR_Activity
                            join d in ds.PAR_ActivitySign on a.id equals d.ActivityID into d1
                            from d2 in d1.DefaultIfEmpty()
                            where a.status != "0" && d2.personID == userId
                            orderby a.month descending
                            select new App_ActivityList
                            {
                                id = a.id,
                                title = a.title,
                                content = a.context,
                                time = a.month,
                                address = "句容党支部",
                                status = ds.PAR_ActivitySign.SingleOrDefault(d => d.ActivityID == a.id && d.personID == userId) == null ? "0" : "1",
                            };
                    x = x.Skip((Convert.ToInt32(PageIndex) - 1) * 10);
                    x = x.Take(10);
                    returnData.data = x.ToArray();
                    returnData.IsOk = 1;
                    returnData.Msg = "success";
                    ds.Dispose(); return returnData;
                }
                else
                {
                    returnData.IsOk = 0;
                    returnData.Msg = "信息不全";
                    ds.Dispose(); return returnData;
                }
            }
            catch (Exception ex)
            {
                returnData.IsOk = 0;
                returnData.Msg = "Error：" + ex.Message;
                ds.Dispose(); return returnData;
            }

        }

        #endregion
        
    }
}
