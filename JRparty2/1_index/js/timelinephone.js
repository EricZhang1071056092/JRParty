$(function () {
    //window.onload = function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    var svc_PhotoTake = svcHeader + "/JRPartyService/Upload/PhotoTake/";
    var svc_tv = svcHeader + "/JRPartyService/picture/";
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
            console.log(data);
            var str = ''; 
            for (var i = 0 in data.rows) { 
                var strimg1 = '';
                var strimg2 = '';
                for (var j = 0; j < data.rows[i].URL.length; j++) {
                    strimg1 = strimg1+'<img src="' + svc_PhotoTake + data.rows[i].URL[j] + '" width="590"style=margin-top:10px;"><br>' 
                }
                if(i==0){                
                str = str + '<li style="margin-top:10px;padding:20px;" >' +
                         '<div class="check">' +
                              '<b class="spring"></b>' +
                              '<b class="line"></b>' +
                         '</div>' +
                         '<div class=" ">' +
                              '<img src="images/pic/top1.jpg" alt=""><br>' +
                              '<span>' + data.rows[i].time + '</span><br>' +//data.rows[i].CreateTime
                         '</div>' +
                         '<div class="content">' +
                              '<b></b>' + strimg1 +// strimg1 + strimg2 +//+ '<h3>' + data.rows[i].content + '</h3><br>'
                         '</div>' +
                        '</li>' 
                } else {                    
                    str = str + '<li style="margin-top:220px;padding:20px;" >' +
                             '<div class="check">' +
                                  '<b class="spring"></b>' +
                                  '<b class="line"></b>' +
                             '</div>' +
                             '<div class=" ">' +
                                  '<img src="images/pic/top1.jpg" alt=""><br>' +
                                  '<span>' + data.rows[i].time + '</span><br>' +//data.rows[i].CreateTime
                             '</div>' +
                             '<div class="content">' +
                                  '<b></b>' + strimg1 +// strimg1 + strimg2 +//+ '<h3>' + data.rows[i].content + '</h3><br>'
                             '</div>' +
                            '</li>'
                }
            }
            
            $("#timeline").html(str);
            $('.featured').orbit();
            var i = 0; //图片标识 
            var img_num = $(".img_ul").children("li").length; //图片个数 
            $(".img_ul li").hide(); //初始化图片 
            play();
            $(function () {
                $(".img_hd ul").css("width", ($(".img_hd ul li").outerWidth(true)) * img_num); //设置ul的长度 
                $(".bottom_a").css("opacity", 0.7);	//初始化底部a透明度 
                if (!window.XMLHttpRequest) {//对ie6设置a的位置 
                    $(".change_a").css("height", $(".change_a").parent().height());
                }
                $(".change_a").focus(function () {
                    this.blur();
                });
                $(".bottom_a").hover(function () {//底部a经过事件 
                    $(this).css("opacity", 1);
                }, function () {
                    $(this).css("opacity", 0.7);
                });
                $(".change_a").hover(function () {//箭头显示事件 
                    $(this).children("span").show();
                }, function () {
                    $(this).children("span").hide();
                });
                $(".img_hd ul li").click(function () {
                    i = $(this).index();
                    play();
                });
                $(".prev_a").click(function () {

                    //i+=img_num;

                    i--;

                    //i=i%img_num;

                    i = (i < 0 ? 0 : i);

                    play();

                });
                $(".next_a").click(function () {

                    i++;

                    //i=i%img_num;

                    i = (i > (img_num - 1) ? (img_num - 1) : i);

                    play();

                });
            });
            function play() {//动画移动

                var img = new Image(); //图片预加载

                img.onload = function () {

                    img_load(img, $(".img_ul").children("li").eq(i).find("img"))

                };

                img.src = $(".img_ul").children("li").eq(i).find("img").attr("src");

                //$(".img_ul").children("li").eq(i).find("img").(img_load($(".img_ul").children("li").eq(i).find("img")));

                $(".img_hd ul").children("li").eq(i).addClass("on").siblings().removeClass("on");

                if (img_num > 7) {//大于7个的时候进行移动

                    if (i < img_num - 3) { //前3个

                        $(".img_hd ul").animate({ "marginLeft": (-($(".img_hd ul li").outerWidth() + 4) * (i - 3 < 0 ? 0 : (i - 3))) });

                    } else if (i >= img_num - 3) {//后3个

                        $(".img_hd ul").animate({ "marginLeft": (-($(".img_hd ul li").outerWidth() + 4) * (img_num - 7)) });

                    }

                }

                if (!window.XMLHttpRequest) {//对ie6设置a的位置

                    $(".change_a").css("height", $(".change_a").parent().height());

                }

            }
            function img_load(img_id, now_imgid) {//大图片加载设置 （img_id 新建的img,now_imgid当前图片）

                if (img_id.width / img_id.height > 1) {

                    if (img_id.width >= $("#play").width())

                        $(now_imgid).width($("#play").width());

                } else {

                    if (img_id.height >= 500) $(now_imgid).height(650);

                }

                $(".img_ul").children("li").eq(i).show().siblings("li").hide(); //大小确定后进行显示

            }
            function imgs_load(img_id) {//小图片加载设置

                if (img_id.width >= $(".img_hd ul li").width()) { img_id.width = 80 };

                //if(img_id.height>=$(".img_hd ul li").height()) {img_id.height=$(".img_hd ul li").height();}

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
});