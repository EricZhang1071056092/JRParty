using JRPartyService;
using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class PositionUpload : IHttpHandler
{
    private static JRPartyService.Position d = new JRPartyService.Position();
    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组
        string result = "";
        try
        {
            bool check = !string.IsNullOrEmpty(context.Request.Params["type"]) && !string.IsNullOrEmpty(context.Request.Params["districtID"]);
            string[] checkSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };
            bool check2 = true;
            if (check)
            {
                int filesLen = file.Length;
                if (filesLen > 9) filesLen = 9;
                if (!string.IsNullOrEmpty(context.Request.Files[0].FileName))
                {
                    for (int i = 0; i < filesLen; i++)
                    {
                        if (Array.IndexOf(checkSuffix, Tools.getSuffix(context.Request.Files[i].FileName.ToLower())) == -1) check2 = false;
                    }
                    if (check2)
                    {
                        string path, filePath, Url;
                        string type, description, area, districtID;
                        type = context.Request.Params["type"];
                        description = context.Request.Params["description"];
                        area = context.Request.Params["area"];
                        districtID = context.Request.Params["districtID"];

                        var returnData = d.addPosition(type, description, area, districtID);
                        if (returnData.success)
                        {
                            for (var i = 0; i < filesLen; i++)
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
                                var returnData2 = d.AddPositionPicture(returnData.data, Url);
                                if (!returnData2.success) i = filesLen;
                                result = ("{\"IsOk\":\"1\",\"Msg\":\"" + returnData2.message + "\"}");
                            }
                        }
                        else
                        {
                            result = ("{\"IsOk\":\"0\",\"Msg\":\"" + returnData.message + "\"}");
                        }
                    }
                    else
                    {
                        result = ("{\"IsOk\":\"0\",\"Msg\":\"Error:上传文件中包含非法类型（仅支持图片上传）！\"}");
                    }
                }
                else
                {
                    result = ("{\"IsOk\":\"1\",\"Msg\":\"success\"}");
                }    
            }
            else
            {
                result = ("{\"IsOk\":\"0\",\"Msg\":\"Error:请补全请求信息！\"}");
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
