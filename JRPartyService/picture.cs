using System;
using System.Web;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JRPartyData;

namespace JumboTCMS.Utils
{
    /// <summary>  
    /// ffmpeg.exe调用  
    /// </summary>  
    public static class ffmpegHelp
    {
        /// <summary>  
        /// 生成FLV视频的缩略图  
        /// </summary>  
        /// <param name="vFileName">视频文件地址</param>  
        /// <param name="FlvImgSize">宽和高参数，如：240*180</param>  
        /// <returns></returns>  
        public static void CatchImg(string number, string vFileName, string FlvImgSize, string Second, string name, string StudyContent)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            try
            {
                //rtsp://[username]:[password]@[ip]:[port]/[codec]/[channel]/[subtype]/av_stream
                //rtsp://admin:admin_96296@172.16.0.31:557/h264/ch10/main/av_stream
                //rtsp://172.16.0.31:557/hikvision://172.16.1.130:8000:9:0?username=admin&password=admin_96296
                //" -i \"" + vFileName + "\" -rtsp_transport tcp -f image2 -an " + Second + " -t 0.1 -s " + FlvImgSize + " \"" + HttpContext.Current.Server.MapPath(flv_img_p) + "\"";
                //-i rtsp://admin:admin@172.16.0.220:554/h264/ch11/main/av_stream -rtsp_transport tcp -f image2 -an 10d.jpg"
                Random random = new Random();
                string flv_img_p = DateTime.Now.ToString("HHmmss")+ (random.Next(1000,9999)).ToString() + "_thumb.jpg";
                     //string Command = " -i \"" + vFileName + "\" -rtsp_transport tcp -f image2 -an " + Second + " -t 0.1 -s " + FlvImgSize + " \"" + flv_img_p + "\"";
                     // string Command = "-i rtsp://admin:admin@172.16.0.220:554/h264/" + vFileName + "/main/av_stream -rtsp_transport tcp -f image2 -ss 0 -vframes 100 " + flv_img_p + "";
                    string Command = "-y -i " + vFileName + " -f image2 -ss 0 -vframes 100 " + flv_img_p + "";
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = @"F:\JRPartyService\JRPartyService\bin\tools\ffmpeg.exe";
                    p.StartInfo.Arguments = Command;
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.StartInfo.WorkingDirectory = "F:\\JRPartyService\\JRPartyService\\picture";
                    //不创建进程窗口  
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();//启动线程  
                    p.WaitForExit();//等待完成  
                    p.Close();//关闭进程  
                    p.Dispose();//释放资源  
                    System.Threading.Thread.Sleep(4000);
                    //注意:图片截取成功后,数据由内存缓存写到磁盘需要时间较长,大概在3,4秒甚至更长;  
                    if (System.IO.File.Exists(flv_img_p))
                    {
                        return;
                    }
                    SavePicture(flv_img_p, name, StudyContent,number);
            }
            catch(Exception)
            {
            }
        }

        public static void SavePicture(string ImageURL, string PartyBranch, string StudyContent, string number)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
           
            try
            {
                if (!string.IsNullOrEmpty(ImageURL))
                {
                    var district = ds.PAR_Picutre.SingleOrDefault(d=>d.number==number);
                    var flag = ds.PAR_ActivityPerform.SingleOrDefault(d => d.districtID ==district.districtID&&d.ActivityID==StudyContent);
                    if (flag == null)
                    {
                        var activity = new PAR_ActivityPerform();
                        var picture = new PAR_Picture_Infro();
                        picture.id = Guid.NewGuid().ToString();
                        picture.ImageURL = ImageURL;
                        picture.PartyBranch = PartyBranch;
                        picture.StudyContent = StudyContent;
                        picture.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        activity.id = Guid.NewGuid().ToString();
                        activity.ActivityID = StudyContent;
                        activity.districtID = district.districtID;
                        activity.status = "1";
                        ds.PAR_Picture_Infro.InsertOnSubmit(picture);
                        ds.PAR_ActivityPerform.InsertOnSubmit(activity);
                        ds.SubmitChanges();
                    }
                    else if(flag.status =="3")
                    {
                        flag.status = "1";
                        var picture = new PAR_Picture_Infro();
                        picture.id = Guid.NewGuid().ToString();
                        picture.ImageURL = ImageURL;
                        picture.PartyBranch = PartyBranch;
                        picture.StudyContent = StudyContent;
                        picture.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        ds.PAR_Picture_Infro.InsertOnSubmit(picture);
                        ds.SubmitChanges();
                    }
                    else
                    {
                        var picture = new PAR_Picture_Infro();
                        picture.id = Guid.NewGuid().ToString();
                        picture.ImageURL = ImageURL;
                        picture.PartyBranch = PartyBranch;
                        picture.StudyContent = StudyContent;
                        picture.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        ds.PAR_Picture_Infro.InsertOnSubmit(picture);
                        ds.SubmitChanges();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}