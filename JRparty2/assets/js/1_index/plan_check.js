$(function () {
    
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
    : "")
    + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    //拿到ID
    var id = getParam('id');
    var title = getParam('title');
    var context = getParam('context').split("@@@");
    $("#title").html(title);
   
    
    for (var i = 0 in context) {
        if (i < context.length) {
            var k = parseInt(parseInt(i) + parseInt("1"))
            $('#month' + k).val(context[i].split("@@")[0]);
            $('#mainText' + k).val(context[i].split("@@")[1]);
            var year = context[i].split("@@")[2]
         // console.log(year.split("-")[0] + '年' + year.split("-")[1] + '月' + year.split("-")[2] + '日');//
           
            $("#cd-timeline").append(
           '<div class="cd-timeline-block">' +
              '<div class="cd-timeline-img cd-picture">' +
                 '<img src="images/party.png" alt="Picture" />' +
              '</div>' +
              '<div class="cd-timeline-content">' +
                 '<h2>' + context[i].split("@@")[0] + '</h2>' +
                 '<p>' + context[i].split("@@")[1] + '</p>' +
                 //'<a href="#" class="cd-read-more">阅读更多</a>' +
                 '<span class="cd-date"style="font-size:20px; color:#ff6a00">' + year.split("-")[0] + '年' + year.split("-")[1] + '月' + year.split("-")[2] + '日' + '</span>' +
              '</div>  ' +
           '</div> ');
        }
    }   
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
var logout = function () {
    $.cookie("JTZH_userID", null, { path: "/" });
    $.cookie("JTZH_districtID", null, { path: '/' })
    window.location.href = "../login.html";
}