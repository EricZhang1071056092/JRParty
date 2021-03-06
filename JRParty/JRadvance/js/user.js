﻿//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
    loadCommonModule('system');
var $table = $('#table'),
          $remove = $('#table-remove'),
          selections = [];
          $table.attr("data-url", "/JRPartyService/Party.svc/getUserList?districtID=" + '01' + "&"); 
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
                                title: '角色',
                                sortable: true,
                                align: 'center'
                            }, {
                                field: 'userName',
                                title: '登录账号',
                                sortable: true,
                                align: 'center',
                            }, {
                                field: 'password',
                                title: '密码',
                                sortable: true,
                                editable: false,
                                align: 'center',

                            }, {
                                field: 'lastTime',
                                title: '最后登录时间',
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
                          //'<a class="check am-btn"style="color:#337ab7"   href="javascript:void(0)"  title="权限" >',
                          //'<i class="glyphicon glyphicon-check"></i>权限',
                          //'</a> ',
                          '<a class="edit am-btn"  style="color:#337ab7" href="javascript:void(0)"  title="编辑" >',
                          '<i class="glyphicon glyphicon-edit"></i>编辑',
                          '</a>',
                          '<a class="remove am-btn"style="color:#337ab7" href="javascript:void(0)" title="登出">',
                          '<i class="glyphicon glyphicon-remove"></i>登出',
                          '</a>'
              ].join('');
          }

          //具体操作：打开网页，编辑，删除
          window.operateEvents = {
              //查看
              'click .check': function (e, value, row, index) {
                  $('#authority_modify_modal .widget-main').html('')
                  $('#authority_modify_modal .widget-main').html('<div id="tree1" class="tree tree-selectable">' +
                                                  '<div class="tree-folder" style="display:none;">' +
                                                      '<div class="tree-folder-header">' +
                                                          '<i class="icon-plus"></i>' +
                                                          '<div class="tree-folder-name"></div>' +
                                                      '</div>' +
                                                      '<div class="tree-folder-content"></div>' +
                                                      '<div class="tree-loader" style="display: none;"></div>' +
                                                  '</div> ' +
                                              '</div>');
               
                
                  $('#authority_modify_modal').modal()
              },
              //编辑 
              'click .edit': function (e, value, row, index) { 
                  $('#table_add_modal .modal-title').html('编辑用户'); 
                  $.ajax({
                      url: svc_sys + "/getOrgan",
                      type: "get",
                      data: {
                          districtID: $.cookie('JTZH_districtID')
                      },
                      success: function (data) { 
                          var organ = ""
                          for (var i in data.rows) {
                              organ = organ + '<option value="' + data.rows[i].id + '">' + data.rows[i].name + '</option>'
                          } 
                          var str = '<div class="form-group">' +
                              '<label for="table_add_modal-name">角色</label>' +
                            '<input type="text" class="form-control" id="table_add_modal-name" placeholder="请输入用户角色">' +
                            '</div>' +
                            '<div class="form-group">' +
                              '<label for="table_add_modal-name">所属部门</label>' +
                            '<select id="table_add_modal-districtID" class="form-control" title="部门"> ' + organ + '</select>' +
                             '</div>' +
                             '<div class="form-group">' +
                             '<label for="#table_add_modal-phone">登录账号(手机号)</label>' +
                            '<input type="text" class="form-control" id="table_add_modal-phone" placeholder=" 请输入登录账号">' +
                            '</div>' +
                             '<div class="form-group">' +
                             '<label for="table_add_modal-password">登录密码</label>' +
                            '<input type="text" class="form-control" id="table_add_modal-password" placeholder=" 请输入登录密码">' +
                            '</div>'
                          $('#table_add_modal .modal-body').html(str);
                          $("#table_add_modal-name").val(row.name);
                          $("#table_add_modal-districtID").val(row.district);
                          $("#table_add_modal-phone").val(row.userName);
                          $("#table_add_modal-password").val(row.password);
                          $('#table_add_modal').modal();
                      }
                  })
                  $('#table_add_modal .btn-success').unbind("click");
                  $('#table_add_modal .btn-success').click(function () {
                      $.ajax({
                          url: svc_sys + "/editUser",
                          type: "GET",
                          data: {
                              id: row.id,
                              name: $('#table_add_modal-name').val(),
                              phone: $('#table_add_modal-phone').val(),
                              password: $('#table_add_modal-password').val(),
                              districtID: $('#table_add_modal-districtID').val(),
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
                          url: svc_sys + "/deleteUser",
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
          $('#table-add').unbind("click");
          $('#table-add').click(function () {
              $.ajax({
                  url: svc_sys + "/getOrgan",
                  type: "get",
                  data: { 
                      districtID: $.cookie('JTZH_districtID')
                  },
                  success: function (data) { 
                      var organ=""
                      for (var i in data.rows) {
                          organ = organ + '<option value="' + data.rows[i].id + '">' + data.rows[i].name + '</option>'
                      }
                      $('#table_add_modal .modal-title').html('新增用户');
                      var str = '<div class="form-group">' +
                          '<label for="table_add_modal-name">角色</label>' +
                        '<input type="text" class="form-control" id="table_add_modal-name" placeholder="请输入用户角色">' +
                        '</div>' +
                        '<div class="form-group">' +
                          '<label for="table_add_modal-name">所属部门</label>' +
                        '<select id="table_add_modal-districtID" class="form-control" title="部门"> ' + organ + '</select>' +
                         '</div>' +
                         '<div class="form-group">' +
                         '<label for="#table_add_modal-phone">登录账号(手机号)</label>' +
                        '<input type="text" class="form-control" id="table_add_modal-phone" placeholder=" 请输入登录账号">' +
                        '</div>' +
                         '<div class="form-group">' +
                         '<label for="table_add_modal-password">登录密码</label>' +
                        '<input type="text" class="form-control" id="table_add_modal-password" placeholder=" 请输入登录密码">' +
                        '</div>'
                      $('#table_add_modal .modal-body').html(str);
                      $('#table_add_modal').modal();
                  }
              })
              
          })
          $('#table_add_modal').find('.btn-success').unbind("click");
          $('#table_add_modal').find('.btn-success').click(function () {
              $.ajax({
                  url: svc_sys + "/addUser",
                  type: "get",
                  data: { 
                      name: $('#table_add_modal-name').val(),
                      phone: $('#table_add_modal-phone').val(),
                      password: $('#table_add_modal-password').val(),
                      districtID: $('#table_add_modal-districtID').val(),
                  }, 
                  success: function (data) {
                      console.log(data.message);
                      $('#table_add_modal').modal('hide');
                      $table.bootstrapTable('refresh');
                  }
              })
          })
      })