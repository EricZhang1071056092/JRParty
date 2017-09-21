/*-------------所有可能加载的脚本--------------*/
require.config({
    paths: {
        "jquery": ["../assets/js/jquery.min"],
        "bootstrap": ["../assets/js/bootstrap.min"],
        "bootstrap-table": ["../assets/js/bootstrap-table"],
        "bootstrap-table-export": ["../assets/js/0-common/bootstrap-table-export"],
        "tableExport": ["../assets/js/0-common/tableExport"],
        "bootstrap-editable": ["../assets/js/0-common/bootstrap-editable"],
        "bootstrap-table-editable": ["../assets/js/0-common/bootstrap-table-editable"],
        "jquery.cookie": ["../assets/js/jquery.cookie"],
        "TileHeadPic": ["../assets/js/0-common/TileHeadPic"],
        "modernizr": ["../assets/js/0-common/modernizr"],
        "leaflet": ["../assets/js/0-common/leaflet"],
        "fileinput": ["../assets/js/fileinput.min"],
        "fileinput_locale_zh": ["../assets/js/fileinput_locale_zh"],
        'ueditor.config': ['../assets/UEditor-utf8-net/ueditor.config'],
        'ueditor': ['../assets/UEditor-utf8-net/ueditor.all.min'],
        "bootstrap-select": ["../assets/js/bootstrap-select.min"],
        "mq-map": [" https://www.mapquestapi.com/sdk/leaflet/v2.2/mq-map.js?key=lYrP4vF3Uk5zgTiGGuEzQGwGIVDGuy24"],
        "mq-routing": ["https://www.mapquestapi.com/sdk/leaflet/v2.2/mq-routing.js?key=lYrP4vF3Uk5zgTiGGuEzQGwGIVDGuy24"],
        "bootstrap-datetimepicker": ["../assets/js/bootstrap-datetimepicker.min"],
        "bootstrap-datetimepicker.zh-CN": ["../assets/js/bootstrap-datetimepicker.zh-CN"],
        "fullcalendar": ["../assets/js/0-common/fullcalendar.min"],
        "fullcalendar-zh-cn": ["../assets/js/0-common/fullcalendar-zh-cn"],
        "moment": ["../assets/js/0-common/moment.min"],
        "notifyme": ["../assets/js/0-common/notifyme"],
        "echarts": ["../assets/js/echarts-all"],
        "baguetteBox": ["../assets/js/0-common/baguetteBox"],
        "ztree": ["../assets/js/0-common/jquery.ztree.all.min"],
        "validationengine-en": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine-en"],
        "validationengine": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine"],
        "fullcalendar2": ["../assets/js/fullcanlendar/fullcal/fullcalendar"],
        "fullcalendar-custom": ["../assets/js/fullcanlendar/jquery-ui-1.8.6.custom.min"],
        "fullcalendar-timepicker-addon": ["../assets/js/fullcanlendar/jquery-ui-timepicker-addon"],
        "qrcode": ["../assets/js/jquery.qrcode.min"],
        "gverify": ["../assets/js/gverify"],
        "menu": ["../assets/js/2_common/menu"],
        
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
var loadCommonModule_CP = function (module, plan) { 
    $('title').html('首页'); 
    $('.menu_list').html('<ul>'+
           ' <li class="plan">'+
               ' <p class="fuMenu"><img src="../assets/img/1_index/mokuai.png"width="20" style="margin-top:21px"/>&nbsp;&nbsp;管理模块&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>'+ 
                '<div class="div1">'+  
                    '<p class="zcd" id="zcd1" >计划发布</p>'+
                    '<p class="zcd" id="zcd2" onclick="window.location.href = "task_c.html"">任务管理</p>'+
                    '<p class="zcd" id="zcd3" onclick="window.location.href = "exam_cs.html"">统计汇总</p>'+
               ' </div>'+
            '</li>'+
            '<li class="">'+
                '<p class="fuMenu"><img src="../assets/img/1_index/camera.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;监控管理&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>'+
                        
                '<div class="div1">'+
                    '<p class="zcd" id="zcd9" onclick="window.location.href = "../2_system/system.html"">监控信息</p>'+
                   ' <p class="zcd" id="zcd10" onclick="window.location.href = "../2_system/system.html"">截屏时间设置</p>'+
               ' </div>'+
           ' </li>'+
           ' <li class="">'+
                '<p class="fuMenu"><img src="../assets/img/1_index/yunwei.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;运维管理&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>'+
                        
               ' <div class="div1"> '+
                   ' <p class="zcd" id="zcd19" onclick="window.location.href = "../2_system/system.html"">组织管理</p>'+
                   ' <p class="zcd" id="zcd20" onclick="window.location.href = "../2_system/user.html"">用户管理</p>'+
                '</div> '+
            '</li> '+
           ' <li class="" onclick="window.location.href = "../0_login/login.html"">'+
               ' <p class="fuMenu" style="text-align:left;margin-left:40px"><img src="../assets/img/1_index/out.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;注销&nbsp;&nbsp;&nbsp;</p>'+

            '</li>'+
        '</ul>'+
    '</div>'+
'</div>');
    $('.centerbar').html(
            ' <ul id="list" class="list">'+
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>'+
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>'+
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>'+
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">'+
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">'+
                '<li class="list-group-item"><a href="index_C.html"><img src="../assets/img/1_index/jihuafabu.png" />&nbsp;&nbsp;计划发布</a></li>'+
                '<li class="list-group-item"><a href="task_C.html"><img src="../assets/img/1_index/renwuguanli.png" />&nbsp;&nbsp;任务管理</a></li>'+
                '<li class="list-group-item"><a href="exam_CS.html"><img src="../assets/img/1_index/kaohehuizong.png" />&nbsp;&nbsp;统计汇总</a></li>' +
            '</ul>');

}
//加载镇级计划
var loadCommonModule_ZP = function (module, plan) { 
    $('title').html('首页'); 
   // $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_Z.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
                '<li class="list-group-item"><a href="index_Z.html"><img src="../assets/img/1_index/jihuafabu.png" />&nbsp;&nbsp;查看计划</a></li>' +
                '<li class="list-group-item"><a href="task_Z.html"><img src="../assets/img/1_index/renwuguanli.png" />&nbsp;&nbsp;任务管理</a></li>' +
                '<li class="list-group-item"><a href="exam_ZS.html"><img src="../assets/img/1_index/kaohehuizong.png" />&nbsp;&nbsp;统计汇总</a></li>' +
            '</ul>');
}
//加载村级计划
var loadCommonModule_P = function (module, plan) { 
    $('title').html('首页'); 
  //  $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/partymember.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
                '<li class="list-group-item"><a href="index.html"><img src="../assets/img/1_index/jihuafabu.png" />&nbsp;&nbsp;查看计划</a></li>' +
                '<li class="list-group-item"><a href="task.html"><img src="../assets/img/1_index/renwuguanli.png" />&nbsp;&nbsp;任务管理</a></li>' +
               // '<li class="list-group-item"><a href="exam_S.html"><img src="../assets/img/1_index/kaohehuizong.png" />&nbsp;&nbsp;考核汇总</a></li>' +
            '</ul>');
}
//加载市级运维
var loadCommonModule_CS = function (module, plan) { 
    $('title').html('运维管理'); 
   // $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
               '<li class="list-group-item"><a href="system.html"><img src="../assets/img/3_system/zuzhi.png" />&nbsp;&nbsp;组织管理</a></li>' +
               '<li class="list-group-item"><a href="user.html"><img src="../assets/img/3_system/yonghuguanli.png" />&nbsp;&nbsp;用户管理</a></li>' +
            '</ul>');

}
//加载镇级运维
var loadCommonModule_ZS = function (module, plan) { 
    $('title').html('运维管理'); 
  //  $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
               '<li class="list-group-item"><a href="system.html"><img src="../assets/img/3_system/zuzhi.png" />&nbsp;&nbsp;组织管理</a></li>' +
               '<li class="list-group-item"><a href="user.html"><img src="../assets/img/3_system/yonghuguanli.png" />&nbsp;&nbsp;用户管理</a></li>' +
            '</ul>');
}
//加载村级运维
var loadCommonModule_S = function (module, plan) { 
    $('title').html('运维管理'); 
    //$('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/partymember.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
               '<li class="list-group-item"><a href="partymember.html"><img src="../assets/img/3_system/zuzhi.png" />&nbsp;&nbsp;党员管理</a></li>' +
               '<li class="list-group-item"><a href="user.html"><img src="../assets/img/3_system/yonghuguanli.png" />&nbsp;&nbsp;用户管理</a></li>' +
            '</ul>');
}

//加载市级监控
var loadCommonModule_CJ = function (module, plan) { 
    $('title').html('监控管理');  
   // $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
                '<li class="list-group-item"><a href="camera_infor.html"><img src="../assets/img/2_camera/jiankong.png" />&nbsp;&nbsp;监控信息</a></li>' +
               '<li class="list-group-item"><a href="timeSet.html"><img src="../assets/img/2_camera/jieping.png" />&nbsp;&nbsp;截屏时间设置</a></li>' +
            '</ul>');
    
    
}
//加载镇级监控
var loadCommonModule_ZJ = function (module, plan) { 
    $('title').html('监控管理'); 
   // $('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/system.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
                '<li class="list-group-item"><a href="camera_infor.html"><img src="../assets/img/2_camera/jiankong.png" />&nbsp;&nbsp;监控信息</a></li>' +
               '<li class="list-group-item"><a href="timeSet.html"><img src="../assets/img/2_camera/jieping.png" />&nbsp;&nbsp;截屏时间设置</a></li>' +
            '</ul>');
}
//加载村级监控
var loadCommonModule_J = function (module, plan) {
    
    $('title').html('监控管理'); 
    //$('.leftbar').html('当前支部：xx党支部');
    $('.centerbar').html(
            ' <ul id="list" class="list">' +
                    '<li id="common_module-nav-manage"><a href="../1_index/index_C.html" id="manage">管理模块</a></li>' +
                    '<li id="common_module-nav-camera"><a href="../3_camera/camera_infor.html" id="camera">监控管理</a></li>' +
                    '<li id="common_module-nav-system"><a href="../2_system/partymember.html" id="system">运维管理</a></li>' +
                '</ul>');
    $('.rightbar').html(
             '<ul id="list_out" class="list_out">' +
                    '<li><a href="../0_login/login.html" id="loginout"><img src="../assets/img/1_index/tuichu.png" />&nbsp;&nbsp;注销</a></li>' +
                '</ul>');
    $('.sidebar').html(
            '<ul class="list-group">' +
                '<li class="list-group-item"><a href="camera_infor.html"><img src="../assets/img/2_camera/jiankong.png" />&nbsp;&nbsp;监控信息</a></li>' +
               '<li class="list-group-item"><a href="timeSet.html"><img src="../assets/img/2_camera/jieping.png" />&nbsp;&nbsp;截屏时间设置</a></li>' +
            '</ul>');
}
//登陆状态检测
var accountCheck = function (authorityID) {
    console.log($.cookie("JTZH_districtName"));
    $('#zhibu').html($.cookie("JTZH_districtName"));
   
    if (typeof ($.cookie("JTZH_userID")) == "undefined") { 
        $('#common-alert').find('.modal-title').html('');
        $('#common-alert').find('.modal-title').html('警告');
        $('#common-alert').find('.modal-body').html('');
        $('#common-alert').find('.modal-body').html('登录信息已过期！1秒后将跳回登录界面。。。');
        $('#common-alert').modal();
        setTimeout(function () {
            window.location.href = "../0_login/login.html"
        }, 1000)
    }  
}
//控制台登陆状态检测
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



