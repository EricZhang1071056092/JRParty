using JRPartyService;
using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class editBrand : IHttpHandler
{
    private static JRPartyService.Brand d = new JRPartyService.Brand();
    public void ProcessRequest(HttpContext context)
    {

        string result = "";
        try
        {
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string path, filePath, Url;
            string id, title, description, districtID;
            id = context.Request.Params["id"];
            title = context.Request.Params["title"];
            description = context.Request.Params["description"];
            districtID = context.Request.Params["districtID"];
            //记录随手拍数据
            var returnData = d.editBrand( id, title,  description,  districtID);
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
                            var returnData2 = d.AddBrandPicture(id, Url);
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
