//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "gverify", ], function ($) {
        accountCheck(); 
        
                var $table = $('#table'),
                $remove = $('#table-remove'),
                selections = [];
              $table.attr("data-url", svc_sys+ "/getPlanList?");
                function initTable() {
                    $table.bootstrapTable({
                        striped: true,
                        height: getHeight(),
                        columns: [
                             [
                                 {
                                    
                                     //状态，选择
                                     field: 'id',
                                     align: 'center',
                                     valign: 'middle',
                                     visible: false
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
                                 }, {
                                     field: 'content',
                                     title: '工作要求',
                                     sortable: true,
                                     align: 'center'
                                 }, {
                                     field: 'month',
                                     title: '截止时间',
                                     sortable: true,
                                     editable: false,
                                     align: 'center'
                                 }, {
                                     //操作栏，所有操作集中一起
                                     field: 'operate',
                                     title: '操作',
                                     align: 'center',
                                     events: operateEvents,
                                     formatter: operateFormatter,
                                     width: 110
                                 }
                             ]
                        ],
                       // data: data.rows
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
                    return [
                                '<a class="check am-btn"style="color:#337ab7"   href="javascript:void(0)"  title="查看" >',
                                '<i class="glyphicon glyphicon-check"></i>跟踪',
                                '</a> ',
                                //'<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                                //'<i class="glyphicon glyphicon-edit"></i>编辑',
                                //'</a>',
                                '<a class="push am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="推送" >',
                                '<i class="glyphicon glyphicon-send"></i>附件',
                                '</a>',
                                //'<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="取消">',
                                //'<i class="glyphicon glyphicon-remove"></i>取消',
                                //'</a>'
                    ].join('');
                }

                //具体操作：打开网页，编辑，删除
                window.operateEvents = {
                    //查看
                    'click .check': function (e, value, row, index) {
                        //window.open("plan_check.html?id=" + row.id + "&title=" + row.title + "&context=" + row.context);
                        window.location.href = 'task_monitor_Z.html?id=' + row.id + "&type=" + row.type + "&month=" + row.month + "&districtID=" + $.cookie('JTZH_districtID');
                    },
                    //编辑
                    'click .edit': function (e, value, row, index) {
                        console.log(row.id);
                        window.location.href = "plan_edit.html?id=" + row.id + "&title=" + row.title + "&context=" + row.context;
                    },
                    //查看附件
                    'click .push': function (e, value, row, index) {
                        $.ajax({
                            type: "GET",
                            url: svc_sys + "/getActivityDetail",
                            data: {
                                id: row.id
                            },
                            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                                console.log(data.rows);
                                var str = '';
                                for (var i in data.data.file) {
                                    str = str + '<div  id="' + data.data.file[i].id + '"><a style="line-height:2.5" href="' + 'http://172.16.0.221:8006/JRPartyService/Upload/Activity/' + data.data.file[i].Url + '"target="_blank"><img src="../assets/img/file.png" />' + data.data.file[i].Url + '</a>' +
                                    '&nbsp;&nbsp;&nbsp; <div>'
                                }

                                $('#common-alert .modal-title').html('');
                                $('#common-alert .modal-title').html('查看附件');
                                $('#common-alert .modal-body').html('');
                                $("#common-alert .modal-body").html(str);
                                $('#common-alert').modal();
                            }
                        })
                    },
                    //删除
                    'click .remove': function (e, value, row, index) {
                        $('#common-alert .modal-title').html('');
                        $('#common-alert .modal-title').html('提示');
                        $('#common-alert .modal-body').html('');
                        $('#common-alert .modal-body').html('<div style="margin-left:87px"><div id="v_container" style="width: 200px;height: 31px;float:left;margin-right:-47px"></div>' +
                                                            '<input type="text"style="width: 200px;height:31px; float:left;border:1px solid #808080;padding-left:20px;" id="code_input" value="" placeholder="请输入验证码"/></div>' +
                                                            '<div id="infor"></div>');
                        var verifyCode = new GVerify("v_container");
                        $('#common-alert').modal();
                        $(".confirm").unbind("click");
                        $(".confirm").click(function () {
                            var res = verifyCode.validate(document.getElementById("code_input").value);
                            if (res) {
                                //前台删除 
                                $table.bootstrapTable('remove', {
                                    field: 'id',
                                    values: [row.id]
                                });
                                //后台删除
                                $.ajax({
                                    type: "GET",
                                    url: svc_sys + "/deletePlan",
                                    data: {
                                        id: row.id
                                    },
                                    success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                                        console.log(data);
                                        $table.bootstrapTable('refresh');
                                        $(".close").click();

                                    },
                                    error: function (data) {
                                        console.log(data);
                                    }
                                })
                            } else {
                                alert("验证码错误");
                            }
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

            if($.cookie("JTZH_push") !="" && $.cookie("JTZH_push") != null){
                var task = $.cookie("JTZH_push");
                $('#table_add_modal .modal-title').html('');
                $('#table_add_modal .modal-title').html('公告消息推送');
                $('#table_add_modal .modal-body').html('');
                $('#table_add_modal .modal-body').html('<table class="table table-striped table-hover table-bordered">' +
                    '<thead><tr><th>组织名称</th><th>内容</th><th>发布日期</th></tr></thead>' +
                    '<tbody>' + task + '</tbody>' + '</table>');
                $('#table_add_modal').modal();
                $.cookie("JTZH_push", '', { expires: 3, path: '/' });
            }
    })