//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'scroll' ], function ($) {
        loadCommonModule_Z();
        accountCheck1();
        var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
           : "")
           + window.location.host;
        var svc_sys = svcHeader + "/JRPartyService/Party.svc"; 
      
        $(".menu_list ul li").click(function () {
            //判断对象是显示还是隐藏
            if ($(this).children(".div1").is(":hidden")) {
                //表示隐藏
				$('.glyphicon').attr('class','glyphicon glyphicon-chevron-right');
				$(this).find('.glyphicon').attr('class','glyphicon glyphicon-chevron-down');
				$(".fuMenu").css('background-color','transparent');
				$(this).find('p').css('background-color','#0077D1');
                if (!$(this).children(".div1").is(":animated")) {
                    $(this).children(".xiala").css({ 'transform': 'rotate(180deg)' });
                    //如果当前没有进行动画，则添加新动画
                    $(this).children(".div1").animate({
                        height: 'show'
                    }, 1000)
                        //siblings遍历div1的元素
                        .end().siblings().find(".div1").hide(1000);
                }
            } else {
                //表示显示
				$(this).find('.glyphicon').attr('class','glyphicon glyphicon-chevron-right');
				$(".fuMenu").css('background-color','transparent');
                if (!$(this).children(".div1").is(":animated")) {
                    $(this).children(".xiala").css({ 'transform': 'rotate(360deg)' });
                    $(this).children(".div1").animate({
                        height: 'hide'
                    }, 1000)
                        .end().siblings().find(".div1").hide(1000);
                }
            }
        });

        $(".plan").children(".div1").animate({
            height: 'show'
        }, 1).end().siblings().find(".div1").hide(1000);
        //阻止事件冒泡，子元素不再继承父元素的点击事件
        $('.div1').click(function (e) {
            e.stopPropagation();
        });
        //点击子菜单为子菜单添加样式，并移除所有其他子菜单样式
        $(".menu_list ul li .div1 .zcd").click(function () {
            //设置当前菜单为选中状态的样式，并移除同类同级别的其他元素的样式
            $(this).addClass("removes").siblings().removeClass("removes");
            //遍历获取所有父菜单元素
            $(".div1").each(function () {
                //判断当前的父菜单是否是隐藏状态
                if ($(this).is(":hidden")) {
                    //如果是隐藏状态则移除其样式
                    $(this).children(".zcd").removeClass("removes");
                }
            });
        });
        $('#zcd1').addClass("removes");
        $.ajax({
            type: "GET",
            url: svc_inform + "/getInformationList",//http://172.16.0.221:8006/JRPartyService/Party.svc/deletePlan?id={id}
            data: {
                districtID: $.cookie("JTZH_districtID"),
                limit: 1000
            },
            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                console.log(data);
                var str = ''
                for (var i in data.rows) {
                    str = str + '<li>' +
                            '<p><a href="#" name="' + data.rows[i].description + '" class="InformationDetail">【' + data.rows[i].districtName + '】:</a><a href="#"name="' + data.rows[i].description + '"  class="a_blue InformationDetail">' + data.rows[i].title + '</a></p>' +
                            '<p><a href="#"style="float:right" >' + data.rows[i].releaseTime + '</a></p>' +
                            '</li>'
                }
                $('#informContent').html(str);
                $('.list_lh li:even').addClass('lieven');
                $("div.list_lh").myScroll({
                    speed: 50, //数值越大，速度越慢
                    rowHeight: 68 //li的高度
                });
                $('#informContent').find('.InformationDetail').click(function () {
                    $('#common-alert2 .modal-body').html(this.name);
                    $('#common-alert2').modal();
                })
            }
        })
   
    })
function ChangeUrl(url) {
    document.getElementById('main').src = url;

}