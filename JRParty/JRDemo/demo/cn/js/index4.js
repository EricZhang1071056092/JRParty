/**
 * Created by ping on 2017/1/4.
 */
var Fselectid=1;
var Sselectid=1;
var Tselectid=0;
var preid=1;
var Spreid=0;
var index=0;
var showIndex=1;
 var number;
$('#f'+Fselectid).html('<img  src="img/sanhuiyike_z.png">');
$('#td'+Fselectid).css('color','#fff100');
document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;

    switch (keyCode){
        case KEY.DOWN:
           // changeDownColor(Fselectid);
            if(index<2) {
                index++;
                if(index==1){
                    if(Fselectid==1){
                        if(showIndex!=Fselectid) {
                            $('#center' + Fselectid).slideToggle();
                            $('#center' + showIndex).slideToggle();
                            showIndex = Fselectid;
                        }
                    }else if(Fselectid==2){
                        if(showIndex!=Fselectid){
                        $('#center'+Fselectid).slideToggle();
                        $('#center'+showIndex).slideToggle();
                            showIndex=Fselectid;
                        }
                    }else if(Fselectid==3){
                        if(showIndex!=Fselectid){
                        $('#center'+Fselectid).slideToggle();
                        $('#center'+showIndex).slideToggle();
                            showIndex=Fselectid;
                        }
                    }else if(Fselectid==4){
                        if(showIndex!=Fselectid){
                            $('#center'+Fselectid).slideToggle();
                            $('#center'+showIndex).slideToggle();
                            showIndex=Fselectid;
                        }

                    }
                    $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/dangydh_b.png">');
                }
                // if(index==2){
                //     if(Sselectid==1) {
                //         $('#third' + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==2) {
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==3){
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==4){
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }
                // }else if(index==3){
                //     setCookie();
                // }
            }
            break;
        case KEY.UP:
            //changeUpColor(Fselectid);
            if(index>0){
                  index--;
                if(index==0){
                    $('#f'+Fselectid).html('<img  src="img/sanhuiyike_z.png">');
                     if(Sselectid==1){
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/dydh.png">');
                    }else if(Sselectid==2){
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zzshh.png">')
                    }else if(Sselectid==3){
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zwh.png">')
                    }else if(Sselectid==4){
                     $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/ztdk.png">')
                    }
                    Sselectid=1;
                }
            }
            break;
        case KEY.LEFT:
            // if(index>0){

            // if(index==1){
            //     $('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid).css('backgroundColor',"transparent");
            //     $('#third'+showIndex).slideToggle();
            //     Tselectid=0;
            // }
            // }
            changeUpColor(Fselectid);
            break;
        case KEY.RIGHT:
            changeDownColor(Fselectid);
            break;
        case KEY.ENTER:
            if(index==1){
            switch (Sselectid){
                case 1:
                    setCookie('两会一课','党员大会');
                    break;
                case 2:
                    setCookie('两会一课','组织生活会');
                    break;
                case 3:
                    setCookie('两会一课','知委会');
                    break;
                case 4:
                    setCookie('两会一课','专题党课');
                    break;
            }

            }
                // if(index<3) {
                //     index++;
                // if(index==1){
                //     $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");
                // }
                // if(index==2){
                //     if(Sselectid==1) {
                //         $('#third' + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==2) {
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==3){
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }else if(Sselectid==4){
                //         $("#third" + Sselectid).slideToggle();
                //         showIndex=Sselectid;
                //     }
                // }else if(index==3){
                //     setCookie();
                // }
                // }
            break
    }
}
function changeDownColor(id) {
    switch (index){
        case 0:
            if(Fselectid<1){
                 preid=Fselectid;
                 Fselectid++;
                 $('#f'+preid).css('backgroundColor','transparent');
                 $('#f'+Fselectid).css('backgroundColor','#FF9900');
                 $('#td'+Fselectid).css('color','#ffff00');
                 $('#td'+preid).css('color','#ffffff');
            }
            break;
        case 1:
            if(Sselectid<4) {
                Spreid = Sselectid;
                Sselectid++;
                // $('#f'+Fselectid+'s'+Spreid).css('backgroundColor',"transparent");
                // $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");
                if(Sselectid==1){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/dangydh_b.png">');
             //       $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zzshh.png">');
                }else if(Sselectid==2){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zzshh_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/dydh.png">');
                }else if(Sselectid==3){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zwh_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zzshh.png">');
                }else if(Sselectid==4){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/ztdk_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zwh.png">');
                }
            }
            break
            // case 2 :
            //     if(Tselectid<3) {
            //         Tpreid = Tselectid;
            //         Tselectid++;
            //         $('#f'+Fselectid+'s'+Sselectid+'t'+Tpreid).css('backgroundColor',"transparent");
            //         $('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid).css('backgroundColor',"#FF9900");
            //     }
            //     break;

    }

}
function changeUpColor(id) {
    switch(index){
        case 0:
            if(Fselectid>1) {
                preid =Fselectid;
                Fselectid--;
                $('#f'+preid).css('backgroundColor','transparent');
                $('#f'+Fselectid).css('backgroundColor','#FF9900');
                $('#td'+Fselectid).css('color','#ffff00');
                $('#td'+preid).css('color','#ffffff');

            }
            break;
        case 1:
            if(Sselectid>1) {
                Spreid = Sselectid;
                Sselectid--;
                // $('#f'+Fselectid+'s'+Spreid).css('backgroundColor',"transparent");
                // $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");
                if(Sselectid==1){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/dangydh_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zzshh.png">');
                }else if(Sselectid==2){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zzshh_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zwh.png">');
                }else if(Sselectid==3){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/zwh_b.png">');
                    $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/ztdk.png">');
                }else if(Sselectid==4){
                    // $('#f'+Fselectid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid).html('<img  src="img/ztdk_b.png">');
                    // $('#f'+Fselectid+'s'+Spreid).html('<img  src="img/zwh.png">');
                }
            }
            break;
        case 2:
            if(Tselectid>1) {
                Tpreid =Tselectid;
                Tselectid--;
                $('#f'+Fselectid+'s'+Sselectid+'t'+Tpreid).css('backgroundColor',"transparent");
                $('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid).css('backgroundColor',"#FF9900");
            }
            break;
    }

}
function setCookie(fTitle,sTitle) {
    // var title=$('#f'+Fselectid+'s'+Sselectid)[0].innerHTML;
    // var content=$('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid)[0].innerHTML;
    //  var str=(title+'.'+content);
    //  document.cookie="title="+ str;
    //  window.location.href="content.html";
    number = getParam('number');
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


    var str = (fTitle + '.' + sTitle);
    document.cookie = "title=" + str;
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
        + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    $.ajax({
        url: svc_sys + "/getInformation?",
        type: "GET",
        data: {
            number: number,
        },
        success: function (data) {
        //    window.location.href = "second.html?number=" + number + "&PartyBranch=" + data.data;
        }
    })
    window.location.href = "second.html?number=" + number + "&PartyBranch=" +'句容党支部';
}

