﻿//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        accountCheck();
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/Tissue.svc/getCheckLeadershipList?districtID=" + $.cookie('JTZH_districtID') + "&");
        function initTable() {
            $table.bootstrapTable({
                striped: true,
                height: getHeight(),
                columns: [
                     [
                         {
                             field: 'id',
                             align: 'center',
                             valign: 'middle',
                             visible: false
                         }, {
                             field: 'id',
                             align: 'center',
                             valign: 'middle',
                             visible: false
                         }, {
                             field: 'name',
                             title: '姓名',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'duty',
                             title: '职务',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'type',
                             title: '类型',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'financialType',
                             title: '财政负担类型',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'status',
                             title: '状态',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'operate',
                             title: '操作类型',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         }, {
                             field: 'sex',
                             title: '性别',
                             sortable: true,
                             align: 'center'
                         }, {
                             field: 'TrainingTitle',
                             title: '专技职称',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         },{
                             field: 'phone',
                             title: '联系方式',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         }, {
                             field: 'nation',
                             title: '民族',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         },{
                             field: 'IDCard',
                             title: '身份证',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         },{
                             field: 'birthDay',
                             title: '出生日期',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         }, {
                             field: 'education',
                             title: '文化水平',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         },{
                             field: 'JionTime',
                             title: '入党时间',
                             sortable: true,
                             editable: false,
                             align: 'center'

                         },{
                             field: 'workTime',
                             title: '工作时间',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         },{
                             //操作栏，所有操作集中一起
                             field: 'operate2',
                             title: '　操作　',
                             align: 'center',
                             events: operateEvents,
                             formatter: operateFormatter
                         }
                     ]
                ],
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
                        //'<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="审核" >',
                        //'<i class="glyphicon glyphicon-check"></i>审核',
                        //'</a>',
                        '<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                        '<i class="glyphicon glyphicon-edit"></i>编辑',
                        '</a><br/>',
                        //  '<a class="push am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="推送" >',
                        //'<i class="glyphicon glyphicon-send"></i>推送',
                        //'</a>',
                        '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="删除">',
                        '<i class="glyphicon glyphicon-remove"></i>删除',
                        '</a>'
            ].join('');
        }

        //具体操作：打开网页，编辑，删除
        window.operateEvents = {
            //编辑
            'click .edit': function (e, value, row, index) { 
                $('#table_add_modal .modal-title').html('详情');
                var str = '<form id="form"><div class="col-md-4">' +
                          '<div class="form-group">' +
                          '<label for="table_add_modal-name">姓名</label>' +
                          '<input type="text" class="form-control"name="name"  id="table_add_modal-name">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-sex">性别</label>' +
                          '<input type="text" class="form-control"name="sex"  id="table_add_modal-sex">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-TrainingTitle">专技职称</label>' +
                          '<input type="text" class="form-control"name="TrainingTitle"  id="table_add_modal-TrainingTitle">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-phone">联系方式</label>' +
                          '<input type="text" class="form-control"name="phone"  id="table_add_modal-phone">' +
                          '</div>' +
                            '<div class="form-group">' +
                          '<label for="table_add_modal-financialType">财政负担类型</label>' +
                          '<input type="text" class="form-control"name="financialType"  id="table_add_modal-financialType">' +
                          '</div>' +
                          '</div>' +
                          '<div class="col-md-4">' +
                            '<div class="form-group">' +
                          '<label for="table_add_modal-nation">民族</label>' +
                          '<input type="text" class="form-control"name="nation"  id="table_add_modal-nation">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-birthDay">出生日期</label>' +
                          '<input type="text" class="form-control form_datetime"name="birthDay" readonly id="table_add_modal-birthDay">' +
                          '</div>' +
                            '<div class="form-group">' +
                          '<label for="table_add_modal-duty">职务</label>' +
                          '<input type="text" class="form-control"name="duty"  id="table_add_modal-duty">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-education">文化水平</label>' +
                          '<input type="text" class="form-control"name="education"  id="table_add_modal-education">' +
                          '</div>' +
                           '</div>' +
                           '<div class="col-md-4">' +
                          '<div class="form-group">' +
                          '<label for="table_add_modal-IDCard">身份证</label>' +
                          '<input type="text" class="form-control"name="IDCard"  id="table_add_modal-IDCard">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-JionTime">加入时间</label>' +
                          '<input type="text" class="form-control form_datetime"name="JionTime" readonly id="table_add_modal-JionTime">' +
                          '</div>' +
                           '<div class="form-group">' +
                          '<label for="table_add_modal-workTime">工作时间</label>' +
                          '<input type="text" class="form-control form_datetime"name="workTime"readonly  id="table_add_modal-workTime">' +
                          '</div>' +
                            '<div class="form-group">' +
                          '<label for="table_add_modal-type">类型</label>' +
                          '<input type="text" class="form-control"name="type"  id="table_add_modal-type">' +
                          '</div></div>' +
                          '</form>'
                $('#table_add_modal .modal-body').html(str);
                $(".form_datetime").datetimepicker({
                    format: 'yyyy-mm-dd',
                    language: 'zh-CN',
                    autoclose: 1,
                    startView: 3,
                    minView: 2,
                    maxView: 2,
                    forceParse: true
                });
                $('#table_add_modal-name').val(row.name);
                $('#table_add_modal-IDCard').val(row.IDCard);
                $('#table_add_modal-sex').val(row.sex),
               $('#table_add_modal-nation').val(row.nation),
               $('#table_add_modal-birthDay').val(row.birthDay),
               $('#table_add_modal-JionTime').val(row.JionTime),
               $('#table_add_modal-workTime').val(row.workTime),
                $('#table_add_modal-duty').val(row.duty),
                $('#table_add_modal-education').val(row.education),
                $('#table_add_modal-TrainingTitle').val(row.TrainingTitle),
                $('#table_add_modal-type').val(row.type),
               $('#table_add_modal-phone').val(row.phone),
               $('#table_add_modal-financialType').val(row.financialType),
               $('#table_add_modal').modal();
                $("#table_add_modal .btn-success").unbind("click");
                $('#table_add_modal').find('.btn-success').click(function () {
                    console.log($('#table_add_modal-districtID').find("option:selected").text());
                    $.ajax({
                        url: svc_tissue + "/editLeadership",
                        type: "GET",
                        data: {
                            id: row.id,
                            name: $('#table_add_modal-name').val(),
                            IDCard: $('#table_add_modal-IDCard').val(),
                            sex: $('#table_add_modal-sex').val(),
                            nation: $('#table_add_modal-nation').val(),
                            birthDay: $('#table_add_modal-birthDay').val(),
                            JionTime: $('#table_add_modal-JionTime').val(),
                            workTime: $('#table_add_modal-workTime').val(),
                            duty: $('#table_add_modal-duty').val(),
                            education: $('#table_add_modal-education').val(),
                            TrainingTitle: $('#table_add_modal-TrainingTitle').val(),
                            type: $('#table_add_modal-type').val(),
                            phone: $('#table_add_modal-phone').val(),
                            financialType: $('#table_add_modal-financialType').val(),
                            districtID: $.cookie("JTZH_districtID"),
                        },
                        success: function (data) {
                            //$table.bootstrapTable 
                            $('#table_add_modal').modal('hide');
                            $table.bootstrapTable('refresh');
                        },
                        error: function (data) {
                            console.log(data.message);
                        }
                    })
                })
            },
            //删除
            'click .remove': function (e, value, row, index) {
                $('#common-alert .modal-title').html('');
                $('#common-alert .modal-title').html('提示');
                $('#common-alert .modal-body').html('');
                $('#common-alert .modal-body').html('<h4>确定要删除该记录吗？</h4>');
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
                        url: svc_tissue + "/deleteLeadership",//http://172.16.0.221:8006/JRPartyService/Party.svc/deletePlan?id={id}
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
      
    })
