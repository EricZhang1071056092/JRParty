using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JRPartyService
{
    public static class Tools
    {

        //-------md5加密-------
        public static string md5(string ConvertString)
        {
            string md5Pwd = string.Empty;

            //使用加密服务提供程序
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //将指定的字节子数组的每个元素的数值转换为它的等效十六进制字符串表示形式。
            md5Pwd = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);

            md5Pwd = md5Pwd.Replace("-", "");

            return md5Pwd;
        }

        //-------修改文件权限-------
        public static void addpathPower(string pathname, string username, string power)
        {

            DirectoryInfo dirinfo = new DirectoryInfo(pathname);

            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)
            {
                dirinfo.Attributes = FileAttributes.Normal;
            }

            //取得访问控制列表   
            DirectorySecurity dirsecurity = dirinfo.GetAccessControl();

            switch (power)
            {
                case "FullControl":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                    break;
                case "ReadOnly":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
                    break;
                case "Write":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));
                    break;
                case "Modify":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Modify, AccessControlType.Allow));
                    break;
            }
            dirinfo.SetAccessControl(dirsecurity);
        }

        //------权限检测------
        /*public static bool SecurityCheck(string id, string obj)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            bool result = false;
            try
            {
                if (id != "26135edc-8d30-11e7-8e20-507b9d5c5f6e")
                {
                    var code = (from u in ds.SYS_User
                                join r2 in ds.SYS_Role on u.roleID equals r2.id into r1
                                from r in r1.DefaultIfEmpty()
                                where u.id == id
                                select r.code).ToArray();
                    if (code != null)
                    {
                        switch (obj)
                        {
                            case "jdInformation":
                                result = jdAnalysis(code[0], 0);
                                break;
                            case "jdProblemIn":
                                result = jdAnalysis(code[0], 1);
                                break;
                            case "jdSetCase":
                                result = jdAnalysis(code[0], 2);
                                break;
                            case "jdUser":
                                result = jdAnalysis(code[0], 3);
                                break;
                            case "jdProblemM":
                                result = jdAnalysis(code[0], 4);
                                break;
                            case "jdVisit":
                                result = jdAnalysis(code[0], 5);
                                break;
                            case "jdOperation":
                                result = jdAnalysis(code[0], 8);
                                break;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            ds.Dispose();
            return result;
        }*/

        //------权限码解析------
        public static bool jdAnalysis(string code, int index)
        {
            try
            {
                var a = code.Substring(index, 1);
                if (a == "T")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //------权限码生成------
        public static string jdCreate(string jdInformation, string jdProblemIn, string jdSetCase, string jdUser, string jdProblemM, string jdVisit, string jdOperation)
        {
            try
            {
                string code = "";
                code = jdInformation == "true" ? code.Insert(0, "T") : code.Insert(0, "F");
                code = jdProblemIn == "true" ? code.Insert(1, "T") : code.Insert(1, "F");
                code = jdSetCase == "true" ? code.Insert(2, "T") : code.Insert(2, "F");
                code = jdUser == "true" ? code.Insert(3, "T") : code.Insert(3, "F");
                code = jdProblemM == "true" ? code.Insert(4, "T") : code.Insert(4, "F");
                code = jdVisit == "true" ? code.Insert(5, "T") : code.Insert(5, "F");
                code = code.Insert(6, "FF");
                code = jdOperation == "true" ? code.Insert(8, "T") : code.Insert(8, "F");
                return code;
            }
            catch
            {
                return "FFFFFFFFF";
            }
        }

        //------获取HTTP Referer------
        public static string GetReferer()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                return HttpContext.Current.Request.UrlReferrer.ToString();
            }
            else
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
            }
        }

        //------获取SERVERADDRESS------
        public static string GetSERVERADDRESS()
        {
            string host = HttpContext.Current.Request.Url.Host;
            int port = HttpContext.Current.Request.Url.Port;
            string serverAddress = "http://" + host + ":" + port;
            if (host == "122.97.218.162") serverAddress = "http://" + host + ":1" + port;
            return serverAddress;
        }

        //-------获取文件名-------
        public static string getFileName(string fileName)
        {
            string[] tempArr = fileName.Split('.');
            string result = "";
            for (var i = 0; i < tempArr.Length-1; i++)
            {
                result += tempArr[i];
            }
            return result;
        }

        //-------获取文件名后缀-------
        public static string getSuffix(string fileName)
        {
            string[] tempArr = fileName.Split('.');
            string result = "";
            foreach (var item in tempArr)
            {
                result = item;
            }
            return result;
        }
    }
}
