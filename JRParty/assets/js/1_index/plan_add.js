$(function () {
    accountCheck();
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
		: "")
		+ window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc"; 
    $(".form_datetime").datetimepicker({
        format: 'yyyy-mm-dd', 
        language: 'zh-CN',  
        autoclose: 1, 
        startView: 3,
        minView:2,
        maxView:2,
        forceParse:true
    }); 
    var i = 1
    $.ajax({
        type: "GET",
        url: svc_sys + "/getPlanType",
        success: function (data) {
            console.log(data.rows);
            var str=''
            for (var s in data.rows) { 
                str = str + '<option value="' + data.rows[s].title + '">'+data.rows[s].title+'</option>'
            }
            console.log(str);
            $('#type1').html(str);
            $(".addbtn").click(function () {
                i++;
                $("#plan_content").append(
                     '<form class="form-inline"id="' + 'form' + i + '" style="margin-top:10px">' +
                            '<div class="form-group">' +
                                '<label for="disabledSelect">任务类型：&nbsp;</label>' +
                                 '<input type="text" style="width:134px" class="form-control" id="'+ 'type' + i +'" placeholder="任务类型">'+
                                //'<select id="' + 'type' + i + '" class="form-control" title="任务类型"> ' + str+ 
                                //'</select>' +
                           '</div> ' +
                            '<div class="form-group">' +
                                '<label for="content">内容：&nbsp;</label>' +
                                '<input type="text"style="width:464px" class="form-control" id="' + 'context' + i + '" placeholder="内容">' +
                            '</div>' +
                            '<div class="form-group"style="margin-left:2px">' +
                                '<label  >月份：</label>' +
                               ' <input size="10" type="text" readonly class="form_datetime form-control" id="' + 'month' + i + '">' +
                                '&nbsp;<a href="#"><img src="../assets/img/1_index/remove.png" onclick="populationDetailOpen(\'' + 'form' + i + '\')" /></a>' +
                          ' </div>' +
                        '</form>'); 
                $(".form_datetime").datetimepicker({
                    format: 'yyyy-mm-dd',
                    language: 'zh-CN',
                    autoclose: 1,
                    startView: 3,
                    minView: 2,
                    maxView: 2,
                    forceParse: true
                });
                //初始化UEditor
                //UE.getEditor('mainText'+i);
                $('.item-input').each(function (i) {
                    console.log($(this).find('img:first').attr('id', 'zoom' + i));
                    console.log($(this).find('img:first').next().attr('id', 'add' + i));
                })

            });
        }
    }) 
    $('#submit').click(function () {
        console.log(i); 
        var m='' ;
        for (var j = 0 ; j < i; j++) {
            var k = parseInt(parseInt(j) + parseInt("1"))
            if (1 < k <= $('form').size() && m.length > 0 && $('#type' + k).val() != '' && $('#context' + k).val() != '' && $('#month' + k).val() != '' && typeof ($('#type' + k).val()) != "undefined" && typeof ($('#context' + k).val()) != "undefined" && typeof ($('#month' + k).val()) != "undefined" && $('#type' + k).val() != "undefined" && $('#context' + k).val() != "undefined" && $('#month' + k).val() != "undefined") {
                console.log($('#type' + k).val(), $('#context' + k).val(), $('#month' + k).val(), m.length);
                m = m + '@@@' + $('#type' + k).val() + '@@' + $('#context' + k).val() + '@@' + $('#month' + k).val();

            } else if ($('#type' + k).val() != '' && $('#context' + k).val() != '' && $('#month' + k).val() != '' && typeof ($('#type' + k).val()) != "undefined" && typeof ($('#context' + k).val()) != "undefined" && typeof ($('#month' + k).val()) != "undefined" && $('#type' + k).val() != "undefined" && $('#context' + k).val() != "undefined" && $('#month' + k).val() != "undefined") {
                console.log($('#type' + k).val(), $('#context' + k).val(), $('#month' + k).val());
                m = m + $('#type' + k).val() + '@@' + $('#context' + k).val() + '@@' + $('#month' + k).val();
            }
        }
        console.log(m);
         
        $.ajax({
            type: "POST",
           // url: svc_sys + "/addPlan",
           // data: {
           //     title: $("#title").val(), 
            //    content: m, 
           // },
            url: svc_sys + "/addPlan",
            contentType: "application/json",
            data: '{"title":"' + $("#title").val() +
                  '","content":"' + m + '"}', 
            dataType: "JSON",
            processData: true,
            success: function (data) {
                console.log(data);
                $('#common-alert .modal-title').html('');
                $('#common-alert .modal-title').html('提示');
                $('#common-alert .modal-body').html('');
                $('#common-alert .modal-body').html('您已成功发布信息！请主动关闭本页面,或者等待3秒后自动跳转。。。');
                $('#common-alert').modal();
                 setTimeout(function () {
                   window.location.href = "index_C.html";
                 }, 1500);
            }
        }) 
    })
})
var logout = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    $.cookie("JTZH_districtID", null, { path: '/' })
    window.location.href = "../login.html";
}
function populationDetailOpen(id) {
    console.log(id);
    //前台删除                    
    $("#" + id + "").remove();
    //  后台删除
    //$.ajax({
    //    type: "GET",
    //    url: svc_bus + "/deleteemPicture",
    //    data: {
    //        id: id
    //    },
    //    success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
    //        console.log(data);
    //        $table.bootstrapTable('refresh');
    //    },
    //    error: function (data) {
    //        console.log(data);
    //    }
    //})

}