$(function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";

    $('#submit').click(function () {
        //var district = $('#district').val();
        var username = $('#username').val();
        var password = $('#password').val();
        console.log(username, password);
        $.ajax({
            url: svc_sys + "/accountLogin", 
            type: "GET",
            data: {
                username: username,
                password: password, 
            }, 
            dataType: "JSON",
            processData: true,
            success: function (data) {
                console.log(data);
                if (data.success == true) {
                    //写cookie
                    $.cookie("JTZH_userID", data.userID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtID", data.districtID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtName", data.districtName, { expires: 3, path: '/' });
                    
                    var str=''
                    for(var i in data.rows){
                    console.log(data.rows[i]); 
                     str=str+   '<tr><td>'+data.rows[i].title+'</td>'+
                                    '<td>'+data.rows[i].type+'</td>'+
                                    '<td>'+data.rows[i].time+'</td></tr>' 
                     }
                    $.cookie("JTZH_expired", str, { expires: 3, path: '/' });
                    //跳转 
                    if (data.districtID.length == 2) {
                        window.location.href = '../1_index/index_C.html'
                    } else if (data.districtID.length == 4) {
                        window.location.href = '../1_index/index_Z.html'
                    }else{
                        window.location.href = '../1_index/index.html?type='+'1'
                    }
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