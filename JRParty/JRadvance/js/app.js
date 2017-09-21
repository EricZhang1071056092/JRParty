/*-------------所有可能加载的脚本--------------*/
require.config({
    paths: {
        "jquery": ["../../assets/js/jquery.min"],
        "bootstrap": ["../../assets/js/bootstrap.min"],
        "bootstrap-table": ["../../assets/js/bootstrap-table"],
        "bootstrap-table-export": ["../../assets/js/bootstrap-table-export"],
        "tableExport": ["../assets/js/0-common/tableExport"],
        "bootstrap-editable": ["../assets/js/0-common/bootstrap-editable"],
        "bootstrap-table-editable": ["../assets/js/0-common/bootstrap-table-editable"],
        "bootstrap-select": ["../../assets/js/bootstrap-select.min"],
        "bootstrap-datetimepicker": ["../../assets/js/bootstrap-datetimepicker.min"],
        "bootstrap-datetimepicker.zh-CN": ["../../assets/js/bootstrap-datetimepicker.zh-CN"],
        "jquery.cookie": ["../../assets/js/jquery.cookie"], 
        "fileinput": ["../../assets/js/fileinput.min"],
        "fileinput_locale_zh": ["../../assets/js/fileinput_locale_zh"],
        "echarts": ["../../assets/js/echarts-all"],
        'ueditor.config': ['../assets/UEditor-utf8-net/ueditor.config'],
        'ueditor': ['../assets/UEditor-utf8-net/ueditor.all.min'], 
        "moment": ["../assets/js/0-common/moment.min"],
        "notifyme": ["../assets/js/0-common/notifyme"], 
        "baguetteBox": ["../assets/js/0-common/baguetteBox"],
        "ztree": ["../assets/js/0-common/jquery.ztree.all.min"],
        "validationengine-en": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine-en"],
        "validationengine": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine"], 
        "qrcode": ["../assets/js/jquery.qrcode.min"],
        "gverify": ["../assets/js/gverify"], 
        "TileHeadPic": ["../assets/js/0-common/TileHeadPic"],
        "modernizr": ["../assets/js/0-common/modernizr"],
        "jqueryui": ["../js/jquery-ui-1.10.2.custom.min"],
        "fullcalendar": ["../js/fullcalendar.min"], 

    },
    shim: {//写清楚依赖，防止加载顺序错误
        jquery: {
            exports: 'jquery'
        },
        "jquery2": {
            exports: 'jquery2'
        },
        bootstrap: {
            deps: ['jquery']
        },
        bootstrap2: {
            deps: ['jquery2']
        },
        "jquery.cookie": {
            deps: ['jquery']
        },
        "jquery.cookie2": {
            deps: ['jquery2']
        },
        "echarts": {
            deps: ['jquery']
        },
        TileHeadPic: {
            deps: ['jquery']
        },
        popmenu: {
            deps: ['jquery']
        },
        jquerygeo: {
            deps: ['jquery']
        },
        "bootstrap-table": {
            deps: ['bootstrap']
        },
        fileinput: {
            deps: ['bootstrap']
        },
        fileinput_locale_zh: {
            deps: ['fileinput']
        },
        "bootstrap-select": {
            deps: ['bootstrap']
        },
        'bootstrap-datetimepicker': {
            deps: ['bootstrap']
        },
        "bootstrap-datetimepicker.zh-CN": {
            deps: ['bootstrap-datetimepicker']
        },
        "ztree": {
            deps: ['jquery']
        },
        "qrcode": {
            deps: ['jquery']
        },
        "jqueryui": {
            deps: ['jquery']
        },
        "fullcalendar": {
            deps: ['jquery']
        },
    }
})


/*---------------接口地址----------------*/
//脚本里用到的所有的转发连接都放在这里
var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
          : "")
          + window.location.host;
var svc_sys = svcHeader + "/JRPartyService/Party.svc";
var svc_uoload = svcHeader + "/JRPartyService/Data";
/*----------------集成方法---------------*/
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
//注销
var logout = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    $.cookie("JTZH_districtID", null, { path: '/' })
    window.location.href = "../login.html";
}

/*----------------------------------加载公用模块-------------------------*/
//主页离开事件
function toPage(page) {
    window.scroll(0, 0);
    switch (page) {
        case "index":
            window.location.href = "../0_index/index.html?type=" + 's'

            break;
        case "infor_add":
            window.location.href = "../5_inforregister/infor_add.html?type=" + 's'

            break;
        case "infor_edit":
            window.location.href = "../5_inforregister/infor_edit.html?type=" + 's'

            break;
        case "system":
            window.location.href = "../7-system/system.html?type=" + 's'
            break;
    }
}
//主页离开事件
function toPage_C(page) {
    window.scroll(0, 0);
    switch (page) {
        case "index":
            window.location.href = "../0_index/index.html?type=" + 'C'
            break;
        case "infor_add":
            window.location.href = "../5_inforregister/infor_add.html?type=" + 'C'
            break;
        case "infor_edit":
            window.location.href = "../5_inforregister/infor_edit.html?type=" + 'C'
            break;
        case "check":
            window.location.href = "../4_check/check.html?type=" + 'C'
            break;
        case "statistics_year":
            window.location.href = "../2_statistics/statistics_year.html?type=" + 'C'
            break;
        case "statistics_district":
            window.location.href = "../2_statistics/statistics_district.html?type=" + 'C'
            break;
        case "statistics_grade":
            window.location.href = "../2_statistics/statistics_grade.html?type=" + 'C'
            break;
        case "statistics_depart":
            window.location.href = "../2_statistics/statistics_depart.html?type=" + 'C'
            break;
        case "user":
            window.location.href = "../3_system/user.html?type=" + 'C'
            break;
        case "role":
            window.location.href = "../3_system/role.html?type=" + 'C'
            break;
        case "linker":
            window.location.href = "../3_system/linker.html?type=" + 'C'
            break;
        case "timeSet":
            window.location.href = "../3_system/timeSet.html?type=" + 'C'
            break;
        case "note":
            window.location.href = "../3_system/note.html?type=" + 'C'
            break;
    }
}
//加载市级计划
var loadCommonModule = function (module) {
    console.log(module);
    $('#common_navbar').html('<div class="container-fluid">'+
            '<div class="navbar-header">'+
            '</div> '+
            '<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">'+
                '<ul class="nav navbar-nav navbar-right" style="margin-right:50px">'+
                    '<li   id="camera"><a href="camrea.html">监控管理 <span class="sr-only">(current)</span></a></li>'+
                    '<li class="dropdown"id="statistics">' +
                        '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">统计管理<span class="caret"></span></a>'+
                        '<ul class="dropdown-menu">'+
                            '<li><a href="area_statistics.html">点位统计</a></li>'+
                            '<li role="separator" class="divider"></li>'+
                            '<li><a href="browse_statistics.html">浏览情况统计</a></li>'+
                        '</ul>'+
                    '</li>'+
                    '<li class="dropdown"id="system">' +
                       ' <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">运维管理<span class="caret"></span></a>'+
                        '<ul class="dropdown-menu">'+
                           ' <li><a href="authority.html">权限管理</a></li>'+
                            '<li role="separator" class="divider"></li>'+
                            '<li><a href="user.html">用户管理</a></li>'+
                        '</ul>'+
                    '</li>'+
                    '<li><a href="index.html">退出</a></li>'+
               ' </ul>'+
            '</div> '+
        '</div> '+           
'</div>');
    $('#' + module).addClass("active");
}
 
var ctrlAccountCheck = function () {
    $.ajax({
        url: svc_sys + "/ctrlAccountCheck",
        data: {
            userID: $.cookie("JTZH_admin")
        },
        success: function (data) {
            var json = eval('(' + data + ')');
            if (json.Code != 1 && json.Code != "1") {
                $('.am-modal-hd').html('');
                $('.am-modal-hd').html('警告<a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>');
                $('.am-modal-bd').html('');
                $('.am-modal-bd').html('登录信息已过期！2秒后将跳回登录界面。。。');
                $('#modal-alert').modal();
                setTimeout(function () {
                    window.location.href = "../ctrllogin.html"
                }, 2000)
            } else {
                $('.informaton-user-img').html('<img src="' + json.Result.portrait + '" />');
                $('.informaton-user-name').html(json.Result.name);
                $('.list-group-item-district').html(data.data.district);
                $('.informaton-user-role').html(json.Result.role);
                $('.informaton-user-last_time').html("<br />" + json.Result.lastTime);
            }
        },
        error: function () {
            $('.am-modal-hd').html('');
            $('.am-modal-hd').html('警告<a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>');
            $('.am-modal-bd').html('');
            $('.am-modal-bd').html('登录信息已过期！2秒后将跳回登录界面。。。');
            $('#modal-alert').modal();
            setTimeout(function () {
                window.location.href = "../ctrllogin.html"
            }, 2000)

        }
    })
}
var clearPopup = function () {
    $(".add-popup").html('');
}

/*-----------------------------------部分插件功能-------------------------------------*/
//获取table高度
function getHeight() {
    return $(window).height() - $('h1').innerHeight(true) - $('#container').innerHeight(true);
}

//人民币大小写转换
var c = "零壹贰叁肆伍陆柒捌玖".split("");
// ["零","壹","贰","叁","肆","伍","陆","柒","捌","玖"]
var _c = {}; // 反向对应关系
for (var i = 0; i < c.length; i++) {
    _c[c[i]] = i;
};

var d = "元***万***亿***万";
var e = ",拾,佰,仟".split(",");
function unit4(arr) {
    var str = "", i = 0;
    while (arr.length) {
        var t = arr.pop();
        str = (c[t] + (t == 0 ? "" : e[i])) + str;
        i++;
    }

    str = str.replace(/[零]{2,}/g, "零");

    str = str.replace(/^[零]/, "");
    str = str.replace(/[零]$/, "");
    if (str.indexOf("零") == 0) {
        str = str.substring(1);
    }
    if (str.lastIndexOf("零") == str.length - 1) {
        str = str.substring(0, str.length - 1);
    }

    return str;
}
function _formatD(a) {
    // 转化整数部分
    var arr = a.split(""), i = 0, result = "";
    while (arr.length) {
        var arr1 = arr.splice(-4, 4);

        var dw = d.charAt(i), unit = unit4(arr1);

        if (dw == '万' && !unit) {
            dw = "";
        }
        result = unit + dw + result;
        i += 4;
    }
    return result == "元" ? "" : result;
}
function _formatF(b) {
    // 转化小数部分
    b = b || "";
    switch (b.length) {
        case 0:
            return "整";
        case 1:
            return c[b] + "角";
        default:
            return c[b.charAt(0)] + "角" + c[b.charAt(1)] + "分";
    }
}
function _format(n) {
    var a = ("" + n).split("."), a0 = a[0], a1 = a[1];
    return _formatD(a0) + _formatF(a1);
}

function parse4(u4) {
    var res = 0;
    while (t = /([零壹贰叁肆伍陆柒捌玖])([拾佰仟]?)/g.exec(u4)) {
        var n = _c[t[1]], d = {
            "": 1,
            "拾": 10,
            "佰": 100,
            "仟": 1000
        }[t[2]];
        res += n * d;
        u4 = u4.replace(t[0], "");
    }
    var result = ("0000" + res);
    return result.substring(result.length - 4);
}
function _parseD(d) {
    var arr = d.replace(/[零]/g, "").split(/[万亿]/), rs = "";
    for (var i = 0; i < arr.length; i++) {
        rs += parse4(arr[i]);
    }
    ;
    return rs.replace(/^[0]+/, "");
};
function _parseF(f) {
    var res = "", t = f.replace(/[^零壹贰叁肆伍陆柒捌玖]+/g, "").split(""); // 去掉单位
    if (t.length) {
        res = ".";
    } else {
        return "";
    }
    ;
    for (var i = 0; (i < t.length && i < 2) ; i++) {
        res += _c[t[i]];
    }
    ;
    return res;
};
function _parse(rmb) {
    var a = rmb.split("元"), a1 = a[1], a0 = a[0];
    if (a.length == 1) {
        a1 = a0;
        a0 = "";
    }
    return _parseD(a0) + _parseF(a1);

};
//小写转大写
function formatRMB(num) {
    var n = Number(num);
    if (!isNaN(num)) {
        if (num == 0) {
            return "零元整";
        } else {
            return _format(n);
        }
    } else {
        return false;
    }
}
//大写转小写
function parseRMB(rmb) {
    if (/^[零壹贰叁肆伍陆柒捌玖元万亿拾佰仟角分整]{2,}$/.test(rmb)) {
        var result = _parse(rmb);
        return rmb == this.formatRMB(result) ? result : result + "(?)";
    } else {
        return false;
    }
};
//生成随机颜色
function getRandomColor() {
    var c = '#';
    var cArray = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    for (var i = 0; i < 6; i++) {
        var cIndex = Math.round(Math.random() * 15);
        c += cArray[cIndex];
    }
    return c;
}


function locateExtent(pointList) {
    if (pointList && pointList.length > 0) {
        var xmin = 0, ymin = 0, xmax = 0, ymax = 0;
        for (var i = 0; i < pointList.length; i++) {
            if (i == 0) {
                xmin = pointList[i].lng;
                ymin = pointList[i].lat;
                xmax = pointList[i].lng;
                ymax = pointList[i].lat;
            }
            else {
                if (xmin > pointList[i].lng) {
                    xmin = pointList[i].lng
                }
                if (ymin > pointList[i].lat) {
                    ymin = pointList[i].lat;
                } if (xmax < pointList[i].lng) {
                    xmax = pointList[i].lng;
                }
                if (ymax < pointList[i].lat) {
                    ymax = pointList[i].lat;
                }
            }
        }

        map.fitBounds([[ymin, xmin], [ymax, xmax]]);
    }
}



