/**
 * 作者：Eric
 * 更新时间：2017.7.25
 */

/*$.ajax({
    url:'',
    type:'post',
    data:{'userId':userid,'password':password},
    dataType:"json",
    // contentType: "application/json",
    cache:false,
    // async:false,
    success:function(response){
        // var item=eval("("+`+")");
        // console.log(item);
    },
    error : function(XMLHttpRequest, textStatus, errorThrown) {
        // view("异常！");
        console.log("接口请求失败！");
    }
});*/

/*// jquery-form.js表单提交
function formSubmit(){
    $("#iForm").ajaxSubmit(function(message) { 
        
    });
    return false; // 必须返回false，否则表单会自己再做一次提交操作，并且页面跳转
}*/

// 时间格式
Date.prototype.format = function(fmt) { 
    var o = { 
        "M+" : this.getMonth()+1,                 //月份 
        "d+" : this.getDate(),                    //日 
        "h+" : this.getHours(),                   //小时 
        "m+" : this.getMinutes(),                 //分 
        "s+" : this.getSeconds(),                 //秒 
        "q+" : Math.floor((this.getMonth()+3)/3), //季度 
        "S"  : this.getMilliseconds()             //毫秒 
    }; 
    if(/(y+)/.test(fmt)) {
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length)); 
    }
    for(var k in o) {
        if(new RegExp("("+ k +")").test(fmt)){
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
        }
    }
    return fmt; 
}

// 获取当前网址、主机地址项目根路径
function serverAddress(){
    var serverAddress=new Object();
    //获取当前网址，如： http://localhost:8080/Tmall/index.jsp 
    serverAddress.curWwwPath=window.document.location.href; 

    //获取主机地址之后的目录如：/Tmall/index.jsp 
    serverAddress.pathName=window.document.location.pathname; 
    serverAddress.pos=serverAddress.curWwwPath.indexOf(serverAddress.pathName); 

    //获取主机地址，如： http://localhost:8080 
    serverAddress.localhostPaht=serverAddress.curWwwPath.substring(0,serverAddress.pos); 

    //获取带"/"的项目名，如：/Tmall 
    serverAddress.projectName=serverAddress.pathName.substring(0,serverAddress.pathName.substr(1).indexOf('/')+1);  

    return serverAddress;
}

// 识别浏览器内核
function identityBrowser(){
    var userAgent = navigator.userAgent,   
    rMsie = /(msie\s|trident.*rv:)([\w.]+)/,   
    rFirefox = /(firefox)\/([\w.]+)/,   
    rOpera = /(opera).+version\/([\w.]+)/,   
    rChrome = /(chrome)\/([\w.]+)/,   
    rSafari = /version\/([\w.]+).*(safari)/;  
    var browser;  
    var version;  
    var ua = userAgent.toLowerCase();  
    function uaMatch(ua) {  
        var match = rMsie.exec(ua);  
        if (match != null) {  
            return { browser : "IE", version : match[2] || "0" };  
        }  
        var match = rFirefox.exec(ua);  
        if (match != null) {  
            return { browser : match[1] || "", version : match[2] || "0" };  
        }  
        var match = rOpera.exec(ua);  
        if (match != null) {  
            return { browser : match[1] || "", version : match[2] || "0" };  
        }  
        var match = rChrome.exec(ua);  
        if (match != null) {  
            return { browser : match[1] || "", version : match[2] || "0" };  
        }  
        var match = rSafari.exec(ua);  
        if (match != null) {  
            return { browser : match[2] || "", version : match[1] || "0" };  
        }  
        if (match != null) {  
            return { browser : "", version : "0" };  
        }  
    }  
    var browserMatch = uaMatch(userAgent.toLowerCase());  
    if (browserMatch.browser) {  
        browser = browserMatch.browser;  
        version = browserMatch.version;  
    }  
    return browser+version; 
}

// 读取Cookie
function getCookie(c_name){
    if (document.cookie.length>0){
        c_start=document.cookie.indexOf(c_name + "=");
        if (c_start!=-1){ 
            c_start=c_start + c_name.length+1 ;
            c_end=document.cookie.indexOf(";",c_start);
            if (c_end==-1){
                c_end=document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start,c_end));
            // return decodeURI(document.cookie.substring(c_start,c_end));
        } 
    }
    return "";
}

// 设置Cookie
function setCookie(c_name,value,time){
    if(time=='session'){

        var isIE = identityBrowser().indexOf('IE')//判断是否是ie核心浏览器  
        
        if(isIE==-1){  
            exdate='session';
        }else{
            exdate='At the end of the Session';
        }
    }else{
        var exdate=new Date();
        exdate.setTime(exdate.getTime()+time*24*3600*1000);
        exdate.toGMTString();
    }    
    document.cookie=c_name+ "=" +escape(value)+((time==null) ? "" : ";expires="+exdate);
    // document.cookie=c_name+ "=" +encodeURI(value)+((time==null) ? "" : ";expires="+exdate);
}

// 登陆超时检验
function security(){
    $.ajax({
        url:'../user/login.php',
        type:'post',
        data:{name:getCookie('userName'),password:getCookie('userPassword')},
        cache:false,
        // async:false,
        success:function(response){
            // console.log(user);
            var item=eval("("+response+")");
            if(item.IsOk!==1){
                var layer=document.createElement("div");
                layer.id="layer";
                var style=
                {
                    backgroundColor:'transparent',
                    opacity:1,
                    position:"absolute",
                    zIndex:10,
                    width:"800px",
                    height:"400px",
                    left:"250px",
                    top:"100px",
                    lineHeight:"400px",
                    overflow:'hidden',
                    textAlign:'center',
                    fontSize:'50px',
                    color:'black'
                };
                layer.innerHTML="登陆超时...";
                for(var i in style)
                    layer.style[i]=style[i];   
                if(document.getElementById("layer")==null)
                {
                    document.body.appendChild(layer);
                    setTimeout("document.body.removeChild(layer)",1000);
                    setTimeout("window.location.href='login.php'",1000);
                }
            }
        }
    });
}

// 关闭弹出层
function exit(layerObj,layerHidden){
    if(layerHidden==undefined)  layerHidden='layerHidden';
    var layer=document.getElementById(layerObj);
    var layerHidden=document.getElementById(layerHidden);

    document.body.removeChild(layerHidden);
    document.body.removeChild(layer);
}

// 分页条
/**
 * count 总页数
 * page 当前页号
 * num 显示的页码数
 * pageSize 页面信息条数
 * userId 用户Id
 * keyword 关键字
 * funName 页面输出函数名
 **/
function pageBar(count,page,num,pageSize,userId,keyword,funName){
    // 数据是否存在
    if(count==0){
        return '';
    }
    num=Math.min(count,num);    // 处理显示的页码数大于总页数的情况
    if(page>count || page<1) return;    // 处理非法页号的情况
    var end=page+Math.floor(num/2)<=count?page+Math.floor(num/2):count; // 计算结束页号
    var start=end-num+1;    // 计算开始页号
    if(start<1){    // 处理开始页号小于1的情况
        end -=start-1;
        start=1;
    }
    var strInnerHTML='';
    if(page!=1){
        strInnerHTML+="<button type='button' onclick='"+funName+"("+pageSize*(page-2)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);' class='backBtn'></button>";
    }else{
        strInnerHTML+="<button type='button' class='backBtn'></button>";
    }
    if(page>=(num+3)/2 && count>num){
        strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"(0,"+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+1+"</button>";
    }
    if(page>=(num+5)/2 && count>num+1){
        if(page<=num){
            strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"(0,"+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+"..."+"</button>";
        }else{
            strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"("+pageSize*(page-num-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+"..."+"</button>";
        }
    }
    for(var i=start;i<=end;i++){
        if(i==page){
            strInnerHTML+="<button type='button' class='pageIndex' style='background-color:#83CAFC;' onclick='"+funName+"("+pageSize*(i-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+i+"</button>";
        }else{
            strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"("+pageSize*(i-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+i+"</button>";
        }
    }
    if(page<=count-(num+3)/2 && count>num+1){
        if(page>count-num){
            strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"("+pageSize*(count-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+"..."+"</button>";
        }else{
            strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"("+pageSize*(page+num-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+"..."+"</button>";
        }
    }
    if(page<=count-(num+1)/2 && count>num){
        strInnerHTML+="<button type='button' class='pageIndex' onclick='"+funName+"("+pageSize*(count-1)+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);'>"+count+"</button>";
    }
    if(page!=count){
        strInnerHTML+="<button type='button' onclick='"+funName+"("+pageSize*page+","+pageSize+",&#39;"+userId+"&#39;,&#39;"+keyword+"&#39;);' class='nextBtn'></button>";
    }else{
        strInnerHTML+="<button type='button' class='nextBtn'></button>"; 
    }
    
    return strInnerHTML;
}

// 计算发布时间与当前时间的差
function getDateDiff(dateStr){
    var result = '';
    var minute = 1000 * 60;
    var hour = minute * 60;
    var day = hour * 24;
    var halfamonth = day * 15;
    var month = day * 30;
    var year = month * 12;
    var now = new Date().getTime();
    var dateTimeStamp=Date.parse(dateStr.replace(/-/gi,"/").replace(/\./gi,"/").replace('年',"/").replace('月',"/").replace('日',"/")); //转化标准时间为时间截，这里包含输入时间格式调整
    var diffValue = now - dateTimeStamp;
    if(diffValue < 0){return "刚刚";}
    var yearC =diffValue/year;
    var monthC =diffValue/month;
    var weekC =diffValue/(7*day);
    var dayC =diffValue/day;
    var hourC =diffValue/hour;
    var minC =diffValue/minute;

    if(yearC>=1){
        result="" + parseInt(yearC) + "年前";
    }
    else if(monthC>=1){
    result="" + parseInt(monthC) + "月前";
    }
    else if(weekC>=1){
    result="" + parseInt(weekC) + "周前";
    }
    else if(dayC>=1){
    result=""+ parseInt(dayC) +"天前";
    }
    else if(hourC>=1){
    result=""+ parseInt(hourC) +"小时前";
    }
    else if(minC>=1){
        result=""+ parseInt(minC) +"分钟前";
    }else{
        result="刚刚";
    }
    return result;
}

// hint---提示内容  go---跳转地址
    function iAlert(hint,go){
      var layer=document.createElement("div");
      var layerHidden=document.createElement('div');
      layer.id="layer";
      layerHidden.id='iLayerHidden';
      
      var style=
        {
            backgroundColor:'white',
            opacity:0.9,
            position:"fixed",
            zIndex:9,
            width:"70%",
            minHeight:"100px",
            left:"15%",
            top:"20%",
            borderRadius:"12px",
            // lineHeight:"400px",
            // overflow:'hidden',
            // color:'white',
            textAlign:'center',
            // fontSize:'50px',
            // border:'1px solid black'
        };
        var styleHidden=
        {
          backgroundColor:"gray",
          opacity:0.5,
          zIndex:8,
          width:"100%",
          height:"100%",
          position:"fixed",
          left:"0px",
          top:"0px",
        }
        
        layer.innerHTML="<br/><div style='padding:0px 10% 12% 10%;'>"+hint+"</div><div style='position:absolute;bottom:10px;right:10px;'></div>";
        for(var i in style)
            layer.style[i]=style[i];
        for(var i in styleHidden)
          layerHidden.style[i]=styleHidden[i];

        $(document.body).css({
            "overflow-x":"hidden",
            "overflow-y":"hidden"
        });
        if(document.getElementById("layer")==null)
        {
            document.body.appendChild(layerHidden);
            document.body.appendChild(layer);
        }
    }

    function iExit(go){
      var layer=document.getElementById('layer');
      var layerHidden=document.getElementById('iLayerHidden');

      $(document.body).css({
            "overflow-x":"auto",
            "overflow-y":"auto"
        });

      document.body.removeChild(layerHidden);
      document.body.removeChild(layer);
      
      if(typeof(go)!='undefined'){
        window.location.href=go; // 提交后的跳转地址，可根据需要更改
      }
    }

    function adaptation() {
        // iPhone5适配
        if (window.screen.width == '320') {
            $("<link>")
                .attr({ rel: "stylesheet",
                    type: "text/css",
                    href: "./asset/css/enrollIphone5.css"
                })
                .appendTo("head");
        }
    }

// 文本控件只能输入数字
function numLimit(obj){
    var keynum;
    var keychar;
    var numcheck;

    // alert(obj); // 这里输入的obj类型为object 值为KeyboardEvent
    // console.log(event.keyCode); // 这里输入的obj类型为object 值为KeyboardEvent
    // event对象详细内容见下文
    // event.keyCode 取回被按下的字符的Unicode码，而 Netscape/Firefox/Opera 使用 event.which。
    keynum=obj.keyCode || obj.which;
    keychar=String.fromCharCode(keynum); //接受一个Unicode值，返回其相应字串
    numcheck=/\d/; // /XXXX/为正则表达式，/d表示数字0~9。
    // test()检测目标字符串是否匹配某个模式，返回布尔值

    // 运行通过的unicode集合
    var limits=new Array('',8,'8'); // index=0时无法查询到
    if(numcheck.test(keychar) || $.inArray(keynum, limits)>0){
        return true;
    }else{
        return false;
    }
};

//get请求数据解析     paramName参数名
function getParam(paramName) {
    var paramValue = "";
    var isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        var arrSource = decodeURI(this.location.search).substring(1, this.location.search.length).split("&");

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