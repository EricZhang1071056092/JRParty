$(function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    //var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    var svc_system = svcHeader + "/JRPartyService/System.svc";
    var svc_information = svcHeader + "/JRPartyService/Information.svc";
    $('#submit').click(function () {
        //var district = $('#district').val();
        var username = $('#username').val();
        var password = $('#password').val(); 
        $.ajax({
            url: svc_system + "/accountLogin",
            type: "GET",
            data: {
                username: username,
                password: password, 
            }, 
            dataType: "JSON",
            processData: true,
            success: function (data) { 
                if (data.success == true) {
                    //写cookie
                    $.cookie("JTZH_userID", data.userID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtID", data.districtID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtName", data.districtName, { expires: 3, path: '/' });                    
                    var str=''
                    for(var i in data.rows){ 
                     str=str+   '<tr><td>'+data.rows[i].title+'</td>'+
                                    '<td>'+data.rows[i].type+'</td>'+
                                    '<td>'+data.rows[i].time+'</td></tr>' 
                    } 
                    $.cookie("JTZH_expired", str, { expires: 3, path: '/' });
                    //公告推送检测
                    $.ajax({
                        url:svc_information + '/infPushCheck',
                        type:'GET',
                        data:{districtId:data.districtID},
                        dataType: "JSON",
                        processData: true,
                        cache:false,
                        // async:false,
                        success:function(item){
                            // var item=eval("("+`+")");
                            //console.log(item);
                            if(item.success){
                                var str='';
                                for(var i in item.data){
                                    str += '<tr><td>'+item.data[i].districtName+'</td>'+
                                        '<td>'+item.data[i].title+'</td>'+
                                        '<td>'+item.data[i].creatTime+'</td></tr>'
                                }
                                $.cookie("JTZH_push", str, { expires: 3, path: '/' });
                            }
                            //跳转
                            if (data.districtID.length == 2) {
                                window.location.href = '../1_index/index_C.html'
                            } else if (data.districtID.length == 4) {
                                window.location.href = '../1_index/index_Z.html'
                            }else{
                                window.location.href = '../1_index/index.html'
                            }
                        }
                    });
                } else {
                    alert(data.message)
                }
            },
            error: function () {

            }
        })
    })
    //回车登录
    window.onload = function () {
        document.getElementById('password').onkeydown = function (event) {
            event = event || window.event;
            if (event.keyCode == 13) {
                $('#submit').click();
            }
        };
    }
})