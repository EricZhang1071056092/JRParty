//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        accountCheck();
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/Position.svc/getcheckPositionList?districtID=" + $.cookie('JTZH_districtID') + "&");
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
                             field: 'districtName',
                             title: '组织名称',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'type',
                             title: '阵地名称',
                             sortable: true,
                             align: 'center'
                         //}, {
                         //    field: 'description',
                         //    title: '内容',
                         //    sortable: true,
                         //    align: 'center'
                             //}, {
                             //    field: 'releaseTime',
                             //    title: '创建时间',
                             //    sortable: true,
                             //    editable: false,
                             //    align: 'center' 
                         //}, {
                         //    field: 'area',
                         //    title: '区域面积',
                         //    sortable: true,
                         //    editable: false,
                         //    align: 'center'
                         }, {
                             field: 'status',
                             title: '状态',
                             sortable: true,
                             align: 'center'
                         }, {
                             field: 'detailoperate',
                             title: '详情',
                             align: 'center',
                            // events: detailoperateEvents,
                             formatter: detailoperateFormatter
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
                onLoadSuccess: function () {
                    var columnName = "districtName";
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
                },
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
        function detailoperateFormatter(value, row, index) {
            var str = ''
            if (row.imageURL != null) {
                for (var i in row.imageURL) {
                    str = str + '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + row.imageURL[i] + '" target="_Blank" title="value">' +
                     ' <img src="' + row.imageURL[i] + '" alt="截图"width="30"> ' +
                     '</a>';
                }
            } else {
                str = str + '暂无'
            }
            return [str].join('');
        }
        //操作图标
        function operateFormatter(value, row, index) {
            
            return [ '<a class="exam am-btn"style="color:#337ab7" href="javascript:void(0)" title="审核">'+
                        '<i class="glyphicon glyphicon-check"></i>审核'+
                        '</a>'].join('');           
        }

        //具体操作：打开网页，编辑，删除
        window.operateEvents = {
            //详情
            'click .check': function (e, value, row, index) {
                window.open('position_detail.html?id=' + row.id)
                //$.ajax({
                //    type: "GET",
                //    url: svc_position + "/getPositionDetail",
                //    data: {
                //        id: row.id
                //    },
                //    success: function (data) {
                //        var strfile = '';
                //        for (var i in data.data.imageURL) {
                //            console.log(data.data.imageURL[i].id);
                //            strfile = strfile + '<div  id="' + data.data.imageURL[i].id + '"><a style="line-height:2.5" href="' + svc_file + data.data.imageURL[i].Url + '"target="_bank"><img src="../assets/img/file.png" />' + data.data.imageURL[i].Url + '</a>&nbsp;&nbsp;&nbsp; </div>'
                //        }
                //        $('#table_check_modal .modal-title').html('详情');
                //        var str = '<h4 style="text-align:center">' + data.data.type + '</h4>' +
                //            '<p  style="text-indent:2em;">' + data.data.description + '</p>' +
                //             '<div>文件：</div><br>' + strfile

                //        $('#table_check_modal .modal-body').html(str);
                //        $('#table_check_modal').modal();
                //    }
                //})
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
                        url: svc_position + "/checkPosition",
                        data: {
                            districtID: $.cookie('JTZH_districtID'),
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
                        url: svc_position + "/checkPosition",
                        data: {
                            districtID: $.cookie('JTZH_districtID'),
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
            //编辑
            'click .edit': function (e, value, row, index) {
                $.ajax({
                    type: "GET",
                    url: svc_position + "/getPositionDetail",
                    data: {
                        id: row.id
                    },
                    success: function (data) {
                        var strfile = '';
                        for (var i in data.data.imageURL) {
                            console.log(data.data.imageURL[i].id);
                            strfile = strfile + '<div  id="'+ data.data.imageURL[i].id + '"><a style="line-height:2.5" href="' + svc_file + data.data.imageURL[i].Url + '"target="_bank"><img src="../assets/img/file.png" />' + data.data.imageURL[i].Url + '</a>&nbsp;&nbsp;&nbsp;<a href="#"style="line-height:2.5"><img src="../assets/img/1_index/remove.png"onclick="delet(\'' + data.data.imageURL[i].id + '\')"width="20" style="margin-top:7px" /></a></div><br>'
                        }
                        $('#table_edit_modal .modal-title').html('编辑阵地');
                        var str = '<form id="editform">' +
                                   '<input type="text" class="" hidden name="districtID" id="table_edit_modal-districtID" >' +
                                    '<input type="text" class="" hidden name="id" id="id" >' +
                                  '</div>' +
                               '<div class="col-md-6"><div class="form-group">' +
                                '<label for="table_add_modal-type">名称</label>' +
                              '<input type="text" class="form-control"name="type" id="table_edit_modal-type" >' +
                              '</div>' +
                               '<div class="form-group">' +
                               '<label for="#table_add_modal-description">内容</label>' +
                               '<textarea class="form-control" rows="2"name="description" id="table_edit_modal-description"></textarea>' +
                              '</div>' +
                              '</div>' +
                               '<div class="col-md-6"><div class="form-group">' +
                               '<label for="table_add_modal-area">面积</label>' +
                              '<input type="number" class="form-control"name="area"  id="table_edit_modal-area">' +
                              '</div>' +
                              ' <div class="form-group">' +
                              '  <label for="exampleInputName2">上传文件：</label>' +
                              ' <input id="input-image-2" name="File" type="file" multiple="multiple" class="file-loading">' +
                              ' </div>' +
                                ' </div>' +
                               '<div class="col-md-12"><div class="form-group">' +
                               '<label for="exampleInputName2">已上传：</label><br>' + strfile
                        ' </div>' +
                        '</div>' +
                        '</form>'
                        $('#table_edit_modal .modal-body').html(str);
                        $("#input-image-2").fileinput({
                            language: 'zh',
                            //uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
                            allowedFileExtensions: ["jpg", "png", "doc", "xls", "xlsx", "docx", "txt"],
                            maxImageWidth: 2400,
                            maxImageHeight: 1800,
                            dropZoneEnabled: true,
                            resizePreference: 'width',
                            showPreview: false,
                            showUpload: false,
                            maxFileCount: 10,
                            maxFileSize: 10000,
                            resizeImage: true,
                            //initialPreview: [
                            //           //初始化显示封面
                            //           "<img src='" + $("#edit_imageURL").val() + "' class='file-preview-image' alt='Desert' title='Desert'>",
                            //],
                        }).on('filepreupload', function () {
                            $(".am-close").click();
                            $('#kv-success-box').html('');
                        }).on('fileuploaded', function (event, data) {
                            $('#kv-success-box').append(data.response.link);
                            $('#kv-success-modal').modal('show');
                        });
                        $('.selectpicker').selectpicker({
                            style: 'btn-default',
                            size: 10
                        });
                        $("#id").val(row.id);
                        $("#table_edit_modal-area").val(row.area);
                        $("#table_edit_modal-description").val(row.description);
                        $("#table_edit_modal-districtID").val($.cookie("JTZH_districtID"));
                        $('#table_edit_modal-type').selectpicker('val', (row.type));
                        $("#table_edit_modal-type").val(row.type);
                        $('#table_edit_modal').modal();
                    }
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
                        url: svc_position + "/deletePosition",//http://172.16.0.221:8006/JRPartyService/Party.svc/deletePlan?id={id}
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
        //新增
        $('#table-add').unbind("click");
        $('#table-add').click(function () {
            $.ajax({
                url: svc_system + "/getOrgan2",
                type: "get",
                data: {
                    districtID: $.cookie('JTZH_districtID')
                },
                success: function (data) {
                    var organ = ""
                    for (var i in data.rows) {
                        organ = organ + '<option value="' + data.rows[i].id + '">' + data.rows[i].name + '</option>'
                    }
                    $('#table_add_modal .modal-title').html('新增阵地');
                    var str = '<form id="form"><div class="col-md-6"><div class="form-group">' +
                              '<label for="table_add_modal-districtID">所属组织</label>' +
                              '<select id="table_add_modal-districtID"name="districtID" class="form-control" title="部门"> ' + organ + '</select>' +
                              '</div>' +
                           '<div class="form-group">' +
                            '<label for="table_add_modal-type">名称</label>' +
                          '<select name="type" id="table_add_modal-type"  class="form-control selectpicker" title="名称"> ' +
                              '<option value="党员教育室">党员教育室</option>' +
                              '<option value="党内关爱室">党内关爱室</option>' +
                              '<option value="组织会议室">组织会议室</option>' +
                              '<option value="党建办公室">党建办公室</option>' +
                              '<option value="为民服务大厅">为民服务大厅</option>' +
                             '<option value="综合展示厅">综合展示厅</option>' +
                              '<option value="党性教育长廊">党性教育长廊</option>' +
                             '<option value="党建文化广场">党建文化广场</option>' +
                             '</select>' +
                          '</div>' +
                           ' <div class="form-group">' +
                          '  <label for="exampleInputName2">上传文件：</label>' +
                          ' <input id="input-image-3" name="File" type="file" multiple class="file-loading">' +
                          ' </div>' +
                             '</div>' +
                           '<div class="col-md-6"><div class="form-group">' +
                           '<label for="#table_add_modal-description">内容</label>' +
                           '<textarea class="form-control" rows="2"name="description" id="table_add_modal-description"></textarea>' +
                          '</div>' +
                           '<div class="form-group">' +
                           '<label for="table_add_modal-area">面积</label>' +
                          '<input type="number" class="form-control"name="area"  id="table_add_modal-area">' +
                          '</div>' +
                             '</div>' +

                          '</form>'
                    $('#table_add_modal .modal-body').html(str);
                    $('.selectpicker').selectpicker({
                        style: 'btn-default',
                        size: 10
                    });
                    $("#input-image-3").fileinput({
                        language: 'zh',
                        //uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
                        allowedFileExtensions: ["jpg", "png", "doc", "xls", "xlsx", "docx", "txt"],
                        maxImageWidth: 2400,
                        maxImageHeight: 1800,
                        dropZoneEnabled: true,
                        resizePreference: 'width',
                        showPreview: false,
                        showUpload: false,
                        maxFileCount: 10,
                        maxFileSize: 10000,
                        resizeImage: true, 
                    }).on('filepreupload', function () {
                        $(".am-close").click();
                        $('#kv-success-box').html('');
                    }).on('fileuploaded', function (event, data) {
                        $('#kv-success-box').append(data.response.link);
                        $('#kv-success-modal').modal('show');
                    });
                    $('#table_add_modal').modal();
                }
            })
        })

    })
function doUpload() {
    var formData = new FormData($("#form")[0]);
    $.ajax({
        url: svc_uoload + '/PositionUpload.ashx?',
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (returndata) {
            alert(returndata);
            location.reload()
        },
        error: function (returndata) {
            alert(returndata);
        }
    });
}
function doEdit() {
    var formData = new FormData($("#editform")[0]);
    $.ajax({
        url: svc_uoload + '/editPosition.ashx?',
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (returndata) {
            alert(returndata);
            location.reload()
        },
        error: function (returndata) {
            alert(returndata);
        }
    });
}
function delet(id) {
    /*---------------接口地址----------------*/
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_position = svcHeader + "/JRPartyService/Position.svc";
    console.log(id);
    $('#' + id).remove();
    $.ajax({
        url: svc_position + '/deletePositionPicture',
        type: 'GET',
        data: {
            id: id
        },
        success: function (returndata) {
            alert('删除成功');
        },
        error: function (returndata) {
            alert('删除失败');
        }
    });
}