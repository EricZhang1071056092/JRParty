/*-------------所有可能加载的脚本--------------*/
require.config({
    paths: {
        "handlebars": ["../assets/js/Basic/handlebars.min"],
        "helper": ["../assets/js/Basic/amazeui.widgets.helper"],
        "amazeui": ["../assets/js/Basic/amazeui.min"],
        "jquery": ["../assets/js/common/jquery.min"],
        "bootstrap": ["../assets/js/common/bootstrap.min"],
        "bootstrap-table": ["../assets/js/common/bootstrap-table"],
        "bootstrap-table-export": ["../assets/js/common/bootstrap-table-export"],
        "tableExport": ["../assets/js/common/tableExport"],
        "echarts": ["../assets/js/ECharts/echarts1"],  
        "echarts2/bar": ["../assets/js/ECharts/chart/bar"],
        "leaflet": ["../assets/js/common/leaflet"],
        "jquery2": ["assets/js/Basic/jquery.min"],
        "amazeui2": ["assets/js/Basic/amazeui.min"],
        "jquery.cookie": ["../assets/js/common/jquery.cookie"],
        "jquery.cookie2": ["assets/js/Basic/jquery.cookie"],
        "transformMap": ["../assets/js/Basic/transformMap"],
        "print": ["../assets/js/Basic/jQuery.print"],
        "fileinput": ["../assets/js/basic/fileinput.min"],
        "canvas-to-blob": ["../assets/js/basic/canvas-to-blob"],
        "markercluster-src": ["../assets/js/common/leaflet/leaflet.markercluster-src"],
        "subgroup-src": ["../assets/js/common/leaflet/subgroup-src"],
        "realworld.388": ["../assets/js/common/leaflet/realworld.388"],
        "bootstrap-select": ["../assets/js/basic/bootstrap-select.min"],
        "fileinput_locale_zh": ["../assets/js/basic/fileinput_locale_zh"],
        "bootstrap-datetimepicker": ["../assets/js/basic/bootstrap-datetimepicker.min"],
        "bootstrap-datetimepicker.zh-CN": ["../assets/js/basic/bootstrap-datetimepicker.zh-CN"],
        "bootstrap-table-zh-cn": ["../assets/js/basic/bootstrap-table-zh-cn"],
        "bootstrap-table-editable": ["../assets/js/basic/bootstrap-table-editable"],
        "bootstrap-editable": ["../assets/js/basic/bootstrap-editable"],
        "fullcustom": ["../assets/js/fullcanlendar/fullcal/fullcalendar"],
        "validationengine-en": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine-en"],
        "validationengine": ["../assets/js/fullcanlendar/formvalidator/js/jquery.validationengine"],
        "jquery-ui-1.8.6.custom": ["../assets/js/fullcanlendar/jquery-ui-1.8.6.custom.min"],
        "jquery-ui-timepicker-addon": ["../assets/js/fullcanlendar/jquery-ui-timepicker-addon"],
        "jqpaginator": ["../assets/js/basic/jqpaginator"],
        "bootstrap-table-key-events": ["../assets/js/basic/bootstrap-table-key-events.min"],
        "baguettebox": ["../assets/js/basic/baguettebox"],
        "leaflet-src": ["https://unpkg.com/leaflet@1.0.1/dist/leaflet-src"],
        "esri-leaflet": ["https://unpkg.com/leaflet@1.0.1/dist/leaflet-srchttps://unpkg.com/esri-leaflet@2.0.4"],

    },
    shim: {
        jquery: {
            exports: 'jquery'
        },
        bootstrap: {
            deps: ['jquery']
        },
        'bootstrap-table': {
            deps: ['bootstrap']
        },
        'bootstrap-table-zh-cn': {
            deps: ['bootstrap-table']
        },
        'bootstrap-table-editable': {
            deps: ['bootstrap-table']
        },
        'bootstrap-editable': {
            deps: ['bootstrap']
        },
        "validationengine-en": {
            deps: ['jquery']
        },
        "validationengine": {
            deps: ['jquery']
        },
        "jquery-ui-timepicker-addon": {
            deps: ['jquery']
        },
        'jquery.cookie': {
            deps: ['jquery']
        },
        'print': {
            deps: ['jquery']
        },
        'bootstrap-datetimepicker': {
            deps: ['bootstrap']
        },
        "bootstrap-datetimepicker.zh-CN": {
            deps: ['bootstrap-datetimepicker']
        },
        fileinput: {
            deps: ['bootstrap']
        },
        fileinput_locale_zh: {
            deps: ['fileinput']
        },
        'amazeui': {
            deps: ['bootstrap']
        },
        'markercluster-src': {
            deps: ['jquery'],
            exports: 'markercluster-src'
        },
        "bootstrap-select": {
            deps: ['bootstrap']
        },
        'subgroup-src': {
            deps: ['jquery'],
            exports: 'subgroup-src'

        },
        'jqpaginator': {
            deps: ['jquery']
        },
        'bootstrap-table-key-events': {
            deps: ['bootstrap-table']
        },
        'bootstrap-table-export': {
            deps: ['bootstrap-table']
        },
        'tableExport': {
            deps: ['bootstrap-table']
        },
        'baguettebox': {
            deps: ['jquery']
        },
        leaflet: {
            exports: 'leaflet'
        },
        "leaflet-src": {
            deps: ['leaflet']
        },
        "esri-leaflet": {
            deps: ['leaflet']
        },
    }
})


/*---------------接口地址----------------*/
//脚本里用到的所有的转发连接都放在这里
var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
		: "")
		+ window.location.host;
var svc_sys = svcHeader + "/YYCService/System.svc";
var svc_pop = svcHeader + "/YYCService/Population.svc";
var svc_party = svcHeader + "/YYCService/Party.svc";
var svc_pub = svcHeader + "/YYCService/Publishment.svc";
var svc_bus = svcHeader + "/YYCService/Business.svc";
var svc_pro = svcHeader + "/YYCService/Property.svc";
var svc_map = svcHeader + "/YYCService/Map.svc";
var svc_uoload = svcHeader + "/YYC/Data";
var svc_news = svcHeader + "/YYCService/Public.svc";
var svc_link = svcHeader + "/YYCService/Personalassistants.svc";

/*----------------集成方法---------------*/
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
/*加载公共模块*/
var loadCommonModule = function (module) {
    $('#common_module-nav').html('<div >' +
         '<img src="../resource/login/img_daohangtiao.png" style=" width:100%;   " />' +

   '</div>' +
  ' <div class="am-collapse am-topbar-collapse am-topbar-inverse" id="doc-topbar-collapse" style="background-color: #eedebb;color:black; ; background-size: 100%; border-bottom-color: blue; background-size: 100%; height: 80px; ">' +
      ' <ul class="am-nav am-nav-pills am-topbar-nav" style="padding-left:3%">' +
          ' <li><a href="../index.html" style="font-size:larger;font-weight:700">首页</a></li>' +
         '  <li id="map_M0000"><a href="../Map/map.html" id="nav-map" style="font-size:larger;font-weight:700">社区地图</a></li>' +
           ' <li id="property_Z0000"><a href="../Property/Property.html" style="font-size:larger;font-weight:700">企业管理</a></li>' +
           ' <li id="party_P0000"><a href="../Party/party-member.html" style="font-size:larger;font-weight:700">智慧党建</a></li>' +
          ' <li id="population_R0000"><a href="../Population/population.html" style="font-size:larger;font-weight:700">人口管理</a></li>' +
          ' <li id="business_B0000"><a href="../Business/Business.html" style="font-size:larger;font-weight:700">政务管理</a></li>' +
         '  <li id="assistant_A0000"><a href="../Assistant/Assistant.html" style="font-size:larger;font-weight:700">个人助理</a></li>' +
          '  <li id="manage_Y0000"><a href="../manage/manage.html" style="font-size:larger;font-weight:700">运维管理</a></li>' +
           '  </ul>' +

    ' <div class="am-topbar-right">' +
    '  <button class="am-btn am-btn-primary am-topbar-btn am-btn-sm signout" onclick="logout()">注销</button>' +
    ' </div>' +
    ' <div class="am-topbar-right">' +
    ' <div class="am-dropdown" data-am-dropdown="{boundary: /".am-topbar/"}">' +
    '  <button class="am-btn am-btn-default am-topbar-btn am-btn-sm am-dropdown-toggle" data-am-dropdown-toggle><span class="am-icon-user"></span></button>' +
    '  <div class="am-dropdown-content">' +
    '     <div data-am-widget="intro" class="am-intro am-cf am-intro-default" style="min-width:400px;color:black">' +
    '   <div class="am-intro-hd">' +
    '    <h2 class="am-intro-title">当前用户信息</h2>' +
    '    <a class="am-intro-more am-intro-more-top " href="#more">修改资料>></a>' +
    ' </div>' +
    '  <div class="am-g am-intro-bd">' +
    ' <div class="am-intro-left am-u-sm-5 informaton-user-img"></div>' +
    '  <div class="am-intro-right am-u-sm-7">' +
    '     <p>用户名：<span class="informaton-user-name"></span></p>' +
    '     <p>职位：<span class="informaton-user-role"></span></p>' +
    '      <p>最后登录时间：<span class="informaton-user-last_time"></span></p>' +
    '    </div>' +
    '   </div>' +
    '  </div>' +
    '  </div>' +
    '   </div>' +
    ' </div>' +
    ' </div>')
    $('#' + module).addClass("am-active");


}




//注销
var logout = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    window.location.href = "../login.html";
}
var logout3 = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    window.location.href = "../login.html";
}
//首页注销
var logout2 = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    window.location.href = "login.html";
}


//登陆状态检测
var accountCheck = function () {
    $('.am-intro-more-top').html('')
    $.ajax({
        url: svc_sys + "/accountCheck",
        data: {
            userID: $.cookie("JTZH_userID")
        },
        success: function (data) {
            var json = eval('(' + data + ')');
            if ($.cookie("JTZH_userID") != "R0001") {
                $("#manage_Y0000").remove();
            }
            for (var i in json.Result.business) {
                if (json.Result.business[i].value == "false") {
                    $("#business_" + json.Result.business[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.party) {
                if (json.Result.party[i].value == "false") {
                    $("#party_" + json.Result.party[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.property) {
                if (json.Result.property[i].value == "false") {
                    $("#property_" + json.Result.property[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.population) {
                if (json.Result.population[i].value == "false") {
                    $("#population_" + json.Result.population[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.map) {
                if (json.Result.map[i].value == "false") {
                    $("#map_" + json.Result.map[i].authorityID + "").remove();
                }
            }



            if (json.Code != 1 && json.Code != "1") {
                $('.am-modal-hd').html('');
                $('.am-modal-hd').html('警告<a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>');
                $('.am-modal-bd').html('');
                $('.am-modal-bd').html('登录信息已过期！2秒后将跳回登录界面。。。');
                $('#modal-alert').modal();
                setTimeout(function () {
                    window.location.href = "../login.html"
                }, 2000)
            } else {
                $('.informaton-user-img').html('<img src="' + json.Result.portrait + '" />');
                $('.informaton-user-name').html(json.Result.name);
                $('.informaton-user-role').html(json.Result.role);
                $('#welcome').html('欢迎' + json.Result.name + '登陆！');
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
                window.location.href = "../login.html"
            }, 2000)

        }
    })
}
//首页登录状态检测
var accountCheck2 = function () {
    $('.am-intro-more-top').html('')
    $.ajax({
        url: svc_sys + "/accountCheck",
        data: {
            userID: $.cookie("JTZH_userID")
        },
        success: function (data) {
            var json = eval('(' + data + ')');
            if ($.cookie("JTZH_userID") != "R0001") {
                console.log($.cookie("JTZH_userID"));
                $("#manage_Y0000").remove();
            }
            for (var i in json.Result.business) {
                if (json.Result.business[i].value == "false") {
                    $("#business_" + json.Result.business[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.party) {
                if (json.Result.party[i].value == "false") {
                    $("#party_" + json.Result.party[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.property) {
                if (json.Result.property[i].value == "false") {
                    $("#property_" + json.Result.property[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.population) {
                if (json.Result.population[i].value == "false") {
                    $("#population_" + json.Result.population[i].authorityID + "").remove();
                }
            }
            for (var i in json.Result.map) {
                if (json.Result.map[i].value == "false") {
                    $("#map_" + json.Result.map[i].authorityID + "").remove();
                }
            }
            if (json.Code != 1 && json.Code != "1") {
                $('.am-modal-hd').html('');
                $('.am-modal-hd').html('警告<a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>');
                $('.am-modal-bd').html('');
                $('.am-modal-bd').html('登录信息已过期！2秒后将跳回登录界面。。。');
                $('#modal-alert').modal();
                setTimeout(function () {
                    window.location.href = "login.html"
                }, 2000)
            } else {
                $('.informaton-user-img').html('<img src="' + json.Result.portrait + '" />');
                $('.informaton-user-name').html(json.Result.name);
                $('.informaton-user-role').html(json.Result.role);
                $('#welcome').html('欢迎' + json.Result.name + '登陆！');
                $('.informaton-user-last_time').html("<br />" + json.Result.lastTime);
            }
        }
    })
}
//修改头像
var head_portrait = function () {
    console.log($.cookie("JTZH_userID"));
    $("#head_picture_upload .am-modal-bd").html('<input id="input-image-3" name="input-image" type="file" class="file-loading" accept="image/*">');
    $("#input-image-3").fileinput({
        language: 'zh', //设置语言
        uploadUrl: svc_uoload + "/editportrait.ashx?userID=" + $.cookie("JTZH_userID"),
        allowedFileExtensions: ["jpg", "png", "gif"],
        maxImageWidth: 100,
        maxImageHeight: 200,
        resizePreference: 'height',
        maxFileCount: 1,
        resizeImage: true
    }).on('filepreupload', function () {
        $(".am-close").click();
        $('#kv-success-box').html('');
    }).on('fileuploaded', function (event, data) {
        $('#kv-success-box').append(data.response.link);
        $('#kv-success-modal').modal('show');
    });
    $('#head_picture_upload').modal({
        width: '800',
        height: '400',
        relatedTarget: this
    });

}



var clearPopup = function () {
    $(".add-popup").html('');
}

/*----------部分插件功能----------*/
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

var contractStr =
    '<div id="contract-container">' +
		'<div class="am-g">' +
			'<div class="am-u-md-6 part-a">' +
				'<div style="text-aline:center">' +
					'<h3>房屋租赁合同</h3>' +
				'</div>' +
                '<div style="font-size:8px">' +
				'<p>甲方：<b>吴江区智慧震泽</b></p>' +
				'<p>乙方：<span id="contract-name" style="text-decoration:underline">________________</span> 证件号：<span id="contract-IDCard" style="text-decoration:underline">________________</span></p>' +
				'<p>根据《中华人民共和国合同法》、《中华人民共和国城市房地产管理法》、《城市房屋租赁管理办法》等国家有关法律、法规规定，为了明确双方的权利和义务，经双方协商，特签订以下房屋租赁合同共同遵守：</p>' +
				'<p>一、房屋的座落及面积</p>' +
				'<p>1、乙方同意承租座落于智慧震泽<span id="contract-location" style="text-decoration:underline">________________</span>之房屋，租赁面积约<span id="contract-area" style="text-decoration:underline">________________</span>㎡</p>' +
				'<p>二、租赁期限</p>' +
				'<p>甲方同意将上述房屋出租给乙方，使用期限为<span id="contract-deadline" style="text-decoration:underline">________________</span>年，租期自<span id="contract-startTime" style="text-decoration:underline">____________</span>始至<span id="contract-endTime" style="text-decoration:underline">________________</span>止。</p>' +
				'<p>三、租金和缴纳租金的办法（币种：人民币，租金不含税）</p>' +
				'<p>1、乙方必须向甲方交纳租金<span  id="contract-rent" style="text-decoration:underline">________________</span>元，人民币（大写）<span id="contract-rentLarge" style="text-decoration:underline">________________</span>，于协议签订时付清。' +
				'</p>' +
				'<p>四、房屋承租用途：乙方承诺所租房屋用于<span id="contract-usage" style="text-decoration:underline">________________</span>经营用途，甲方按现状出租，按照双方约定用途，涉及房屋经营的许可证照办理、消防报批、相关审批手续等所有事宜全部由乙方自行负责解决，如因乙方无法办理相关用途的证件许可，一切责任乙方自负。</p>' +
				'<p>五、转租、分租</p>' +
                '<p>.......</p>' +
				//'<p>本合同项下的房屋不得进行转租、分租，如发现擅自转租、分租情况，即属乙方违约，甲方有权单方面解除与乙方的租赁合同，追究乙方的相关违约责任及造成的损失。</p>' +
				'<p>六、甲乙双方权利和义务</p>' +
                '<p>.......</p>' +
				//'<p>1、租赁期内，房屋主体建筑结构自然损坏应由甲方负资修理，若人为损坏、失火等因素引起的损失及维修责任全部由乙方承担并负责赔偿。</p>' +
				//'<p>2、出租房屋禁止经营危险品行业，不得存放和使用液化气灶等明火设备，出租房屋内禁止人员居住、留宿，否则，由此产生的一切责任及后果全部由乙方自行承担，同时甲方有权单方面解除合同，收回房屋，已收租金不予退还。</p>' +
				//'<p>3、租赁期间（包括装修期）发生的水、电、电话费、燃气、物业管理费等一切因实际使用房屋产生的费用均由乙方承担。甲方或甲方委托部门应协助乙方处理好与房屋所在地区有关部门的关系，包括通电通水。如上述费用有甲方代缴的，乙方必须在甲方交付后一月内支付给甲方。</p>' +
				//'<p>4、甲方同意乙方对房屋进行适当装修，但不得变动和破坏房屋的主体结构和承重墙结构，变动房屋结构必须提前7天以书面形式报告甲方，在得到甲方书面认可后方可改变房屋结构，装修费及装修行为由乙方负责。乙方设立店面招牌或广告牌时必须经规划、城管等相关主管部门审批并向甲方进行设计方案备档，装修及经营过程中发生的一切包括安全在内的事故与甲方无关。但甲方对乙方装修方案及其它事项的备案并不意味着对乙方上述行为合法性的认可。</p>' +
				//'<p>5、合同期满或甲方按规定终止租赁合同，则在终止日起七个自然日内乙方应将房屋完好移交给甲方，乙方自行购置的动产部分由乙方自行处理，装修的不动产部分归甲方所有，不作任何补偿，如超过一周尚留在租赁房屋内的乙方物品，视为乙方自动放弃所有权，甲方有权任意处置，并不用补偿或承担乙方因上述资产毁损的任何责任并在到期后可另行租赁他人。如因乙方原因导致房屋出现毁损的，甲方可依法要求乙方作价赔偿。</p>' +
				//'<p>6、租赁期间，因承租房屋发生的一切经济纠纷、民事纠纷、社会性负担、相关规费、一切意外情况及安全事故全部责任由乙方负责，与甲方无关。乙方必须严格按照相关法律、法规，服从社区、工商、城管等职能部门的管理，如有不符合公安、消防、工商、卫生许可等要求而引起一切责任由乙方承担。</p>' +
				//'<p>7、乙方向甲方所付租赁费，如乙方中途终止合同，甲方不予退回。如租赁期满前，由甲方另行委托拍租，同等条件下乙方享有优先租赁权。房屋租赁期满，如乙方无正当理由拒绝交还钥匙，甲方有权在七天后停水停电并自行进入出租房屋，并可处理乙方在租赁房屋内的物品而不必承担责任，同时追究乙方逾期不搬迁的赔偿责任，即乙方每天应赔偿甲方因此所受的损矢（按当年每天应房屋租金标准的3倍计算），必要时甲方可以向甲方所在地人民法院起诉。造成甲方损失，乙方还应承担赔偿责任。（赔偿责任包括但不限于已产生的租金、诉讼费、律师费等）</p>' +
				//'<p> 8、租赁期限内，如因政府拆迁、村道规划、整改等不可抗力情况需终止租赁合同的，乙方必须无条件配合搬出，</p>' +
                '</div>' +
            '</div>' +
			'<div class="am-u-md-6 part-b">' +
				'<div style="text-align:right">' +
					'<h3 style="color :red" id="contract-contractID">111111</h3>' +
				'</div>' +
                '<div style="font-size:8px">' +
				'<p>双方均不作为违约，甲方不予补偿，租金按实际使用时间根据当年租金分摊后进行结算。（房屋拆迁补偿款归甲方所有，如政府拆迁部门明确给予承租人的拆迁补偿费除外）。</p>' +
				'<p>9、其他<span id="contract-remark" style="text-decoration:underline">________________</span>。</p>' +
				'<p>七、提前解除（终止）本合同的条件</p>' +
                '<p>.......</p>' +
				//'<p>（一）、合同的法定解除</p>' +
				//'<p>l、经双方协商一致，提前终止本合同的；</p>' +
				//'<p>2、因不可抗力或政府行为而使本合同无法履行致使本合同提前终止的；</p>' +
				//'<p>（二）、单方面终止本合同的条件</p>' +
				//'<p>乙方有下列情形之一的，甲方有权终止租赁合同、收回房屋且已收租金不予退回：如造成甲方损失的，乙方应予以赔偿。</p>' +
				//'<p>l、利用房屋进行非法活动，有损公共利益的；</p>' +
				//'<p>2、累计拖欠房屋租金一个月以上；</p>' +
				//'<p>3、擅自改变房屋结构；</p>' +
				//'<p>4、乙方进行转租、分租的；</p>' +
				//'<p>甲方有下列情形之一的，乙方有权终止租赁合同并按剩余时间要求退回租金：</p>' +
				//'<p>1、未按合同约定交付房屋的；</p>' +
				//'<p>2、已处于危险状态无法正常使用不肯修理的。</p>' +
				'<p> 八、违约责任</p>' +
                '<p>.......</p>' +
				//'<p>任何一方违反本合同项下任一条款或擅自终止合同，应按国家有关法规，承担违约责任。若乙方逾期缴纳租金，除应如数补缴外，还需支付按逾期租金每日千分之五的违约金，如逾期15天以上，除缴纳相应的违约金外，再追加应付未付款项20%作为赔偿金。</p>' +
				'<p>九、争议的解决办法</p>' +
                '<p>.......</p>' +
				//'<p>房屋租赁期间发生争议，甲乙双方应当友好协商予以解决；协商不成的，任何一方均可向租赁物所在地的人民法院起诉。</p>' +
				'<p>十、其他双方约定事项</p>' +
                '<p>.......</p>' +
				//'<p>1、房屋用水、用电甲方如按月抄表，乙方必须配合并当月缴清全部费用。甲方开具相应用水、用电收据。借故拖欠，甲方有权采取停电、停水措施。</p>' +
				//'<p>2、如因经营需要，涉及水、电等需另行安装及增容的，由乙方自行负责申请并承担相应费用，租赁期满后，增容的设备、设施无偿归甲方所有。</p>' +
				'<p>十一、合同生效</p>' +
				'<p>本合同经各方代表人（或委托人）签字/加盖公章后即生效。</p>' +
				'<p>本合同一式三份，甲方持二份、乙方持一份，均具有同等效力。</p>' +
				'<div style="text-indent:2em">乙方：&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp甲方：智慧震泽</div>' +
				'<div style="text-indent:2em">电话：&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp电话：63506590（收费组）</div>' +
				'<div style="text-indent:2em">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp63503709（维修水电）</div>' +
				'<div style="text-indent:2em">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp签约时间：<span id="contract-creatTime"></span></div>' +
                  '<p style=" font-size:18px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp 收费人： <span  id="contract-contractUser"> </span></div>' +
                '</p>' +
				'<div>' +

				'</div>' +
			'</div>' +
		'</div>' +
	'</div>';

//var contractStr =
//    '<div id="contract-container">' +
//		'<div class="am-g">' +
//			'<div class="am-u-md-6 part-a">' +
//				'<div style="text-aline:center">' +
//					'<h3>房屋租赁合同</h3>' +
//				'</div>' +
//                '<div style="font-size:8px">' +
//				'<p>甲方：<b>吴江区智慧震泽</b></p>' +
//				'<p>乙方：<span id="contract-name" style="text-decoration:underline">________________</span> 证件号：<span id="contract-IDCard" style="text-decoration:underline">________________</span></p>' +
//				'<p>根据《中华人民共和国合同法》、《中华人民共和国城市房地产管理法》、《城市房屋租赁管理办法》等国家有关法律、法规规定，为了明确双方的权利和义务，经双方协商，特签订以下房屋租赁合同共同遵守：</p>' +
//				'<p>一、房屋的座落及面积</p>' +
//				'<p>1、乙方同意承租座落于智慧震泽<span id="contract-location" style="text-decoration:underline">________________</span>之房屋，租赁面积约<span id="contract-area" style="text-decoration:underline">________________</span>㎡</p>' +
//				'<p>二、租赁期限</p>' +
//				'<p>甲方同意将上述房屋出租给乙方，使用期限为<span id="contract-deadline" style="text-decoration:underline">________________</span>年，租期自<span id="contract-startTime" style="text-decoration:underline">____________</span>始至<span id="contract-endTime" style="text-decoration:underline">________________</span>止。</p>' +
//				'<p>三、租金和缴纳租金的办法（币种：人民币，租金不含税）</p>' +
//				'<p>1、乙方必须向甲方交纳租金<span  id="contract-rent" style="text-decoration:underline">________________</span>元，人民币（大写）<span id="contract-rentLarge" style="text-decoration:underline">________________</span>，于协议签订时付清。' +
//				'</p>' +
//				'<p>四、房屋承租用途：乙方承诺所租房屋用于<span id="contract-usage" style="text-decoration:underline">________________</span>经营用途，甲方按现状出租，按照双方约定用途，涉及房屋经营的许可证照办理、消防报批、相关审批手续等所有事宜全部由乙方自行负责解决，如因乙方无法办理相关用途的证件许可，一切责任乙方自负。</p>' +
//				'<p>五、转租、分租</p>' +
//                '<p>.......</p>' +
//				//'<p>本合同项下的房屋不得进行转租、分租，如发现擅自转租、分租情况，即属乙方违约，甲方有权单方面解除与乙方的租赁合同，追究乙方的相关违约责任及造成的损失。</p>' +
//				'<p>六、甲乙双方权利和义务</p>' +
//                '<p>.......</p>' +
//				//'<p>1、租赁期内，房屋主体建筑结构自然损坏应由甲方负资修理，若人为损坏、失火等因素引起的损失及维修责任全部由乙方承担并负责赔偿。</p>' +
//				//'<p>2、出租房屋禁止经营危险品行业，不得存放和使用液化气灶等明火设备，出租房屋内禁止人员居住、留宿，否则，由此产生的一切责任及后果全部由乙方自行承担，同时甲方有权单方面解除合同，收回房屋，已收租金不予退还。</p>' +
//				//'<p>3、租赁期间（包括装修期）发生的水、电、电话费、燃气、物业管理费等一切因实际使用房屋产生的费用均由乙方承担。甲方或甲方委托部门应协助乙方处理好与房屋所在地区有关部门的关系，包括通电通水。如上述费用有甲方代缴的，乙方必须在甲方交付后一月内支付给甲方。</p>' +
//				//'<p>4、甲方同意乙方对房屋进行适当装修，但不得变动和破坏房屋的主体结构和承重墙结构，变动房屋结构必须提前7天以书面形式报告甲方，在得到甲方书面认可后方可改变房屋结构，装修费及装修行为由乙方负责。乙方设立店面招牌或广告牌时必须经规划、城管等相关主管部门审批并向甲方进行设计方案备档，装修及经营过程中发生的一切包括安全在内的事故与甲方无关。但甲方对乙方装修方案及其它事项的备案并不意味着对乙方上述行为合法性的认可。</p>' +
//				//'<p>5、合同期满或甲方按规定终止租赁合同，则在终止日起七个自然日内乙方应将房屋完好移交给甲方，乙方自行购置的动产部分由乙方自行处理，装修的不动产部分归甲方所有，不作任何补偿，如超过一周尚留在租赁房屋内的乙方物品，视为乙方自动放弃所有权，甲方有权任意处置，并不用补偿或承担乙方因上述资产毁损的任何责任并在到期后可另行租赁他人。如因乙方原因导致房屋出现毁损的，甲方可依法要求乙方作价赔偿。</p>' +
//				//'<p>6、租赁期间，因承租房屋发生的一切经济纠纷、民事纠纷、社会性负担、相关规费、一切意外情况及安全事故全部责任由乙方负责，与甲方无关。乙方必须严格按照相关法律、法规，服从社区、工商、城管等职能部门的管理，如有不符合公安、消防、工商、卫生许可等要求而引起一切责任由乙方承担。</p>' +
//				//'<p>7、乙方向甲方所付租赁费，如乙方中途终止合同，甲方不予退回。如租赁期满前，由甲方另行委托拍租，同等条件下乙方享有优先租赁权。房屋租赁期满，如乙方无正当理由拒绝交还钥匙，甲方有权在七天后停水停电并自行进入出租房屋，并可处理乙方在租赁房屋内的物品而不必承担责任，同时追究乙方逾期不搬迁的赔偿责任，即乙方每天应赔偿甲方因此所受的损矢（按当年每天应房屋租金标准的3倍计算），必要时甲方可以向甲方所在地人民法院起诉。造成甲方损失，乙方还应承担赔偿责任。（赔偿责任包括但不限于已产生的租金、诉讼费、律师费等）</p>' +
//				//'<p> 8、租赁期限内，如因政府拆迁、村道规划、整改等不可抗力情况需终止租赁合同的，乙方必须无条件配合搬出，</p>' +
//                '</div>' +
//            '</div>' +
//			'<div class="am-u-md-6 part-b">' +
//				'<div style="text-align:right">' +
//					'<h3 style="color :red" id="contract-contractID">111111</h3>' +
//				'</div>' +
//                '<div style="font-size:8px">' +
//				'<p>双方均不作为违约，甲方不予补偿，租金按实际使用时间根据当年租金分摊后进行结算。（房屋拆迁补偿款归甲方所有，如政府拆迁部门明确给予承租人的拆迁补偿费除外）。</p>' +
//				'<p>9、其他<span id="contract-remark" style="text-decoration:underline">________________</span>。</p>' +
//				'<p>七、提前解除（终止）本合同的条件</p>' +
//                '<p>.......</p>' +
//				//'<p>（一）、合同的法定解除</p>' +
//				//'<p>l、经双方协商一致，提前终止本合同的；</p>' +
//				//'<p>2、因不可抗力或政府行为而使本合同无法履行致使本合同提前终止的；</p>' +
//				//'<p>（二）、单方面终止本合同的条件</p>' +
//				//'<p>乙方有下列情形之一的，甲方有权终止租赁合同、收回房屋且已收租金不予退回：如造成甲方损失的，乙方应予以赔偿。</p>' +
//				//'<p>l、利用房屋进行非法活动，有损公共利益的；</p>' +
//				//'<p>2、累计拖欠房屋租金一个月以上；</p>' +
//				//'<p>3、擅自改变房屋结构；</p>' +
//				//'<p>4、乙方进行转租、分租的；</p>' +
//				//'<p>甲方有下列情形之一的，乙方有权终止租赁合同并按剩余时间要求退回租金：</p>' +
//				//'<p>1、未按合同约定交付房屋的；</p>' +
//				//'<p>2、已处于危险状态无法正常使用不肯修理的。</p>' +
//				'<p> 八、违约责任</p>' +
//                '<p>.......</p>' +
//				//'<p>任何一方违反本合同项下任一条款或擅自终止合同，应按国家有关法规，承担违约责任。若乙方逾期缴纳租金，除应如数补缴外，还需支付按逾期租金每日千分之五的违约金，如逾期15天以上，除缴纳相应的违约金外，再追加应付未付款项20%作为赔偿金。</p>' +
//				'<p>九、争议的解决办法</p>' +
//                '<p>.......</p>' +
//				//'<p>房屋租赁期间发生争议，甲乙双方应当友好协商予以解决；协商不成的，任何一方均可向租赁物所在地的人民法院起诉。</p>' +
//				'<p>十、其他双方约定事项</p>' +
//                '<p>.......</p>' +
//				//'<p>1、房屋用水、用电甲方如按月抄表，乙方必须配合并当月缴清全部费用。甲方开具相应用水、用电收据。借故拖欠，甲方有权采取停电、停水措施。</p>' +
//				//'<p>2、如因经营需要，涉及水、电等需另行安装及增容的，由乙方自行负责申请并承担相应费用，租赁期满后，增容的设备、设施无偿归甲方所有。</p>' +
//				'<p>十一、合同生效</p>' +
//				'<p>本合同经各方代表人（或委托人）签字/加盖公章后即生效。</p>' +
//				'<p>本合同一式三份，甲方持二份、乙方持一份，均具有同等效力。</p>' +
//				'<div style="text-indent:2em">乙方：&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp甲方：智慧震泽</div>' +
//				'<div style="text-indent:2em">电话：&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp电话：63506590（收费组）</div>' +
//				'<div style="text-indent:2em">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp63503709（维修水电）</div>' +
//				'<div style="text-indent:2em">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp签约时间：<span id="contract-creatTime"></span></div>' +
//                  '<p style=" font-size:18px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp 收费人： <span  id="contract-contractUser"> </span></div>' +
//                '</p>' +
//				'<div>' +

//				'</div>' +
//			'</div>' +
//		'</div>' +
//	'</div>';

var contractStrPrint =
    '<div id="contract-container-print" style="font-family:simsun">' +
		'<div class="am-g">' +
			'<div class="am-u-md-6 part-a" style="margin-bottom:10px;">' +
				'<div style="margin-left:20%; font-size:22px;font-weight:bold">' +
				 '<p style=" font-size:18px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <span  id="contract-contractID-print"style="margin-left:84%"> </span></div>' +

				'</div>' +
                '<p>&nbsp</p>' +
                '<div style="font-size:13px">' +

                //乙方：               证件号：
				'<p style=" font-size:18px;margin-bottom:0px;padding-bottom:0px;margin-left:2%">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <span id="contract-name-print"></span> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span id="contract-IDCard-print"style=" margin-left:5%"></span>    &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span id="contract-remark-print" style="margin-left:25%"></span></p>' +
                //其他
                '<p style="text-indent:44em;font-size:18px;margin-top:5px;padding-left:20px;"> &nbsp&nbsp&nbsp&nbsp<span id=" " ></span></p>' +


                '<p style="text-indent:2em;font-size:18px;">&nbsp</p>' +
                //地址                  面积
				'<p style="text-indent:6em;font-size:18px;"> <span id="contract-location-print" style=" margin-left:12%"></span> <span id="contract-area-print"style="margin-left:11%" > </span> </p>' +

               //期限                                                                                                     开始                                                                       结束
				'<p style="text-indent:15em;font-size:18px;margin-top:12px;margin-left:25px">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span id="contract-deadline-print"></span>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span id="contract-startTime-print";> </span>至<span id="contract-endTime-print"> </span><span> 止</span></p>' +

                //租金              大写
				'<p style="text-indent:11em;font-size:18px;margin-top:12px;margin-left:25px"> &nbsp&nbsp<span  id="contract-rent-print" > </span> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span id="contract-rentLarge-print"style="margin-left:40px">></span>&nbsp' +

                //用途    
				'<p style="text-indent:17em;font-size:18px;margin-top:9px">&nbsp <span id="contract-usage-print"></span>&nbsp </p>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                '<br/>' +
                 '<br/>' +
                '<br/>' +
                '<br/>' +
                 '<br/>' +
                 '<br/>' +
                   '<br/>' +
                 '<br/>' +
                   '<br/>' +
                 '<div style="text-indent:35em;font-size:21px;margin-top:3px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span  id="contract-creatTime-print"style="margin-left:20%"> </span></div>' +
              '<p style=" font-size:18px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <span style="margin-left:41%"> 收费人： <span  id="contract-contractUser-print"> </span></div>' +



            '</div>' +
			'<div class="am-u-md-6 part-b" style="margin-top:30px;margin-bottom:30px;">' +

				'</div>' +
			'</div>' +
		'</div>' +
	'</div>';
var shoujuStr =
    '<div id="contract-container">' +
		'<div class="am-g">' +

			'<div class="am-u-md-12">' +

              '<div class="am-popup-bd">' +
                '<form action="" class="am-form" id="myform" data-am-validator>' +
                    '<fieldset>' +
                     ' <div class="am-form-inline">' +
                        '<div class="am-form-group">' +
                            '<label for="shouju_contractID">合同编号：</label>' +
                            '<input type="text" id="shouju_contractID" placeholder="合同编号"   />' +
                        '</div>' +
                        '<div class="am-form-group"style="margin-left:12%">' +
                            '<label for="shouju_name">姓名：</label>' +
                            '<input type="text" id="shouju_name" placeholder="姓名"  />' +
                        '</div>' +
                         '</div>' +
                          ' <div class="am-form-inline">' +
                        '<div class="am-form-group">' +
                            '<label for="shouju_money">金额：</label>' +
                            '<input type="text" id="shouju_money" placeholder="金额"  />' +
                        '</div>' +
                         '<div class="am-form-group"style="margin-left:12%">' +
                            '<label for="shouju_paytype_lager">金额大写：</label>' +
                            '<input type="text" id="shouju_paytype_lager" placeholder="金额大写" />' +
                        '</div>' +

                          '</div>' +
                           ' <div class="am-form-inline">' +
                         '<div class="am-form-group">' +
                            '<label for="shouju_paytype">结算方式：</label>' +
                            '<select id="shouju_paytype" required>' +
                                '<option value="现金">现金</option>' +
                                '<option value="卡付">卡付</option>' +
                                '<option value="转账">转账</option>' +
                            '</select>' +
                        '</div>' +
                          '<div class="am-form-group"style="margin-left:12%">' +
                            '<label for="shouju_content">内容：</label>' +
                            '<input type="text" id="shouju_content" placeholder="内容" />' +
                        '</div>' +
                         '</div>' +
                        '<div class="am-form-group">' +
                            '<label for="shouju_creatTime">创建时间：</label>' +
                            '<input type="text" id="shouju_creatTime" placeholder="创建时间" />' +
                        '</div>' +


                    '</fieldset>' +
                '</form>' +

                '<button class="  am-btn-secondary  am-btn-block"    id="money_submit">提交</button>' +
            '</div>' +
            '</div>' +

		'</div>' +
	'</div>';
var shoujuStr2 =
    '<div id="shouju-container-print" style="font-family:simsun">' +
		 ' <div style=";display:inline-block">' +
            ' <div style="width:180mm;height:86mm;">' +
            '<h1 style="text-decoration:underline;text-indent:2em">&nbsp;&nbsp;江苏省农村合作经济组织内部结算凭证&nbsp;&nbsp;&nbsp;</h1>' +
            '<div style="text-indent:1em" ><span style="float:left">交款单位（人）：</span><span id="shouju_name_preview"style="float:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 收款日期<span id="shouju_year_preview">&nbsp;&nbsp;</span> 年<span id="shouju_month_preview">&nbsp;&nbsp;</span > 月<span id="shouju_day_preview">&nbsp;&nbsp;</span>日<span ></span></div>' +
'<table border="2"style="width:672px">' +
    '<tbody>' +
        '<tr>' +
            '<td>' +
                '<div style="text-indent:2em">' +
                    '<p style="line-height:18mm"><span style="float:left">收款内容：</span><span id="shouju_paycontent_preview"style="float:left;text-decoration:underline"> </span><span>结算方式：</span><span id="shouju_paytype_preview"style=" text-decoration:underline"> </span></p>' +
                                '<p><span style="float:left;">收款金额（大写）：</span><span id="shouju_paytype_lager_preview" style="float:left;text-decoration:underline"> </span>&nbsp;&nbsp;</p>' +
                                '<p  ><span style="float:left;text-indent:9em"> ￥：</span><span  id="shouju_money_preview"style="float:left;text-decoration:underline"> </span>&nbsp;&nbsp; </p>' +
                            '</div>' +
                        '</td>' +
                    '</tr>' +
                    '<tr>' +
                        '<td>' +
                            '<div style="text-indent:2em">' +
                                '<p>注明：&nbsp;&nbsp; ①本凭证仅限于农村合作经济组织内部结算使用</p>' +
                                '<p> ②本凭证须加盖单位财务专用章有效</p>' +
                            '</div>' +
                        '</td>' +
                    '</tr>' +
                '</tbody>' +
           ' </table>' +
           ' <div style="text-indent:2em"> 收款单位（章）： &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id="shouju_user_preview">收款人：<span id="shouju_userName_preview"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></div> ' +
'</div>' +
   ' </div>' +
   ' <div style="position:fixed;top:100px; display:inline-block; width:14px">第一联存</div>' +
    '<div style="position:fixed;top:250px; display:inline-block; width:14px">根</div>' +
	'</div>';
var shoujuStrPrint =
   '<div id="shouju-container-print" style="font-family:simsun">' +
		 ' <div style=";display:inline-block">' +
            ' <div style="width:180mm;height:86mm;">' +
            '<h1 style="text-decoration:underline;text-indent:2em">&nbsp;&nbsp;江苏省农村合作经济组织内部结算凭证&nbsp;&nbsp;&nbsp;</h1>' +
            '<div style="text-indent:1em" ><span style="float:left">交款单位（人）：</span><span id="shouju_name_print"style="float:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> 收款日期<span id="shouju_year_print">&nbsp;&nbsp;</span> 年<span id="shouju_month_print">&nbsp;&nbsp;</span > 月<span id="shouju_day_print">&nbsp;&nbsp;</span>日<span ></span></div>' +
'<table border="2"style="width:672px">' +
    '<tbody>' +
        '<tr>' +
            '<td>' +
                '<div style="text-indent:2em">' +
                    '<p style="line-height:18mm"><span style="float:left">收款内容：</span><span id="shouju_paycontent_print"style="float:left;text-decoration:underline"> </span><span>结算方式：</span><span id="shouju_paytype_print"style=" text-decoration:underline:float:left"> </span></p>' +
                                '<p><span style="float:left;">收款金额（大写）：</span><span id="shouju_paytype_lager_print" style="float:left;text-decoration:underline"> </span>&nbsp;&nbsp;</p>' +
                                '<p  ><span style="float:left;text-indent:9em"> ￥：</span><span  id="shouju_money_print"style="float:left;text-decoration:underline"> </span>&nbsp;&nbsp; </p>' +
                            '</div>' +
                        '</td>' +
                    '</tr>' +
                    '<tr>' +
                        '<td>' +
                            '<div style="text-indent:2em">' +
                                '<p>注明：&nbsp;&nbsp; ①本凭证仅限于农村合作经济组织内部结算使用</p>' +
                                '<p> ②本凭证须加盖单位财务专用章有效</p>' +
                            '</div>' +
                        '</td>' +
                    '</tr>' +
                '</tbody>' +
           ' </table>' +
           ' <div style="text-indent:2em"> 收款单位（章）：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span id="shouju_user_print">收款人：<span id="shouju_userName_print"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></div> ' +
'</div>' +
   ' </div>' +
   ' <div style="position:fixed;top:100px; display:inline-block; width:14px">第一联存</div>' +
    '<div style="position:fixed;top:250px; display:inline-block; width:14px">根</div>' +
	'</div>';
var water_Str =
  ' <div style="font-family:simsun;margin-left:10%">' +
       ' <div style="display:inline-block">' +
         '   <div style="width:110mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">&nbsp;&nbsp;盛泽渔业镇村水费收据&nbsp;&nbsp;&nbsp;</h1>' +
               ' <span style="float:left">编号：</span><span id="water_Str_waterID"style="float:left"></span>' +
               ' <span style="float:left;margin-left:13%">日期：</span><span id="water_Str_date"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="water_Str_address"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_Str_name"></span></td>' +
                       ' <td colspan="2">&nbsp;<span id="water_Str_usernum"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起吨</td>' +
                     '   <td>本月止吨</td> ' +
                      '  <td>实用吨数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="water_Str_ton_start"></span></td>' +
                      '  <td>&nbsp;<span id="water_Str_ton_end"></span></td> ' +
                       ' <td>&nbsp;<span id="water_Str_ton"></span></td>' +
                       ' <td>&nbsp;<span id="water_Str_price"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="water_Str_money"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="water_Str_money_large"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="water_Str_reader"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="water_Str_receiver"></span></div>' +
            '</div>' +
              '   <div style="width:110mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">&nbsp;&nbsp;盛泽渔业镇村水费收据&nbsp;&nbsp;&nbsp;</h1>' +
               ' <span style="float:left">编号：</span><span id="water_Str_waterID2"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="water_Str_date2"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="water_Str_address2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_Str_name2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_Str_usernum2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起吨</td>' +
                     '   <td>本月止吨</td> ' +
                      '  <td>实用吨数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="water_Str_ton_start2"></span></td>' +
                      '  <td>&nbsp;<span id="water_Str_ton_end2"></span></td> ' +
                       ' <td>&nbsp;<span id="water_Str_ton2"></span></td>' +
                       ' <td>&nbsp;<span id="water_Str_price2"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="water_Str_money2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="water_Str_money_large2"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="water_Str_reader2"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="water_Str_receiver2"></span></div>' +
            '</div>' +
       ' </div>' +
    '</div>';
var water_StrPrint =
   ' <div id="water-container-print"  style="font-family:simsun;margin-left:3%">' +
       ' <div style="display:inline-block">' +
         '   <div style="width:91mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">盛泽渔业镇村水费收据</h1>' +
               ' <span style="float:left">编号：</span><span id="water_StrPrint_waterID"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="water_StrPrint_date"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_address"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_name"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_usernum"></span></td>' +

                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起吨</td>' +
                     '   <td>本月止吨</td> ' +
                      '  <td>实用吨数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="water_StrPrint_ton_start"></span></td>' +
                      '  <td>&nbsp;<span id="water_StrPrint_ton_end"></span></td> ' +
                       ' <td>&nbsp;<span id="water_StrPrint_ton"></span></td>' +
                       ' <td>&nbsp;<span id="water_StrPrint_price"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="water_StrPrint_money"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="water_StrPrint_money_large"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="water_StrPrint_reader"></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="water_StrPrint_receiver"></span></div>' +
            '</div>' +
              '<div style="width:91mm;height:86mm;float:left;margin-left:3mm">' +
             '   <h1 style=" text-indent:2em">盛泽渔业镇村电费收据</h1>' +
               ' <span style="float:left">编号：</span><span id="water_StrPrint_waterID2"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="water_StrPrint_date2"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_address2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_name2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="water_StrPrint_usernum2"></span></td>' +

                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起吨</td>' +
                     '   <td>本月止吨</td> ' +
                      '  <td>实用吨数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="water_StrPrint_ton_start2"></span></td>' +
                      '  <td>&nbsp;<span id="water_StrPrint_ton_end2"></span></td> ' +
                       ' <td>&nbsp;<span id="water_StrPrint_ton2"></span></td>' +
                       ' <td>&nbsp;<span id="water_StrPrint_price2"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="water_StrPrint_money2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="water_StrPrint_money_large2"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="water_StrPrint_reader2"></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="water_StrPrint_receiver2"></span></div>' +
            '</div>' +
       ' </div>' +
    '</div>';
var ele_Str =
  ' <div style="font-family:simsun;margin-left:10%">' +
       ' <div style="display:inline-block">' +
         '   <div style="width:110mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">&nbsp;&nbsp;盛泽渔业镇村电费收据&nbsp;&nbsp;&nbsp;</h1>' +
               ' <span style="float:left">编号：</span><span id="ele_Str_eleID"style="float:left"></span>' +
               ' <span style="float:left;margin-left:13%">日期：</span><span id="ele_Str_date"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="ele_Str_address"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="ele_Str_name"></span></td>' +
                       ' <td colspan="2">&nbsp;<span id="ele_Str_usernum"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起度</td>' +
                     '   <td>本月止度</td> ' +
                      '  <td>实用度数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="ele_Str_deg_start"></span></td>' +
                      '  <td>&nbsp;<span id="ele_Str_deg_end"></span></td> ' +
                       ' <td>&nbsp;<span id="ele_Str_deg"></span></td>' +
                       ' <td>&nbsp;<span id="ele_Str_price"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="ele_Str_money"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="ele_Str_money_large"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="ele_Str_reader"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="ele_Str_receiver"></span></div>' +
            '</div>' +
              '   <div style="width:110mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">&nbsp;&nbsp;盛泽渔业镇村电费收据&nbsp;&nbsp;&nbsp;</h1>' +
               ' <span style="float:left">编号：</span><span id="ele_Str_eleID2"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="ele_Str_date2"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="ele_Str_address2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="ele_Str_name2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="ele_Str_usernum2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起度</td>' +
                     '   <td>本月止度</td> ' +
                      '  <td>实用度数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="ele_Str_deg_start2"></span></td>' +
                      '  <td>&nbsp;<span id="ele_Str_deg_end2"></span></td> ' +
                       ' <td>&nbsp;<span id="ele_Str_deg2"></span></td>' +
                       ' <td>&nbsp;<span id="ele_Str_price2"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="ele_Str_money2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="ele_Str_money_large2"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="ele_Str_reader2"></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="ele_Str_receiver2"></span></div>' +
            '</div>' +
       ' </div>' +
    '</div>';
var ele_StrPrint =
   ' <div id="ele-container-print"  style="font-family:simsun;margin-left:3%">' +
       ' <div style="display:inline-block">' +
         '   <div style="width:91mm;height:86mm;float:left">' +
             '   <h1 style=" text-indent:2em">盛泽渔业镇村电费收据</h1>' +
               ' <span style="float:left">编号：</span><span id="ele_StrPrint_eleID"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="ele_StrPrint_date"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '<td colspan="2">&nbsp;<span id="ele_StrPrint_address"></span></td>' +
                      '<td colspan="2">&nbsp;<span id="ele_StrPrint_name"></span></td>' +
                      '<td colspan="2">&nbsp;<span id="ele_StrPrint_usernum"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起度</td>' +
                     '   <td>本月止度</td> ' +
                      '  <td>实用度数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="ele_StrPrint_deg_start"></span></td>' +
                      '  <td>&nbsp;<span id="ele_StrPrint_deg_end"></span></td> ' +
                       ' <td>&nbsp;<span id="ele_StrPrint_deg"></span></td>' +
                       ' <td>&nbsp;<span id="ele_StrPrint_price"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="ele_StrPrint_money"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="ele_StrPrint_money_large"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="ele_StrPrint_reader"></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="ele_StrPrint_receiver"></span></div>' +
            '</div>' +
              '   <div style="width:91mm;height:86mm;float:left;margin-left:3mm">' +
             '   <h1 style=" text-indent:2em">盛泽渔业镇村电费收据</h1>' +
               ' <span style="float:left">编号：</span><span id="ele_StrPrint_eleID2"style="float:left"></span>' +
               ' <span style="float:left;margin-left:15%">日期：</span><span id="ele_StrPrint_date2"></span>' +
               ' <table border="3" width="100%" cellspacing="0" cellpadding="0" height="195">' +
                   ' <tr>' +
                      '  <td colspan="2">地址</td>' +
                      '  <td colspan="2">户名（厂名）</td>' +
                      '  <td colspan="2">用户号</td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td colspan="2">&nbsp;<span id="ele_StrPrint_address2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="ele_StrPrint_name2"></span></td>' +
                      '  <td colspan="2">&nbsp;<span id="ele_StrPrint_usernum2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                      '  <td>本月起度</td>' +
                     '   <td>本月止度</td> ' +
                      '  <td>实用度数</td>' +
                      '  <td>单价</td>' +
                      '  <td colspan="2">金额</td>' +
                  '  </tr>' +
                   ' <tr>' +
                      '  <td>&nbsp;<span id="ele_StrPrint_deg_start2"></span></td>' +
                      '  <td>&nbsp;<span id="ele_StrPrint_deg_end2"></span></td> ' +
                       ' <td>&nbsp;<span id="ele_StrPrint_deg2"></span></td>' +
                       ' <td>&nbsp;<span id="ele_StrPrint_price2"></span></td> ' +
                       ' <td colspan="2">&nbsp;<span id="ele_StrPrint_money2"></span></td>' +
                   ' </tr>' +
                   ' <tr>' +
                        '<td colspan="6">金额大写：<span id="ele_StrPrint_money_large2"></span></td>' +
                   ' </tr>' +
               '</table>' +
               ' <div><span>抄表员：</span><span id="ele_StrPrint_reader2"></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>收款员：</span><span id="ele_StrPrint_receiver2"></span></div>' +
            '</div>' +
       ' </div>' +
    '</div>';