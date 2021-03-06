﻿//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN','jquery.ztree.core','jquery.ztree.excheck'], function ($) {
        accountCheck();
        var $table = $('#table'),
        $remove = $('#table-remove'),
        selections = [];
        $table.attr("data-url", "/JRPartyService/Information.svc/getInformationList?districtID=" + $.cookie('JTZH_districtID') + "&");
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
                             field: 'districtName',
                             title: '组织名称',
                             sortable: true,
                             align: 'center',
                             width: 110
                         }, {
                             field: 'title',
                             title: '标题',
                             sortable: true,
                             align: 'center',
                             width: 110  
                         }, {
                             field: 'description',
                             title: '内容',
                             sortable: true,
                             editable: false,
                             align: 'center', 
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
                $('#common-alert .modal-body').html('<div class="col-md-6"><div class="form-group">' +
                   '<label for="table_add_modal-name">请输入密码</label>' +
                   '<input type="text" class="form-control" id="table_add_modal-name" placeholder=" ">' +
               '</div>');
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
                var str = '<div class="form-group">' +
                               '<label for="table_add_modal-title">标题</label>' +
                                  '<input type="text" class="form-control" id="table_add_modal-title" >' +
                          '</div>' +
                          ' <div class="form-group">' +
                                '<label for="table_add_modal-description">内容</label>' +
                                 '<textarea class="form-control" rows="2"name="description" id="table_add_modal-description"></textarea>' +
                          '</div>'

                $('#table_add_modal .modal-body').html(str);
                $('#table_add_modal-title').val(row.title);
                $('#table_add_modal-description').val(row.description);
                $('#table_add_modal').modal();
                $("#table_add_modal .btn-success").unbind("click");
                $('#table_add_modal').find('.btn-success').click(function () {
                    $.ajax({
                        url: svc_inform + "/editInformation",
                        type: "GET",
                        data: {
                            id:row.id,
                            description: $('#table_add_modal-description').val(),
                            title: $('#table_add_modal-title').val(),
                            districtID: $.cookie("JTZH_districtID"),
                        },
                        success: function (data) {
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
                        url: svc_inform + "/deleteInformation",//http://172.16.0.221:8006/JRPartyService/Party.svc/deletePlan?id={id}
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
        $('#table-add').click(function () {
            $('#table_add_modal .modal-title').html('新增');
            var str = '<div class="form-group">' +
                           '<label for="table_add_modal-title">标题</label>' +
                              '<input type="text" class="form-control" id="table_add_modal-title" >' +
                      '</div>' +
                      '<div class="form-group">' +
                            '<label for="table_add_modal-description">内容</label>' +
                             '<textarea class="form-control" rows="2"name="description" id="table_add_modal-description"></textarea>' +
                       '</div>'+
                '<span style="float:left">推送对象：</span><div id="pushObjects" class="ztree" style="float:left">'+
                '</div>'+
                '<button type="button" id="pushObjCheck" style="padding:5px;outline: none;border:1px solid #CCCCCC;background-color:#21A121;border-radius:5px;color:white">全选</button>';


            $('#table_add_modal .modal-body').html(str); 
            $('#table_add_modal').modal(); 
            $("#table_add_modal .btn-success").unbind("click");
            $('#table_add_modal').find('.btn-success').click(function () { 
                $.ajax({
                    url: svc_inform + "/addInformation",
                    type: "GET",
                    data: { 
                        description: $('#table_add_modal-description').val(),
                        title: $('#table_add_modal-title').val(),
                        districtID:  $.cookie("JTZH_districtID"),
                    },
                    success: function (data) { 
                        $('#table_add_modal').modal('hide');
                        $table.bootstrapTable('refresh');
                        var treeObj=$.fn.zTree.getZTreeObj("pushObjects"),
                            nodes=treeObj.getCheckedNodes(true),
                            objs="";
                        for(var i=0;i<nodes.length;i++){
                            objs+=nodes[i].id + ",";
                        }
                        objs=objs.substr(0,objs.length-1);
                        $.ajax({
                            url:svc_inform + '/infPushIn',
                            type:'GET',
                            data:{districtId:$.cookie("JTZH_districtID"),title:$('#table_add_modal-title').val(),objs:objs},
                            dataType:"json",
                            // contentType: "application/json",
                            cache:false,
                            // async:false,
                            success:function(response){
                                // var item=eval("("+`+")");
                                //console.log(item);
                            },
                            error : function(XMLHttpRequest, textStatus, errorThrown) {
                                // view("异常！");
                                console.log("接口请求失败！");
                            }
                        });
                    } 
                })
            });
            $.ajax({
                url:svc_inform+'/getObjList',
                type:'GET',
                dataType:"json",
                // contentType: "application/json",
                cache:false,
                // async:false,
                success:function(response){
                    // var item=eval("("+`+")");
                    console.log(response);
                    var setting = {check: {enable: true,chkboxType: { "Y" : "", "N" : "" }},data: {simpleData: {enable: true}},view:{showIcon:false}};
                    var treeData=new Array();
                    var town=response.data.subAuthority;
                    treeData[0]={id:response.data.authorityID,pId:"0",name:response.data.moduleName};
                    for(var i in town){
                        treeData.push({id:town[i].authorityID,pId:response.data.authorityID,name:town[i].moduleName});
                        if(town[i].subAuthority.length>0){
                            var village=town[i].subAuthority;
                            for(var j in village){
                                treeData.push({id:village[j].authorityID,pId:town[i].authorityID,name:village[j].moduleName});
                            }
                        }
                    }
                    $.fn.zTree.init($("#pushObjects"), setting, treeData);
                },
                error : function(XMLHttpRequest, textStatus, errorThrown) {
                    // view("异常！");
                    console.log("接口请求失败！");
                }
            });
            $('#pushObjCheck').click(function(){
                var zTree = $.fn.zTree.getZTreeObj("pushObjects");
                if($(this).text()=="全选"){
                    zTree.checkAllNodes(true);
                    $(this).text("清空");
                    $(this).css({'background-color':'white','color':'#21A121'});
                }else{
                    zTree.checkAllNodes(false);
                    $(this).text("全选");
                    $(this).css({'background-color':'#21A121','color':'white'});
                }
            })
        })
    });