using System;
using System.Web;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class ActivityReplenish : IHttpHandler
{
    private static JRPartyService.Dynamic d = new JRPartyService.Dynamic();
    public void ProcessRequest(HttpContext context)
    {

        string result = "";
        try
        {
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组

            string path, filePath, ImageUrl;
            string districtID, snId, content, flag;
            districtID = context.Request.Params["districtID"];
            snId = context.Request.Params["snId"];
            content = context.Request.Params["content"];
            flag = context.Request.Params["flag"];

            //记录随手拍数据
            var returnData = d.replenishActivity(districtID, snId, content, flag);
            if (returnData.IsOk == 1)
            {
                //存储图片
                int fileLen = file.Length;
                if (fileLen > 9) fileLen = 9;
                if (fileLen > 0)
                {
                    if (!string.IsNullOrEmpty(context.Request.Files[0].FileName))
                    {
                        for (var i = 0; i < fileLen; i++)
                        {
                            string id = Guid.NewGuid().ToString();
                            path = context.Server.MapPath("..\\Upload\\PhotoTake");
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
                            var returnData2 = d.appPhoto2PhotoTake(returnData.data, ImageUrl);
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
                result = ("{\"IsOk\":\"0\",\"Msg\":\"Error:" + returnData.Msg + "\"}");
            }
        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"0\",\"Msg\":\"上传失败:" + ex + "\"}");
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
