using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.Web;
using NPOI.SS.UserModel;
using System.Collections;
using MLDSData;

namespace MLDSService.Methods
{
    public static class ExcelAnalysis
    {
        private static MLDSDataContext ds = new MLDSDataContext();
        /*-------------房屋导入----------------*/
        public static void excelSaveBuilding(List<DataContracts.Excel_Building> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_Building en = enlist[i];
                //查询是否已存在此人
                var thisBuilding = ds.POP_Building.SingleOrDefault(d => d.plot == en.plot && d.houseNum == en.houseNum && d.structure == en.structure && d.floor == en.floor && d.roomNum == en.roomNum);
                if (thisBuilding == null)
                {
                    //新增
                    var x = new POP_Building();
                    x.id = Guid.NewGuid().ToString();
                    x.districtID = en.districtID;
                    x.plot = en.plot;
                    x.houseNum = en.houseNum;
                    x.x = en.x;
                    x.y = en.y;
                    x.structure = en.structure;
                    x.floor = en.floor;
                    x.roomNum = en.roomNum;
                    ds.POP_Building.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();

            //for (int i = 0; i < enlist.Count; i++)
            //{
            //    MLDSService.DataContracts.List_BasicPopulation enA = enlist[i];
            //    if (enA.IsExcelVaildateOK == false) // 上面验证不通过，不进行此步验证
            //    {
            //        continue;
            //    }

            //    for (int j = i + 1; j < enlist.Count; j++)
            //    {
            //        MLDSService.DataContracts.List_BasicPopulation enB = enlist[j];
            //        // 判断必填列是否全部重复
            //        if (enA.Name == enB.Name)
            //        {
            //            enA.IsExcelVaildateOK = false;
            //            enB.IsExcelVaildateOK = false;
            //            errorMsg.AppendLine("第" + (i + 1) + "行与第" + (j + 1) + "行的必填列重复了");
            //        }
            //    }
            //}
            
        }

        /*-------------基础人口导入--------------*/
        public static void excelSavePopulation(List<DataContracts.Excel_Population> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_Population en = enlist[i];
                //查询是否已存在此人
                var thisBuilding = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisBuilding == null)
                {
                    //新增
                    var x = new POP_Basic();
                    x.id = Guid.NewGuid().ToString();
                    x.districtID = en.districtID;
                    x.name = en.name;
                    x.IDCard = en.IDCard;
                    x.sex = en.sex;
                    x.nation = en.nation;
                    x.bookletNum = en.bookletNum;
                    x.relationship = en.relationship;
                    x.censusRegister = en.censusRegister;
                    x.populationType = en.populationType;
                    x.marriageStatus = en.marriageStatus;
                    x.politicsStatus = en.politicsStatus;
                    x.educationDegree = en.educationDegree;
                    x.status = 1;
                    x.phone = en.phone;
                    x.workPlace = en.workPlace;
                    var address = ds.POP_Building.Where(d => d.districtID == en.districtID && d.houseNum == en.houseNum).ToArray();
                    if (address.Length > 0)
                    {
                        x.addressID = address[0].id;
                    }
                    if (x.relationship == "户主")
                    {
                        var y = new POP_Family();
                        y.id = Guid.NewGuid().ToString();
                        y.addressID = x.addressID;
                        y.bookletNum = x.bookletNum;
                        y.bookletType = x.populationType;
                        y.districtID = x.districtID;
                        y.houseHolderID = x.id;
                        y.status = 1;
                        ds.POP_Family.InsertOnSubmit(y);
                    }
                    ds.POP_Basic.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();

        }

        /*-------------党员导入---------------*/
        public static void excelSavePartyMember(List<DataContracts.Excel_PartyMember> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_PartyMember en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new POP_Party();
                    x.id = Guid.NewGuid().ToString();
                    x.districtID = thisGuy.districtID;
                    x.populationID = thisGuy.id;
                    x.department = en.department;
                    x.joinTime = en.joinTime;
                    x.status = 1;
                    ds.POP_Party.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();
        }
        /*-------------民兵导入---------------*/
        public static void excelSaveMilitary(List<DataContracts.Excel_Military> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_Military en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new POP_Military();
                    x.id = Guid.NewGuid().ToString();
                    x.districtID = thisGuy.districtID;
                    x.populationID = thisGuy.id;
                    x.isActive = en.isActive;
                    x.isBasic = en.isBasic;
                    x.isDisable = en.isDisable;
                    x.leaveTime = en.leaveTime;
                    x.joinTime = en.joinTime;
                    x.status = 1;
                    ds.POP_Military.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();
        }
        /*-------------工作人员导入---------------*/
        public static void excelSaveLeader(List<DataContracts.Excel_Leader> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_Leader en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new POP_Leader();
                    x.id = Guid.NewGuid().ToString();
                    x.districtID = thisGuy.districtID;
                    x.populationID = thisGuy.id;
                    x.duty = en.duty;
                    x.status = 1;
                    ds.POP_Leader.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();
        }
        /*-------------少儿医保导入---------------*/
        public static void excelSaveChildrenInsurance(List<DataContracts.Excel_ChildrenInsurance> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_ChildrenInsurance en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new BUS_ChildrenInsurance();
                    x.id = Guid.NewGuid().ToString();
                    x.populationID = thisGuy.id;
                    x.districtID = thisGuy.districtID;
                    x.institutionID = en.institutionID;
                    x.peopleID = en.peopleID;
                    x.name = en.name;
                    x.IDCard = en.IDCard;
                    x.sex = en.sex;
                    x.accountProperty = en.accountProperty;
                    x.studentNum = en.studentNum;
                    x.exemptionType = en.exemptionType;
                    x.exemptionID = en.exemptionID;
                    x.contract = en.contract;
                    x.phone = en.phone;
                    x.year = en.year;
                    x.status = "1";
                    ds.BUS_ChildrenInsurance.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();
        }
        /*-------------农村医疗导入---------------*/
        public static void excelSaveRuralInsurance(List<DataContracts.Excel_RuralInsurance> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_RuralInsurance en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new BUS_RuralInsurance();
                    x.id = Guid.NewGuid().ToString();
                    x.populationID = thisGuy.id;
                    x.districtID = thisGuy.districtID;
                    x.peopleID = en.peopleID;
                    x.name = en.name;
                    x.IDCard = thisGuy.IDCard;
                    x.sex = thisGuy.sex;
                    x.exemptionType = en.exemptionType;
                    x.exemptionID = en.exemptionID;
                    x.phone = thisGuy.phone;
                    x.year = en.year;
                    x.status = "1";
                    ds.BUS_RuralInsurance.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }

            }
            ds.SubmitChanges();
        }

        /*-------------残疾人导入---------------*/
        public static void excelSaveDisabled(List<DataContracts.Excel_Disabled> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_Disabled en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    //新增
                    var x = new POP_Disabled();
                    x.id = Guid.NewGuid().ToString();
                    x.populationID = thisGuy.id;
                    x.districtID = thisGuy.districtID;
                    x.disableNum = en.disableNum;
                    x.disablelevel = en.disablelevel;
                    x.relieffunds = en.relieffunds;
                    x.guardian = en.guardian;
                    x.explain = en.explain;
                    ds.POP_Disabled.InsertOnSubmit(x);
                }
                else
                {
                    continue;
                }
            }
            ds.SubmitChanges();
        }
        /*-------------农田导入---------------*/
        public static void excelSaveFarmLand(List<DataContracts.Excel_FarmLand> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_FarmLand en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                var thisFamily = new POP_Family();
                var x = new AGR_FarmLand();
                x.id = Guid.NewGuid().ToString();
                x.address = en.address;
                x.area = en.area;
                x.farmLandID = en.farmLandID;
                if (thisGuy != null)
                {
                    x.populationID = thisGuy.id;
                    x.districtID = thisGuy.districtID;
                    thisFamily = ds.POP_Family.SingleOrDefault(d => d.bookletNum == thisGuy.bookletNum && d.districtID == thisGuy.districtID);
                    if (thisFamily != null)
                    {
                        x.familyID = thisFamily.id;
                    }
                }
                ds.AGR_FarmLand.InsertOnSubmit(x);
            }
            ds.SubmitChanges();
        }   
        /*-------------楼栋二次匹配---------------*/
        public static void excelFitBuilding(List<DataContracts.Excel_FitBuilding> enlist)
        {
            for (int i = 0; i < enlist.Count; i++)
            {
                DataContracts.Excel_FitBuilding en = enlist[i];
                //查询是否已存在此人
                var thisGuy = ds.POP_Basic.SingleOrDefault(d => d.name == en.name && d.IDCard == en.IDCard);
                if (thisGuy != null)
                {
                    var thisBuilding = ds.POP_Building.SingleOrDefault(d => d.id == thisGuy.addressID);
                    //var thisFamily = ds.POP_Family.SingleOrDefault(d => d.bookletNum == thisGuy.bookletNum);
                    if (thisBuilding != null)
                    {
                        thisBuilding.x = en.x;
                        thisBuilding.y = en.y;
                    }
                    else
                    {
                        var thisFamily = ds.POP_Family.SingleOrDefault(d => d.houseHolderID == thisGuy.id);
                        if (thisFamily != null)
                        {
                            var x = new POP_Building();
                            x.id = Guid.NewGuid().ToString();
                            thisFamily.addressID = x.id;
                            x.districtID = thisGuy.districtID;
                            x.x = en.x;
                            x.y = en.y;
                            x.structure = "1";
                            x.houseNum = thisGuy.bookletNum;
                            x.floor = en.floor;
                            x.plot = en.plot;
                            x.roomNum = en.plot;                            
                            ds.POP_Building.InsertOnSubmit(x);
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                else
                {
                    continue;
                }
            }
            ds.SubmitChanges();
        }
    }
}
