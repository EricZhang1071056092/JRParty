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
            url: svc_sys + "/getStat",
            data: {
                districtID: $.cookie('JTZH_districtID'),
            },
            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                console.log(data.data);
                var districtName = [];
                var complete = []
                var Incomplete = []
                var expired = []
                for (var i in data.data) {
                    districtName[i] = data.data[i].districtName;
                    complete[i] = data.data[i].complete;
                    Incomplete[i] = data.data[i].Incomplete;
                    expired[i] = data.data[i].expired;
                }
                var option = {
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    title: {
                        text: '历年任务完成情况'
                    },
                    legend: {
                        data: ['已完成', '未完成', '已过期']
                    },
                    toolbox: {
                        show: true,
                        feature: {
                        }
                    },
                    calculable: true,
                    yAxis: [
                        {
                            type: 'value'
                        }
                    ],
                    xAxis: [
                        {
                            type: 'category',
                            data: districtName
                        }
                    ],
                    series: [
                         {
                             name: '已过期',
                             type: 'bar',
                             stack: '总量',
                             itemStyle: { normal: { color: '#FF3333', label: { show: true, position: 'insideRight' } } },
                             data: expired
                         },
                         {
                             name: '未完成',
                             type: 'bar',
                             stack: '总量',
                             itemStyle: { normal: { color: '#009FCC	', label: { show: true, position: 'insideRight' } } },
                             data: Incomplete
                         },
                        {
                            name: '已完成',
                            type: 'bar',
                            stack: '总量',
                            itemStyle: { normal: { color: '#66DD00', label: { show: true, position: 'insideRight' } } },
                            data: complete
                        },



                    ]
                };

                EChartX = echarts.init(document.getElementById('main'), 'macarons');
                EChartX.setOption(option);

            }
        })


        $('#switch').click(function () {
             
                window.location.href = "exam_CS.html"
            })

    })