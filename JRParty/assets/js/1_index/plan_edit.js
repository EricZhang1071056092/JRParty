$(function () {
    accountCheck();
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
		: "")
		+ window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    //拿到ID
    var id = getParam('id');
    var title = getParam('title');
    var context = getParam('context').split("@@@");
    $("#title").val(title);
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
    var i = 1;
    $.ajax({
        type: "GET",
        url: svc_sys + "/getPlanType",
        success: function (data) {
            console.log(data.rows);
            var str = ''
            for (var s in data.rows) { 
                str = str + '<option value="' + data.rows[s].title + '">' + data.rows[s].title + '</option>'
            } 
            $('#type1').html(str); 
            for (var j = 2; j <= context.length; j++) {
                $("#plan_content").append(
                     '<form class="form-inline"id="'+'form'+j+'" style="margin-top:10px">' +
                            '<div class="form-group">' +
                                '<label for="disabledSelect">任务类型：&nbsp;</label>' +
                                 '<input type="text" style="width:134px" class="form-control" id="' + 'type' + j + '" placeholder="任务类型">' +
                                //'<select id="' + 'type' + j + '" class="form-control" title="任务类型"> ' + str +
                                //'</select>' +
                            '</div> ' +
                            '<div class="form-group">' +
                                '<label for="content">内容：&nbsp;</label>' +
                                '<input type="text"style="width:464px" class="form-control" id="' + 'context' + j + '" >' +
                            '</div>' +
                            '<div class="form-group"style="margin-left:2px">' +
                                '<label  >月份：</label>' +
                               ' <input size="10" type="text" readonly class="form_datetime form-control" id="' + 'month' + j + '">' +
                                '&nbsp;<a href="#"class="srcId"><img src="../assets/img/1_index/remove.png"onclick="populationDetailOpen(\'' +'form' + j + '\')" /></a>' +
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
            }

            for (var i = 0 in context) {
                if (i < context.length) {
                    var k = parseInt(parseInt(i) + parseInt("1"))
                    $('#type' + k).val(context[i].split("@@")[0]);
                    // $('#type' + k).val("知委会");
                    $('#month' + k).val(context[i].split("@@")[2]);
                    $('#context' + k).val(context[i].split("@@")[1]);
                  // console.log(context[i].split("@@")[0], context[i].split("@@")[1], context[i].split("@@")[2]);
                }
            } 
            $(".form_datetime").datetimepicker({
                format: 'yyyy-mm-dd',
                language: 'zh-CN',
                autoclose: 1,
                startView: 3,
                minView: 2,
                maxView: 2,
                forceParse: true
            });
            var i = context.length;
            $("#number").html(i);
            console.log(i);
            $(".addbtn").click(function () { 
                i++; 
                $("#number").html(i);
                console.log( $("#number").html());
                $("#plan_content").append(
                     '<form class="form-inline"id="' + 'form' + i + '" style="margin-top:10px">' +
                            '<div class="form-group">' +
                                '<label for="disabledSelect">任务类型：&nbsp;</label>' +
                                '<input type="text" style="width:134px" class="form-control" id="' + 'type' + i + '" placeholder="任务类型">' +
                                //'<select id="' + 'type' + i + '" class="form-control" title="任务类型"> ' +str+ 
                                //'</select>' +
                           '</div> ' +
                            '<div class="form-group">' +
                                '<label for="content">内容：&nbsp;</label>' +
                                '<input type="text"style="width:464px" class="form-control" id="' + 'context' + i + '"  >' +
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
                $('.item-input').each(function (i) {
                    console.log($(this).find('img:first').attr('id', 'zoom' + i));
                    console.log($(this).find('img:first').next().attr('id', 'add' + i));
                }) 
                
            });
            $('#submit').click(function () { 
                var m = '';
                for (var j = 0 ; j < parseInt($("#number").html()) ; j++) { 
                    var k = parseInt(parseInt(j) + parseInt("1"))  
                    if (1<k <= $('form').size()&&m.length>0 && $('#type' + k).val() != '' && $('#context' + k).val() != '' && $('#month' + k).val() != '' && typeof ($('#type' + k).val()) != "undefined" && typeof ($('#context' + k).val()) != "undefined" && typeof ($('#month' + k).val()) != "undefined" && $('#type' + k).val() != "undefined" && $('#context' + k).val() != "undefined" && $('#month' + k).val() != "undefined") {
                        console.log($('#type' + k).val(), $('#context' + k).val(), $('#month' + k).val(),m.length);
                        m = m + '@@@' + $('#type' + k).val() + '@@' + $('#context' + k).val() + '@@' + $('#month' + k).val();
                       
                    } else if ($('#type' + k).val() != '' && $('#context' + k).val() != '' && $('#month' + k).val() != '' && typeof ($('#type' + k).val()) != "undefined" && typeof ($('#context' + k).val()) != "undefined" && typeof ($('#month' + k).val()) != "undefined" && $('#type' + k).val() != "undefined" && $('#context' + k).val() != "undefined" && $('#month' + k).val() != "undefined") {
                        console.log($('#type' + k).val(), $('#context' + k).val(), $('#month' + k).val());
                        m = m + $('#type' + k).val() + '@@' + $('#context' + k).val() + '@@' + $('#month' + k).val() ;
                    }
                } 
                $.ajax({
                    type: "POST",
                    url: svc_sys + "/editPlan",
                    contentType: "application/json", 
                    data: '{"title":"' + $("#title").val() +
                         '","content":"' + m +
                      '","id":"' + id + '"}',
                    dataType: "JSON",
                    processData: true,
                    success: function (data) {
                        console.log(data);
                        $('#common-alert .modal-title').html('');
                        $('#common-alert .modal-title').html('提示');
                        $('#common-alert .modal-body').html('');
                        $('#common-alert .modal-body').html('您已成功编辑信息！请主动关闭本页面,或者等待3秒后自动跳转。。。');
                        $('#common-alert').modal();
                        setTimeout(function () {
                            window.location.href = "index_C.html";
                        }, 1500);
                    }
                })
            })
          

        }
    })
   
}) 
//删除函数
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