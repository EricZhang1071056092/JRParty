//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        accountCheck();
        //if ($.cookie('JTZH_districtID').length == 2) {
        //    loadCommonModule_CP('C', 'plan');
        //} else if ($.cookie('JTZH_districtID').length == 4) {
        //    loadCommonModule_ZP('C', 'plan');
        //} else {
        //    loadCommonModule_P('C', 'plan');
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

        $(".plan").children(".div1").animate({
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
        $table.attr("data-url", "/JRPartyService/Party.svc/getCheckActivityList?districtID=" + $.cookie('JTZH_districtID') + "&");
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
                //data: data1
                //onLoadSuccess: function () {
                //    var columnName = "title";
                //    mergeTable(columnName);
                //    function mergeTable(field) {
                //        $table = $("#table");
                //        var obj = getObjFromTable($table, field);
                //        for (var item in obj) {
                //            $("#table").bootstrapTable('mergeCells', {
                //                index: obj[item].index,
                //                field: field,
                //                colspan: 1,
                //                rowspan: obj[item].row,
                //            });
                //        }
                //    }
                //    function getObjFromTable($table, field) {
                //        var obj = [];
                //        var maxV = $table.find("th").length;

                //        var columnIndex = 0;
                //        var filedVar;
                //        for (columnIndex = 0; columnIndex < maxV; columnIndex++) {
                //            filedVar = $table.find("th").eq(columnIndex).attr("data-field");
                //            if (filedVar == field) break;

                //        }
                //        var $trs = $table.find("tbody > tr");
                //        var $tr;
                //        var index = 0;
                //        var content = "";
                //        var row = 1;
                //        for (var i = 0; i < $trs.length; i++) {
                //            $tr = $trs.eq(i);
                //            var contentItem = $tr.find("td").eq(columnIndex).html();
                //            //exist
                //            if (contentItem.length > 0 && content == contentItem) {
                //                row++;
                //            } else {
                //                //save
                //                if (row > 1) {
                //                    obj.push({ "index": index, "row": row });
                //                }
                //                index = i;
                //                content = contentItem;
                //                row = 1;
                //            }
                //        }
                //        if (row > 1) obj.push({ "index": index, "row": row });
                //        return obj;
                //    }
                //},
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
                        '<a class=" am-btn"style="color:#337ab7;margin:0;width:33px;text-decoration : none" href="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'" target="_Blank" title="value">',
                        ' <img src="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'" alt="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'"width="30"> ',
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
                       '<a class=" am-btn"style="color:#337ab7;margin:0;width:33px;text-decoration : none" href="http://172.16.0.221:8006/JRPartyService/Upload/PhotoTake/' + row.picture + '" target="_Blank" title="value">',
                            ' <img src="http://172.16.0.221:8006/JRPartyService/Upload/PhotoTake/' + row.picture + '" alt="http://172.16.0.221:8006/JRPartyService/Upload/PhotoTake/' + row.picture + '"width="30"> ',
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
                        '<a class=" am-btn"style="color:#337ab7;margin:0;width:33px;text-decoration : none" href="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'" target="_Blank" title="value">',
                        ' <img src="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'" alt="http://172.16.0.221:8006/JRPartyService/picture/'+row.TVPicture+'"width="30"> ',
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
                //$('#common-alert2 .modal-body').html('');
                //$('#common-alert2 .modal-body').html('<div class="form-group"style="line-height:20px"><label class="radio-inline">' +
                                                        //  '<input type="radio" name="inlineRadioOptions" id="inlineRadio1" value="true">通过' +
                                                        //  '</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' +
                                                        //  '<label class="radio-inline">' +
                                                         // ' <input type="radio" name="inlineRadioOptions" id="inlineRadio2" value="false">不通过' +
                                                         // '</label></div>');
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
            ////查看
            //'click .check': function (e, value, row, index) { 
            //    window.location.href = 'task_monitor_check.html?id=' + row.id + "&type=" + row.type + "&month=" + row.month + "&districtID=" + $.cookie('JTZH_districtID');
            //},
            ////编辑districtID=" + 
            ////附件
            //'click .edit': function (e, value, row, index) {
            //    $.ajax({
            //        type: "GET",
            //        url: svc_sys + "/getActivityFile",
            //        data: {
            //            id: row.id
            //        },
            //        success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
            //            console.log(data.rows);
            //            var str = '';
            //            for (var i in data.rows) {
            //                str = '<a style="line-height:2.5" href="' + 'http://localhost:8006/JRPartyService/Upload/Activity/' + data.rows[i] + '"><img src="../assets/img/file.png" />' + data.rows[i] + '</a><br>'
            //            }
            //            $('#common-alert .modal-title').html('');
            //            $('#common-alert .modal-title').html('提示');
            //            $('#common-alert .modal-body').html('');
            //            $("#common-alert .modal-body").html('<div class="form-group">' +  
            //                 '<label for="exampleInputName2">已上传：</label><br>' + str+ '</div>'); 
            //            $('#common-alert').modal();
            //        }
            //    })

            //},
            ////推送
            //'click .push': function (e, value, row, index) {
            //    $('#common-alert .modal-title').html('');
            //    $('#common-alert .modal-title').html('提示');
            //    $('#common-alert .modal-body').html('');
            //    $('#common-alert .modal-body').html('<h4>确定要推送该任务到移动端吗？</h4>');
            //    $('#common-alert').modal();
            //    $(".confirm").click(function () { 
            //        $.ajax({
            //            type: "GET",
            //            url: SVC_System + "/deleteAddressList",
            //            data: {
            //                id: row.id
            //            },
            //            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
            //                console.log(data);
            //                $table.bootstrapTable('refresh');
            //            },
            //            error: function (data) {
            //                console.log(data);
            //            }
            //        })
            //    })
            //},
            ////删除
            //'click .remove': function (e, value, row, index) {
            //    $('#common-alert .modal-title').html('');
            //    $('#common-alert .modal-title').html('提示');
            //    $('#common-alert .modal-body').html('');
            //    $('#common-alert .modal-body').html('<h4>确定要删除该记录吗？</h4>');
            //    $('#common-alert').modal();
            //    $(".confirm").click(function () {
            //        //前台删除 
            //        $table.bootstrapTable('remove', {
            //            field: 'id',
            //            values: [row.id]
            //        });
            //        //后台删除
            //        $.ajax({
            //            type: "GET",
            //            url: SVC_System + "/deleteAddressList",
            //            data: {
            //                id: row.id
            //            },
            //            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
            //                console.log(data);
            //                $table.bootstrapTable('refresh');
            //            },
            //            error: function (data) {
            //                console.log(data);
            //            }
            //        })
            //    })
            //}
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
            window.location.href = 'task_add.html';
            //$('#table_add_modal .modal-title').html('新增用户');
            //var str = '<div class="form-group">' +
            //    '<label for="table_add_modal-name">姓名</label>' +
            //    '<input type="text" class="form-control" id="table_add_modal-name" placeholder="请填写联系人姓名">' +
            //  '</div>' +
            //    '<div class="form-group">' +
            //    '<label for="table_add_modal-userName">联系方式</label>' +
            //    '<input type="text" class="form-control" id="table_add_modal-phone" placeholder="请填写联系方式">' +
            //  '</div>' +
            //    '<div class="form-group">' +
            //    '<label for="table_add_modal-role">部门</label>' +
            //    '<select class="form-control"title="请选择联系人部门" id="table_add_modal-department">';
            ////for (var i in data.data) {
            ////    str += '<option value="' + data.data[i].id + '">' + data.data[i].roleName + '</option>'
            ////}
            //str += '</select>' + '</div>'
            //$('#table_add_modal .modal-body').html(str);
            //$('.selectpicker').selectpicker({
            //    style: 'btn-default',
            //    size: 10
            //});

            //$('#table_add_modal').modal();
        })
        //$('#table_add_modal').find('.btn-success').click(function () {
        //    $.ajax({
        //        url: SVC_System + "/addAddressList",
        //        type: "get",
        //        data: {
        //            name: $('#table_add_modal-name').val(),
        //            phone: $('#table_add_modal-phone').val(),
        //            department: $('#table_add_modal-department').val(),

        //        },
        //        success: function (data) {
        //            alert(data.message);
        //        }
        //    })
        //})
    })