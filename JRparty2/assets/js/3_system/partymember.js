﻿//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', 'qrcode'], function ($) {
        accountCheck();  
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/System.svc/getPoliticList?districtID=" + $.cookie('JTZH_districtID') + "&");
       
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
                             field: 'name',
                             title: '姓名',
                             sortable: true,
                             align: 'center'
                         }, {
                             field: 'sex',
                             title: '性别',
                             sortable: true,
                             align: 'center'
                         }, {
                             field: 'IDCard',
                             title: '身份证',
                             sortable: true,
                             align: 'center'

                         }, {
                             field: 'educationDegree',
                             title: '学历',
                             sortable: true,
                             editable: false,
                             align: 'center'
                             //}, {
                             //    field: 'time',
                             //    title: '入党时间',
                             //    sortable: true,
                             //    editable: false,
                             //    align: 'center'
                         }, { 
                             field: 'operate',
                             title: '操作',
                             align: 'center',
                             events: operateEvents,
                             formatter: operateFormatter
                         }
                     ]
                ],
                // data: data1
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
                        //'<a class="check am-btn"style="color:#337ab7"   href="javascript:void(0)"  title="查看" >',
                        //'<i class="glyphicon glyphicon-check"></i>查看',
                        //'</a> ',
                        '<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                        '<i class="glyphicon glyphicon-edit"></i>编辑',
                        '</a>',
                        //'<a class="push am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="推送" >',
                        //'<i class="glyphicon glyphicon-send"></i>推送',
                        //'</a>',
                        '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="登出">',
                        '<i class="glyphicon glyphicon-remove"></i>登出',
                        '</a>'
            ].join('');
        }

        //具体操作：打开网页，编辑，删除
        window.operateEvents = {            
            //编辑
            'click .edit': function (e, value, row, index) {
                
                $('#table_add_modal .modal-title').html('编辑组织'); 
                var str = '<div class="col-md-6"><div class="form-group">' +
                           '<label for="table_add_modal-name">姓名</label>' +
                           '<input type="text" class="form-control" id="table_add_modal-name" placeholder=" ">' +
                       '</div>' +
                        '<div class="form-group">' +
                           '<label for="table_add_modal-IDCard">身份证</label>' +
                           '<input type="text" class="form-control" id="table_add_modal-IDCard" placeholder=" ">' +
                       '</div>' +
                        '<div class="form-group">' +
                           '<label for="table_add_modal-phone">联系方式</label>' +
                           '<input type="text" class="form-control" id="table_add_modal-phone" placeholder=" ">' +
                       '</div>' +
                        '<div class="form-group">' +
                           '<label for="table_add_modal-name">性别</label>' +
                           '<input type="text" class="form-control" id="table_add_modal-sex" placeholder=" ">' +
                       '</div></div>' +
             '<div class="col-md-6"><div class="form-group">' +
                           '<label for="table_add_modal-nation">民族</label>' +
                           '<input type="text" class="form-control" id="table_add_modal-nation" placeholder=" ">' +
                       '</div>' +
             '<div class="form-group">' +
                          '<label for="table_add_modal-workPlace">工作地点</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-workPlace" placeholder=" ">' +
                      '</div>' +
                       '<div class="form-group">' +
                          '<label for="table_add_modal-educationDegree">学历</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-educationDegree" placeholder=" ">' +
                      '</div></div>'
                $('#table_add_modal .modal-body').html(str);  
                $('#table_add_modal-name').val(row.name);
                $('#table_add_modal-IDCard').val(row.IDCard);
                $('#table_add_modal-phone').val(row.phone);
                $('#table_add_modal-sex').val(row.sex);
                $('#table_add_modal-nation').val(row.nation);
                $('#table_add_modal-workPlace').val(row.workPlace);
                $('#table_add_modal-educationDegree').val(row.educationDegree);
                $('#table_add_modal').modal();
                $('#table_add_modal .btn-success').unbind("click");
                $('#table_add_modal .btn-success').click(function () {
                    $.ajax({
                        url: svc_system + "/editPolitic",
                        type: "GET",
                        data: {
                            id: row.id,
                            name: $('#table_add_modal-name').val(),
                            districtID: $.cookie('JTZH_districtID'),
                            IDCard: $('#table_add_modal-IDCard').val(),
                            phone: $('#table_add_modal-phone').val(),
                            sex: $('#table_add_modal-sex').val(),
                            nation: $('#table_add_modal-nation').val(),
                            workPlace: $('#table_add_modal-workPlace').val(),
                            educationDegree: $('#table_add_modal-educationDegree').val(),
                        },
                        success: function (data) {
                            console.log(data);
                            //$table.bootstrapTable
                            $('#table_add_modal').modal('hide');
                            $table.bootstrapTable('refresh');
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
                $(".confirm").unbind("click");
                $(".confirm").click(function () {
                    //前台删除 
                    $table.bootstrapTable('remove', {
                        field: 'id',
                        values: [row.id]
                    });
                    //后台删除
                    $.ajax({
                        type: "GET",
                        url: svc_system + "/deletePolitic",
                        data: {
                            id: row.id
                        },
                        success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                          
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

        //新增
        $('#table-add').click(function () { 
            $('#table_add_modal .modal-title').html('新增党员');
            var str = ' <div class="col-md-6"><div class="form-group">' +
                          '<label for="table_add_modal-name">姓名</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-name" placeholder="请填写姓名">' +
                      '</div>' +
                       '<div class="form-group">' +
                          '<label for="table_add_modal-IDCard">身份证</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-IDCard" placeholder="请填写身份证">' +
                      '</div>' +
                       '<div class="form-group">' +
                          '<label for="table_add_modal-phone">联系方式</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-phone" placeholder="请填写联系方式">' +
                      '</div>' +
                       '<div class="form-group">' +
                          '<label for="table_add_modal-name">性别</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-sex" placeholder="请填写性别">' +
                      '</div></div>' +
            '<div class="col-md-6"><div class="form-group">' +
                          '<label for="table_add_modal-nation">民族</label>' +
                          '<input type="text" class="form-control" id="table_add_modal-nation" placeholder="请填写民族">' +
                      '</div>' +
            '<div class="form-group">' +
                         '<label for="table_add_modal-workPlace">工作地点</label>' +
                         '<input type="text" class="form-control" id="table_add_modal-workPlace" placeholder="请填写工作地点">' +
                     '</div>' +
                      '<div class="form-group">' +
                         '<label for="table_add_modal-educationDegree">学历</label>' +
                         '<input type="text" class="form-control" id="table_add_modal-educationDegree" placeholder="请填写学历">' +
                     '</div></div>'
          
            $('#table_add_modal .modal-body').html(str);
            $("#inputUrl").val("http://dnt.dkill.net")
            //没有中文就可以这么简单
            $('#code').qrcode("http://dnt.dkill.net");
            //URL演示
            $("#url_btn").click(function () {
                outputQRCod($("#inputUrl").val(), 256, 256); 
            });
            $('#table_add_modal').modal();
        })
        $('#table_add_modal').find('.btn-success').unbind("click");
        $('#table_add_modal').find('.btn-success').click(function () {
            $.ajax({
                url: svc_system + "/addPolitic",
                type: "get",
                data: {
                    name: $('#table_add_modal-name').val(), 
                    districtID: $.cookie('JTZH_districtID'),
                    IDCard: $('#table_add_modal-IDCard').val(),
                    phone: $('#table_add_modal-phone').val(), 
                    sex: $('#table_add_modal-sex').val(), 
                    nation: $('#table_add_modal-nation').val(), 
                    workPlace: $('#table_add_modal-workPlace').val(), 
                    educationDegree: $('#table_add_modal-educationDegree').val(), 
                     
                },
                    success: function (data) {
                        $('#table_add_modal').modal('hide');
                        $table.bootstrapTable('refresh');
                    }
                })
        })
        $('#structure').click(function () {
            console.log(123);
            window.location.href = "system_structure_cun.html"
        })
        //中文字符处理
        function toUtf8(str) {
            var out, i, len, c;
            out = "";
            len = str.length;
            for (i = 0; i < len; i++) {
                c = str.charCodeAt(i);
                if ((c >= 0x0001) && (c <= 0x007F)) {
                    out += str.charAt(i);
                } else if (c > 0x07FF) {
                    out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
                    out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
                    out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
                } else {
                    out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
                    out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
                }
            }
            return out;
        }

        //生成二维码
        function outputQRCod(txt, width, height) {
            //先清空
            $("#code").empty();
            //中文格式转换
            var str = toUtf8(txt);
            //生成二维码
            $("#code").qrcode({
                render: "canvas",
                width: width,
                height: height,
                text: str
            });
        }
    })