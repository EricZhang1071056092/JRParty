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
                            tooltip: {
                                trigger: 'axis'
                            },
                            title: {
                                text: '监控点位分布统计',
                                x: 'center'
                            },
                            legend: {
                                data: ['数量'],
                                x: 'left'

                            },
                            toolbox: {
                                show: true,
                                y: 'bottom',
                                feature: {
                                    mark: { show: true },
                                    dataView: { show: true, readOnly: false },
                                    magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                                    restore: { show: true },
                                    saveAsImage: { show: true }
                                }
                            },
                            calculable: true, 
                            xAxis: [
                                {
                                    type: 'category',
                                    splitLine: { show: false },
                                    data: ['茅山镇', '白兔镇', '下蜀镇']
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    position: 'right',
                                    max: 24,
                                }
                            ],
                             
                            series: [
                                {
                                    name: '数量',
                                    type: 'bar',
                                    stack: '总量',
                                   // itemStyle: { normal: { color: '#FF3333', label: { show: true, position: 'insideRight' } } },
                                    data:  [2,4,1]
                                }, 
                                {
                                    name: '搜索引擎细分',
                                    type: 'pie',
                                    tooltip: {
                                        trigger: 'item',
                                        formatter: '{a} <br/>{b} : {c} ({d}%)'
                                    },
                                    center: [290, 130],
                                    radius: [0, 50],
                                    itemStyle: {
                                        normal: {
                                            label: {
                                                show: true,
                                                formatter: '{b} : {c} ({d}%)'
                                            },
                                            labelLine: {
                                                length: 20
                                            }
                                        }
                                    },
                                    data: [
                                        { value: 3, name: '茅山镇' },
                                        { value: 1, name: '白兔镇' },
                                        { value: 7, name: '下蜀镇' },

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