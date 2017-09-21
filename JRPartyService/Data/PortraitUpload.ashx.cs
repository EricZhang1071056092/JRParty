using System;
using System.Web;
using JRPartyService;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class PortraitUpload : IHttpHandler
{
    private static Dynamic d = new Dynamic();
    public void ProcessRequest(HttpContext context)
    {
        
        string result = "";
        try
        {
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string path, filePath, ImageUrl;
            string clientID = context.Request.Params["clientID"].ToString();
            path = context.Server.MapPath("..\\Upload\\Portrait");
            string id = Guid.NewGuid().ToString();
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            filePath = path + "\\" + id + ".png";

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            file[0] = context.Request.Files[0];
            file[0].SaveAs(filePath);//存储图片完毕
            ImageUrl = id + ".png";
            var returnDate = d.savePortrait(clientID, ImageUrl);
            result = ("{\"IsOk\":\"" + returnDate.IsOk + "\",\"Msg\":\"" + returnDate.Msg + "\",\"Data\":{\"imageURL\":\"" + returnDate.data + "\"}}");        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"0\",\"Msg\":\"上传失败:" + ex + "\",\"Data\":{\"imageURL\":\"null\"}}");
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
