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
    $("#input-image-3").fileinput({
        language: 'zh',
        //uploadUrl: svc_uoload + "/ActivityUpload.ashx?activityID=" + row.id,
        allowedFileExtensions: ["jpg", "png", "doc", "xls", "xlsx", "docx","txt"],
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
    $("#districtID").val($.cookie("JTZH_districtID"))
    $('#submit').click(function () { 
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
                   window.location.href = "index_CC.html";
                 }, 1500);
            }
        }) 
    })
})
function doUpload() { 
    var formData = new FormData($("#form")[0]);
    console.log(formData);
    $.ajax({
        url:svcHeader + '/JRPartyService/Data/ActivityUpload.ashx?',
        type: 'POST',
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (returndata) {
            alert('发布成功！');
        },
        error: function (returndata) {
            console.log('发布失败');
        }
    });
}