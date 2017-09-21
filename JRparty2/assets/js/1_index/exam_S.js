//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'echarts'], function ($) {
        accountCheck();
        if ($.cookie('JTZH_districtID').length == 2) {
            loadCommonModule_CP('C', 'plan');
        } else if ($.cookie('JTZH_districtID').length == 4) {
            loadCommonModule_ZP('C', 'plan');
        } else {
            loadCommonModule_P('C', 'plan');
        } 
        var option = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            legend: {
                data: ['已完成',   '未完成']
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
                    data: ['2010年', '2011年', '2012年', '2013年', '2014年', '2015年', '2016年', '2017年']
                }
            ],
            series: [
                {
                    name: '已完成',
                    type: 'bar',
                    stack: '总量',
                    itemStyle: { normal: { label: { show: true, position: 'insideRight' } } },
                    data: [320, 302, 301, 334, 390, 330, 320, 302, 301, 334, 390, 330]
                }, 
                {
                    name: '未完成',
                    type: 'bar',
                    stack: '总量',
                    itemStyle: { normal: { label: { show: true, position: 'insideRight' } } },
                    data: [220, 182, 191, 234, 290, 330, 310, 302, 301, 334, 390, 330]
                },

            ]
        };

        EChartX = echarts.init(document.getElementById('main'), 'macarons');
        EChartX.setOption(option); 
        $('#switch').click(function () {
            window.location.href = "exam_C.html"
        })
        
    })