//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
        accountCheck();
        if ($.cookie('JTZH_districtID').length == 2) {
            loadCommonModule_CS('C', 'plan');
        } else if ($.cookie('JTZH_districtID').length == 4) {
            loadCommonModule_ZS('C', 'plan');
        } else {
            loadCommonModule_S('C', 'plan');
        } 
        //新增
        $('#table-add').click(function () {

            $('#table_add_modal .modal-title').html('新增用户');
            var str = '<div class="form-group">' +
                '<label for="table_add_modal-name">姓名</label>' +
                '<input type="text" class="form-control" id="table_add_modal-name" placeholder="请填写联系人姓名">' +
              '</div>' +
                '<div class="form-group">' +
                '<label for="table_add_modal-userName">联系方式</label>' +
                '<input type="text" class="form-control" id="table_add_modal-phone" placeholder="请填写联系方式">' +
              '</div>' +
                '<div class="form-group">' +
                '<label for="table_add_modal-role">部门</label>' +
                '<select class="form-control"title="请选择联系人部门" id="table_add_modal-department">';
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
        })
        $('#table_add_modal').find('.btn-success').click(function () {
            $.ajax({
                url: SVC_System + "/addAddressList",
                type: "get",
                data: {
                    name: $('#table_add_modal-name').val(),
                    phone: $('#table_add_modal-phone').val(),
                    department: $('#table_add_modal-department').val(),

                },
                success: function (data) {
                    alert(data.message);
                }
            })
        })
    })