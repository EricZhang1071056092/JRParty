using JRPartyService;
using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class VolunteerUpload : IHttpHandler
{
    private static JRPartyService.Team d = new JRPartyService.Team();
    public void ProcessRequest(HttpContext context)
    {

        string result = "";
        try
        {
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string path, filePath, Url;
            string name, sex, IDCard, nation, birthDay, JionTime, workTime, duty, education, TrainingTitle, type, phone, financialType, districtID;
            name = context.Request.Params["name"];
            IDCard = context.Request.Params["IDCard"];
            sex = context.Request.Params["sex"];
            nation = context.Request.Params["nation"];
            birthDay = context.Request.Params["birthDay"];
            JionTime = context.Request.Params["JionTime"];
            workTime = context.Request.Params["workTime"];
            duty = context.Request.Params["duty"];
            education = context.Request.Params["education"];
            TrainingTitle = context.Request.Params["TrainingTitle"];
            type = context.Request.Params["type"];
            phone = context.Request.Params["phone"];
            financialType = context.Request.Params["financialType"];
            districtID = context.Request.Params["districtID"];
            //记录随手拍数据
            var returnData = d.addVolunteer(name, IDCard, sex, nation, birthDay, JionTime, workTime, duty, education, TrainingTitle, type, phone, financialType, districtID);
            if (returnData.success)
            {
                int fileLen = file.Length;
                if (fileLen > 9) fileLen = 9;
                if (fileLen > 0)
                {
                    if (!string.IsNullOrEmpty(context.Request.Files[0].FileName))
                    {
                        for (var i = 0; i < fileLen; i++)
                        {
                            path = context.Server.MapPath("..\\Upload\\Activity");
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            filePath = path + "\\" + context.Request.Files[i].FileName;

                            Url = context.Request.Files[i].FileName;
                            if (System.IO.File.Exists(filePath))
                            {
                                Url = Tools.getFileName(context.Request.Files[i].FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + Tools.getSuffix(context.Request.Files[i].FileName);
                                filePath = path + "\\" + Url;
                            }
                            file[i] = context.Request.Files[i];
                            file[i].SaveAs(filePath);//存储图片完毕
                            var returnData2 = d.AddVolunteerPicture(returnData.data, Url);
                            if (!returnData2.success) i = fileLen;
                            result = ("{\"IsOk\":\"1\",\"Msg\":\"" + returnData2.message + "\"}");
                        }
                    }
                    else
                    {
                        result = ("{\"IsOk\":\"1\",\"Msg\":\"success\"}");
                    }
                }
                else
                {
                    result = ("{\"IsOk\":\"1\",\"Msg\":\"success\"}");
                }
            }
            else
            {
                result = ("{\"IsOk\":\"0\",\"Msg\":\"" + returnData.message + "\"}");
            }

        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"0\",\"Msg\":\"Error:" + ex + "\"}");
        }
        context.Response.Write(result);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
