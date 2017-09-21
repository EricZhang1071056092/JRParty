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
        "print": ["../assets/js/print"],
        "easyui": ["../assets/js/easyui/jquery.easyui.min"],
        "scroll": ["../inform/js/scroll"],
        "jquery.ztree.core":["../assets/js/jquery.ztree.core"],
        "jquery.ztree.excheck":["../assets/js/jquery.ztree.excheck"],
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
        "print": {
            deps: ['jquery']
        },
        "easyui": {
            deps: ['jquery']
        },
        "scroll": {
            deps: ['jquery']
        },
        "jquery.ztree.core": {
            deps: ['jquery']
        },
        "jquery.ztree.excheck": {
            deps: ['jquery.ztree.core']
        },
    }
})


/*---------------接口地址----------------*/
//脚本里用到的所有的转发连接都放在这里
var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
          : "")
          + window.location.host;
//var svc_sys = svcHeader + "/JRPartyService/Party.svc";
//var svc_uoload = svcHeader + "/JRPartyService/Data";
var svc_sys =svcHeader + "/JRPartyService/Party.svc";
var svc_system = svcHeader +"/JRPartyService/System.svc";
var svc_dynamic =svcHeader + "/JRPartyService/Dynamic.svc"; 
var svc_tv = svcHeader + "/JRPartyService/picture/";
var svc_PhotoTake = svcHeader +"/JRPartyService/Upload/PhotoTake/"; 
var svc_uoload = svcHeader + "/JRPartyService/Data";
var svc_file = svcHeader + "/JRPartyService/Upload/Activity/";
var svc_position = svcHeader + "/JRPartyService/Position.svc";
var svc_tissue = svcHeader + "/JRPartyService/Tissue.svc";
var svc_team = svcHeader + "/JRPartyService/Team.svc";
var svc_regime = svcHeader + "/JRPartyService/Regime.svc";
var svc_brand = svcHeader + "/JRPartyService/Brand.svc";
var svc_inform = svcHeader + "/JRPartyService/Information.svc";
/*----------------集成方法---------------*/
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
//退出
var logout = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    $.cookie("JTZH_districtID", null, { path: '/' })
    window.location.href = "../login.html";
}

/*----------------------------------加载公用模块-------------------------*/

//加载市级计划
var loadCommonModule_C = function (module, plan) {
    $('title').html('句容党建');
    $('.menu_list').html('<ul>' +
                        '<li class="plan"style="background:red">' +
                            '<p class="fuMenu" style="background-color:#0077d1;"><img src="../assets/img/1_index/huodong.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本活动&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd1"onclick="ChangeUrl(\'' + '../1_index/index_CC.html' + '\')">任务发布</p>' +
                                '<p class="zcd" id="zcd2"onclick="ChangeUrl(\'' + '../1_index/task_c.html' + '\')">任务管理</p>' +
                                '<p class="zcd" id="zcd3"onclick="ChangeUrl(\'' + '../1_index/exam_c.html' + '\')">统计汇总</p>' +
                           ' </div>' +
                        '</li>' +
                          '<li class="position"id="nav_position" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhengdi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本阵地&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd4"onclick="ChangeUrl(\'' + '../4_position/position_C.html' + '\')">基本阵地</p>' + 
                           ' </div>' +
                        '</li>' +
                         '<li class=""id="nav_organ">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zuzhi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本组织&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd5"onclick="ChangeUrl(\'' + '../5_organ/Tissue_C.html' + '\')">组织机构</p>' +
                                '<p class="zcd" id="zcd6"onclick="ChangeUrl(\'' + '../5_organ/leadership_C.html' + '\')">领导班子</p>' +
                           ' </div>' +
                        '</li>' +
                        '<li class=""id="nav_team">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/duiwu.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本队伍&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd7"onclick="ChangeUrl(\'' + '../6_team/team_C.html' + '\')">发展党员</p>' +
                                '<p class="zcd" id="zcd8"onclick="ChangeUrl(\'' + '../6_team/volunteer_C.html' + '\')">党员志愿者</p>' +
                                '<p class="zcd" id="zcd9"onclick="ChangeUrl(\'' + '../6_team/ReserveCadre_C.html' + '\')">后备干部队伍</p>' +
                           ' </div>' +
                        '</li>' +
                           '<li class="regime"id="nav_regime" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhidu-.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本制度&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd10"onclick="ChangeUrl(\'' + '../7_Regime/regime_C.html' + '\')">基本制度</p>' +
                           ' </div>' +
                        '</li>' +
                          '<li class="brand"id="nav_brand" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/pingpai.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;党建品牌&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd11"onclick="ChangeUrl(\'' + '../8_brand/brand_C.html' + '\')">党建品牌</p>' +
                           ' </div>' +
                        '</li>' +
                        '<li class="">' +
                           ' <p class="fuMenu"><img src="../assets/img/1_index/guanli-.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;系统管理&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd12"onclick="ChangeUrl(\'' + '../2_system/inform.html' + '\')"  >发布公告</p>' +
                                '<p class="zcd" id="zcd13"onclick="ChangeUrl(\'' + '../3_camera/camera_infor.html' + '\')"  >监控信息</p>' +
                                '<p class="zcd" id="zcd14"onclick="ChangeUrl(\'' + '../3_camera/timeset.html' + '\')" >截屏时间设置</p>' +
                               ' <p class="zcd" id="zcd15"onclick="ChangeUrl(\'' + '../2_system/user.html' + '\')">用户管理</p>' +
                            '</div>' +
                        '</li>' +
                        '<li class="" onclick="window.location.href = \'' + '../0_login/login.html' + '\'">' +
                            '<p class="fuMenu" style="text-align:left;padding-left:20px"><img src="../assets/img/1_index/tuichu.png" width="20" style="margin-top:21px;width:20%;margin-right:14px;" />退出</p>' +
                        '</li>' +
            '</ul>');
}
//加载镇级计划
var loadCommonModule_Z = function (module, plan) {
    $('title').html('句容党建');
    $('.menu_list').html('<ul>' +
                    '<li class="plan">' +
                        '<p class="fuMenu" style="background-color:#0077d1;"><img src="../assets/img/1_index/huodong.png" width="20" style="margin-top: 21px;" />&nbsp;&nbsp;基本活动&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd1"onclick="ChangeUrl(\'' + '../1_index/index_zz.html' + '\')">任务查看</p>' +
                            '<p class="zcd" id="zcd2"onclick="ChangeUrl(\'' + '../1_index/task_z.html' + '\')">任务管理</p>' +
                            '<p class="zcd" id="zcd4" onclick="ChangeUrl(\'' + '../1_index/task_check.html' + '\')">任务审核</p>' +
                            '<p class="zcd" id="zcd3"onclick="ChangeUrl(\'' + '../1_index/exam_zs.html' + '\')">统计汇总</p>' +
                       ' </div>' +
                    '</li>' +
                      '<li class="position"id="nav_position" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhengdi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本阵地&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd5"onclick="ChangeUrl(\'' + '../4_position/position_Z.html' + '\')">基本阵地</p>' +
                                '<p class="zcd" id="zcd6"onclick="ChangeUrl(\'' + '../4_position/position_check.html' + '\')">阵地审核</p>' +
                           ' </div>' +
                        '</li>' +

                    '<li class=""id="nav_organ">' +
                        '<p class="fuMenu"><img src="../assets/img/1_index/zuzhi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本组织&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd7"onclick="ChangeUrl(\'' + '../5_organ/Tissue_Z.html' + '\')">组织机构</p>' +
                            '<p class="zcd" id="zcd8"onclick="ChangeUrl(\'' + '../5_organ/leadership_Z.html' + '\')">领导班子</p>' +
                            '<p class="zcd" id="zcd8"onclick="ChangeUrl(\'' + '../5_organ/leadership_check_Z.html' + '\')">领导班子审核</p>' +
                       ' </div>' +
                    '</li>' +
                     '<li class=""id="nav_team">' +
                        '<p class="fuMenu"><img src="../assets/img/1_index/duiwu.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本队伍&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd9"onclick="ChangeUrl(\'' + '../6_team/team_Z.html' + '\')">发展党员</p>' +
                            '<p class="zcd" id="zcd10"onclick="ChangeUrl(\'' + '../6_team/volunteer_Z.html' + '\')">党员志愿者</p>' +
                            '<p class="zcd" id="zcd11"onclick="ChangeUrl(\'' + '../6_team/ReserveCadre_Z.html' + '\')">后备干部队伍</p>' +
                       ' </div>' +
                    '</li>' +
                      '<li class="regime"id="nav_regime" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhidu-.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本制度&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd12"onclick="ChangeUrl(\'' + '../7_Regime/regime_Z.html' + '\')">基本制度</p>' +
                           ' </div>' +
                  
                     '<li class="brand"id="nav_brand" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/pingpai.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;党建品牌&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd13"onclick="ChangeUrl(\'' + '../8_brand/brand_Z.html' + '\')">党建品牌</p>' +
                           ' </div>' +
                        '</li>' +
                    '<li class="">' +
                       ' <p class="fuMenu"><img src="../assets/img/1_index/guanli-.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;系统管理&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                           '<p class="zcd" id="zcd14"onclick="ChangeUrl(\'' + '../2_system/inform.html' + '\')"  >发布公告</p>' +
                           '<p class="zcd" id="zcd15"onclick="ChangeUrl(\'' + '../2_system/user_Z.html' + '\')">用户管理</p>' +
                        '</div>' +
                    '</li>' +
                    '<li class="" onclick="window.location.href = \'' + '../0_login/login.html' + '\'">' +
                        '<p class="fuMenu" style="text-align:left;padding-left:20px"><img src="../assets/img/1_index/tuichu.png" width="20" style="margin-top:21px;width:20%;margin-right:14px;" />退出</p>' +
                    '</li>' +
        '</ul>' +
    '</div>' +
'</div>');
}
//加载村级计划
var loadCommonModule_cun = function (module, plan) {
    $('title').html('句容党建');
    $('.menu_list').html('<ul>' +
                    '<li class="plan">' +
                        '<p class="fuMenu" style="background-color:#0077d1;"><img src="../assets/img/1_index/huodong.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本活动&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd1"onclick="ChangeUrl(\'' + 'index_cun.html' + '\')"  >任务查看</p>' +
                            '<p class="zcd" id="zcd2"onclick="ChangeUrl(\'' + 'task.html' + '\')"  >任务管理</p>' +
                       ' </div>' +
                    '</li>' +
                      '<li class="position"id="nav_position" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhengdi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本阵地&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd4"onclick="ChangeUrl(\'' + '../4_position/position.html' + '\')">基本阵地</p>' +
                           ' </div>' +
                        '</li>' +
                      '<li class=""id="nav_organ">' +
                        '<p class="fuMenu"><img src="../assets/img/1_index/zuzhi.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本组织&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd3"onclick="ChangeUrl(\'' + '../5_organ/Tissue.html' + '\')">组织机构</p>' +
                            '<p class="zcd" id="zcd4"onclick="ChangeUrl(\'' + '../5_organ/leadership.html' + '\')">领导班子</p>' +
                            '<p class="zcd" id="zcd8"onclick="ChangeUrl(\'' + '../5_organ/leadership_check.html' + '\')">领导班子待审核</p>' +
                       ' </div>' +
                    '</li>' +
                      '<li class=""id="nav_team">' +
                        '<p class="fuMenu"><img src="../assets/img/1_index/duiwu.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本队伍&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                            '<p class="zcd" id="zcd5"onclick="ChangeUrl(\'' + '../6_team/team.html' + '\')">发展党员</p>' +
                            '<p class="zcd" id="zcd6"onclick="ChangeUrl(\'' + '../6_team/volunteer.html' + '\')">党员志愿者</p>' +
                            '<p class="zcd" id="zcd7"onclick="ChangeUrl(\'' + '../6_team/ReserveCadre.html' + '\')">后备干部队伍</p>' +
                       ' </div>' +
                    '</li>' +
                      '<li class="regime"id="nav_regime" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/zhidu-.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;基本制度&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd10"onclick="ChangeUrl(\'' + '../7_Regime/regime.html' + '\')">基本制度</p>' +
                           ' </div>' +
                      '</li>' +
                    '<li class="brand"id="nav_brand" style="background:red">' +
                            '<p class="fuMenu"><img src="../assets/img/1_index/pingpai.png" width="20" style="margin-top: 21px; " />&nbsp;&nbsp;党建品牌&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                            '<div class="div1">' +
                                '<p class="zcd" id="zcd11"onclick="ChangeUrl(\'' + '../8_brand/brand.html' + '\')">党建品牌</p>' +
                           ' </div>' +
                        '</li>' +
                    '<li class="">' +
                       ' <p class="fuMenu"><img src="../assets/img/1_index/guanli-.png" width="20" style="margin-top:21px" />&nbsp;&nbsp;系统管理&nbsp;&nbsp;&nbsp;<i class="glyphicon glyphicon-chevron-right"></i></p>' +
                        '<div class="div1">' +
                           '<p class="zcd" id="zcd12"onclick="ChangeUrl(\'' + '../2_system/inform.html' + '\')"  >发布公告</p>' +
                           '<p class="zcd" id="zcd19"onclick="ChangeUrl(\'' + '../2_system/partymember.html' + '\')">党员管理</p>' +
                           '<p class="zcd" id="zcd20"onclick="ChangeUrl(\'' + '../2_system/user_cun.html' + '\')">用户管理</p>' +
                        '</div>' +
                    '</li>' +
                  
                    '<li class="" onclick="window.location.href = \'' + '../0_login/login.html' + '\'">' +
                        '<p class="fuMenu" style="text-align:left;padding-left:20px"><img src="../assets/img/1_index/tuichu.png" width="20" style="margin-top:21px;width:20%;margin-right:14px;" />退出</p>' +
                    '</li>' +
        '</ul>' +
    '</div>' +
'</div>');

}
//登陆状态检测
var accountCheck1 = function (authorityID) {
    $('#zhibu').html($.cookie("JTZH_districtName"));

    if (typeof ($.cookie("JTZH_userID")) == "undefined" || $.cookie("JTZH_userID") == "") {
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
var accountCheck = function (authorityID) {

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



