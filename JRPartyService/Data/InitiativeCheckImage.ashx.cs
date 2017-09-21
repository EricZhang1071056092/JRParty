using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class InitiativeCheckImage : IHttpHandler
{
    private static MLDSService.Dynamic d = new MLDSService.Dynamic();
    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        try
        {
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string path, filePath, ImageUrl;
            string InitiativeCheckID = Guid.NewGuid().ToString();
            //记录随手拍数据

            for (var i = 0; i < file.Length; i++)
            {
                string id = Guid.NewGuid().ToString();
                path = context.Server.MapPath("..\\Upload\\InitiativeCheck");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                filePath = path + "\\" + id + ".png";

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                file[i] = context.Request.Files[i];
                file[i].SaveAs(filePath);//存储图片完毕
                ImageUrl = id + ".png";
                d.saveInitiativeCheckImage(InitiativeCheckID, ImageUrl);

            }
            result = ("{\"IsOk\":\"1\",\"Msg\":\"上传成功\",\"InitiativeCheckID\":\"" + InitiativeCheckID + "\"}");

        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"0\",\"Msg\":\"上传失败:" + ex.Message + "\",\"InitiativeCheckID\":\"null\"}");
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
