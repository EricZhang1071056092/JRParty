using System;
using System.Linq;
using JRPartyData;
using JRPartyService.DataContracts.Lib;
using JRPartyService.DataContracts.AppConfig;
using JumboTCMS.Utils;
using JRPartyService.DataContracts;
using System.Timers;
using System.ServiceModel.Activation;
using System.Collections.Generic;

namespace JRPartyService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Party”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Party.svc 或 Party.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Position : IPosition
    {

        #region 基本阵地

        /*-----------------查看基本阵地---------*/
        public CommonOutPutT_M<List_Position[]> getPositionList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Position[]> returnData = new CommonOutPutT_M<List_Position[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);
            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };
                string[] positions =new string[8]{"党员教育室","党内关爱室","组织会议室","党建办公室","为民服务大厅","综合展示厅","党性教育长廊","党建文化广场" };

                IQueryable<List_Position> x = null;
                x = from a in ds.POS_basic
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where positions.Contains(a.type)
                    && subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                    select new List_Position
                    {
                        id = a.id,
                        area = a.area,
                        description = a.description,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        districtID = a.districtID,
                        releaseTime = a.releaseTime,
                        type = a.type,
                        status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过"),
                        imageURL = (from b in ds.PAR_ActivityReleaseFile
                                    where a.id == b.activityID && (imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3)) || imgSuffix.Contains(b.Url.Substring(b.Url.Length - 4, 4)))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray()
                    };
                string[] y = (from a in ds.POS_basic
                              join b in ds.SYS_District on a.districtID equals b.id into b1
                              from b2 in b1.DefaultIfEmpty()
                              where positions.Contains(a.type) && subDistrict.Contains(a.districtID)
                              && (a.type.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                              group b2 by b2.districtName into p
                              select p.Key).ToArray();
                List<List_Position> xList = new List<List_Position>();
                foreach (var item in y)
                {
                    string attachTo = (from a in ds.SYS_District
                                       where a.districtName == item
                                       select a.attachTo).FirstOrDefault();
                    var town = ds.SYS_District.SingleOrDefault(d => d.id == attachTo);
                    for (int i = 0; i < 8; i++)
                    {
                        var temp = new List_Position();
                        temp.type = positions[i];
                        temp.districtName = item;
                        temp.town = town.districtName;
                        temp.districtID = town.id;
                        foreach (var item2 in x)
                        {
                            if (item2.districtName == item && item2.type == positions[i])
                            {
                                temp.id = item2.id;
                                temp.area = item2.area;
                                temp.description = item2.description;
                                temp.releaseTime = item2.releaseTime;
                                temp.status = item2.status;
                                temp.imageURL = item2.imageURL;
                            }
                        }
                        xList.Add(temp);
                    }
                }
                IOrderedEnumerable<List_Position> xList2 = null;
                switch (sort)
                {
                    case "type":
                        xList2 = xList.OrderBy(c => c.type);
                        break;
                    case "area":
                        xList2 = xList.OrderBy(c => c.area);
                        break;
                    case "description":
                        xList2 = xList.OrderBy(c => c.description);
                        break;
                    case "districtName":
                        xList2 = xList.OrderBy(c => c.districtName);
                        break;
                    default:
                        xList2 = xList.OrderBy(c => c.districtName);
                        break;
                }
                IEnumerable<List_Position> xList3 = null;
                returnData.total = xList2.ToArray().Length;
                xList3 = xList2.Skip(offset);
                xList3 = xList3.Take(limit);
                returnData.rows = xList3.ToArray();
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

        /*-----------------查看基本阵地---------*/
        public CommonOutPutT_M<List_Position[]> get3PositionList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Position[]> returnData = new CommonOutPutT_M<List_Position[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);
            //搜索是否为空
            if (!String.IsNullOrEmpty(search))
            {
                //排序字段是否为空
                if (!String.IsNullOrEmpty(sort))
                {
                    try
                    {
                        IQueryable<List_Position> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.type ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "area":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.area ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                default:
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                            }

                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.type descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "area":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.area descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby a.description descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                default:
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                        orderby b2.districtName descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Position>().Length;
                        x = x.Skip<List_Position>(offset);
                        x = x.Take<List_Position>(limit);
                        returnData.rows = x.ToArray<List_Position>();
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
                        IQueryable<List_Position> x = from a in ds.POS_basic
                                                      join b in ds.SYS_District on a.districtID equals b.id into b1
                                                      from b2 in b1.DefaultIfEmpty()
                                                      where  subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                                                      orderby b2.districtName descending
                                                      select new List_Position
                                                      {
                                                          id = a.id,
                                                          area = a.area,
                                                          description = a.description,
                                                          districtName = b2.districtName,
                                                          districtID = a.districtID,
                                                          releaseTime = a.releaseTime,
                                                          type = a.type,
                                                          status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                                      };

                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Position>().Length;
                        x = x.Skip<List_Position>(offset);
                        x = x.Take<List_Position>(limit);
                        returnData.rows = x.ToArray<List_Position>();
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
                        IQueryable<List_Position> x = null;
                        if (order == "asc" || order == "")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby a.type ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "area":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby a.area ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby a.description ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby b2.districtName ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                default:
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby b2.districtName ascending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                            }
                        }
                        else if (order == "desc")
                        {
                            switch (sort)
                            {
                                case "type":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby a.type descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "area":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where subDistrict.Contains(a.districtID)
                                        orderby a.area descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "description":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where  subDistrict.Contains(a.districtID)
                                        orderby a.description descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                case "districtName":
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where   subDistrict.Contains(a.districtID)
                                        orderby b2.districtName descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                                default:
                                    x = from a in ds.POS_basic
                                        join b in ds.SYS_District on a.districtID equals b.id into b1
                                        from b2 in b1.DefaultIfEmpty()
                                        where   subDistrict.Contains(a.districtID)
                                        orderby b2.districtName descending
                                        select new List_Position
                                        {
                                            id = a.id,
                                            area = a.area,
                                            description = a.description,
                                            districtName = b2.districtName,
                                            districtID = a.districtID,
                                            releaseTime = a.releaseTime,
                                            type = a.type,
                                            status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                        };
                                    break;
                            }
                        }
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Position>().Length;
                        x = x.Skip<List_Position>(offset);
                        x = x.Take<List_Position>(limit);
                        returnData.rows = x.ToArray<List_Position>();
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
                        IQueryable<List_Position> x = from a in ds.POS_basic
                                                      join b in ds.SYS_District on a.districtID equals b.id into b1
                                                      from b2 in b1.DefaultIfEmpty()
                                                      where  subDistrict.Contains(a.districtID)
                                                      orderby b2.districtName ascending
                                                      select new List_Position
                                                      {
                                                          id = a.id,
                                                          area = a.area,
                                                          description = a.description,
                                                          districtName = b2.districtName,
                                                          districtID = a.districtID,
                                                          releaseTime = a.releaseTime,
                                                          type = a.type,
                                                          status = (a.status == "1") ? "待审核" : ((a.status == "2") ? "审核通过" : "审核不通过")
                                                      };
                        returnData.success = true;
                        returnData.message = "Success";
                        returnData.total = x.ToArray<List_Position>().Length;
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
        /*-----------------查看单条基本阵地---------*/
        public CommonOutputT<POS_Detail> getPositionDetail(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<POS_Detail> returnData = new CommonOutputT<POS_Detail>();
            try
            {
                var x = (from a in ds.POS_basic
                         join b in ds.SYS_District on a.districtID equals b.id into b1
                         from b2 in b1.DefaultIfEmpty()
                         where a.id == id
                         select new POS_Detail
                         {
                             id = a.id,
                             area = a.area,
                             description = a.description,
                             districtName = b2.districtName,
                             type = a.type, 
                             releaseTime = a.releaseTime,
                             imageURL = (from b in ds.PAR_ActivityReleaseFile where a.id == b.activityID select b).ToArray()
                         }).ToArray();
                returnData.success = true;
                returnData.message = "Success";
                returnData.data = x[0];
                ds.Dispose(); return returnData;
            }
            catch (Exception ex)
            {
                returnData.success = true;
                returnData.message = "Error:" + ex.Message;
                ds.Dispose(); return returnData;
            }
        }
        /*-----------------新增/更新基本阵地---------*/
        public CommonOutputT<string> addPosition(string type, string description, string area, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutputT<string> returnData = new CommonOutputT<string>();
            try
            {
                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(districtID))
                {
                    var thisGuy = ds.POS_basic.SingleOrDefault(d => d.districtID == districtID && d.type == type);
                    if (thisGuy == null)
                    {
                        var x = new POS_basic();
                        x.id = Guid.NewGuid().ToString();
                        x.area = area;
                        x.description = description;
                        x.type = type;
                        x.districtID = districtID;
                        x.status = "1";
                        x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        ds.POS_basic.InsertOnSubmit(x);
                        returnData.data = x.id;
                    }
                    else
                    {
                        thisGuy.area = area;
                        thisGuy.description = description;
                        thisGuy.status = "1";
                        thisGuy.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        returnData.data = thisGuy.id;
                    }
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                }
                else
                {
                    returnData.message = "Error:请补全请求信息!"; 
                }
            }
            catch (Exception ex)
            {
                returnData.message = "Error:" + ex;
            }
            ds.Dispose();
            return returnData;
        }
        /*-----------------编辑基本阵地---------*/
        public CommonOutput editPosition(string id, string type, string description, string area, string districtID)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id)&& !string.IsNullOrEmpty(type))
            {
                try
                {
                    var x = ds.POS_basic.SingleOrDefault(d => d.id == id);
                    x.area = area;
                    x.description = description;
                    x.type = type;
                    x.districtID = districtID;
                    x.status = "1";
                    x.releaseTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
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
        /*-----------------取消基本阵地---------*/
        public CommonOutput deletePosition(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var x = ds.POS_basic.SingleOrDefault(d => d.id == id);
                    if (x != null)
                    {
                        ds.POS_basic.DeleteOnSubmit(x);
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
        /*-----------------新增基本阵地附带文件（同任务图片一个表）---------*/
        public CommonOutput AddPositionPicture(string positionID, string Url)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(positionID) || !string.IsNullOrEmpty(Url))
            {
                try
                {
                    var x = new PAR_ActivityReleaseFile();
                    x.id = Guid.NewGuid().ToString();
                    x.activityID = positionID;
                    x.Url = Url;
                    ds.PAR_ActivityReleaseFile.InsertOnSubmit(x);
                    ds.SubmitChanges();
                    returnData.success = true;
                    returnData.message = "success";
                    ds.Dispose(); return returnData;
                }
                catch (Exception ex)
                {
                    returnData.success = false;
                    returnData.message = "Error:" + ex.Message;
                    ds.Dispose(); return returnData;
                }
            }
            else
            {
                returnData.success = false;
                returnData.message = "Error:photoTakeID或imageURL无效！";
                ds.Dispose(); return returnData;
            }
        }

        /*-----------------删除基本阵地附带文件---------*/
        public CommonOutput deletePositionPicture(string id)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var file = ds.PAR_ActivityReleaseFile.SingleOrDefault(d => d.id == id);
                    if (file != null)
                    {
                        ds.PAR_ActivityReleaseFile.DeleteOnSubmit(file);
                        ds.SubmitChanges();
                        returnData.success = true;
                        returnData.message = "success";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        returnData.success = false;
                        returnData.message = "id错误";
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
        /*-----------------基本阵地审核---------*/
        public CommonOutput checkPosition(string id, string districtID, string IsOK)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutput returnData = new CommonOutput();
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(IsOK))
            {
                try
                {
                    var thisActivity = ds.POS_basic.SingleOrDefault(d => d.id == id);
                    if (thisActivity == null)
                    {
                        returnData.success = false;
                        returnData.message = "不存在该任务！";
                        ds.Dispose(); return returnData;
                    }
                    else
                    {
                        if (IsOK == "true")
                        {
                            thisActivity.status = "2";
                            ds.SubmitChanges();
                            returnData.success = true;
                            returnData.message = "success";
                            ds.Dispose(); return returnData;
                        }
                        else
                        {
                            thisActivity.status = "3";
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
        /*-----------------查看待审核基本阵地---------*/
        public CommonOutPutT_M<List_Position[]> getcheckPositionList(string districtID, int offset, int limit, string order, string search, string sort)
        {
            JRPartyDataContext ds = new JRPartyDataContext();
            CommonOutPutT_M<List_Position[]> returnData = new CommonOutPutT_M<List_Position[]>();
            var subDistrict = Methods.CommonMethod.getSubDistrict(districtID);
            try
            {
                if (limit == 0) limit = 10;
                if (String.IsNullOrEmpty(order)) order = "asc"; // desc
                if (String.IsNullOrEmpty(search)) search = "";
                string[] imgSuffix = new string[8] { "jpg", "png", "gif", "jpeg", "JPG", "PNG", "GIF", "JPEG" };
                string[] positions = new string[8] { "党员教育室", "党内关爱室", "组织会议室", "党建办公室", "为民服务大厅", "综合展示厅", "党性教育长廊", "党建文化广场" };

                IQueryable<List_Position> x = null;
                x = from a in ds.POS_basic
                    join b in ds.SYS_District on a.districtID equals b.id into b1
                    from b2 in b1.DefaultIfEmpty()
                    where a.status == "1" && positions.Contains(a.type)
                    && subDistrict.Contains(a.districtID) && (a.type.Contains(search) || a.area.Contains(search) || a.description.Contains(search) || b2.districtName.Contains(search))
                    select new List_Position
                    {
                        id = a.id,
                        area = a.area,
                        description = a.description,
                        town = ds.SYS_District.SingleOrDefault(c => c.id == a.districtID.Substring(0, 4)).districtName,
                        districtName = b2.districtName,
                        districtID = a.districtID,
                        releaseTime = a.releaseTime,
                        type = a.type,
                        status = "待审核",
                        imageURL = (from b in ds.PAR_ActivityReleaseFile
                                    where a.id == b.activityID && (imgSuffix.Contains(b.Url.Substring(b.Url.Length - 3, 3)) || imgSuffix.Contains(b.Url.Substring(b.Url.Length - 4, 4)))
                                    select Tools.GetSERVERADDRESS() + "/JRPartyService/Upload/Activity/" + b.Url).ToArray()
                    };
                //string[] y = (from a in ds.POS_basic
                //              join b in ds.SYS_District on a.districtID equals b.id into b1
                //              from b2 in b1.DefaultIfEmpty()
                //              where positions.Contains(a.type) && subDistrict.Contains(a.districtID)
                //              && (a.type.Contains(search) || a.description.Contains(search) || a.releaseTime.Contains(search) || b2.districtName.Contains(search))
                //              group b2 by b2.districtName into p
                //              select p.Key).ToArray();
                //List<List_Position> xList = new List<List_Position>();
                //foreach (var item in y)
                //{
                //    string attachTo = (from a in ds.SYS_District
                //                       where a.districtName == item
                //                       select a.attachTo).FirstOrDefault();
                //    var town = ds.SYS_District.SingleOrDefault(d => d.id == attachTo);
                //    for (int i = 0; i < 8; i++)
                //    {
                //        var temp = new List_Position();
                //        temp.type = positions[i];
                //        temp.districtName = item;
                //        temp.town = town.districtName;
                //        temp.districtID = town.id;
                //        foreach (var item2 in x)
                //        {
                //            if (item2.districtName == item && item2.type == positions[i])
                //            {
                //                temp.id = item2.id;
                //                temp.area = item2.area;
                //                temp.description = item2.description;
                //                temp.releaseTime = item2.releaseTime;
                //                temp.status = item2.status;
                //                temp.imageURL = item2.imageURL;
                //            }
                //        }
                //        xList.Add(temp);
                //    }
                //}
                //IOrderedEnumerable<List_Position> xList2 = null;
                switch (sort)
                {
                    case "type":
                        x = x.OrderBy(c => c.type);
                        break;
                    case "area":
                        x = x.OrderBy(c => c.area);
                        break;
                    case "description":
                        x = x.OrderBy(c => c.description);
                        break;
                    case "districtName":
                        x = x.OrderBy(c => c.districtName);
                        break;
                    default:
                        x = x.OrderBy(c => c.districtName);
                        break;
                }
                //IEnumerable<List_Position> xList3 = null;
                returnData.total = x.ToArray().Length;
                x = x.Skip(offset);
                x = x.Take(limit);
                returnData.rows = x.ToArray();
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
        #endregion

    }
}
