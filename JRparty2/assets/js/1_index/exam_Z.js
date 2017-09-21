//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'echarts'], function ($) {
        accountCheck();
        //if ($.cookie('JTZH_districtID').length == 2) {
        //    loadCommonModule_CP('C', 'plan');
        //} else if ($.cookie('JTZH_districtID').length == 4) {
        //    loadCommonModule_ZP('C', 'plan');
        //} else {
        //    loadCommonModule_P('C', 'plan');
        //}
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
        $.ajax({
            type: "GET",
            url: svc_sys + "/getStatPie",
            data: {
                districtID: $.cookie('JTZH_districtID'),
            },
            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                console.log(data.data); 
                var option1 = {
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b}: {c} ({d}%)"
                    },
                    color: ['green', 'red', 'blue'],
                    title: {
                        text: '任务完成状态统计',
                        x: 'left'
                    },
                    legend: {
                        orient: 'horizontal',
                        left: 'left',
                        data: ['已完成', '未完成', '已过期']
                    },
                    series: [
                        {
                            name: '访问来源',
                            type: 'pie',
                            radius: '55%',
                            center: ['50%', '60%'],
                            data: [
                                { value: data.data.complete, name: '已完成' },
                                { value: data.data.Incomplete, name: '未完成' },
                                { value: data.data.expired, name: '已过期' },
                            ],
                            itemStyle: {
                                emphasis: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            }
                        }
                    ]
                };
                EChartX = echarts.init(document.getElementById('main1'), 'macarons');
                EChartX.setOption(option1);
                var option2 = {
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b}: {c} ({d}%)"
                    },
                    title: {
                        text: '未完成统计',
                        x: 'center'
                    },
                    //legend: {
                    //    orient: 'vertical',
                    //    left: 'left',
                    //    data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
                    //},
                    series: [
                        {
                            name: '访问来源',
                            type: 'pie',
                            radius: '55%',
                            center: ['50%', '60%'],
                            data: [
                               { value: Incomplete[0], name: districtName[0] },
                                { value: Incomplete[1], name: districtName[1] },
                                { value: Incomplete[2], name: districtName[2] },
                                { value: Incomplete[3], name: districtName[3] },
                                { value: Incomplete[4], name: districtName[4] }
                            ],
                            itemStyle: {
                                emphasis: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            }
                        }
                    ]
                };
                EChartX = echarts.init(document.getElementById('main2'), 'macarons');
                EChartX.setOption(option2);
                var option3 = {
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b}: {c} ({d}%)"
                    },
                    title: {
                        text: '已过期统计',
                        x: 'center'
                    },
                    //legend: {
                    //    orient: 'vertical',
                    //    left: 'left',
                    //    data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
                    //},
                    series: [
                        {
                            name: '访问来源',
                            type: 'pie',
                            radius: '55%',
                            center: ['50%', '60%'],
                            data: [
                                 { value: expired[0], name: districtName[0] },
                                { value: expired[1], name: districtName[1] },
                                { value: expired[2], name: districtName[2] },
                                { value: expired[3], name: districtName[3] },
                                { value: expired[4], name: districtName[4] }
                            ],
                            itemStyle: {
                                emphasis: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            }
                        }
                    ]
                };
                EChartX = echarts.init(document.getElementById('main3'), 'macarons');
                EChartX.setOption(option3);

            }
        })


         
        $('#switch').click(function () {
            window.location.href = "exam_ZS.html"
        })

    })