/**
 * Created by ping on 2017/1/4.
 */
var Fselectid = 1;  //上方列表选中项
var Sselectid = 1;  //下方列表选中项
var preFselectid = 0;  //上方列表上一次的选中项
var preSselectid = 0;  //下方列表上一次的选中项
var showIndex = 0;
var showTop = 1;      //当前显示上方的页码
var showBottom = 1;     //当前显示下方的页码
var text;
var leftSize;
var enNums=0;
window.iData='';
var alertSelect=false;
var number = getStbNo();  //获取机顶盒号
number = number=="Unknown"?getParam('number'):number;
// var number = '02276164350000346';
var PartyBranch;   //支部名称
var showLeftIndex = 1;
var date=new Date();
var mon=date.getMonth()+1;
var year=date.getFullYear();
var serviceTime=null;
// console.log(date);
var month='0'+mon;
iPosition(mon);
var bottomSize;
var  colors=new Array('','#7ECEF4','#BDDA76','#F9C066','#978BC1','#F2A6C7','#F3A07C','#EE7785','#00DEC3','#F49F00','#9C0A8E','#EFC100','transparent');

function iPosition(mon) {
    var mon=mon-1;
    if(mon<6){
        $('#down_1').css({'left':mon*128+'px','top':'67px'});
    }else{
        $('#down_1').css({'left':(mon-6)*128+'px','top':'258px'});

    }
}

var AndroidV=window.navigator.appVersion;
var AndroidV1=AndroidV.substr(AndroidV.indexOf("Android")+8,3);
var AndroidV2=AndroidV.substr(AndroidV.indexOf("Android")+8,5);

if(number.substring(0,6)=="021176"){
	$('.text').css("font-size","15px");
}else if(number.substring(0,6)=="022476"){
	$('.text').css("font-size","15px");
	$('body').css("margin-top","46px");
}

// if(AndroidV1 < 4.4 || (AndroidV2=="4.2.2" && AndroidV.indexOf("Chrome") >-1)){
	// $.getScript("js/common2.js"); 
// }

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
//获取机顶盒
function getStbNo() {
    try {
        if (typeof hardware != 'undefined' && hardware.STB)
            return hardware.STB.serialNumber;
        else return "Unknown";
    }
    catch (err) {
        return "Unknown";

    }
}

var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
        : "")
    + window.location.host;
var svc_sys = svcHeader + "/JRPartyService/Television.svc";

if(year>2016){
	// $('#testOut').html(number);
	$.ajax({
		url: svc_sys + "/getInformation?",
		type: "GET",
		data: {
			number: number,
		},
		success: function (data) {
			if(!data.success && data!=null){
				iAlert("<div style='font-size:25px;margin-top:50px;'>您尚未绑定机顶盒号，无法访问<br/>请联系管理员<div>");
				setTimeout(window.close,4000);
			}
			PartyBranch = data.data;
			$('#title_span')[0].innerHTML=PartyBranch;
			getLeftData();
			getBottomData();
		}
	})
}else{
	iAlert("<div style='font-size:25px;margin-top:50px;'>您的机顶盒当前时间异常（"+new Date().format("yyyy-MM-dd hh:mm:ss")+"）<br/>请检查网络等原因<div>");
}

document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;
    //$('#testOut').text(keyCode);
    switch (keyCode) {
		case KEY.DOWN:
        case KEY.DOWN2:
		
			if(Sselectid<bottomSize && bottomSize!=1 || Sselectid==5){
				changeDownColor();
            }
            break;
        case KEY.UP:
		case KEY.UP2:
            changeUpColor();
            break;
        case KEY.LEFT:
		case KEY.LEFT2:
			showBottom=1;
            if(mon>1){
                mon--;
                iPosition(mon);
                getBottomData();
            }
            break;
        case KEY.RIGHT:
		case KEY.RIGHT2:
			showBottom=1;
            if(mon<12){
                mon++;
                iPosition(mon);
                getBottomData();
            }
            break;
        case KEY.ENTER:
            /*var title= $('#Shape1_t'+Sselectid)[0].innerHTML;
            var content=$('#Shape1_c'+Sselectid)[0].innerHTML;*/
			if(alertSelect){
				iExit();
				alertSelect=false;
			}else{
				if(mon==date.getMonth()+1){
					if(iData.length>0){
						setCookie();
					}
				}
			}
            

            /*if(Fselectid==1&&$("#num1")[0].innerText>=mon){
                if(!(title=="暂无计划" && content=="暂无任务")){
                    setCookie(Sselectid);
                }
            }else if(Fselectid==2&&$("#num2")[0].innerText>=mon){
                if(!(title=="暂无计划" && content=="暂无任务")){
                    setCookie(Sselectid);
                }
            }else if(Fselectid==3&&$("#num3")[0].innerText>=mon){
                if(!(title=="暂无计划" && content=="暂无任务")){
                    setCookie(Sselectid);
                }
            }*/
            break;
    }
    return false;
}
function getBottomData() {  //获取下方数据
    Sselectid=1;
    for(var i=0;i<5;i++){
        $('#in'+(i+1)).val("");
        $('#Shape1_c'+(i+1))[0].innerHTML="";
        $('#Shape1li'+(i+1)).css('display','none');
        $('#Shape1li'+(i+1)).css('background-image','url(img/xialabeij.png)');
        $('#Shape1_c'+(i+1)).css('color',"#000000");
    }
	
    if(parseInt(mon)<10){
        month="0"+mon;
    }else{
        month=mon;
    }
	
    $.ajax({
        url: svc_sys + "/getTopBoxActivityListByMonth?",
        type: "GET",
        data: {
            districtName: PartyBranch,
            month: month,
            PageIndex: showBottom,
        },
        success: function (data) {
            //console.log(data);
			enNums=data.message;
            iData=data.rows;
            bottomSize=iData.length;
            if(data.rows.length>0){
                bottomSize=iData.length;
                for(var i=0;i<iData.length;i++){
                    var content=iData[i].type+'['+iData[i].title+']';
                    if(content.length>13){
                        content=content.substr(0,13)+'..';
                    }
                    $('#Shape1_c'+(i+1))[0].innerHTML=content;
                    $('#in'+(i+1)).val(iData[i].id);
                    $('#Shape1li'+(i+1)).css('display','block');
                }
                $('#Shape1_c'+1).css('color',"#ffffff");

                $('#Shape1li'+1).css('background-image','url(img/xuanzhong.png)');

            }else{
                $('#Shape1li'+1).css('background-image','url(img/xuanzhong.png)');
                $('#Shape1_c'+(1))[0].innerHTML="暂无任务";
                $('#Shape1_c'+1).css('color',"#ffffff");
                $('#Shape1li'+1).css('display','block');
            }
        }
    })
}
function getData(){
    if(showLeftIndex<leftSize){
        showLeftIndex++;
        getLeftData();
    }
}
function changeLeftColor() {
    if( $('#num1')[0].innerHTML>1){
        $('#num1')[0].innerHTML=parseInt($('#num1')[0].innerHTML)-3;
        $('#num2')[0].innerHTML=parseInt($('#num2')[0].innerHTML)-3;
        $('#num3')[0].innerHTML=parseInt($('#num3')[0].innerHTML)-3;
        var color1=$("#num1")[0].innerHTML;
        var color2=$("#num2")[0].innerHTML;
        var color3=$("#num3")[0].innerHTML;
        $('#Shape1').css('background-image',"url('img/Shape"+color1+".png')");
        $('#Shape2').css('background-image',"url('img/Shape"+color2+".png')");
        $('#Shape3').css('background-image',"url('img/Shape"+color3+".png')");
        $('#hr1').attr('color',colors[$('#num1')[0].innerHTML]);
        $('#hr2').attr('color',colors[$('#num2')[0].innerHTML]);
        $('#hr3').attr('color',colors[$('#num3')[0].innerHTML]);
        if(  $('#num3')[0].innerHTML==9){
            $('#hr3').show();
        }
        $("#down_1").css('left',"586px");
    }
    Fselectid=3;
}     //左移改变月份选中
function changeRightColor() {

    if($('#num3')[0].innerHTML<=9){

        $('#num1')[0].innerHTML=parseInt($('#num1')[0].innerHTML)+3;
        $('#num2')[0].innerHTML=parseInt($('#num2')[0].innerHTML)+3;
        $('#num3')[0].innerHTML=parseInt($('#num3')[0].innerHTML)+3;
        var color1=$("#num1")[0].innerHTML;
        var color2=$("#num2")[0].innerHTML;
        var color3=$("#num3")[0].innerHTML;
        $('#Shape1').css('background-image',"url('img/Shape"+color1+".png')");
        $('#Shape2').css('background-image',"url('img/Shape"+color2+".png')");
        $('#Shape3').css('background-image',"url('img/Shape"+color3+".png')");
        $('#hr1').attr('color',colors[$('#num1')[0].innerHTML]);
        $('#hr2').attr('color',colors[$('#num2')[0].innerHTML]);
        $('#hr3').attr('color',colors[$('#num3')[0].innerHTML]);
        if($('#num3')[0].innerHTML==12){
            $('#hr3').hide();
        }
        $("#down_1").css('left',"0px");
    }
    Fselectid=1;

}     //右移改变月份选中
function changeUpColor() {
    if(Sselectid>1){
        preSselectid=Sselectid;
        Sselectid--;
        $('#Shape1li'+Sselectid).css('background-image','url(img/xuanzhong.png)');
        $('#Shape1li'+preSselectid).css('background-image','url(img/xialabeij.png)');
        $('#Shape1_c'+Sselectid).css('color',"#ffffff");
        $('#Shape1_c'+preSselectid).css('color',"#000000");
    }else if(Sselectid==1){
        if(showBottom>=2){
            showBottom--;
            getBottomData();
        }
    }
}      //上移改变任务选中
function changeDownColor() {

    if(Sselectid<=4){

        preSselectid=Sselectid;
        Sselectid++;
        $('#Shape1li'+Sselectid).css('background-image','url(img/xuanzhong.png)');
        $('#Shape1li'+preSselectid).css('background-image','url(img/xialabeij.png)');
        $('#Shape1_c'+Sselectid).css('color',"#ffffff");
        $('#Shape1_c'+preSselectid).css('color',"#000000");
    }else if(Sselectid==5){
        showBottom++;
        getBottomData();
    }
}    //下移改变任务选中
function setCookie() {
	if(enNums<3){
		window.location.href = 'demo.html?number=' + number + '&PartyBranch=' + PartyBranch +"&StudyId="+iData[Sselectid-1].id+ '&StudyTitle='+iData[Sselectid-1].title+'&StudyContent=' + iData[Sselectid-1].context + '&StudyType='+iData[Sselectid-1].type;
	}else{
		iAlert("<div style='font-size:25px;margin-top:50px;'>今日可执行数已经用完<div>");
		alertSelect=true;
	}
}   //跳转到下一页
function getLeftData() {
    $.ajax({
        url: svc_sys + "/getTopBoxActivityListByDistrict?",
        type: "GET",
        data: {
            districtName: PartyBranch,
            PageIndex: showLeftIndex,
        },
        success: function (data) {
            //console.log(data);

            leftSize=data.total;
            if (data.complete.length > 0 && data.complete.length<=3) {
                for(var i=0;i<data.complete.length;i++){
                    $('#f'+(i+1))[0].innerHTML=data.complete[i];
                }
            }else{
                var strHtml;
                strHtml="<marquee direction='up' scrollamount='2' height='120px'>";
                for(var i=0;i<data.complete.length;i++){
                    strHtml+="<div class='leftdiv'>"+data.complete[i]+"</div>";
                }
                strHtml+="</marquee>";
                $('#complete')[0].innerHTML=strHtml;
            }
            if (data.Incomplete.length > 0 && data.Incomplete.length<=3) {
                for(var i=0;i<data.Incomplete.length;i++){
                    $('#z'+(i+1))[0].innerHTML=data.Incomplete[i];
                }
            }else{
                var strHtml;
                strHtml="<marquee direction='up' scrollamount='2' height='120px'>";
                for(var i=0;i<data.Incomplete.length;i++){
                    strHtml+="<div class='leftdiv'>"+data.Incomplete[i]+"</div>";
                }

                strHtml+="</marquee>";
                $('#incomplete')[0].innerHTML=strHtml;

            }
            if (data.expired.length > 0 && data.expired.length<=3) {
                for(var i=0;i<data.expired.length;i++){
                    $('#y'+(i+1))[0].innerHTML=data.expired[i];
                }
            }else{
                var strHtml;
                strHtml="<marquee direction='up' scrollamount='2' height='120px' scrolldelay='100'>";
                for(var i=0;i<data.expired.length;i++){
                    strHtml+="<div class='leftdiv'>"+data.expired[i]+"</div>";
                }
                strHtml+="</marquee>";
                $('#yiguoqi')[0].innerHTML=strHtml;
            }
        }
    })
}
