//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN', "fileinput", "fileinput_locale_zh"], function ($) {
        loadCommonModule('camera');
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/Party.svc/getCameraList?districtID=" + '01' + "&");
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
                             title: '位置',
                             sortable: true,
                             align: 'center',
                         }, {
                             field: 'ChannelID',
                             title: '型号',
                             sortable: true,
                             align: 'center'

                         }, {
                             field: 'IP',
                             title: '安装时间',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'number',
                             title: '流地址',
                             sortable: true,
                             editable: false,
                             align: 'center'
                         }, {
                             field: 'port',
                             title: '简介',
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
                        selections = getIdSelections();
                    });

            //加号展开
            $table.on('expand-row.bs.table', function (e, index, row, $detail) {
                $detail.html('<image src="' + row.ImageUrl + '">');
                $.get('LICENSE', function (res) {
                    $detail.html(res.replace(/\n/g, '<br>'));
                });
            });
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
                        
                        '<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                        '<i class="glyphicon glyphicon-edit"></i>编辑',
                        '</a>', 
                        '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="删除">',
                        '<i class="glyphicon glyphicon-remove"></i>删除',
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
                               '<label for="table_add_modal-position">位置：</label>' +
                               '<input type="text" class="form-control" id="table_add_modal-position" placeholder="位置">' +
                          '</div>' +
                           '<div class="form-group">' +
                              '<label for="table_add_modal-address">摄像头流地址：</label>' +
                              '<input type="text" class="form-control" id="table_add_modal-address" placeholder="摄像头流地址">' +
                          '</div>' +
                          '<div class="form-group">' +
                              '<label for="table_add_modal-type">型号：</label>' +
                              '<input type="text" class="form-control" id="table_add_modal-type" placeholder="型号">' +
                          '</div>' +
                           '<div class="form-group">' +
                              '<label for="table_add_modal-setTime">安装时间：</label>' +
                              '<input type="text" readonly class="form_datetime form-control" id="table_add_modal-setTime">' +
                          '</div>' +
                          '<div class="form-group">' +
                              '<label for="table_add_modal-remark">简介：</label>' +
                              '<input type="text" class="form-control" id="table_add_modal-remark" placeholder="简介">' +
                         '</div>' +
                         '<div class="form-group">' +
                              '<label for="input-image-3">图片：</label>' +
                              '<input id="input-image-3" name="input-image" type="file" multiple class="file-loading" ></div>'
                $('#table_add_modal .modal-body').html(str);
                $('.selectpicker').selectpicker({
                    style: 'btn-default',
                    size: 10
                });
                $(".form_datetime").datetimepicker({
                    format: 'yyyy-mm-dd',
                    language: 'zh-CN',
                    autoclose: 1,
                    startView: 3,
                    minView: 2,
                    maxView: 2,
                    forceParse: true
                });
                $("#input-image-3").fileinput({
                    language: 'zh',
                    // uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
                    allowedFileExtensions: ["jpg", "png", "doc", "xls", "xlsx", "docx"],
                    maxImageWidth: 2400,
                    maxImageHeight: 1800,
                    dropZoneEnabled: true,
                    resizePreference: 'width',
                    showPreview: false,
                    maxFileCount: 10,
                    maxFileSize: 10000,
                    resizeImage: true,
                    initialPreview: [
                               //初始化显示封面
                               "<img src='" + '../img/dlxx2.png' + "' class='file-preview-image' alt='Desert' title='Desert'>",
                    ],
                }).on('filepreupload', function () {
                    $(".am-close").click();
                    $('#kv-success-box').html('');
                }).on('fileuploaded', function (event, data) {
                    $('#kv-success-box').append(data.response.link);
                    $('#kv-success-modal').modal('show');
                });
                $('#table_add_modal-position').val(row.name);
                $('#table_add_modal-address').val(row.ChannelID);
                $('#table_add_modal-type').val(row.IP);
                $('#table_add_modal-setTime').val();
                $('#table_add_modal-remark').val(row.number);
                $('#table_add_modal-ChannelID').val(row.userName);
                $('#table_add_modal-password').val(row.password);
                $('#table_add_modal').modal();
                $("#table_add_modal .btn-success").unbind("click");
                $('#table_add_modal .btn-success').click(function () {
                    $.ajax({
                        url: svc_sys + "/editCamera",
                        type: "GET",
                        data: {
                            id: row.id,
                            position: $('#table_add_modal-position').val(),
                            address: $('#table_add_modal-address').val(),
                            type: $('#table_add_modal-type').val(),
                            setTime: $('#table_add_modal-setTime').val(),
                            remark: $('#table_add_modal-remark').val(),
                            defimg: $('#table_add_modal-ChannelID').val(),
                            userID: $('#table_add_modal-userName').val(),
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
                    '../../assets/js/bootstrap-table.js',
                    '../../assets/js/bootstrap-table-export.js',
                    '../../assets/js/tableExport.js',
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
            $('#table_add_modal .modal-title').html('新增');
            var str = '<div class="form-group">' +
                 '<label for="table_add_modal-position">位置：</label>' +
                 '<input type="text" class="form-control" id="table_add_modal-position" placeholder="位置">' +
            '</div>' +
             '<div class="form-group">' +
                '<label for="table_add_modal-address">摄像头流地址：</label>' +
                '<input type="text" class="form-control" id="table_add_modal-address" placeholder="摄像头流地址">' +
            '</div>' +
            '<div class="form-group">' +
                '<label for="table_add_modal-type">型号：</label>' +
                '<input type="text" class="form-control" id="table_add_modal-type" placeholder="型号">' +
            '</div>' +
             '<div class="form-group">' +
                '<label for="table_add_modal-setTime">安装时间：</label>' +
                '<input type="text" readonly class="form_datetime form-control" id="table_add_modal-setTime">' +
            '</div>' +
            '<div class="form-group">' +
                '<label for="table_add_modal-remark">简介：</label>' +
                '<input type="text" class="form-control" id="table_add_modal-remark" placeholder="简介">' +
           '</div>' +
           '<div class="form-group">' +
                '<label for="input-image-3">图片：</label>' +
                '<input id="input-image-3" name="input-image" type="file" multiple class="file-loading" ></div>'
              
            //for (var i in data.data) {
            //    str += '<option value="' + data.data[i].id + '">' + data.data[i].roleName + '</option>'
            //} 
            $('#table_add_modal .modal-body').html(str);
            $('.selectpicker').selectpicker({
                style: 'btn-default',
                size: 10
            });
            $(".form_datetime").datetimepicker({
                format: 'yyyy-mm-dd',
                language: 'zh-CN',
                autoclose: 1,
                startView: 3,
                minView: 2,
                maxView: 2,
                forceParse: true
            });
            $("#input-image-3").fileinput({
                language: 'zh',
               // uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
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
                //           "<img src='" +  '../img/dlxx2.png' + "' class='file-preview-image' alt='Desert' title='Desert'>",
                //],
            }).on('filepreupload', function () {
                $(".am-close").click();
                $('#kv-success-box').html('');
            }).on('fileuploaded', function (event, data) {
                $('#kv-success-box').append(data.response.link);
                $('#kv-success-modal').modal('show');
            });
            $('#table_add_modal').modal();
            $("#table_add_modal .btn-success").unbind("click");
            $('#table_add_modal').find('.btn-success').click(function () {
                $.ajax({
                    url: svc_sys + "/addCamera",
                    type: "GET",
                    data: {
                        position: $('#table_add_modal-position').val(),
                        address: $('#table_add_modal-address').val(),
                        type: $('#table_add_modal-type').val(),
                        setTime: $('#table_add_modal-setTime').val(),
                        remark: $('#table_add_modal-remark').val(),
                        defimg: $('#table_add_modal-ChannelID').val(),
                        userID: $('#table_add_modal-userName').val(),
                    },
                    success: function (data) {
                        console.log(data); 
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