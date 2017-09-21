﻿//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
        accountCheck();
        //if ($.cookie('JTZH_districtID').length == 2) {
        //    loadCommonModule_CJ('C', 'plan');
        //} else if ($.cookie('JTZH_districtID').length == 4) {
        //    loadCommonModule_ZJ('C', 'plan');
        //} else {
        //    loadCommonModule_J('C', 'plan');
        //}
        $(".menu_list ul li").click(function () {
            //判断对象是显示还是隐藏
            if ($(this).children(".div1").is(":hidden")) {
                //表示隐藏
                if (!$(this).children(".div1").is(":animated")) {
                    $(this).children(".xiala").css({ 'transform': 'rotate(180deg)' });
                    //如果当前没有进行动画，则添加新动画
                    $(this).children(".div1").animate({
                        height: 'show'
                    }, 1000)
                        //siblings遍历div1的元素
                        .end().siblings().find(".div1").hide(1000);
                }
            } else {
                //表示显示
                if (!$(this).children(".div1").is(":animated")) {
                    $(this).children(".xiala").css({ 'transform': 'rotate(360deg)' });
                    $(this).children(".div1").animate({
                        height: 'hide'
                    }, 1000)
                        .end().siblings().find(".div1").hide(1000);
                }
            }
        });

        $(".camera").children(".div1").animate({
            height: 'show'
        }, 1).end().siblings().find(".div1").hide(1000);
        //阻止事件冒泡，子元素不再继承父元素的点击事件
        $('.div1').click(function (e) {
            e.stopPropagation();
        });
        //点击子菜单为子菜单添加样式，并移除所有其他子菜单样式
        $(".menu_list ul li .div1 .zcd").click(function () {
            //设置当前菜单为选中状态的样式，并移除同类同级别的其他元素的样式
            $(this).addClass("removes").siblings().removeClass("removes");
            //遍历获取所有父菜单元素
            $(".div1").each(function () {
                //判断当前的父菜单是否是隐藏状态
                if ($(this).is(":hidden")) {
                    //如果是隐藏状态则移除其样式
                    $(this).children(".zcd").removeClass("removes");
                }
            });
        });
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/Party.svc/getCameraList?districtID=" + $.cookie('JTZH_districtID') + "&");
        $table.bootstrapTable('mergeCells', {
            index: 0,
            field: 'name',
            colspan: 1,
            rowspan: 3
        });
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
                             field: 'name',
                             title: '地名',
                             sortable: true,
                             align: 'center',
                         }, {
                             field: 'ChannelID',
                             title: '通道号',
                             sortable: true,
                             align: 'center'

                         }, {
                             field: 'IP',
                             title: 'IP通道地址',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'number',
                             title: '机顶盒序列号',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'port',
                             title: '端口号',
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
                
                //data: data1
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
                            alert("获取信息失败！");
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
                        //'<i class="glyphicon glyphicon-check"></i>查看监控',
                        //'</a> ',
                        '<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                        '<i class="glyphicon glyphicon-edit"></i>编辑',
                        '</a>',
                        //'<a class="push am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="推送" >',
                        //'<i class="glyphicon glyphicon-send"></i>推送',
                        //'</a>',
                        '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="取消">',
                        '<i class="glyphicon glyphicon-remove"></i>取消',
                        '</a>'
            ].join('');
        }

        //具体操作：打开网页，编辑，删除
        window.operateEvents = {
            //查看
            'click .check': function (e, value, row, index) {

            },
            //编辑
            'click .edit': function (e, value, row, index) {

                $('#table_add_modal .modal-title').html('编辑');
                //查看测试按钮失效

                var str = '<div class="form-group">' +
              '<label for="table_add_modal-name">地名</label>' +
              '<input type="text" class="form-control" id="table_add_modal-name" placeholder="地名">' +
              '</div>' +
              '<div class="form-group">' +
              '<label for="table_add_modal-ChannelID">通道号</label>' +
              '<input type="text" class="form-control" id="table_add_modal-ChannelID" placeholder="通道号">' +
              '</div>' +
               '<div class="form-group">' +
              '<label for="table_add_modal-IP">IP通道地址</label>' +
              '<input type="text" class="form-control" id="table_add_modal-IP" placeholder="IP通道地址">' +
              '</div>' +
                '<div class="form-group">' +
             '<label for="table_add_modal-port">端口号</label>' +
             '<input type="text" class="form-control" id="table_add_modal-port" placeholder="端口号">' +
             '</div>' +
              '<label for="table_add_modal-userName">用户名</label>' +
              '<input type="text" class="form-control" id="table_add_modal-userName" placeholder="用户名">' +
              '</div>' +
                '<div class="form-group">' +
             '<label for="table_add_modal-password">密码</label>' +
             '<input type="password" class="form-control" id="table_add_modal-password" placeholder="密码">' +
             '</div>' +
                '<div class="form-group">' +
              '<label for="table_add_modal-number">机顶盒序列号</label>' +
              '<input type="text" class="form-control" id="table_add_modal-number" placeholder="机顶盒序列号">' +
              '</div>'
                $('#table_add_modal .modal-body').html(str);
                $('#table_add_modal-name').val(row.name);
                $('#table_add_modal-ChannelID').val(row.ChannelID);
                $('#table_add_modal-IP').val(row.IP);
                $('#table_add_modal-port').val(row.port);
                $('#table_add_modal-number').val(row.number);
                $('#table_add_modal-userName').val(row.userName);
                $('#table_add_modal-password').val(row.password);
                $('#table_add_modal').modal();
                $("#table_add_modal .btn-success").unbind("click");
                $('#table_add_modal .btn-success').click(function () {
                    $.ajax({
                        url: svc_sys + "/editCamera",
                        type: "GET",
                        data: {
                            id: row.id,
                            name: $('#table_add_modal-name').val(),
                            IP: $('#table_add_modal-IP').val(),
                            port: $('#table_add_modal-port').val(),
                            password: $('#table_add_modal-password').val(),
                            number: $('#table_add_modal-number').val(),
                            ChannelID: $('#table_add_modal-ChannelID').val(),
                            userName: $('#table_add_modal-userName').val(),
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
                $(".confirm").click(function () {
                    //前台删除 
                    $table.bootstrapTable('remove', {
                        field: 'id',
                        values: [row.id]
                    });
                    //后台删除
                    $.ajax({
                        type: "GET",
                        url: svc_sys + "/deleteCamera",
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

        //新增
        $('#table-add').click(function () {

            var columnName = "name";
            mergeTable(columnName);


            function mergeTable(field) {
                $table = $("#table");
                var obj = getObjFromTable($table, field);

                for (var item in obj) {
                    $("#table").bootstrapTable('mergeCells', {
                        index: obj[item].index,
                        field: field,
                        colspan: 1,
                        rowspan: obj[item].row,
                    });
                }


            }

            function getObjFromTable($table, field) {
                var obj = [];
                var maxV = $table.find("th").length;

                var columnIndex = 0;
                var filedVar;
                for (columnIndex = 0; columnIndex < maxV; columnIndex++) {
                    filedVar = $table.find("th").eq(columnIndex).attr("data-field");
                    if (filedVar == field) break;

                }
                var $trs = $table.find("tbody > tr");
                var $tr;
                var index = 0;
                var content = "";
                var row = 1;
                for (var i = 0; i < $trs.length; i++) {
                    $tr = $trs.eq(i);
                    var contentItem = $tr.find("td").eq(columnIndex).html();
                    //exist
                    if (contentItem.length > 0 && content == contentItem) {
                        row++;
                    } else {
                        //save
                        if (row > 1) {
                            obj.push({ "index": index, "row": row });
                        }
                        index = i;
                        content = contentItem;
                        row = 1;
                    }
                }
                if (row > 1) obj.push({ "index": index, "row": row });
                return obj;
            }

            $('#table_add_modal .modal-title').html('新增');
            var str = '<div class="form-group">' +
            '<label for="table_add_modal-name">地名</label>' +
            '<input type="text" class="form-control" id="table_add_modal-name" placeholder="地名">' +
            '</div>' +
            '<div class="form-group">' +
            '<label for="table_add_modal-ChannelID">通道号</label>' +
            '<input type="text" class="form-control" id="table_add_modal-ChannelID" placeholder="通道号">' +
            '</div>' +
             '<div class="form-group">' +
            '<label for="table_add_modal-IP">IP通道地址</label>' +
            '<input type="text" class="form-control" id="table_add_modal-IP" placeholder="IP通道地址">' +
            '</div>' +
              '<div class="form-group">' +
           '<label for="table_add_modal-port">端口号</label>' +
           '<input type="text" class="form-control" id="table_add_modal-port" placeholder="端口号">' +
           '</div>' +
            '<label for="table_add_modal-userName">用户名</label>' +
            '<input type="text" class="form-control" id="table_add_modal-userName" placeholder="用户名">' +
            '</div>' +
              '<div class="form-group">' +
           '<label for="table_add_modal-password">密码</label>' +
           '<input type="text" class="form-control" id="table_add_modal-password" placeholder="密码">' +
           '</div>' +
              '<div class="form-group">' +
            '<label for="table_add_modal-number">机顶盒序列号</label>' +
            '<input type="text" class="form-control" id="table_add_modal-number" placeholder="机顶盒序列号">' +
            '</div>'
            //for (var i in data.data) {
            //    str += '<option value="' + data.data[i].id + '">' + data.data[i].roleName + '</option>'
            //}
            str += '</select>' + '</div>'
            $('#table_add_modal .modal-body').html(str);
            $('.selectpicker').selectpicker({
                style: 'btn-default',
                size: 10
            });

            $('#table_add_modal').modal();
            $("#table_add_modal .btn-success").unbind("click");
            $('#table_add_modal').find('.btn-success').click(function () {
                $.ajax({
                    url: svc_sys + "/addCamera",
                    type: "GET",
                    data: {
                        name: $('#table_add_modal-name').val(),
                        IP: $('#table_add_modal-ChannelID').val(),
                        port: $('#table_add_modal-IP').val(),
                        password: $('#table_add_modal-password').val(),
                        number: $('#table_add_modal-number').val(),
                        ChannelID: $('#table_add_modal-ChannelID').val(),
                        userName: $('#table_add_modal-userName').val(),
                    },
                    success: function (data) {
                        console.log(data);
                        //$table.bootstrapTable 
                        $('#table_add_modal').modal('hide');
                        $table.bootstrapTable('refresh'); 
                    },
                    error: function (data) {
                        console.log(data.message);
                    }
                })
            })
        })

    })