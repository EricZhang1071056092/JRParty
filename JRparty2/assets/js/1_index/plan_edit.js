$(function () {
    accountCheck();
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
		: "")
		+ window.location.host;
   var svc_sys = svcHeader + "/JRPartyService/Party.svc";
  //  var svc_sys = "http://172.16.0.221:8006/JRPartyService/Party.svc/";
    $(".form_datetime").datetimepicker({
        format: 'yyyy-mm-dd',
        language: 'zh-CN',
        autoclose: 1,
        startView: 3,
        minView: 2,
        maxView: 2,
        forceParse: true
    });
    //拿到ID
    var id = getParam('id'); 
    $.ajax({
        url: svc_sys + "/getActivityDetail",
        type: "get",
        data: {
            id: id,
        },
        success: function (data) {
            console.log(data.data);
            $("#id").val(data.data.id);
            $("#title").val(data.data.title);
            $("#month").val(data.data.month);
            $("#content").val(data.data.content);
            $("#districtID").val($.cookie("JTZH_districtID"));
            $("#type").val(data.data.type);
            var str=''
            for (var i in data.data.file) {
                str = str + '<div  id="' + data.data.file[i].id + '"><a style="line-height:2.5" href="' + 'http://172.16.0.221:8006/JRPartyService/Upload/Activity/' + data.data.file[i].Url + '"target="_blank"><img src="../assets/img/file.png" />' + data.data.file[i].Url + '</a>' +
                '&nbsp;&nbsp;&nbsp;<a href="#"><img src="../assets/img/1_index/remove.png"onclick="delet(\'' + data.data.file[i].id + '\')"width="20"  /></a><div>'
            }
            $("#lastFile").append(str);
        }
    })
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
})
function doUpload() {
    var formData = new FormData($("#form")[0]); 
    $.ajax({
        url: svcHeader +'/JRPartyService/Data/editActivity.ashx?',
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
function delet(id) {
    console.log(id);
    $('#' + id).remove();
    $.ajax({
        url: 'http://172.16.0.221:8006/JRPartyService/Party.svc/deleteActivityFile',
        type: 'GET',
        data: {
        id:id
        } ,        
        success: function (returndata) {
            alert(returndata);
        },
        error: function (returndata) {
            alert(returndata);
        }
    });
}