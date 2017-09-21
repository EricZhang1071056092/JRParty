$(function () {
    var id = getParam('id'); 
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
      : "")
      + window.location.host;
    var svc_team = svcHeader + "/JRPartyService/Team.svc";
    var svc_file = svcHeader + "/JRPartyService/Upload/Activity/";
    $.ajax({
        type: "GET",
        url: svc_team + "/getVolunteerActivityDetail",
        data: {
            id: id
        },
        success: function (data) {
            $('#releaseTime').html(data.data.releaseTime);
            //$('#districtName').html(data.data.districtName);
            $('#districtName').html(data.data.districtName);
            var strfile = '';
            for (var i in data.data.imageURL) {
                strfile = strfile + ' <a id="' + data.data.imageURL[i].id + '" class="activity_image" href="' + svc_file + data.data.imageURL[i].Url + '"><img alt="' + svc_file + data.data.imageURL[i].Url + '" src="' + svc_file + data.data.imageURL[i].Url + '" width="110" height="62"/></a> '
            }
            $('#content_img').append(strfile);
            $(".activity_image").fancybox();
        }
    });
   

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

})