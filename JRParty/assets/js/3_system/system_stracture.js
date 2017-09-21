//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
        accountCheck();
        //if ($.cookie('JTZH_districtID').length == 2) {
        //    loadCommonModule_CS('C', 'plan');
        //} else if ($.cookie('JTZH_districtID').length == 4) {
        //    loadCommonModule_ZS('C', 'plan');
        //} else {
        //    loadCommonModule_S('C', 'plan');
        //}
        $(".menu_list ul li").click(function () {
            //判断对象是显示还是隐藏
            if ($(this).children(".div1").is(":hidden")) {
                //表示隐藏
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
                if (!$(this).children(".div1").is(":animated")) {
                    $(this).children(".xiala").css({ 'transform': 'rotate(360deg)' });
                    $(this).children(".div1").animate({
                        height: 'hide'
                    }, 1000)
                        .end().siblings().find(".div1").hide(1000);
                }
            }
        });

        $(".system").children(".div1").animate({
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
        $(function () {
            $.ajax({
                url: svc_sys + "/getOrganTree",
                type: "GET",
                success: function (data) {
                    var str1 = ''
                    var str2 = ''
                    for (var i in data.rows) {
                        console.log(data.rows[i].SubOrgan.length);
                        if (data.rows[i].SubOrgan.length == 0) {
                            str1 = '<li>' +
                          '<span><i class="icon-minus-sign"></i>' + data.rows[i].name + '</span>  ' +
                             '<ul>';
                        } else {
                            str1 = '<li>' +
                            '<span><i class="icon-plus-sign"></i>' + data.rows[i].name + '</span>  ' +
                               '<ul>';
                        }
                       
                        for (var j in data.rows[i].SubOrgan) {
                            console.log(data.rows[i].SubOrgan[j]);
                            str1 = str1 + '<li class="leaf">' + '<span><i class="icon-leaf"></i>' + data.rows[i].SubOrgan[j] + '</span> ' +
                                        '</li>'
                        }
                        str1 = str1 + '</ul>' + '</li>'
                        str2 += str1
                    }
                    $('#tree_two').html(str2);
                    $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
                    $('#tree_two').find('.leaf').hide('fast');
                    console.log($(".sidebar").height(), $(".rightbar_tree").height());
                    if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                        $(".sidebar").css("height", $(".rightbar_tree").height())
                    }
                    $('.tree li.parent_li > span').on('click', function (e) {

                        var children = $(this).parent('li.parent_li').find(' > ul > li');

                        if (children.is(":visible")) {

                            children.hide('fast');

                            $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
                            if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                                $(".sidebar").css("height", $(".rightbar_tree").height())
                            }

                        } else {

                            children.show('fast');

                            $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
                            if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                                $(".sidebar").css("height", $(".rightbar_tree").height())
                            }
                        }

                        e.stopPropagation();

                    });
                }
            });
          //  $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');

          

        });
        $('#structure').click(function () {
            window.location.href = "system.html"
        })

    })