require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select', 'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
    accountCheck('bus00201');
    loadCommonModule('business'); 
    var $table = $('#table'),
    $remove = $('#table-remove'),
    selections = [];
    //表格传区域ID参数
    $table.attr("data-url", "/JTSQService/Business.svc/getBusinessList?districtID=" + $.cookie('JTZH_districtID') + "&");
    function initTable() {
        $table.bootstrapTable({
            striped: true,
            height: getHeight(),
            columns: [
                [
                    {
                        //    //状态，选择
                        //    field: 'state',
                        //    checkbox: true,
                        //    align: 'center',
                        //    valign: 'middle'
                        //}, {
                        //状态，选择
                        field: 'id',
                        align: 'center',
                        valign: 'middle',
                        visible: false
                    }, {

                        field: 'business',
                        title: '业务',
                        sortable: true,
                        align: 'center'
                    }, {
                        field: 'service',
                        title: '服务',
                        sortable: true,
                        editable: false,
                        align: 'center'
                    }, {
                        field: 'name',
                        title: '姓名',
                        sortable: true,
                        editable: false,
                        align: 'center'
                    }, {
                        field: 'IDCard',
                        title: '身份证',
                        sortable: true,
                        editable: false,
                        align: 'center'
                    }, {
                        field: 'phone',
                        title: '电话',
                        sortable: true,
                        editable: false,
                        align: 'center'
                    }, {
                        field: 'createTime',
                        title: '创建时间',
                        sortable: true,
                        editable: false,
                        align: 'center'
                    }, {
                        field: 'processTime',
                        title: '受理时间',
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
                        //操作栏，所有操作集中一起
                        field: 'operate',
                        title: '操作',
                        align: 'center',
                        events: operateEvents,
                        formatter: operateFormatter
                    }
                ]
            ],
            //data:data1
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
            var ids = getIdSelections();
            console.log(ids.toString());
            $.ajax({
                url: SVC_DYNC + "/deleteMultiInformation?",    //后台webservice里的方法名称 
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
            '<a class=" check am-btn"   href="javascript:void(0)"  title="查看详情" >',
            '<i class="glyphicon glyphicon-edit"></i>查看详情',
            '</a>  ',
            '<a class=" edit am-btn"   href="javascript:void(0)"  title="编辑" >',
            '<i class="glyphicon glyphicon-edit"></i>编辑',
            '</a>  ',
            '<a class="remove am-btn" href="javascript:void(0)" title="删除">',
            '<i class="glyphicon glyphicon-remove"></i>删除',
            '</a>'
        ].join('');
    }

    //具体操作：打开网页，编辑，删除
    window.operateEvents = {
        ////查看详情
        'click .check': function (e, value, row, index) {
            var str = '<div class="row">' +
                  '<div class="col-md-1">' +
                          '<br/>' +
                  '</div>' +
                  '<div class="col-md-10">' +
                      '<ul class="list-group">' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;业务：</span><span class="business"></span> </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;服务：</span><span class="service"></span> </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;姓名：</span><span class="name"></span> </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;身份证：</span><span class="IDCard"></span>  </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;电话：</span><span class="phone"></span>  </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;备注：</span><span class="remark"></span>  </li>' +
                           '<li class="list-group-item check"><span class="list-group-item-title">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;状态：</span><span class="status"></span>  </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title ">创建时间：</span><span class="createTime"></span> </li>' +
                          '<li class="list-group-item check"><span class="list-group-item-title ">受理时间：</span><span class="processTime"></span> </li>' +
                      '</ul>' +
                  '</div>' +
                   '<div class="col-md-1">' +
                          '<br/>' +
                  '</div>' +
          '</div>'
            $('#html-peek .modal-body').html('');
            $('#html-peek .modal-body').html(str);
            $('.type').html(row.type);
            $('.name').html(row.name);
            $('.processTime').html(row.processTime);
            $('.IDCard').html(row.IDCard);
            $('.createTime').html(row.createTime);
            $('.business').html(row.business);
            $('.service').html(row.service);
            $('.remark').html(row.remark);
            $('.phone').html(row.phone);
            $('.status').html(row.status);
            $('#html-peek').modal();

        },

        //编辑
        'click .edit': function (e, value, row, index) {
            //console.log('你点了这一行数据: ' + JSON.stringify(row));
            //console.log(row);//拿到id，进行操作   
            //window.location.href = "DYNC_Information_Edit.html?id=" + row.id;
            //新增业务
       
            $('#table_add_modal .modal-title').html('编辑业务');
            $('#table_add_modal .modal-body').html(
                    '<form role="form" class="row form-horizontal">' +
                                '<div class="form-group">' +
                                    '<label class="col-sm-3 control-label" for="">业务类型</label>' +
                                    '<div class="col-sm-8" id="dropdownbusiness">' +
                                    '</div>' +
                                '</div>' +
                                '<div class="form-group">' +
                                    '<label class="col-sm-3 control-label" for="">服务名称</label>' +
                                    '<div class="col-sm-8" id="dropdownservice">' +
                                        '<select class="form-control selectpicker" id="serviceName" title="请选择"></select>' +
                                    '</div>' +
                               '</div>' +
                               ' <div class="form-group">' +
                                    '  <label class="col-sm-3 control-label" for="remark">备注信息</label>' +
                                     '<div class="col-sm-8">' +
                                        '<textarea class="form-control" id="remark" style="resize: none" rows="3"></textarea>' +
                                    '</div>' +
                                  ' </div>' +
                                  '<div class="form-group">' +
                                    '<label class="col-sm-3 control-label" for="IDCard">身份证</label>' +
                                    '<div class="col-sm-8">' +
                                        '<input class="form-control" type="text" id="IDCard" placeholder="请输入身份证以匹配其他信息">' +
                                    '</div>' +
                                '</div>' +
                                '<div class="form-group">' +
                                    '<label class="col-sm-3 control-label" for="name">姓名</label>' +
                                    '<div class="col-sm-8">' +
                                        '<input class="form-control" type="text" id="name" placeholder="自动匹配，无需输入" disabled>' +
                                    '</div>' +
                                '</div>' +

                               '<div class="form-group">' +
                                    '<label class="col-sm-3 control-label" for="phone">联系方式</label>' +
                                    '<div class="col-sm-8">' +
                                       ' <input class="form-control" type="text" id="phone" placeholder="自动匹配，无需输入" disabled>' +
                                   ' </div>' +
                               ' </div>' +
                             ' <div class="form-group">' +
                                     '<div class="col-sm-1"></div>' +
                                    '<label class="col-sm-2 control-label" for="userID">接待人</label>' +
                                    '  <div class="col-sm-3"><input class="form-control" type="text" id="userID" placeholder="" disabled></div>' +

                                    '  <label class="col-sm-2 control-label" for="">是否处理</label>' +
                                     '  <div class="col-sm-3 checkbox"><input type="checkbox"name="chkItem" id="Isdeal"></div>' +
                                   ' </div>' +
                       ' </form>');

            $('.selectpicker').selectpicker({
                style: 'btn-default',
                size: 10
            });
              
            //获取处理人信息
            $(function () {
                $.ajax({
                    url: SVC_POP + "/getNameByuserID",
                    type: "GET",
                    data: {
                        id: $.cookie("JTZH_userID")
                    },
                    success: function (data) {
                        console.log(data.data);
                        $('#userID').val(data.data.name);
                    }

                });
            })
            //获取业务列表
            $(function () {
                $.ajax({
                    type: "GET",
                    url: SVC_BUS + "/getbusinessName",
                    success: function (data) {
                        console.log(data);
                        var str1 = '';
                        for (var i in data.rows) {
                            str1 += '<option value=' + data.rows[i].id + '>' + data.rows[i].businessName + '</option>';
                        }
                        var str = '<select class="form-control selectpicker" id="businessName" title="' + row.business + '">' + str1 + '</select>';
                        console.log(str);
                        $("#dropdownbusiness").append(str);
                        $('.selectpicker').selectpicker({
                            style: 'btn-default',
                            size: 10
                        });
                        $.ajax({
                            url: SVC_BUS + "/getserviceNameBybusinessID",
                            type: "GET",
                            data: {
                                id: $('#businessName').val()
                            },
                            success: function (data) {
                                console.log(data);
                                var str1 = '';
                                for (var i in data.rows) {
                                    str1 += '<option value=' + data.rows[i].serviceName + '>' + data.rows[i].serviceName + '</option>';
                                }
                                var str = '<select class="form-control selectpicker" id="serviceName" title="' + row.service + '">' + str1 + '</select>';
                                console.log(str);
                                $("#dropdownservice").html(str);
                                $('.selectpicker').selectpicker({
                                    style: 'btn-default',
                                    size: 10
                                });
                            }
                        });
                    }
                }
            );
            });
            //通过业务类型获取对应业务下的服务
            $('#dropdownbusiness').change(function () {
                $("#dropdownservice").html('');
                console.log($('#dropdownbusiness').val())
                $.ajax({
                    url: SVC_BUS + "/getserviceNameBybusinessID",
                    type: "GET",
                    data: {
                        id: $('#businessName').val()
                    },
                    success: function (data) {
                        console.log(data);
                        var str1 = '';
                        for (var i in data.rows) {
                            str1 += '<option value=' + data.rows[i].serviceName + '>' + data.rows[i].serviceName + '</option>';
                        }
                        var str = '<select class="form-control selectpicker" id="serviceName" title="请选择">' + str1 + '</select>';
                        console.log(str);
                            
                        $("#dropdownservice").html(str);
                        $('.selectpicker').selectpicker({
                            style: 'btn-default',
                            size: 10
                        });
                    }
                });
            })
           
            //通过身份证号获取办理人信息
            $('#IDCard').change(function () {
                console.log($('#IDCard').val())
                $.ajax({
                    url: SVC_POP + "/getSinglePopulationByIDCard",
                    type: "GET",
                    data: {
                        IDCard: $('#IDCard').val(),
                        districtID: $.cookie("JTZH_districtID")
                    },
                    success: function (data) {
                        console.log(data.data);
                        if (data.success == true) {
                            $('#table_add_modal .alert').hide();
                            $('#name').val(data.data.name);
                            $('#phone').val(data.data.phone);
                            $('#table_add_modal .btn-success').click(function () {
                                var date = new Date();
                                var year = date.getFullYear();
                                 
                                var month = date.getMonth() + 1;
                                if (month.toString.length == 1) {
                                    month = "0" + month;
                                }
                                var day = date.getDate();
                                if (day.toString.length == 1) {
                                    day = "0" + day;
                                }                               
                                var hour = date.getHours();
                                var minute = date.getMinutes();
                                var second = date.getSeconds();
                                if ($('#Isdeal').is(':checked')) {
                                    if ($('#remark').val() != null && $('#serviceName').val() != null && $('#IDCard').val() != null) {

                                        $.ajax({
                                            url: SVC_BUS + "/addBusiness",
                                            type: "GET",
                                            data: {
                                                IDCard: $('#IDCard').val(),
                                                serviceName: $('#serviceName').val(),
                                                remark: $('#remark').val(),
                                                userID: $('#userID').val(),
                                                districtID: $.cookie('JTZH_districtID'),
                                                status: 2,
                                                createTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second,
                                                processTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second
                                            },
                                            success: function (data) {
                                                if (data.success == true) {
                                                    $table.bootstrapTable('refresh');
                                                    $('#table_add_modal').modal('hide');
                                                } else {
                                                    $('#table_add_modal .alert').html("网络错误！");
                                                    $('#table_add_modal .alert').show();
                                                }

                                            },
                                            error: function () {
                                                $('#table_add_modal .alert').html("网络错误！");
                                                $('#table_add_modal .alert').show();
                                            }
                                        })
                                    } else {
                                        $('#table_add_modal .alert').html("信息填写不全！");
                                        $('#table_add_modal .alert').show();
                                    }
                                }
                                else {
                                    if ($('#remark').val() != null && $('#serviceName').val() != null && $('#IDCard').val() != null) {
                                        $.ajax({
                                            url: SVC_BUS + "/addBusiness",
                                            type: "GET",
                                            data: {
                                                IDCard: $('#IDCard').val(),
                                                serviceName: $('#serviceName').val(),
                                                remark: $('#remark').val(),
                                                userID: $('#userID').val(),
                                                districtID: $.cookie('JTZH_districtID'),
                                                createTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second,
                                                status: 1,
                                                processTime: null,
                                            },
                                            success: function (data) {
                                                if (data.success == true) {
                                                    $table.bootstrapTable('refresh');
                                                    $('#table_add_modal').modal('hide');
                                                } else {
                                                    $('#table_add_modal .alert').html("网络错误！");
                                                    $('#table_add_modal .alert').show();
                                                }

                                            },
                                            error: function () {
                                                $('#table_add_modal .alert').html("网络错误！");
                                                $('#table_add_modal .alert').show();
                                            }
                                        })
                                    } else {
                                        $('#table_add_modal .alert').html("信息填写不全！");
                                        $('#table_add_modal .alert').show();
                                    }
                                }
                            })
                        } else {
                            $('#table_add_modal .alert').html("网络错误！");
                            $('#table_add_modal .alert').show();
                        }

                    }
                })
            })
            $('#table_add_modal .alert').hide();
            $('#remark').val(row.remark);
            $('#IDCard').val(row.IDCard);
            $('#name').val(row.name);
            $('#phone').val(row.phone);
            $('#userID').val(row.remark);
            console.log(row.business);
            if (row.business = '计划生育') {
                var type = 1
            } else if (row.business = '民政残联') {
                var type = 2
            } else if (row.business = '社保') {
                var type = 3
            }

            $('#business').selectpicker('val', type);//默认选中    
            if (row.status=="已受理") {
                $("[name = chkItem]:checkbox").attr("checked", true);
        }
                $('#table_add_modal').modal();
        },

        //删除
        'click .remove': function (e, value, row, index) {
            //前台删除
            console.log(row.id);
            $table.bootstrapTable('remove', {
                field: 'id',
                values: [row.id]
            });
            //后台删除
            $.ajax({
                type: "GET",
                url: SVC_DYNC + "/deleteInformation",
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
        }
    };


    //获取table高度
    function getHeight() {
        return $(window).height() - $('h1').innerHeight(true) - $('#container').innerHeight(true);
    }

    //扩展功能
    $(function () {
        var scripts = [
                '../assets/js/0-common/bootstrap-table.js',
                '../assets/js/0-common/bootstrap-table-export.js',
                '../assets/js/0-common/tableExport.js',
                '../assets/js/0-common/bootstrap-table-editable.js',
                '../assets/js/0-common/bootstrap-editable.js'
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
    //新增业务
    $('#table-add').click(function () {
        $('#table_add_modal .modal-title').html('新增业务');
        $('#table_add_modal .modal-body').html(
                '<form role="form" class="row form-horizontal">' +
                            '<div class="form-group">' +
                                '<label class="col-sm-3 control-label" for="">业务类型</label>' +
                                '<div class="col-sm-8" id="dropdownbusiness">' +
                                '</div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<label class="col-sm-3 control-label" for="">服务名称</label>' +
                                '<div class="col-sm-8" id="dropdownservice">' +
                                    '<select class="form-control selectpicker" id="serviceName" title="请选择"></select>' +
                                '</div>' +
                           '</div>' +
                           ' <div class="form-group">' +
                                '  <label class="col-sm-3 control-label" for="remark">备注信息</label>' +
                                 '<div class="col-sm-8">' +
                                    '<textarea class="form-control" id="remark" style="resize: none" rows="3"></textarea>' +
                                '</div>' +
                              ' </div>' +
                              '<div class="form-group">' +
                                '<label class="col-sm-3 control-label" for="IDCard">身份证</label>' +
                                '<div class="col-sm-8">' +
                                    '<input class="form-control" type="text" id="IDCard" placeholder="请输入身份证以匹配其他信息">' +
                                '</div>' +
                            '</div>' +
                            '<div class="form-group">' +
                                '<label class="col-sm-3 control-label" for="name">姓名</label>' +
                                '<div class="col-sm-8">' +
                                    '<input class="form-control" type="text" id="name" placeholder="自动匹配，无需输入" disabled>' +
                                '</div>' +
                            '</div>' +

                           '<div class="form-group">' +
                                '<label class="col-sm-3 control-label" for="phone">联系方式</label>' +
                                '<div class="col-sm-8">' +
                                   ' <input class="form-control" type="text" id="phone" placeholder="自动匹配，无需输入" disabled>' +
                               ' </div>' +
                           ' </div>' +
                         ' <div class="form-group">' +
                                 '<div class="col-sm-1"></div>' +
                                '<label class="col-sm-2 control-label" for="userID">接待人</label>' +
                                '  <div class="col-sm-3"><input class="form-control" type="text" id="userID" placeholder="" disabled></div>' +

                                '  <label class="col-sm-2 control-label" for="">是否处理</label>' +
                                 '  <div class="col-sm-3 checkbox"><input type="checkbox" id="Isdeal"></div>' +
                               ' </div>' +
                   ' </form>');

        $('.selectpicker').selectpicker({
            style: 'btn-default',
            size: 10
        });
        $('#table_add_modal .alert').hide();
        $('#table_add_modal').modal();
        //获取处理人信息
        $(function () {
            $.ajax({
                url: SVC_POP + "/getNameByuserID",
                type: "GET",
                data: {
                    id: $.cookie("JTZH_userID")
                },
                success: function (data) {
                    console.log(data.data);
                    $('#userID').val(data.data.name);
                }

            });
        })
        //获取业务列表
        $(function () {
            $.ajax({
                type: "GET",
                url: SVC_BUS + "/getbusinessName",
                success: function (data) {
                    console.log(data);
                    var str1 = '';
                    for (var i in data.rows) {
                        str1 += '<option value=' + data.rows[i].id + '>' + data.rows[i].businessName + '</option>';
                    }
                    var str = '<select class="form-control selectpicker" id="businessName" title="请选择">' + str1 + '</select>';
                    console.log(str);
                    $("#dropdownbusiness").append(str);
                    $('.selectpicker').selectpicker({
                        style: 'btn-default',
                        size: 10
                    });
                    console.log($('#businessName').val());
                }
            }
        );
        });
        //通过业务类型获取对应业务下的服务
        $('#getContent').change(function () {
            console.log($('#getContent').val())
            $.ajax({
                url: SVC_BUS + "/getserviceNameBybusinessID",
                type: "GET",
                data: {
                    id: $('#businessName').val()
                },
                success: function (data) {
                    console.log(data);
                    var str1 = '';
                    for (var i in data.rows) {
                        str1 += '<option value=' + data.rows[i].serviceName + '>' + data.rows[i].serviceName + '</option>';
                    }
                    var str = '<select class="form-control selectpicker" id="serviceName" title="请选择">' + str1 + '</select>';
                    console.log(str);
                    $("#dropdownservice").html(str);
                    $('.selectpicker').selectpicker({
                        style: 'btn-default',
                        size: 10
                    });
                }
            });
        })
        //通过身份证号获取办理人信息
        $('#IDCard').change(function () {
            console.log($('#IDCard').val())
            $.ajax({
                url: SVC_POP + "/getSinglePopulationByIDCard",
                type: "GET",
                data: {
                    IDCard: $('#IDCard').val(),
                    districtID: $.cookie("JTZH_districtID")
                },
                success: function (data) {
                    console.log(data.data);
                    if (data.success == true) {
                        $('#table_add_modal .alert').hide();
                        $('#name').val(data.data.name);
                        $('#phone').val(data.data.phone);
                        $("#table_add_modal .btn-success").unbind("click")
                        $('#table_add_modal .btn-success').click(function () {
                            var date = new Date();
                            var year = date.getFullYear();
                            var month = date.getMonth() + 1;
                            if (month.toString.length == 1) {
                                month = "0" + month;
                            }
                            var day = date.getDate();
                            if (day.toString.length == 1) {
                                day = "0" + day;
                            }
                            var hour = date.getHours();
                            var minute = date.getMinutes();
                            var second = date.getSeconds();
                            if ($('#Isdeal').is(':checked')) {
                                if ($('#remark').val() != null && $('#serviceName').val() != null && $('#IDCard').val() != null) {

                                    $.ajax({
                                        url: SVC_BUS + "/addBusiness",
                                        type: "GET",
                                        data: {
                                            IDCard: $('#IDCard').val(),
                                            serviceName: $('#serviceName').val(),
                                            remark: $('#remark').val(),
                                            userID: $('#userID').val(),
                                            districtID: $.cookie('JTZH_districtID'),
                                            status: 2,
                                            createTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second,
                                            processTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second
                                        },
                                        success: function (data) {
                                            if (data.success == true) {
                                                $table.bootstrapTable('refresh');
                                                $('#table_add_modal').modal('hide');
                                            } else {
                                                $('#table_add_modal .alert').html("网络错误！");
                                                $('#table_add_modal .alert').show();
                                            }

                                        },
                                        error: function () {
                                            $('#table_add_modal .alert').html("网络错误！");
                                            $('#table_add_modal .alert').show();
                                        }
                                    })
                                } else {
                                    $('#table_add_modal .alert').html("信息填写不全！");
                                    $('#table_add_modal .alert').show();
                                }
                            }
                            else {
                                if ($('#remark').val() != null && $('#serviceName').val() != null && $('#IDCard').val() != null) {
                                    $.ajax({
                                        url: SVC_BUS + "/addBusiness",
                                        type: "GET",
                                        data: {
                                            IDCard: $('#IDCard').val(),
                                            serviceName: $('#serviceName').val(),
                                            remark: $('#remark').val(),
                                            userID: $('#userID').val(),
                                            districtID: $.cookie('JTZH_districtID'),
                                            createTime: year + '年' + month + '月' + day + '日 ' + hour + ':' + minute + ':' + second,
                                            status: 1,
                                            processTime: null,
                                        },
                                        success: function (data) {
                                            if (data.success == true) {
                                                $table.bootstrapTable('refresh');
                                                $('#table_add_modal').modal('hide');
                                            } else {
                                                $('#table_add_modal .alert').html("网络错误！");
                                                $('#table_add_modal .alert').show();
                                            }

                                        },
                                        error: function () {
                                            $('#table_add_modal .alert').html("网络错误！");
                                            $('#table_add_modal .alert').show();
                                        }
                                    })
                                } else {
                                    $('#table_add_modal .alert').html("信息填写不全！");
                                    $('#table_add_modal .alert').show();
                                }
                            }
                        })
                    } else {
                        $('#table_add_modal .alert').html("网络错误！");
                        $('#table_add_modal .alert').show();
                    }

                }
            })
        })
    })

})