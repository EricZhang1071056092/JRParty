
window.onload = function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    //var svc_sys = "http://172.16.0.221:8006/JRPartyService/Party.svc";
    var id = getParam('id');
    var districtID = getParam('districtID');
    console.log(id, districtID);
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
        url: svc_sys + "/getActivityPicture",
        type: "GET",
        data: {
            districtID: districtID,
            id: id,
        },
        dataType: "JSON",
        processData: true,
        success: function (data) {
            var str = '';
            for (var i = 0 in data.rows) {
                if (i == 0) {
                    console.log(data.rows[i].ImageURL);
                    str = str + '<li  >' +
            '<div class="check">' +
                '<b class="spring"></b>' +
                '<b class="line"></b>' +
            '</div>' +
            '<div class="thumb">' +
                '<img src="images/pic/top1.jpg" alt=""><br>' +
                '<span>' + data.rows[i].CreateTime + '</span>' +
            '</div>' +
            '<div class="content">'+
                '<b></b>' +
                '<img src="' +svcHeader + '/JRPartyService/picture/' + data.rows[i].ImageURL + '"  width="510px" />' +
            '</div>' + 
        '</li>'
                } else {
                    str = str + '<li>' + 
           '<div class="check">' + 
              '<b class="spring"></b>' + 
              '<b class="line"></b>' + 
          '</div>' + 
          '<div class="thumb">' + 
              '<img src="images/pic/top1.jpg" alt=""><br>' + 
              '<span>' + data.rows[i].CreateTime + '</span>' + 
          '</div>' +

          '<div class="content">' +
              '<b></b>' +
              '<img src="' + svcHeader + '/JRPartyService/picture/' + data.rows[i].ImageURL + '"  width="510px" />' +
          '</div>' + 
      '</li>'
                }
                $("#timeline").html(str);
            }

        }
    })

    
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
};