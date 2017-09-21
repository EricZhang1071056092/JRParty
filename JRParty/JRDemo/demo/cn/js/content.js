/**
 * Created by Administrator on 2017/6/2.
 */
/**
 * Created by ping on 2017/1/4.
 */
/*---------------接口地址----------------*/
//脚本里用到的所有的转发连接都放在这里
var cookies = document.cookie;
var arr = cookies.split('=');
var arrStr = arr[1].split('.');
document.getElementById("title-span").innerText = arrStr[0];
document.getElementById("content-span").innerText = arrStr[1];

var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
        : "")
        + window.location.host;
var svc_sys = svcHeader + "/JRPartyService/Party.svc";
//申明一个全局变量存放间隔函数 
var setIntervalFun = null;
//每隔10秒钟执行一次setXSJYLTime()这个函数 
//setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);
//停止执行间隔函数 
//if (setIntervalFun == null) {
//    clearInterval(setIntervalFun);
//}
var number = getParam('number');
var PartyBranch= getParam('PartyBranch');
document.getElementById("number-span").innerHTML=number;
document.getElementById("partyBranch-span").innerHTML=PartyBranch;
//console.log(number,PartyBranch);
//解析值 
function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = decodeURI(this.location.search).substring(1, this.location.search.length).split("&");

        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}
function setXSJYLTime() {
 
//console.log( arrStr[0]+','+arrStr[1]);
    //console.log(123);
    $.ajax({
        url: svc_sys + "/getPicture?",
        type: "GET",
        data: {
            number: number ,
            StudyContent: arrStr[0]+','+arrStr[1],
        },
        success: function (data) {
            //console.log(data);
        }
    })
}



var FOCUS = "BUT_0";
var LFOCUS = "";
document.onirkeypress = keyDown;
document.onkeydown = keyDown;

function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;
    switch (keyCode) {
        case KEY.DOWN:
            switch (FOCUS) {
                case "BUT_0": 
                 /*   setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);*/
                    $('#start').hide();
                    $('#startFouce').show();
                    LFOCUS = FOCUS;
                    FOCUS = "BUT_00";
                    break
                case "BUT_00": 
                    $('#shouye').hide();
                    $('#shouyeFouce').show();
                    $('#startFouce').hide();
                    $('#start').show();
                    FOCUS = "BUT_2"
                    break;
                case "BUT_1": 
                    $('#shouye').hide();
                    $('#shouyeFouce').show();
                    $("#endFouce").hide();
                    $("#end").show();
                    FOCUS = "BUT_2"
                    break;
            }
            break;
        case KEY.UP:
            switch (FOCUS) {
                case "BUT_2": 
                    clearInterval(setIntervalFun); 
                    $("#endFouce").show();
                    $("#end").hide();
                    $('#shouyeFouce').hide();
                    $('#shouye').show();
                    FOCUS = "BUT_1";
                    break;
                case "BUT_3": 
                    $("#fanhui").show();
                    $("#fanhuiFouce").hide();
                    $('#end').hide();
                    $('#endFouce').show();
                    FOCUS = "BUT_1";
                    break;
            }
            break;
        case KEY.LEFT:
            switch (FOCUS) {
                case "BUT_0": 
                /*    setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);*/
                    $("#start").show();
                    $("#startFouce").hide();
                    FOCUS = "BUT_00";
                    break;
                case "BUT_1": 
             /*       setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);*/
                    $("#startFouce").show();
                    $("#start").hide();
                    $("#endFouce").hide();
                    $("#end").show();
                    FOCUS = "BUT_00"
                    break;
                case "BUT_3": 
                    $("#fanhui").show();
                    $("#fanhuiFouce").hide();
                    $('#shouye').hide();
                    $('#shouyeFouce').show();
                    FOCUS = "BUT_2";
                    break;
            }
            break;
        case KEY.RIGHT:
            switch (FOCUS) {
                case "BUT_00": 
                    clearInterval(setIntervalFun);
                    $("#endFouce").show();
                    $("#end").hide();
                    $("#start").show();
                    $("#startFouce").hide();
                    FOCUS = "BUT_1";
                    break;
                case "BUT_2":
                    $("#fanhuiFouce").show();
                    $("#fanhui").hide();
                    $('#shouyeFouce').hide();
                    $('#shouye').show();
                    FOCUS = "BUT_3";
                    break;
            }
            break;
        case KEY.ENTER:
            switch (FOCUS) {
                case "BUT_0": 
                /*    setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);*/
                    $('#startFouce').show();
                    $('#start').hide();
                    FOCUS = "BUT_00";
                    break;
                case "BUT_00":
                    alert('开始截屏');
                    setIntervalFun = setInterval("setXSJYLTime()", 1000 * 10);
                      break;
                case "BUT_1":
                    alert('结束截屏');
                    break;
                case "BUT_2":
                    window.location.href = "http://172.16.0.211:9005/smartCity/jurong/app/party-building/html";
                    break;
                case "BUT_3":
                    //alert(1);
                    window.location.href = "index.html";
                    break;
            }
            break
    }
}