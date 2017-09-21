/**
 * Created by ping on 2017/1/4.
 */
$(function (){
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    var number = getParam('number');
    //console.log(number);
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

    $.ajax({
        url: svc_sys + "/getClass?",
        type: "GET",
        success: function (data) {
            var m = 1;
            if (data.success == true) {
                for (var i = 0 in data.data) {
                    if (i == 0) {
                        $("#left-ul").html('<li id="' + 'f' + m + '" style="height:42px;text-align: center;line-height: 42px;font-size: 22px; font-weight: 700; background:#FF9900">' + data.data[i].title + '</li>');
                        $.ajax({
                            url: svc_sys + "/getSubClass?",
                            type: "GET",
                            data: {
                                id: m
                            },
                            success: function (data2) {
                                //console.log(m);
                                var second = '<ul id="second-ul" class="right-ul"style="width:600px;padding-left: 116px;position: absolute;z-index: 1">';
                                for (var j = 0 in data2.data) {
                                    //console.log(data2.data[j].title);
                                    var n = parseInt(parseInt(j) + parseInt(1));
                                    second = second + '<li id="' + 'f1' + 's' + n + '">' + data2.data[j].title + '</li>' +
                                        '<li>' +
                                          ' <div id="' + 'third' + n + '" class="ulli" style="color:white; display:none;background-color:darkslateblue; position: relative; z-index: 2; text-align: center;">' +
                                        '</div>' +

                                       '</li>'
                                }
                                $("#right").html(second);
                            }
                        })
                    } else {
                        $("#f1").html('<li id="' + 'f' + m + '">' + data.data[i].title + '</li>');
                    }
                }
                m++;
            } else {
                //alert(data.accountLoginResult.message)
            }
        },
        error: function () {

        }
    })


    var Fselectid = 1;
    var Sselectid = 1;
    var Tselectid = 0;
    var preid = 0;
    var Spreid = 0;
    var index = 0;
    var showIndex = 0;
    $('#f' + Fselectid).css('backgroundColor', '#FF9900');
    document.onirkeypress = keyDown;
    document.onkeydown = keyDown;
    function keyDown(evt) {
        evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
        var keyCode = evt.keyCode ? evt.keyCode : evt.which;

        switch (keyCode) {
            case KEY.DOWN:
                changeDownColor(Fselectid);
                return false;
                break;
            case KEY.UP:
                changeUpColor(Fselectid);
                return false;
                break;
            case KEY.LEFT:
                if (index > 0) {
                    index--;
                    if (index == 0) {
                        $('#f' + Fselectid + 's' + Sselectid).css('backgroundColor', "transparent");
                        Sselectid = 1;
                    }
                    if (index == 1) {
                        $('#f' + Fselectid + 's' + Sselectid + 't' + Tselectid).css('backgroundColor', "transparent");
                        $('#third' + showIndex).slideToggle();
                        Tselectid = 0;
                    }

                }
                return false;
                break;
            case KEY.RIGHT:
            case KEY.ENTER:
                if (index < 3) {
                    if (index == 0) {
                        $('#f' + Fselectid + 's' + Sselectid).css('backgroundColor', "#FF9900");
                    }
                    if (index == 1) {
                        if (Sselectid == 1) {
                            $.ajax({
                                url: svc_sys + "/getClassContext?",
                                type: "GET",
                                data: {
                                    id: 1,
                                },
                                success: function (data3) {
                                    var third = '<ul class="lili">'
                                    for (var k = 0 in data3.data) {
                                     //   console.log(data3.data[k].context);
                                        var s = parseInt(parseInt(k) + parseInt(1));
                                        third = third + '<li id="' + 'f1s' + 1 + 't' + s + '" >' + data3.data[k].context + '</li>'

                                    }
                                    third = third + '</ul> '
                                   // console.log(third);
                                    $('#third' + Sselectid).html(third);
                                    //$(this).prop("disabled", true);
                                    $('#third' + Sselectid).slideToggle();

                                }
                            })
                            // $('#third' + Sselectid).slideToggle();
                            showIndex = Sselectid;
                        } else if (Sselectid == 2) {
                            $.ajax({
                                url: svc_sys + "/getClassContext?",
                                type: "GET",
                                data: {
                                    id: Sselectid,
                                },
                                success: function (data3) {
                                    var third = '<ul class="lili">'
                                    for (var k = 0 in data3.data) { 
                                       // console.log(data3.data[k].context);
                                        var s = parseInt(parseInt(k) + parseInt(1));
                                        third = third + '<li id="' + 'f1s' + 2 + 't' + s + '" >' + data3.data[k].context + '</li>'

                                    }
                                    third = third + '</ul> '
                                    //console.log(third);
                                    $('#third' + Sselectid).html(third);
                                    $('#third' + Sselectid).slideToggle();
                                }
                            })
                            // $("#third" + Sselectid).slideToggle();
                            showIndex = Sselectid;
                        } else if (Sselectid == 3) {
                            $.ajax({
                                url: svc_sys + "/getClassContext?",
                                type: "GET",
                                data: {
                                    id: Sselectid,
                                },
                                success: function (data3) {
                                    var third = '<ul class="lili">'
                                    for (var k = 0 in data3.data) {
                                        //console.log(data3.data[k].context);
                                        var s = parseInt(parseInt(k) + parseInt(1));
                                        third = third + '<li id="' + 'f1s' + Sselectid + 't' + s + '" >' + data3.data[k].context + '</li>'
                                    }
                                    third = third + '</ul> '
                                    //console.log(third);
                                    $('#third' + Sselectid).html(third);
                                    $('#third' + Sselectid).slideToggle();
                                }
                            }) 
                            showIndex = Sselectid;
                        } else if (Sselectid == 4) {
                            $.ajax({
                                url: svc_sys + "/getClassContext?",
                                type: "GET",
                                data: {
                                    id: Sselectid,
                                },
                                success: function (data3) {
                                    var third = '<ul class="lili">'
                                    for (var k = 0 in data3.data) {
                                        //console.log(data3.data[k].context);
                                        var s = parseInt(parseInt(k) + parseInt(1));
                                        third = third + '<li id="' + 'f1s' + Sselectid + 't' + s + '" >' + data3.data[k].context + '</li>'
                                    }
                                    third = third + '</ul> '
                                    //console.log(third);
                                    $('#third' + Sselectid).html(third);
                                    $('#third' + Sselectid).slideToggle();
                                }
                            })
                            showIndex = Sselectid;
                        }
                    } else if (index == 2) { 
                        setCookie();
                    }
                    index++;
                }
                return false;
                break;
        }
    }
    function changeDownColor(id) {
        switch (index) {
            case 0:
                if (Fselectid < 3) {
                    preid = Fselectid;
                    Fselectid++;
                    $('#f' + preid).css('backgroundColor', 'transparent');
                    $('#f' + Fselectid).css('backgroundColor', '#FF9900');
                }
                break;
            case 1:
                if (Sselectid < 4) {
                    Spreid = Sselectid;
                    Sselectid++;
                    $('#f' + Fselectid + 's' + Spreid).css('backgroundColor', "transparent");
                    $('#f' + Fselectid + 's' + Sselectid).css('backgroundColor', "#FF9900");

                }
                break
            case 2:
                if (Tselectid < 3) {
                    Tpreid = Tselectid;
                    Tselectid++;
                    $('#f' + Fselectid + 's' + Sselectid + 't' + Tpreid).css('backgroundColor', "transparent");
                    $('#f' + Fselectid + 's' + Sselectid + 't' + Tselectid).css('backgroundColor', "#FF9900");
                }
                break;

        }

    }
    function changeUpColor(id) {
        switch (index) {
            case 0:
                if (Fselectid > 1) {
                    preid = Fselectid;
                    Fselectid--;
                    $('#f' + preid).css('backgroundColor', 'transparent');
                    $('#f' + Fselectid).css('backgroundColor', '#FF9900');

                }
                break;
            case 1:
                if (Sselectid > 1) {
                    Spreid = Sselectid;
                    Sselectid--;
                    $('#f' + Fselectid + 's' + Spreid).css('backgroundColor', "transparent");
                    $('#f' + Fselectid + 's' + Sselectid).css('backgroundColor', "#FF9900");

                }
                break;
            case 2:
                if (Tselectid > 1) {
                    Tpreid = Tselectid;
                    Tselectid--;
                    $('#f' + Fselectid + 's' + Sselectid + 't' + Tpreid).css('backgroundColor', "transparent");
                    $('#f' + Fselectid + 's' + Sselectid + 't' + Tselectid).css('backgroundColor', "#FF9900");
                }
                break;
        }

    }
    function setCookie() {
var number = getParam('number');
        //console.log(number);
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

        var title = $('#f' + Fselectid + 's' + Sselectid)[0].innerHTML;
        var content = $('#f' + Fselectid + 's' + Sselectid + 't' + Tselectid)[0].innerHTML;
        var str = (title + '.' + content);
        document.cookie = "title=" + str;
 
$.ajax({
        url: svc_sys + "/getInformation?",
        type: "GET",
        data:{
            number:number,
        },
        success: function (data) { 
 window.location.href = "content.html?number=" + number+"&PartyBranch="+data.data;
            
        }
    })
       
    }



});
