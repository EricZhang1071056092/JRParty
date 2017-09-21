using System;
using System.Web;
using NPOI;
using MLDSService;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 保存头像文件
/// </summary>
[System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
[System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]

public class ExcelImport : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        StringBuilder errorMsg = new StringBuilder(); // 错误信息
        string result = "";
        try
        {

            #region 1.获取Excel文件并转换为一个List集合

            // 1.1存放Excel文件到本地服务器
            HttpPostedFile[] file = new HttpPostedFile[context.Request.Files.Count];//不确定文件数组; // 获取上传的文件
            string fileType = context.Request.Params["fileType"].ToString(); // 保存文件并获取文件路径
            string path, filePath;
            path = context.Server.MapPath("..\\Upload\\Excel");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            filePath = path + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            file[0] = context.Request.Files[0];
            file[0].SaveAs(filePath);//存储图片完毕
            // 单元格抬头
            // key：实体对象属性名称，可通过反射获取值
            // value：属性对应的中文注解
            switch (fileType)
            {
                case "building"://房屋
                    Dictionary<string, string> cellheaderBuilding = new Dictionary<string, string> {
                    { "districtID", "区域" },
                    { "plot", "小区/组" },
                    { "houseNum", "门牌号" },
                    { "x", "纬度" },
                    { "y", "经度" },
                    { "structure", "房屋结构" },
                    { "floor", "层数" },
                    { "roomNum", "房间号" },
                };
                    List<MLDSService.DataContracts.Excel_Building> enlistBuilding = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_Building>(cellheaderBuilding, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveBuilding(enlistBuilding);
                    break;
                case "fitBuilding"://匹配房屋人口
                    Dictionary<string, string> cellheaderFitBuilding = new Dictionary<string, string> {
                        { "name", "姓名" },
                        { "IDCard", "身份证" },
                        { "plot", "小区/组" },
                        { "houseNum", "门牌号" },
                        { "structure", "房屋结构" },
                        { "floor", "层数" },
                        { "roomNum", "房间号" },
                        { "districtID", "区域" },
                        { "x", "纬度" },
                        { "y", "经度" },
                };
                    List<MLDSService.DataContracts.Excel_FitBuilding> enlistFitBuilding = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_FitBuilding>(cellheaderFitBuilding, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelFitBuilding(enlistFitBuilding);
                    break;
                case "population"://基础人口
                    Dictionary<string, string> cellheaderPopulation = new Dictionary<string, string> {
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "phone", "电话" },
                    { "districtID", "区域" },
                    { "sex", "性别" },
                    { "nation", "民族" },
                    { "marriageStatus", "婚姻状况" },
                    { "politicsStatus", "政治面貌" },
                    { "censusRegister", "户籍地址" },
                    { "plot", "小区/组" },
                    { "houseNum", "门牌号" },
                    { "structure", "房屋结构" },
                    { "floor", "楼层" },
                    { "roomNum", "房间号" },
                    { "bookletNum", "户口本号" },
                    { "populationType", "人口类型" },
                    { "workPlace", "工作单位" },
                    { "educationDegree", "教育程度" },
                    { "relationship", "与户主关系" }
                };
                    List<MLDSService.DataContracts.Excel_Population> enlistPopulation = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_Population>(cellheaderPopulation, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSavePopulation(enlistPopulation);
                    break;
                case "partyMember"://党员
                    Dictionary<string, string> cellheaderpartyMember = new Dictionary<string, string> {
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "joinTime", "入党时间" },
                    { "department", "所属支部" }
                };
                    List<MLDSService.DataContracts.Excel_PartyMember> enlistPartyMember = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_PartyMember>(cellheaderpartyMember, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSavePartyMember(enlistPartyMember);
                    break;
                case "military"://民兵
                    Dictionary<string, string> cellheaderMilitary = new Dictionary<string, string> {
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "joinTime", "入伍时间" },
                    { "leaveTime", "退伍时间" },
                    { "isActive", "是否现役" },
                    { "isBasic", "是否基干民兵" },
                    { "isDisable", "是否伤残民兵" }
                };
                    List<MLDSService.DataContracts.Excel_Military> enlistMilitary = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_Military>(cellheaderMilitary, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveMilitary(enlistMilitary);
                    break;
                case "leader"://工作人员
                    Dictionary<string, string> cellheaderLeader = new Dictionary<string, string> {
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "duty", "职务" }
                };
                    List<MLDSService.DataContracts.Excel_Leader> enlistLeader = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_Leader>(cellheaderLeader, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveLeader(enlistLeader);
                    break;
                case "ChildrenInsurance"://少儿医保
                    Dictionary<string, string> cellheaderChildrenInsurance = new Dictionary<string, string> {
                    { "institutionID", "单位编号" },
                    { "peopleID", "人员编号" },
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "sex", "性别" },
                    { "accountProperty", "户口性质" },
                    { "studentNum", "学号" },
                    { "exemptionType", "免缴种类" },
                    { "exemptionID", "免缴号码" },
                    { "contract", "联系人" },
                    { "phone", "联系方式" },
                    { "year", "年份" }
                };
                    List<MLDSService.DataContracts.Excel_ChildrenInsurance> ChildrenInsurance = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_ChildrenInsurance>(cellheaderChildrenInsurance, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveChildrenInsurance(ChildrenInsurance);
                    break;
                case "RuralInsurance"://农村医疗
                    Dictionary<string, string> cellheaderRuralInsurance = new Dictionary<string, string> {
                    { "peopleID", "人员编号" },
                    { "name", "姓名" },
                    { "IDCard", "身份证" },
                    { "sex", "性别" },
                    { "phone", "联系方式" },
                    { "exemptionType", "免缴种类" },
                    { "exemptionID", "免缴号码" },
                    { "year", "年份" }
                };
                    List<MLDSService.DataContracts.Excel_RuralInsurance> RuralInsurance = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_RuralInsurance>(cellheaderRuralInsurance, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveRuralInsurance(RuralInsurance);
                    break;
                case "Disabled"://残疾人
                    Dictionary<string, string> cellheaderDisabled = new Dictionary<string, string> {
                    { "name", "姓名" },
                    { "sex", "性别" },
                    { "IDCard", "身份证" },
                    { "disableNum", "残疾证号" },
                    { "disablelevel", "残疾等级" },
                    { "relieffunds", "救助金额" },
                    { "guardian", "监护人" },
                    { "explain", "备注说明" },
                };
                    List<MLDSService.DataContracts.Excel_Disabled> Disabled = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_Disabled>(cellheaderDisabled, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveDisabled(Disabled);
                    break;
                case "farmLand"://农田
                    Dictionary<string, string> cellheaderFarmLand = new Dictionary<string, string> {
                    { "farmLandID", "塘口号" },
                    { "name", "姓名" },
                    { "address", "塘口地址" },
                    { "IDCard", "身份证" },
                    { "area", "区域" }
                };
                    List<MLDSService.DataContracts.Excel_FarmLand> enlistFarmLand = MLDSService.Methods.ExcelHelper.ExcelToEntityList<MLDSService.DataContracts.Excel_FarmLand>(cellheaderFarmLand, filePath, out errorMsg);
                    MLDSService.Methods.ExcelAnalysis.excelSaveFarmLand(enlistFarmLand);
                    break;
            }

            #endregion
            result = ("{\"IsOk\":\"1\",\"Msg\":\"上传成功\"}");

        }
        catch (Exception ex)
        {
            result = ("{\"IsOk\":\"1\",\"Msg\":\"" + ex.Message + "\"}");
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

