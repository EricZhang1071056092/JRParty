//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        accountCheck();
        $.ajax({
            type: "GET",
            url: svc_sys + "/getCheckActivityList",
            data: {
                limit: 10,
                districtID:$.cookie('JTZH_districtID')
            },
            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的 
                var $table = $('#table'),
                $remove = $('#table-remove'),
                selections = [];
                //$table.attr("data-url", "/JRPartyService/Party.svc/getCheckActivityList?districtID=" + $.cookie('JTZH_districtID') + "&");
                function initTable() {
                    $table.bootstrapTable({
                        striped: true,
                        height: getHeight(),
                        columns: [
                              [
                                  {
                                      //状态，选择
                                      field: 'state',
                                      checkbox: true,
                                      align: 'center',
                                      valign: 'middle'
                                  }, {
                                      //状态，选择
                                      field: 'id',
                                      align: 'center',
                                      valign: 'middle',
                                      visible: false
                                  }, {
                                      field: 'flag',
                                      title: '组织名称',
                                      sortable: true,
                                      align: 'center'
                                  }, {
                                      field: 'title',
                                      title: '计划名称',
                                      sortable: true,
                                      align: 'center'
                                  }, {
                                      field: 'type',
                                      title: '任务类型',
                                      sortable: true,
                                      align: 'center'
                                      //}, {
                                      //    field: 'pushStatus',
                                      //    title: '推送状态',
                                      //    sortable: true,
                                      //    align: 'center'

                                  }, {
                                      field: 'content',
                                      title: '工作要求',
                                      sortable: true,
                                      editable: false,
                                      align: 'center'
                                      // }, {
                                      //    field: 'percentage',
                                      //    title: '状态',
                                      //    sortable: true,
                                      //    editable: false,
                                      //    align: 'center'
                                  }, {
                                      field: 'month',
                                      title: '任务期限',
                                      sortable: true,
                                      editable: false,
                                      align: 'center'
                                  }, {
                                      //操作栏，所有操作集中一起
                                      field: 'operate',
                                      title: '操作',
                                      align: 'center',
                                      events: operateEvents,
                                      formatter: operateFormatter
                                  }
                              ]
                        ],
                        data: data.rows
                       
                    });

                    // sometimes footer render error.超时操作
                    setTimeout(function () {
                        $table.bootstrapTable('resetView');
                    }, 200);

                    //checkBox多选操作（删除），多选之后就使得删除按钮可用，删除操作还需另写
                    $table.on('check.bs.table uncheck.bs.table ' +
                            'check-all.bs.table uncheck-all.bs.table', function () {
                                $remove.prop('disabled', !$table.bootstrapTable('getSelections').length);
                                //console.log($remove);

                                // save your data, here just save the current page，这里需要考虑编辑保存的操作
                                selections = getIdSelections();

                                //console.log(selections);//可以通过向后台输出数组这样的形式来操作
                                //console.log(JSON.stringify(selections));
                                // push or splice the selections if you want to save all data selections，若想对所有保存项操作，需要进行数据拼接

                            });

                    //加号展开
                    $table.on('expand-row.bs.table', function (e, index, row, $detail) {
                        $detail.html('<image src="' + row.ImageUrl + '">');
                        $.get('LICENSE', function (res) {
                            $detail.html(res.replace(/\n/g, '<br>'));
                        });
                    });
                    //输出所有
                    //$table.on('all.bs.table', function (e, name, args) {
                    //    console.log(name, args);
                    //});

                    //删除按钮操作，具体操作还需请求接口，当前为前台删除，此方法好处是可以不需要刷新
                    //需要判断选择的是一个还是多个，分别请求不同的接口
                    $remove.click(function () {
                        $('#common-alert .modal-title').html('');
                        $('#common-alert .modal-title').html('提示');
                        $('#common-alert .modal-body').html('');
                        $('#common-alert .modal-body').html('<h4>确定要删除该记录吗？</h4>');
                        $('#common-alert').modal();
                        $(".confirm").click(function () {
                            var ids = getIdSelections();
                            console.log(ids.toString());
                            $.ajax({
                                url: SVC_POP + "/deleteMultiBasicPopulation?",    //后台webservice里的方法名称 
                                data: {
                                    idStr: ids.toString()
                                },
                                type: "get",
                                success: function (data) {
                                    console.log(data);
                                    $table.bootstrapTable('remove', {
                                        field: 'id',
                                        values: ids
                                    });
                                    $remove.prop('disabled', true);//删除后按钮继续disabled
                                },
                                error: function (msg) {
                                    alert("获取角色信息失败！");
                                }
                            })
                        });
                    })
                    //调整浏览器窗口大小
                    $(window).resize(function () {
                        $table.bootstrapTable('resetView', {
                            height: getHeight()
                        });
                    });


                }
                //通过选中选择ID
                function getIdSelections() {
                    return $.map($table.bootstrapTable('getSelections'), function (row) {
                        return row.id
                    });
                }
                //？
                function responseHandler(res) {
                    $.each(res.rows, function (i, row) {
                        row.state = $.inArray(row.id, selections) !== -1;
                    });
                    return res;
                }
                //？
                function detailFormatter(index, row) {
                    var html = [];
                    $.each(row, function (key, value) {
                        html.push('<p><b>' + key + ':</b> ' + value + '</p>');
                    });
                    return html.join('');
                }
                //操作图标
                function operateFormatter(value, row, index) {            
                    console.log(row.TVPicture,row.picture);
                    if(row.TVPicture.length<1&&row.picture.length<1){
                        return [ 
                             '<a class="exam am-btn"style="color:#337ab7" href="javascript:void(0)" title="审核">',
                             '<i class="glyphicon glyphicon-check"></i>审核&nbsp;&nbsp;&nbsp;',
                             '</a>',
                            '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                             '<i class="glyphicon glyphicon-minus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                             '</a>',
                             '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="纪要" >',
                             '<i class="glyphicon glyphicon-minus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;', 
                            '</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', 
                        ].join('');
                    }else if(row.TVPicture.length>1&&row.picture.length>1){
                        return [ 
                                     '<a class="exam am-btn"style="color:#337ab7" href="javascript:void(0)" title="审核">',
                                    '<i class="glyphicon glyphicon-check"></i>审核&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                   '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                    '<i class="glyphicon  glyphicon-plus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                      '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="纪要" >',
                                    '<i class="glyphicon glyphicon-plus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                    '</a>', 
                                    '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_tv + row.TVPicture + '" target="_Blank" title="value">',
                                    ' <img src="' + svc_tv + row.TVPicture + '" alt="截图"width="30"> ',
                                   '</a>', 
                        ].join('');
                    }else if(row.TVPicture.length<1&&row.picture.length>1){
                        return [ 
                                     '<a class="exam am-btn"style="color:#337ab7" href="javascript:void(0)" title="审核">',
                                    '<i class="glyphicon glyphicon-check"></i>审核&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                   '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                    '<i class="glyphicon  glyphicon-minus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                      '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="纪要" >',
                                    '<i class="glyphicon glyphicon-plus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                    '</a>',                         
                                   '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_PhotoTake + row.picture + '" target="_Blank" title="value">',
                                        ' <img src=' + svc_PhotoTake + row.picture + ' alt="截图"width="30"> ',
                                        '</a>',
                        ].join('');
                    }else {
                        return [ 
                                     '<a class="exam am-btn"style="color:#337ab7" href="javascript:void(0)" title="审核">',
                                    '<i class="glyphicon glyphicon-check"></i>审核&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                   '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                    '<i class="glyphicon  glyphicon-plus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                      '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="纪要" >',
                                    '<i class="glyphicon glyphicon-minus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                    '</a>', 
                                    '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_tv + row.TVPicture + '" target="_Blank" title="value">',
                                    ' <img src="' + svc_tv + row.TVPicture + '" alt="截图"width="30"> ',
                                   '</a>', 
                        ].join('');
                    }
                }
                //具体操作：打开网页，编辑，删除
                window.operateEvents = {
                    //手机摄图
                    'click .appCheck': function (e, value, row, index) {
                 
                        window.open("timephone.html?id=" + row.PhonePicture + "&districtID=" + row.percentage)
                    },
                    //监控截屏
                    'click .check': function (e, value, row, index) {
                        window.open("time.html?id=" + row.PhonePicture + "&districtID=" + row.percentage)
                    },
                    //审核
                    'click .exam': function (e, value, row, index) {
                        $('#common-alert2 .modal-title').html('');
                        $('#common-alert2 .modal-title').html('审核'); 
                        $('#common-alert2').modal();
                        $(".confirm2").unbind("click");
                        $(".confirm2").click(function () {
                            // console.log($('input:radio[name="inlineRadioOptions"]:checked').val());
                            $.ajax({
                                type: "GET",
                                url: svc_sys + "/checkActivity",
                                data: {
                                    districtID: row.percentage,
                                    id: row.id,
                                    IsOK: "true"//$('input:radio[name="inlineRadioOptions"]:checked').val(),
                                },
                                success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                                    console.log(data);
                                    $table.bootstrapTable('refresh');
                                } 
                            })
                        })
                        $(".cancel2").unbind("click");
                        $(".cancel2").click(function () {
                            // console.log($('input:radio[name="inlineRadioOptions"]:checked').val());
                            $.ajax({
                                type: "GET",
                                url: svc_sys + "/checkActivity",
                                data: {
                                    districtID: row.percentage,
                                    id: row.id,
                                    IsOK: "false"//$('input:radio[name="inlineRadioOptions"]:checked').val(),
                                },
                                success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                                    console.log(data);
                                    $table.bootstrapTable('refresh');
                                } 
                            })
                        })
                    },
                    
                };

                //获取table高度
                function getHeight() {
                    return $(window).height() - $('h1').innerHeight(true) - $('#container').innerHeight(true);
                }

                //扩展功能
                $(function () {
                    var scripts = [
                            '../assets/js/bootstrap-table.js',
                            '../assets/js/bootstrap-table-export.js',
                            '../assets/js/tableExport.js',
                    ],
                        eachSeries = function (arr, iterator, callback) {
                            callback = callback || function () { };
                            if (!arr.length) {
                                return callback();
                            }
                            var completed = 0;
                            var iterate = function () {
                                iterator(arr[completed], function (err) {
                                    if (err) {
                                        callback(err);
                                        callback = function () { };
                                    }
                                    else {
                                        completed += 1;
                                        if (completed >= arr.length) {
                                            callback(null);
                                        }
                                        else {
                                            iterate();
                                        }
                                    }
                                });
                            };
                            iterate();
                        };

                    eachSeries(scripts, getScript, initTable);
                });
                //加载辅助js文件
                function getScript(url, callback) {
                    var head = document.getElementsByTagName('head')[0];
                    var script = document.createElement('script');
                    script.src = url;

                    var done = false;
                    // Attach handlers for all browsers
                    script.onload = script.onreadystatechange = function () {
                        if (!done && (!this.readyState ||
                                this.readyState == 'loaded' || this.readyState == 'complete')) {
                            done = true;
                            if (callback)
                                callback();

                            // Handle memory leak in IE
                            script.onload = script.onreadystatechange = null;
                        }
                    };

                    head.appendChild(script);

                    // We handle everything using the script element injection
                    return undefined;
                } 
            }})
    })