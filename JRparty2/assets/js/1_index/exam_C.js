//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'print'], function ($) {
        accountCheck();
        $.ajax({
            url: svc_sys + "/getActivity",
            type: "GET",
            success: function (data) {
                var str = '';
                for (var i in data.rows) {
                    str = str + '<li class="leftmenuItem" id="' + data.rows[i].id + '" onclick="ChangeUrl2(\'' + data.rows[i].id + '\')"><a href="#topAnchor" class="leftsubItem">' + data.rows[i].type + '</a></li>'
                }
                $('.leftbar2').html(str) 
            }
        })
        $.ajax({
            url: svc_sys + "/getStat",
            type: "GET",
            data: {
                districtID: $.cookie("JTZH_districtID"),
            },
            success: function (data) {
                var str3 = '<th>村名</th>';
                for (var i in data.type) {
                    str3 = str3 + '<th class="x'+i+'">' + data.type[i].name + '<input type="hidden" value="'+data.type[i].id+'" /></th>'
                }

                var str4 = '';
                for (var i in data.rows) {
                    var m = data.rows[i].split('","');
                    var n = m[0].split(",")
                    var str2 = '';
                    for (var j=0;j<n.length;j++) {
						if (j == 0) {
							str2 = str2 + '<th class="y'+i+'" style="display:none;">' + n[j] + '</th>';
						} else if(j == 1) {
							str2 = str2 + '<th>' + n[j] + '</th>';
						} else {
							str2 = str2 + '<td id="'+(j-2)+','+i+'" class="SBnt">' + n[j] + '</td>';
						}
                    }
                    str4 = str4 + '<tr>' + str2 + '</tr>'
                }
                $('#statistic').html('<div style="text-align:center;"id="tabletop"style="margin:0 auto; min-width:300px; "><button id="ele2"  class="btn btn-primary btn-sm" style="float:left;margin-top:4px;margin-left:5px">打印</button><h4>任务一览表</h4>' +
               '<table class="table table-condensed table-bordered table-compact"style="margin:0 auto; min-width:300px; " id="print_content">' +
                   '<thead>' + '<tr>' + str3 + '</tr>' + '</thead>' + '<tbody>' + str4 + '</tbody>' + ' </table></div>');
                $("table").width(100 * data.rows[0].split('","')[0].split(",").length);
                $("table").height(25 * n.length);
                $("#tabletop").height(35 * n.length);
                console.log(25 * n.length, $("#tabletop").height());
                var t = document.getElementsByTagName("td");
                for (var i = 0; i < t.length ; i++) {
                    if ($(t[i]).html() == '已完成') { $(t[i]).toggleClass('completestyle'); }
                    else if ($(t[i]).html() == '进行中') { $(t[i]).toggleClass('experiedstyle'); }
                    else { $(t[i]).toggleClass('incompletestyle'); }
                }
                $("#ele2").on('click', function () {
                    //Print ele2 with default options
                    // $.print("#print_content");
                    $("#print_content").print({
                        //Use Global styles
                        globalStyles: false,
                        //Add link with attrbute media=print
                        mediaPrint: false,
                        //Custom stylesheet
                        stylesheet: "../assets/css/1_index/exam.css",
                        //Print in a hidden iframe
                        iframe: false,
                        //Don't print this
                        noPrintSelector: ".avoid-this",
                        //Add this at top
                        //  prepend: "Hello World!!!<br/>",
                        //Add this on bottom
                        append: "<br/>Buh Bye!"
                    });
                });
				$(".SBnt").click(function(){
					var x=$(this).attr('id').split(',')[0];
					var y=$(this).attr('id').split(',')[1];
					if($(this).text()!='未完成'){
						$.ajax({
							url: svc_sys + "/getActivityPicture",
							type:'GET',
							data:{'id':$('.x'+x+' input').val(),'districtID':$('.y'+y).text()},
							dataType:"json",
							// contentType: "application/json",
							cache:false,
							// async:false,
							success:function(response){
								// var item=eval("("+`+")");
								//console.log(response);

								if(response.rows.length>0){
                                    $('#iDetail').attr('href','./time.html?id='+$('.x'+x+' input').val()+'&districtID='+$('.y'+y).text());
                                    document.getElementById("iDetail").click();
								}else{
                                    $('#iDetail').attr('href','./timephone.html?id='+$('.x'+x+' input').val()+'&districtID='+$('.y'+y).text());
                                    document.getElementById("iDetail").click();
								}
							},
							error : function(XMLHttpRequest, textStatus, errorThrown) {
								// view("异常！");
								console.log("接口请求失败！");
							}
						});	
					}
				})
            }
        })

    })
function ChangeUrl2(url) {
    $('#' + url).addClass("removes2").siblings().removeClass("removes2");
    //遍历获取所有父菜单元素
    $(".leftbar2").each(function () {
        //判断当前的父菜单是否是隐藏状态
        if ($(this).is(":hidden")) {
            //如果是隐藏状态则移除其样式
            $(this).children(".leftmenuItem").removeClass("removes2");
        }
    });
    $('#statistic').html('<div class="row"style="left:180px;right:0;position:fixed">' +
                         '<div data-options="'+'region:'+'"west"'+',split:true'+'">' +
                         ' <div class="col-xs-6"data-options="' + 'region:' + '"west"' + ',split:true' + '"  id="statistic_pie"style="min-height:500px;padding-top:4%;border-right:1px solid #ddd;"></div>' +
                         ' <div class="col-xs-6"data-options="' + 'region:' + '"east"' + ',split:true' + '" id="statistic_table">' +
                         '<div  class="col-xs-6 col-md-6"id="complete"style="overflow:auto;">已完成<br></div>' +
                         '<div class="col-xs-6 col-md-6"id="incomplete"style="overflow:auto;">未完成<br></div></div>' +
                         '</div>' +
                         '</div>');
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
         : "")
         + window.location.host;
    //var svc_sys ='http://122.97.218.162:18006'+ "/JRPartyService/Party.svc";
    $.ajax({
        type: "GET",
        url: svc_sys + "/getStatPie",
        data: {
            districtID: $.cookie('JTZH_districtID'),
            id: url
        },
        success: function (data2) {
            console.log(data2);

            var option = {
               // backgroundColor: '#ddd',
                title: {
                    //text: '任务完成情况', 
                    x: 'center',
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                color: ['#191970', '#DC143C'],
                toolbox: {
                    show: true,
                    feature: {

                        dataView: { show: true, readOnly: false },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                legend: {
                    orient: 'vertical',
                    left: 'left',
                    data: ['已完成', '未完成']
                },
                series: [
                    {
                        name: '任务完成情况',
                        type: 'pie',
                        radius: '55%',
                        center: ['50%', '60%'],
                        data: [
                            { value: data2.complete, name: '已完成' },
                            { value: data2.incomplete, name: '未完成' },
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
            EChartX = echarts.init(document.getElementById('statistic_pie'), 'roma');
            EChartX.setOption(option);
            var incompleteNamestr = ''
            for (var i in data2.incompleteName) {
                incompleteNamestr = incompleteNamestr + '<tr><td>' + data2.incompleteName[i] + '</td></tr>'
            }
            $('#incomplete').html('<div style="text-align:center"><h4>未完成</h4></div>' +
            '<table class="table table-condensed table-bordered   table-compact">' +
         '<tbody>' + incompleteNamestr + '</tbody>' +' </table>');
            var completeNamestr = ''
            for (var i in data2.completeName) {
                completeNamestr = completeNamestr + '<tr><td>' + data2.completeName[i] + '</td></tr>'
            }
            $('#complete').html('<div style="text-align:center"><h4>已完成</h4></div>' +
            '<table class="table table-condensed table-bordered   table-compact">' +
         '<tbody>' + completeNamestr + '</tbody>' +' </table>');

        }
    })
}