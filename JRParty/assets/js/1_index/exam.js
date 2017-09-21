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
        $.ajax({
            type: "GET",
            url: svc_sys + "/getActivityTime",
            success: function (data) {
                var str1 = ''
                var time = data.rows
                time.reverse()
                for (var i in time) {
                    str1 = str1 + '<option id="' + time[i] + '" value="' + time[i] + '">' + time[i] + '</option>';
                }
                $('#partybranch').html(str1);
                $('#partybranch').val(time[0]);
                //通过党支部\内容获取时间
                $.ajax({
                    url: svc_sys + "/getActivityListByTime",
                    type: "GET",
                    data: {
                        time: $('#partybranch').val(),
                    },
                    success: function (data) {
                        var str2 = ''
                        for (var i in data.rows) {
                            str2 = str2 + '<option id="' + data.rows[i].id + '" value="' + data.rows[i].id + '">' + data.rows[i].content + '</option>';
                        }
                        $('#task').html(str2);
                        $('#task').val(data.rows[0].id);
                        console.log($('#task').val());
                        var $table = $('#table'),
                        $remove = $('#table-remove'),
                        selections = [];
                        initTable()
                        //类型选择  
                        $table.bootstrapTable('refresh', {
                            url: "/JRPartyService/Party.svc/checkActivity?id=" + $('#task').val() + "&"
                        });
                    }
                });

                $('#partybranch').change(function () {
                    console.log($('#partybranch').val())
                    $.ajax({
                        url: svc_sys + "/getActivityListByTime",
                        type: "GET",
                        data: {
                            time: $('#partybranch').val(),
                        },
                        success: function (data) {
                            var str2 = ''
                            for (var i in data.rows) {
                                console.log(data.rows[i].content);
                                str2 = str2 + '<option id="' + data.rows[i].id + '" value="' + data.rows[i].id + '">' + data.rows[i].content + '</option>';
                            }
                            console.log(str2);
                            $('#task').html(str2);
                            $('#task').val(data.rows[0].id);
                            console.log($('#task').val());
                            //类型选择  
                            $table.bootstrapTable('refresh', {
                                url: "/JRPartyService/Party.svc/checkActivity?id=" + $('#task').val() + "&"
                            });

                        }
                    });
                })
                $('#task').change(function () {
                    console.log($('#task').val());
                    //类型选择  
                    $table.bootstrapTable('refresh', {
                        url: "/JRPartyService/Party.svc/checkActivity?id=" + $('#task').val() + "&"
                    });
                })
            },
            error: function (data) {
                console.log(data);
            }
        })

        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        //$table.attr("data-url", "/JRPartyService/Party.svc/checkActivity?id=" + $('#task').val() + "&");
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
                             //}, {
                             //    field: 'districtName',
                             //    title: '二级组织',
                             //    sortable: true,
                             //    valign: 'middle',
                             //    align: 'center'
                         }, {
                             field: 'SubdistrictName',
                             title: '党员名称',
                             sortable: true,
                             editable: false,
                             align: 'center'
                             //}, {
                             //    field: 'record',
                             //    title: '综合分数',
                             //    sortable: true,
                             //    editable: false,
                             //    align: 'center'
                         }, {
                             field: 'percentage',
                             title: '完成情况',
                             sortable: true,
                             editable: false,
                             align: 'center'
                             //}, {
                             //    field: 'status',
                             //    title: '状态',
                             //    sortable: true,
                             //    editable: false,
                             //    align: 'center'
                             //}, {
                             //    field: 'month',
                             //    title: '任务期限',
                             //    sortable: true,
                             //    editable: false,
                             //    align: 'center'

                             //}, {
                             //    //操作栏，所有操作集中一起
                             //    field: 'operate',
                             //    title: '操作',
                             //    align: 'center',
                             //    events: operateEvents,
                             //    formatter: operateFormatter
                         }
                     ]
                ],
                // data: data1,

            });

            // sometimes footer render error.超时操作
            setTimeout(function () {
                $table.bootstrapTable('resetView');
            }, 200);
            $table.bootstrapTable('mergeCells', {
                index: 1,
                field: 'context',
                colspan: 1,
                rowspan: 3
            });
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
        $('#statistic').hide();
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

                        '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="详情" >',
                        '<i class="glyphicon glyphicon-search"></i>详情&nbsp;&nbsp;&nbsp;',
                        '</a>',
                        '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="取消">',
                        '<i class="glyphicon glyphicon-envelope"></i>回复',
                        '</a>'
            ].join('');
        }

        //具体操作：打开网页，编辑，删除
        window.operateEvents = {
            //查看
            'click .check': function (e, value, row, index) {
                window.open("time.html")
            },
            //编辑
            'click .edit': function (e, value, row, index) {
                window.location.href = 'task_edit.html';
            },
            //推送
            'click .push': function (e, value, row, index) {
                $('#common-alert .modal-title').html('');
                $('#common-alert .modal-title').html('提示');
                $('#common-alert .modal-body').html('');
                $('#common-alert .modal-body').html('<h4>确定要推送该任务吗？</h4>');
                $('#common-alert').modal();
                $(".confirm").click(function () {
                    $.ajax({
                        type: "GET",
                        url: SVC_System + "/deleteAddressList",
                        data: {
                            id: row.id
                        },
                        success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                            console.log(data);
                            $table.bootstrapTable('refresh');
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    })
                })
            },
            //删除
            'click .remove': function (e, value, row, index) {
                $('#common-alert .modal-title').html('');
                $('#common-alert .modal-title').html('提示');
                $('#common-alert .modal-body').html('');
                $('#common-alert .modal-body').html('<div class="form-group">' +
                            '<label class="control-label">回复内容</label>' +
                            '<textarea class="form-control" rows="3" id="peek" placeholder="请输入回复内容，50字以内。。。。"></textarea>' +
                        '</div>');
                $('#common-alert').modal();
                $(".confirm").click(function () {
                    //前台删除 
                    $table.bootstrapTable('remove', {
                        field: 'id',
                        values: [row.id]
                    });
                    //后台删除
                    $.ajax({
                        type: "GET",
                        url: SVC_System + "/deleteAddressList",
                        data: {
                            id: row.id
                        },
                        success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                            console.log(data);
                            $table.bootstrapTable('refresh');
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    })
                })
            }
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
        $('#switch').click(function () {
            window.location.href = "exam_CS.html"
        })

    })