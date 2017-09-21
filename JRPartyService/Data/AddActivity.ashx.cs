using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class AddActivity : IHttpHandler
{
    private static JRPartyService.Party d = new JRPartyService.Party();
    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        try
        {
           // HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string title;
            string content;
            title = context.Request.Params["title"];
            content = context.Request.Params["content"];
            //存储图片

            var t = d.addPlan(title,content);
            if (t.success == true)
            {
                result = ("{\"IsOk\":\"1\",\"Msg\":\"上传成功\"}");
            }
            else
            {
                result = ("{\"IsOk\":\"0\",\"Msg\":\"上传失败\"}");
            }
        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"0\",\"Msg\":\"上传失败:" + ex.Message + "\"}");
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
