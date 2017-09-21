//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'echarts'], function ($) {
        accountCheck(); 
        $(".menu_list ul li").click(function () {
			console.log(1);
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
            success: function (data2) {
                console.log(data2); 
                $.ajax({
                    type: "GET",
                    url: svc_sys + "/getStat",
                    data: {
                        districtID: $.cookie('JTZH_districtID'),
                    },
                    success: function (data) {//成功后 
                       console.log(data);
                        var districtName = [];
                        var complete = [];
                        var Incomplete = [];
                        var expired = [];

                        for (var i in data.data) {
                            districtName[i] = data.data[i].districtName;
                            complete[i] = data.data[i].complete;
                            Incomplete[i] = data.data[i].Incomplete;
                            expired[i] = data.data[i].expired;
                        }
                        console.log(districtName,complete,Incomplete,expired,data2.data.complete1,data2.data.Incomplete1, data2.data.expired1);
                        var option = {
                            tooltip: {
                                trigger: 'axis'
                            },
                            toolbox: {
                                show: true,
                                y: 'bottom',
                                feature: {
                                    mark: { show: true },
                                    dataView: { show: true, readOnly: false },
                                    //magicType: { show: true, type: [  'bar', 'stack', 'tiled'] },
                                    restore: { show: true },
                                    saveAsImage: { show: true }
                                }
                            },
                            calculable: true,
                            legend: {
                                data: ['已完成', '未完成', '已过期']
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    splitLine: { show: false },
                                    data: districtName
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    position: 'right',
                                    max:24
                                }
                            ],
                            series: [
                                {
                                    name: '已过期',
                                    type: 'bar',
                                    stack: '总量',
                                    barWidth:70,
                                    itemStyle: { normal: { color: '#FF3333', label: { show: true, position: 'insideRight' } } },
                                    data: expired
                                },
                                 {
                                     name: '未完成',
                                     type: 'bar',
                                     stack: '总量',
                                     barWidth:70,
                                     itemStyle: { normal: { color: '#009FCC	', label: { show: true, position: 'insideRight' } } },
                                     data: Incomplete
                                 },
                                {
                                    name: '已完成',
                                    type: 'bar',
                                    stack: '总量', 
                                     barWidth:70,      
                                    itemStyle: { normal: { color: '#66DD00', label: { show: true, position: 'insideRight' } } },
                                    data: complete
                                },
                                {
                                    name: '搜索引擎细分',
                                    type: 'pie',
                                    tooltip: {
                                        trigger: 'item',
                                        formatter: '{b} : {c} ({d}%)'
                                    },
                                    center: [290, 130],
                                    radius: [0, 50],
                                    itemStyle: {
                                        normal: {
                                    label: { 
                                        show: true, 
                                        formatter: '{b} : {c} ({d}%)',
                                        position: 'outer'
                                    },
                                            labelLine: {
                                                length: 20
                                            }
                                        }
                                    },
                                    data: [
                                        { value: data2.data.complete1, name: '已完成' },
                                        { value: data2.data.Incomplete1, name: '未完成' },
                                        { value: data2.data.expired1, name: '已过期' },

                                    ]
                                }
                            ]
                        };
                        EChartX = echarts.init(document.getElementById('main1'), 'macarons');
                        EChartX.setOption(option);
                    }
                })
            }
        })


        $('#switch').click(function () {
            window.location.href = "exam_C.html"
        })

    })