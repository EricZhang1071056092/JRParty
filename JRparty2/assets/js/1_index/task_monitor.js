//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        accountCheck();
        var id = getParam('id');
        var month = getParam('month');
        var type = getParam('type');
        var districtID = getParam('districtID');
        $('#title').html(month + '--' + type);
        //解析值 
        function getParam(paramName) {
            paramValue = "";
            isFound = false;
            if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
                arrSource = decodeURI(this.location.search).substring(1, this.location.search.length).split("&");

                i = 0;
                while (i < arrSource.length && !isFound) {
                    if (arrSource[i].indexOf("=") > 0) {
                        if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                            paramValue = arrSource[i].split("=")[1];
                            isFound = true;
                        }
                    }
                    i++;
                }
            }
            return paramValue;
        }
        
           
                var $table = $('#table'),
                $remove = $('#table-remove'),
                selections = [];
                 $table.attr("data-url", "/JRPartyService/Party.svc/getSubActivityList?id=" + id + "&");
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
                                     field: 'districtName',
                                     title: '组织名称',
                                     sortable: false,
                                     align: 'center'
                                 }, {
                                     field: 'record',
                                     title: '工作进度',
                                     sortable: false,
                                     editable: false,
                                     align: 'center'
                                 }, {
                                     field: 'SubdistrictName',
                                     title: '下属组织',
                                     sortable: false,
                                     editable: false,
                                     align: 'center'
                                 }, {
                                     field: 'percentage',
                                     title: '完成情况',
                                     sortable: false,
                                     editable: false,
                                     align: 'center',
                                     cellStyle: function cellStyle(value, row, index) {
                                         if (value == "进行中") {
                                             return {
                                                 classes: 'text-nowrap another-class',
                                                 css: { "color": "#0000", "font-weight": "bold" },
                                             };
                                         } else if (value == "未完成") {
                                             return {
                                                 classes: 'text-nowrap another-class',
                                                 css: { "color": "#FF0000", "font-weight": "bold" },
                                                 value: '1',
                                             };
                                         } else if (value == "待审核") {
                                             return {
                                                 classes: 'text-nowrap another-class',
                                                 css: { "color": "blue", "font-weight": "bold" },
                                                 value: '1',
                                             };
                                         } else {
                                             return {
                                                 classes: 'text-nowrap another-class',
                                                 css: { "color": "#61db61", "font-weight": "bold" },

                                             };
                                         }
                                     }
                                 }, {
                                     //操作栏，所有操作集中一起
                                     field: 'operate',
                                     title: '记录查看',
                                     align: 'center',
                                     events: operateEvents,
                                     formatter: operateFormatter
                                 }
                             ]
                        ],
                    onLoadSuccess: function (data) {
                            $("#complete").html(data.completeNum);
                            $("#Incomplete").html(data.IncompleteNum);
                            // $("#expired").html(data.expireNum);
                            var columnName = "districtName";
                            mergeTable(columnName);
                            var columnName2 = "record";
                            mergeTable2(columnName2);
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
                            function mergeTable2(field) {
                                $table = $("#table");
                                var obj = getObjFromTable2($table, "districtName");

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
                            function getObjFromTable2($table, field) {
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
                    console.log(row.TVPicture, index);
                    if (row.flag0 == '0' && row.flag1 == '0') {//0是手机1是电视
                        return [
                             '<a class="uptask am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="上传任务" >',
                             '<i class="glyphicon glyphicon-upload"></i>上传任务&nbsp;&nbsp;&nbsp;',
                             '</a>',
                             '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                             '<i class="glyphicon glyphicon-minus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                             '</a>',
                             '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="纪要" >',
                             '<i class="glyphicon glyphicon-minus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                             '</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
                        ].join('');
                    } else if (row.flag0 == '1' && row.flag1 == '1') {
                        return [
                             '<a class="uptask am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="上传任务" >',
                             '<i class="glyphicon glyphicon-upload"></i>上传任务&nbsp;&nbsp;&nbsp;',
                             '</a>',
                                    '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                    '<i class="glyphicon glyphicon-plus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                    '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="手机摄图" >',
                                    '<i class="glyphicon glyphicon-plus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                    '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_tv + row.TVPicture + '" target="_Blank" title="value">',
                                    ' <img src="' + svc_tv + row.TVPicture + '" alt="截图"width="30"> ',
                                    '</a>',
                        ].join('');
                    } else if (row.flag0 == '1' && row.flag1 == '0') {
                        return [
                             '<a class="uptask am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="上传任务" >',
                             '<i class="glyphicon glyphicon-upload"></i>上传任务&nbsp;&nbsp;&nbsp;',
                             '</a>',
                                    '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                    '<i class="glyphicon glyphicon-minus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                    '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="手机摄图" >',
                                    '<i class="glyphicon glyphicon-plus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                    '</a>',
                                    '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_PhotoTake + row.PhonePicture + '" target="_Blank" title="value">',
                                    ' <img src="' + svc_PhotoTake + row.PhonePicture + '" alt="截图"width="30"> ',
                                    '</a>',
                        ].join('');
                    } else {
                        return [
                             '<a class="uptask am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="上传任务" >',
                             '<i class="glyphicon glyphicon-upload"></i>上传任务&nbsp;&nbsp;&nbsp;',
                             '</a>',
                                   '<a class="check am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="监控截屏" >',
                                   '<i class="glyphicon glyphicon-plus-sign"></i>电视截屏&nbsp;&nbsp;&nbsp;',
                                   '</a>',
                                   '<a class="appCheck am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="手机摄图" >',
                                   '<i class="glyphicon glyphicon-minus-sign"></i>手机摄图&nbsp;&nbsp;&nbsp;',
                                   '</a>',
                                   '<a class=" am-btn"style="margin:0;width:33px;text-decoration : none" href="' + svc_tv + row.TVPicture + '" target="_Blank" title="value">',
                                   ' <img src="' + svc_tv  +row.TVPicture + '" alt="截图"width="30"> ',
                                   '</a>',
                        ].join('');
                    }
                }


                //具体操作：打开网页，编辑，删除
                window.operateEvents = {
                    //手机摄图
                    'click .appCheck': function (e, value, row, index) {
                        window.open("timephone.html?id=" + id + "&districtID=" + row.id)

                    },
                    //监控截屏
                    'click .check': function (e, value, row, index) {

                        window.open("time.html?id=" + id + "&districtID=" + row.id)
                    },
                    //上传任务
                    'click .uptask': function (e, value, row, index) {
                        console.log();
                        $('#upload_file .modal-title').html('上传任务完成图片');
                        var str = '<form id="form">' +
                                  '<div class="form-group" style="padding:0 15px">' +
                                   '<label for="exampleInputName2">上传文件：</label>' +
                                   ' <input type="text" hidden name="content" id="content" placeholder="内容">' +
                                   ' <input type="text"hidden name="flag" id="flag" placeholder="标识">' +
                                   ' <input type="text" name="districtID" id="districtID"hidden  placeholder="区域ID">' +
                                   ' <input type="text" name="snId" hidden id="id" placeholder="任务ID" >' +
                                  ' <input id="input-image-3" name="Files" type="file" multiple class="file-loading">';
                        $('#upload_file .modal-body').html(str);
                        $("#input-image-3").fileinput({
                            language: 'zh',
                            //uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
                            allowedFileExtensions: ["jpg", "png", "doc", "xls", "xlsx", "docx"],
                            maxImageWidth: 2400,
                            maxImageHeight: 1800,
                            dropZoneEnabled: true,
                            resizePreference: 'width',
                            showPreview: false,
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
                        $("#content").val('');
                        $("#flag").val('1');
                        $("#districtID").val(row.id); 
                        $("#id").val(id);
                        $('#upload_file').modal();
                    
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
            
    })
function doUpload() {
    var formData = new FormData($("#form")[0]);
    $.ajax({
        url: 'http://172.16.0.221:8006/JRPartyService/Data/ActivityReplenish.ashx?',
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (returndata) {
            alert(returndata);
        },
        error: function (returndata) {
            alert(returndata);
        }
    });
}