$(function () {
    //window.onload = function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    var districtID = getParam('districtID');
    var id = getParam('id');
    console.log(districtID, id);
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
    $.ajax({
        url: svc_sys + "/getPictureByActivity",
        type: "GET",
        data: {
            districtID: districtID,
            id: id
        },
        success: function (data) {
           var str = '';
            for (var i = 0 in data.rows) {
                if (i == 0) {
                    var strimg = ''
                    for (var j in data.rows[i].URL) {
                        console.log(data.rows[i].URL[j]);
                        strimg = strimg + '<img src="' + 'http://172.16.0.221:8006/JRPartyService/Upload/PhotoTake/' + data.rows[i].URL[j] + '"  width="710px" />'
                    }
                    strimg2 = '<div class="main tab mt15">' +
                                       '<div class="slide">' +
                                       '<ul id="bigSlideUl" class="slide-ul clearfix">' +
                                       ' <li> ' +
                                       '<img width="230" height="350" src="lunbo/gallary-141027163823/images/1353485118249.jpg" />' +
                                       '<span class="pic-txt"> </span>' +
                                                   '</li>' +
                                               '<li>' +
                                       ' <img width="230" height="350" src="lunbo/gallary-141027163823/images/1353485118249.jpg" />' +
                                       ' <span class="pic-txt"> </span>' +
                                                  ' </li>' +
                                               '<li>' +
                                       '<img width="230" height="350" src="lunbo/gallary-141027163823/images/1353485118249.jpg" />' +
                                       '<span class="pic-txt"> </span>' +
                                       '</li>' +
                                       '<li>' +
                                       '<img width="230" height="350" srch="lunbo/gallary-141027163823/images/1353485118249.jpg" />' +
                                       '<span class="pic-txt"> </span>' +
                                       '</li>' +
                                       '<li>' +
                                       '<img width="230" height="350" srch="lunbo/gallary-141027163823/images/1353485118249.jpg" />' +
                                       ' <span class="pic-txt"> </span>' +
                                       ' </li>' +
                                       ' </ul>' +
                                       ' </div>' +
                                       '<ul id="smallSlideUl" class="info-btn clearfix">' +
                                       '<li class="info-cur" id="mypic0" sid="0"><span>1</span></li>' +
                                       '<li id="mypic1" sid="1"><span>2</span></li>' +
                                           '<li id="mypic2" sid="2"><span>3</span></li>' +
                                          ' <li id="mypic3" sid="3"><span>4</span></li>' +
                                       '<li id="mypic4" sid="4"><span>5</span></li>' +
                                       ' </ul>' +
                                      ' <em class="tab-shadow"></em>' +
                                   '</div>'
                    console.log(strimg);
                    str = str + '<li  >' +
            '<div class="check">' +
                '<b class="spring"></b>' +
                '<b class="line"></b>' +
            '</div>' +
            '<div class=" ">' +
                '<img src="images/pic/top2.jpg" alt=""><br>' +
                '<span>' + data.rows[i].time + '</span>' +
            '</div>' +
            '<div class="content">' +
                '<b></b>' + strimg +
            '</div>' +
        '</li>'
                } else {
                    var strimg = ''
                    for (var j in data.rows[i].URL) {
                        console.log(data.rows[i].URL[j]);
                        strimg = strimg + '<img src="' + 'http://172.16.0.221:8006/JRPartyService/Upload/PhotoTake/' + data.rows[i].URL + '"  width="710px" />'
                    }
                    console.log(strimg);
                    str = str + '<li>' +
           '<div class="check">' +
              '<b class="spring"></b>' +
              '<b class="line"></b>' +
          '</div>' +
          '<div class=" ">' +
              '<img src="images/pic/top1.jpg" alt=""><br>' +
              '<span>' + data.rows[i].time + '</span>' +
          '</div>' +

          '<div class="content">' +
              '<b></b>' + strimg +
          '</div>' +

      '</li>'
                }
                $("#timeline").html(str);
            }

        }
    })

    //$("#timeline").html('<li class="active">'+

    //			'<div class="check">'+

    //				'<b class="spring"></b>'+

    //				'<s></s>'+

    //				'<b class="line"></b>'+

    //			'</div>'+

    //			'<div class="thumb">'+

    //				'<img src="images/pic/top1.jpg" alt="">'+

    //				'<span>10:00</span>'+

    //			'</div>'+

    //			'<div class="content">'+ 
    //				'<b></b>'+ 
    //				'<img src=" http://localhost:8097/JRPartyService/picture/094902_thumb.jpg"  height="500px" />' + 
    //			'</div>'+

    //		'</li>'+ 
    //		'<li>'+

    //			'<div class="check">'+

    //				'<b class="spring"></b>'+

    //				'<s></s>'+

    //				'<b class="line"></b>'+

    //			'</div>'+

    //			'<div class="thumb">'+

    //				'<img src="images/pic/top1.jpg" />'+

    //				'<span>7:00</span>'+

    //			'</div>'+


    //			'<div class="content">' +
    //				'<b></b>' +
    //				'<img src=" http://localhost:8097/JRPartyService/picture/094902_thumb.jpg"  height="500px" />' +
    //			'</div>' +

    //		'</li>'+

    //		'<li>'+

    //			'<div class="check">'+

    //				'<b class="spring"></b>'+

    //				'<s></s>'+

    //				'<b class="line"></b>'+

    //			'</div>'+

    //			'<div class="thumb">'+

    //				'<img src="images/pic/top1.jpg" />'+

    //				'<span>21:30</span>'+

    //			'</div>'+


    //			'<div class="content">' +
    //				'<b></b>' +
    //				'<img src=" http://localhost:8097/JRPartyService/picture/094902_thumb.jpg"  height="500px" />' +
    //			'</div>' +

    //		'</li>'+

    //		'<li>'+

    //			'<div class="check">'+

    //			'	<b class="spring"></b>'+

    //				'<s></s>'+

    //				'<b class="line"></b>'+

    //			'</div>'+

    //			'<div class="thumb">'+

    //				'<img src="images/pic/top1.jpg" />'+

    //				'<span>20:00</span>'+

    //			'</div>'+


    //			'<div class="content">' +
    //				'<b></b>' +
    //				'<img src=" http://localhost:8097/JRPartyService/picture/094902_thumb.jpg"  height="500px" />' +
    //			'</div>' +

    //		'</li>')
    var
 		//记录当前已经添加active类的li的索引号
 		curIndex = 0,

 		//查找所有被点击的元素对象
 		timeLine = document.getElementById("timeline"),
 		clickArea = timeLine.getElementsByTagName("s"),

 		//查找所有li元素对象
 		timePoint = timeLine.getElementsByTagName("li");

    //为每个被点击的对象绑定单击事件
    for (var i = 0, len = clickArea.length; i < len; i++) {
        (function (i) {
            clickArea[i].onclick = function () {
                //为被点击的时间点li添加active类
                timePoint[i].className = "active";

                //根据索引号变量的值，去除上一个li的active类
                timePoint[curIndex].className = "";

                //将索引号变量值更新为被点击的li的索引号
                curIndex = i;
            };
        })(i);
    }
});