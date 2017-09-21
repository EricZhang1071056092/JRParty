//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh", "echarts"], function ($) {
    loadCommonModule('statistics');
    //$.ajax({
    //    type: "GET",
    //    url: svc_sys + "/getStatPie",
    //    data: {
    //        districtID:'01',
    //    },
    //    success: function (data) {
    //        var districtName2 = [];
    //        var complete2 = [];
    //        var Incomplete2 = [];
    //        var expired2 = []; 
    //        for (var i in data.data) {
    //            districtName2[i] = data.data[i].districtName;
    //            complete2[i] = data.data[i].complete;
    //            Incomplete2[i] = data.data[i].Incomplete;
    //            expired2[i] = data.data[i].expired;
    //        }
    //        $.ajax({
    //            type: "GET",
    //            url: svc_sys + "/getStat",
    //            data: {
    //                districtID: '01',
    //            },
    //            success: function (data) {//成功后 
    //                var districtName = [];
    //                var complete = [];
    //                var Incomplete = [];
    //                var expired = [];

    //                for (var i in data.data) {
    //                    districtName[i] = data.data[i].districtName;
    //                    complete[i] = data.data[i].complete;
    //                    Incomplete[i] = data.data[i].Incomplete;
    //                    expired[i] = data.data[i].expired;
    //                }

    var option = {
        timeline: {
            data: [
                '2017-01-01', '2017-02-01', '2017-03-01', '2017-04-01', '2017-05-01',
                '2017-06-01', '2017-07-01', '2017-08-01', '2017-09-01', '2017-10-01'
            ],
            label: {
                formatter: function (s) {
                    return s.slice(0, 12);
                }
            },
            autoPlay: true,
            playInterval: 1000
        },
        options: [
            {
                title: {
                    'text': '监控平台浏览情况统计',
                    'subtext': '数据来自国家统计局'
                },
                tooltip: { 'trigger': 'axis' },
                legend: {
                    x: 'right',
                    'data': ['浏览数目'],
                    'selected': {
                        '浏览数目': true, 
                    }
                },
                toolbox: {
                    'show': true,
                    orient: 'vertical',
                    x: 'right',
                    y: 'center',
                    'feature': {
                        'mark': { 'show': true },
                        'dataView': { 'show': true, 'readOnly': false },
                        'magicType': { 'show': true, 'type': ['line', 'bar', 'stack', 'tiled'] },
                        'restore': { 'show': true },
                        'saveAsImage': { 'show': true }
                    }
                },
                calculable: true,
                grid: { 'y': 80, 'y2': 100 },
                xAxis: [{
                    'type': 'category',
                    'axisLabel': { 'interval': 0 },
                    'data': [
                        '白兔镇', '下蜀镇', '后白镇', '茅山镇',

                    ]
                }],
                yAxis: [
                    {
                        'type': 'value',
                        'name': '浏览数目（次）',
                        'max':500
                    }, 
                ],
                series: [
                    {
                        'name': '浏览数目',
                        'type': 'bar',
                        'markLine': {
                            symbol: ['arrow', 'none'],
                            symbolSize: [4, 2],
                            itemStyle: {
                                normal: {
                                    lineStyle: { color: 'orange' },
                                    barBorderColor: 'orange',
                                    label: {
                                        position: 'left',
                                        formatter: function (params) {
                                            return Math.round(params.value);
                                        },
                                        textStyle: { color: 'orange' }
                                    }
                                }
                            },
                            'data': [{ 'type': 'average', 'name': '平均值' }]
                        },
                        'data': [1, 2, 3, 4]
                    },
                ]
            },
            {
                title: { 'text': '1月浏览情况' },
                series: [
                    { 'data': [100, 200, 200, 500] },
                ]
            },
            {
                title: { 'text': '2月浏览情况' },
                series: [
                   { 'data': [100, 125, 234, 323] },
                ]
            },
            {
                title: { 'text': '3月浏览情况' },
                series: [
                    { 'data': [150, 342, 123, 451] },
                ]
            },
            {
                title: { 'text': '4月浏览情况' },
                series: [
                   { 'data': [100, 200, 200, 300] },
                ]
            },
            {
                title: { 'text': '5月浏览情况' },
                series: [
                   { 'data': [100, 267, 245, 389] },
                ]
            },
            {
                title: { 'text': '6月浏览情况' },
                series: [
                     { 'data': [100, 200, 200, 300] },
                ]
            },
            {
                title: { 'text': '7月浏览情况' },
                series: [
                     { 'data': [100, 245, 267, 390] },
                ]
            },
            {
                title: { 'text': '8月浏览情况' },
                series: [
                    { 'data': [100, 267, 256, 334] },
                ]
            },
            {
                title: { 'text': '9月浏览情况' },
                series: [
                    { 'data': [100, 234, 221, 343] },
                ]
            }, {
                title: { 'text': '10月浏览情况' },
                series: [
                    { 'data': [100, 234, 221, 343] },
                ]
            }
        ]
    };
    EChartX = echarts.init(document.getElementById('main'), 'macarons');
    EChartX.setOption(option);
    //            }
    //        })
    //    }
    //})

})