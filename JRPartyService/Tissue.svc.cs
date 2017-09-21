using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using JRPartyService.Methods;
using System.ServiceModel.Activation;
using System.Collections.Generic;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Tissue : ITissue
    {

        #region 组织机构

        /*-----------------查看组织1---------*/
        public CommonOutPutT_M<List_Tissue[]> getTissueList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Tissue[]> returnData = new CommonOutPutT_M<List_Tissue[]>();
            var subDistrict = CommonMethod.getSubDistrict(districtID);
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<List_Tissue> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "tissueName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.tissueName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "leaderName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.leaderName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "type":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.type ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                default:
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.tissueName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "tissueName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.tissueName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "leaderName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.leaderName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "type":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.type descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                default:
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.tissueName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Tissue>().Length;
                        x = x.Skip<List_Tissue>(offset);
                        x = x.Take<List_Tissue>(limit);
                        returnData.rows = x.ToArray<List_Tissue>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else
                {
                    try
                    {
                        IQueryable<List_Tissue> x = from a in ds.TIS_basic
                                                    join b in ds.SYS_District on a.districtID equals b.id into b1
                                                    from b2 in b1.DefaultIfEmpty()
                                                    where subDistrict.Contains(a.districtID) && (a.tissueName.Contains(search) || a.type.Contains(search) || a.leaderName.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                                    orderby a.tissueName descending
                                                    select new List_Tissue
                                                    {
                                                        id = a.id,
                                                        districtName = b2.districtName,
                                                        leaderName = a.leaderName,
                                                        type = a.type,
                                                        populationNum = a.populationNum,
                                                        tissueName = a.tissueName,
                                                        description = a.description,
                                                    };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Tissue>().Length;
                        x = x.Skip<List_Tissue>(offset);
                        x = x.Take<List_Tissue>(limit);
                        returnData.rows = x.ToArray<List_Tissue>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<List_Tissue> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "tissueName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.tissueName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "leaderName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.leaderName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.description ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby b2.districtName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "type":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.type ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                default:
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.tissueName ascending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "tissueName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.tissueName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "leaderName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.leaderName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.description descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby b2.districtName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                case "type":
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.type descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                                default:
                                    x = from a in ds.TIS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.tissueName descending
                                        select new List_Tissue
                                        {
                                            id = a.id,
                                            districtName = b2.districtName,
                                            leaderName = a.leaderName,
                                            type = a.type,
                                            populationNum = a.populationNum,
                                            tissueName = a.tissueName,
                                            description = a.description,
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Tissue>().Length;
                        x = x.Skip<List_Tissue>(offset);
                        x = x.Take<List_Tissue>(limit);
                        returnData.rows = x.ToArray<List_Tissue>();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
                else//二者都为空，即启动状态
                {

                    try
                    {
                        IQueryable<List_Tissue> x = from a in ds.TIS_basic
                                                    join b in ds.SYS_District on a.districtID equals b.id into b1
                                                    from b2 in b1.DefaultIfEmpty()
                                                    where subDistrict.Contains(a.districtID)
                                                    orderby a.tissueName ascending
                                                    select new List_Tissue
                                                    {
                                                        id = a.id,
                                                        districtName = b2.districtName,
                                                        leaderName = a.leaderName,
                                                        type = a.type,
                                                        populationNum = a.populationNum,
                                                        tissueName = a.tissueName,
                                                        description = a.description,
                                                    };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Tissue>().Length;
                        returnData.rows = (x.Skip(offset)).Take(limit).ToArray();
                        ds.Dispose(); return returnData;
                    }
                    catch (Exception ex)
                    {
                        returnData.success = true;
                        returnData.message = "Error:" + ex.Message;
                        ds.Dispose(); return returnData;
                    }
                }
            }

        }

        /*-----------------查看组织2---------*/
        public CommonOutputT<TissueList2> getTissueList2(string districtId)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<TissueList2> result = new CommonOutputT<TissueList2>();
            try
            {
                var subDistrict = CommonMethod.getSubDistrict(districtId);
                if (!string.IsNullOrEmpty(districtId))
                {
                    TissueList2 data = new TissueList2();
                    data.name = "句容市委";
                    SYS_District[] town = null;
                    List<TissueList2> tempList = new List<TissueList2>();
                    if (districtId.Length == 2)
                    {
                        town = (from t in ds.SYS_District
                                where t.attachTo == districtId && t.id.Length == 4
                                select t).ToArray();
                    }
                    else
                    {
                        town = (from t in ds.SYS_District where t.id == districtId select t).ToArray();
                    }
                    foreach (var item in town)
                    {
                        TissueList2 temp2 = new TissueList2();
                        List<TissueList2> tempList2 = new List<TissueList2>();
                        temp2.name = item.districtName;
                        var village = (from v in ds.SYS_District
                                       where v.attachTo == item.id && v.id.Length == 6
                                       select v).ToArray();
                        foreach (var item2 in village)
                        {
                            TissueList2 temp3 = new TissueList2();
                            List<TissueList2> tempList3 = new List<TissueList2>();
                            temp3.name = item2.districtName;
                            var t = (from a in ds.TIS_basic where a.districtID== item2.id select a).ToArray();
                            foreach (var item3 in t)
                            {
                                TissueList2 temp4 = new TissueList2();
                                temp4.name = item3.tissueName;
                                List<TissueList2> tempList4 = new List<TissueList2>();
                                TissueList2 tempL = new TissueList2();
                                TissueList2 tempP = new TissueList2();
                                TissueList2 tempT = new TissueList2();
                                TissueList2 tempD = new TissueList2();
                                tempL.name = "领导人";
                                tempL.title = item3.leaderName;
                                tempP.name = "人数";
                                tempP.title = item3.populationNum;
                                tempT.name = "类型";
                                tempT.title = item3.type;
                                tempD.name = "介绍";
                                tempD.title = item3.description;
                                tempList4.Add(tempL);
                                tempList4.Add(tempP);
                                tempList4.Add(tempT);
                                tempList4.Add(tempD);
                                temp4.children = tempList4.ToArray();
                                tempList3.Add(temp4);
                            }
                            temp3.children = tempList3.ToArray().Length > 0 ? tempList3.ToArray() : null;
                            tempList2.Add(temp3);
                        }
                        temp2.children = tempList2.ToArray().Length > 0 ? tempList2.ToArray() : null;
                        tempList.Add(temp2);
                    }
                    data.children = tempList.ToArray().Length > 0 ? tempList.ToArray() : null;
                    result.success = true;
                    result.message = "success";
                    result.data = data;
                }
                else
                {
                    result.message = "Error:请补全请求信息！";
                }
            }
            catch(Exception ex)
            {
                result.message = "Error:" + ex;
            }
            ds.Dispose();
            return result;
        }

        /*-----------------新增组织---------*/
        public CommonOutputT<string> addTissue(string leaderName, string populationNum, string tissueName, string description, string type, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(tissueName) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new TIS_basic();
                    x.id = Guid.NewGuid().ToString();
                    x.leaderName = leaderName;
                    x.populationNum = populationNum;
                    x.type = type;
                    x.tissueName = tissueName;
                    x.description = description;
                    x.districtID = districtID;
                    ds.TIS_basic.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.data = x.id;
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------编辑组织---------*/
        public CommonOutput editTissue(string id, string leaderName, string populationNum, string tissueName, string description, string type, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(tissueName) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = ds.TIS_basic.SingleOrDefault(d => d.id == id);
                    x.leaderName = leaderName;
                    x.populationNum = populationNum;
                    x.type = type;
                    x.tissueName = tissueName;
                    x.description = description;
                    x.districtID = districtID;
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------取消组织---------*/
        public CommonOutput deleteTissue(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.TIS_basic.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.TIS_basic.DeleteOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.success = false;
                        returnData.message = "信息有误！";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }

        #endregion

        #region 领导班子

        /*-----------------查看领导班子---------*/
        public CommonOutPutT_M<TIS_LeadingShip[]> getLeadershipList(string districtID, int offset, int limit, string order, string search, string sort, string AFName, string AFNation, string AFSex, string AFDuty, string AFEducation, string AFBirthday, string AFJionTime, string AFWorkTime, string AFType, string AFFinancialType)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<TIS_LeadingShip[]> returnData = new CommonOutPutT_M<TIS_LeadingShip[]>();
            var subDistrict = CommonMethod.getSubDistrict(districtID);
            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";

                if (string.IsNullOrEmpty(AFName)) AFName = "";
                if (string.IsNullOrEmpty(AFNation)) AFNation = "";
                if (string.IsNullOrEmpty(AFSex)) AFSex = "";
                if (string.IsNullOrEmpty(AFDuty)) AFDuty = "";
                if (string.IsNullOrEmpty(AFEducation)) AFEducation = "";
                if (string.IsNullOrEmpty(AFBirthday)) AFBirthday = "";
                if (string.IsNullOrEmpty(AFJionTime)) AFJionTime = "";
                if (string.IsNullOrEmpty(AFWorkTime)) AFWorkTime = "";
                if (string.IsNullOrEmpty(AFType)) AFType = "";
                if (string.IsNullOrEmpty(AFFinancialType)) AFFinancialType = "";

                IQueryable<TIS_LeadingShip> x = null;
                x = from a in ds.POP_LeadingShip
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where subDistrict.Contains(a.districtID)
                    && (a.name.Contains(search) || a.IDCard.Contains(search) || a.duty.Contains(search) || b2.districtName.Contains(search))
                    && (a.name.Contains(AFName) && a.nation.Contains(AFNation) && a.sex.Contains(AFSex) && a.duty.Contains(AFDuty) && a.education.Contains(AFEducation) && a.birthDay.Contains(AFBirthday) && a.JionTime.Contains(AFJionTime) && a.workTime.Contains(AFWorkTime) && a.type.Contains(AFType) && a.financialType.Contains(AFFinancialType))
                    select new TIS_LeadingShip
                    {
                        id = a.id,
                        birthDay = a.birthDay,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == b2.attachTo).districtName,
                        districtName = b2.districtName,
                        type = a.type,
                        duty = a.duty,
                        education = a.education,
                        financialType = a.financialType,
                        IDCard = a.IDCard,
                        JionTime = a.JionTime,
                        name = a.name,
                        nation = a.nation,
                        phone = a.phone,
                        sex = a.sex,
                        TrainingTitle = a.TrainingTitle,
                        workTime = a.workTime
                    };
                switch (sort)
                {
                    case "town":
                        x = x.OrderBy(c => c.town);
                        break;
                    case "name":
                        x = x.OrderBy(c => c.name);
                        break;
                    case "IDCard":
                        x = x.OrderBy(c => c.IDCard);
                        break;
                    case "duty":
                        x = x.OrderBy(c => c.duty);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    default:
                        x = x.OrderBy(c => c.town);
                        break;
                }
                
                returnData.total = x.ToArray<TIS_LeadingShip>().Length;
                x = x.Skip<TIS_LeadingShip>(offset);
                x = x.Take<TIS_LeadingShip>(limit);
                returnData.rows = x.ToArray<TIS_LeadingShip>();
                if (order == "desc") returnData.rows = returnData.rows.Reverse().ToArray();
                returnData.success = true;
                returnData.message = "Success";
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
            }
            ds.Dispose();
            return returnData;
        }

        /*-----------------新增领导班子---------*/
        public CommonOutputT<string> addLeadership(string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty) && !string.IsNullOrEmpty(districtID))
            {
                try
                {
                    var x = new POP_CheckLeadingShip();
                    x.id = Guid.NewGuid().ToString();
                    x.name = name;
                    x.IDCard = IDCard;
                    x.type = type;
                    x.sex = sex;
                    x.nation = nation;
                    x.birthDay = birthDay;
                    x.JionTime = JionTime;
                    x.workTime = workTime;
                    x.duty = duty;
                    x.education = education;
                    x.TrainingTitle = TrainingTitle;
                    x.phone = phone;
                    x.financialType = financialType;
                    x.districtID = districtID;
                    x.operate = "新增";
                    x.status = "1";                 // 1标识待审核；2标识审核通过；3标识审核不通过
                    ds.POP_CheckLeadingShip.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.data = x.id;
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------编辑领导班子---------*/
        public CommonOutput editLeadership(string id, string name, string IDCard, string sex, string nation, string birthDay, string JionTime, string workTime, string duty, string education, string TrainingTitle, string type, string phone, string financialType, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(duty))
            {
                try
                {
                    var x = ds.POP_CheckLeadingShip.SingleOrDefault(d => d.id == id);
                    if (x == null)
                    {
                        x.id = id;
                        x.name = name;
                        x.IDCard = IDCard;
                        x.type = type;
                        x.sex = sex;
                        x.nation = nation;
                        x.birthDay = birthDay;
                        x.JionTime = JionTime;
                        x.workTime = workTime;
                        x.duty = duty;
                        x.education = education;
                        x.TrainingTitle = TrainingTitle;
                        x.phone = phone;
                        x.financialType = financialType;
                        x.districtID = districtID;
                        x.operate = "修改";
                        x.status = "1";                 // 1标识待审核；2标识审核通过；3标识审核不通过
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        x.name = name;
                        x.IDCard = IDCard;
                        x.type = type;
                        x.sex = sex;
                        x.nation = nation;
                        x.birthDay = birthDay;
                        x.JionTime = JionTime;
                        x.workTime = workTime;
                        x.duty = duty;
                        x.education = education;
                        x.TrainingTitle = TrainingTitle;
                        x.phone = phone;
                        x.financialType = financialType;
                        x.districtID = districtID;
                        x.operate = "修改";
                        x.status = "1";                 // 1标识待审核；2标识审核通过；3标识审核不通过
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------取消领导班子---------*/
        public CommonOutput deleteLeadership(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var y = ds.POP_CheckLeadingShip.SingleOrDefault(d => d.id == id);
                    if (y != null)
                    {
                        y.operate = "删除";
                        y.status = "1";
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        var thisLeadingShip = ds.POP_LeadingShip.SingleOrDefault(d => d.id == id);
                        var x = new POP_CheckLeadingShip();
                        x.id = id;
                        x.name = thisLeadingShip.name;
                        x.IDCard = thisLeadingShip.IDCard;
                        x.type = thisLeadingShip.type;
                        x.sex = thisLeadingShip.sex;
                        x.nation = thisLeadingShip.nation;
                        x.birthDay = thisLeadingShip.birthDay;
                        x.JionTime = thisLeadingShip.JionTime;
                        x.workTime = thisLeadingShip.workTime;
                        x.duty = thisLeadingShip.duty;
                        x.education = thisLeadingShip.education;
                        x.TrainingTitle = thisLeadingShip.TrainingTitle;
                        x.phone = thisLeadingShip.phone;
                        x.financialType = thisLeadingShip.financialType;
                        x.districtID = thisLeadingShip.districtID;
                        ds.POP_CheckLeadingShip.InsertOnSubmit(x);
                        ds.SubmitChanges();
                        returnData.success = false;
                        returnData.message = "信息有误！";
                        ds.Dispose(); return returnData;
                    }
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------待审核列表领导班子---------*/
        public CommonOutPutT_M<TIS_LeadingShip[]> getCheckLeadershipList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<TIS_LeadingShip[]> returnData = new CommonOutPutT_M<TIS_LeadingShip[]>();
            var subDistrict = CommonMethod.getSubDistrict(districtID);
            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";

                IQueryable<TIS_LeadingShip> x = null;
                x = from a in ds.POP_CheckLeadingShip
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where subDistrict.Contains(a.districtID) && (a.name.Contains(search) || a.IDCard.Contains(search) || a.duty.Contains(search) || b2.districtName.Contains(search))
                    select new TIS_LeadingShip
                    {
                        id = a.id,
                        birthDay = a.birthDay,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == b2.attachTo).districtName,
                        districtName = b2.districtName,
                        type = a.type,
                        duty = a.duty,
                        education = a.education,
                        financialType = a.financialType,
                        IDCard = a.IDCard,
                        JionTime = a.JionTime,
                        name = a.name,
                        nation = a.nation,
                        phone = a.phone,
                        sex = a.sex,
                        TrainingTitle = a.TrainingTitle,
                        workTime = a.workTime,
                        operate = a.operate,
                        status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核未通过")
                    };
                switch (sort)
                {
                    case "town":
                        x = x.OrderBy(c => c.town);
                        break;
                    case "name":
                        x = x.OrderBy(c => c.name);
                        break;
                    case "IDCard":
                        x = x.OrderBy(c => c.IDCard);
                        break;
                    case "duty":
                        x = x.OrderBy(c => c.duty);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    default:
                        x = x.OrderBy(c => c.town);
                        break;
                }

                returnData.total = x.ToArray<TIS_LeadingShip>().Length;
                x = x.Skip<TIS_LeadingShip>(offset);
                x = x.Take<TIS_LeadingShip>(limit);
                returnData.rows = x.ToArray<TIS_LeadingShip>();
                if (order == "desc") returnData.rows = returnData.rows.Reverse().ToArray();
                returnData.success = true;
                returnData.message = "Success";
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
            }
            ds.Dispose();
            return returnData;
        }
        /*-----------------领导班子审核---------*/
        public CommonOutput checkActivity(string id, string districtID, string IsOK)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(IsOK))
            {
                try
                {
                    var thisLeadingShip = ds.POP_CheckLeadingShip.SingleOrDefault(d => d.id == id);
                    if (thisLeadingShip == null)
                    {
                        returnData.success = false;
                        returnData.message = "不存在该任务！";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        if (IsOK == "true")               //审核通过，修改领导班子列表内容
                        {
                            thisLeadingShip.status = "2";
                            if (thisLeadingShip.operate == "新增")
                            {
                                var x = new POP_LeadingShip();
                                x.id = id;
                                x.name = thisLeadingShip.name;
                                x.IDCard = thisLeadingShip.IDCard;
                                x.type = thisLeadingShip.type;
                                x.sex = thisLeadingShip.sex;
                                x.nation = thisLeadingShip.nation;
                                x.birthDay = thisLeadingShip.birthDay;
                                x.JionTime = thisLeadingShip.JionTime;
                                x.workTime = thisLeadingShip.workTime;
                                x.duty = thisLeadingShip.duty;
                                x.education = thisLeadingShip.education;
                                x.TrainingTitle = thisLeadingShip.TrainingTitle;
                                x.phone = thisLeadingShip.phone;
                                x.financialType = thisLeadingShip.financialType;
                                x.districtID = thisLeadingShip.districtID;
                                ds.POP_LeadingShip.InsertOnSubmit(x);
                                ds.SubmitChanges();
                                returnData.success = true;
                                returnData.message = "success";
                                ds.Dispose(); return returnData;
                            }
                            else if (thisLeadingShip.operate == "修改")
                            {
                                var x = ds.POP_LeadingShip.SingleOrDefault(d => d.id == id);
                                x.name = thisLeadingShip.name;
                                x.IDCard = thisLeadingShip.IDCard;
                                x.type = thisLeadingShip.type;
                                x.sex = thisLeadingShip.sex;
                                x.nation = thisLeadingShip.nation;
                                x.birthDay = thisLeadingShip.birthDay;
                                x.JionTime = thisLeadingShip.JionTime;
                                x.workTime = thisLeadingShip.workTime;
                                x.duty = thisLeadingShip.duty;
                                x.education = thisLeadingShip.education;
                                x.TrainingTitle = thisLeadingShip.TrainingTitle;
                                x.phone = thisLeadingShip.phone;
                                x.financialType = thisLeadingShip.financialType;
                                x.districtID = thisLeadingShip.districtID;
                                ds.SubmitChanges();
                                returnData.success = true;
                                returnData.message = "success";
                                ds.Dispose(); return returnData;
                            }
                            else
                            {
                                var x = ds.POP_LeadingShip.SingleOrDefault(d => d.id == id);
                                ds.POP_LeadingShip.DeleteOnSubmit(x);
                                ds.SubmitChanges();
                                returnData.success = true;
                                returnData.message = "success";
                                ds.Dispose(); return returnData;
                            }
                        }
                        else                             //审核不通过，修改领导班子列表内容
                        {
                            thisLeadingShip.status = "3";
                            ds.SubmitChanges();
                            returnData.success = true;
                            returnData.message = "success";
                            ds.Dispose(); return returnData;
                        }
                    }
                }
                catch (Exception)
                {
                    returnData.success = false;
                    returnData.message = "网络错误！";
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "请确认输入了所有必要信息";
                ds.Dispose(); return returnData;
            }
        }

        #endregion

    }
}
