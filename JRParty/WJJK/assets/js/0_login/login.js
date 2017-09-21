$(function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";

    $('#submit').click(function () {
        window.location.href = '../2_map/map.html'
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
                if (data.accountLoginResult.success == true) {
                    //写cookie
                    $.cookie("JTZH_userID", data.accountLoginResult.userID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtID", data.accountLoginResult.districtID, { expires: 3, path: '/' });
                    $.cookie("JTZH_districtName", data.accountLoginResult.districtName, { expires: 3, path: '/' });
                   
                    //跳转 
                    if (data.accountLoginResult.districtID.length == 2) {
                        window.location.href = '../1_index/index_C.html'
                    } else if (data.accountLoginResult.districtID.length == 4) {
                        window.location.href = '../1_index/index_Z.html'
                    }else{
                        window.location.href = '../1_index/index.html'
                    }
                } else {
                    alert(data.accountLoginResult.message)
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