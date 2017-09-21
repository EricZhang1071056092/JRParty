/**
 * Created by ping on 2017/1/4.
 */
var Fselectid=1;
var Sselectid=1;
var Tselectid=0;
var preid=0;
var Spreid=0;
var index=0;
var showIndex=0;
$('#f'+Fselectid).css('backgroundColor','#FF9900');
document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;

    switch (keyCode){
        case KEY.DOWN:
            changeDownColor(Fselectid);
            break;
        case KEY.UP:
            changeUpColor(Fselectid);
            break;
        case KEY.LEFT:
            if(index>0){
              index--;
            if(index==0){
                $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"transparent");
                Sselectid=1;
            }
            if(index==1){
                $('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid).css('backgroundColor',"transparent");
                $('#third'+showIndex).slideToggle();
                Tselectid=0;
            }

            }
            break;
        case KEY.RIGHT:
        case KEY.ENTER:
                if(index<3) {
                    index++;
                if(index==1){
                    $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");
                }
                if(index==2){
                    if(Sselectid==1) {
                        $('#third' + Sselectid).slideToggle();
                        showIndex=Sselectid;
                    }else if(Sselectid==2) {
                        $("#third" + Sselectid).slideToggle();
                        showIndex=Sselectid;
                    }else if(Sselectid==3){
                        $("#third" + Sselectid).slideToggle();
                        showIndex=Sselectid;
                    }else if(Sselectid==4){
                        $("#third" + Sselectid).slideToggle();
                        showIndex=Sselectid;
                    }
                }else if(index==3){
                    setCookie();
                }
                }
            break
    }
}
function changeDownColor(id) {
    switch (index){
        case 0:
            if(Fselectid<3){
                preid=Fselectid;
                Fselectid++;
                 $('#f'+preid).css('backgroundColor','transparent');
                $('#f'+Fselectid).css('backgroundColor','#FF9900');
            }
            break;
        case 1:
            if(Sselectid<4) {
                Spreid = Sselectid;
                Sselectid++;
                $('#f'+Fselectid+'s'+Spreid).css('backgroundColor',"transparent");
                $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");

            }
            break
            case 2 :
                if(Tselectid<3) {
                    Tpreid = Tselectid;
                    Tselectid++;
                    $('#f'+Fselectid+'s'+Sselectid+'t'+Tpreid).css('backgroundColor',"transparent");
                    $('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid).css('backgroundColor',"#FF9900");
                }
                break;

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

            }
            break;
        case 1:
            if(Sselectid>1) {
                Spreid = Sselectid;
                Sselectid--;
                $('#f'+Fselectid+'s'+Spreid).css('backgroundColor',"transparent");
                $('#f'+Fselectid+'s'+Sselectid).css('backgroundColor',"#FF9900");

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
function setCookie() {
 var number = getParam('number');
        console.log(number);
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
   var title=$('#f'+Fselectid+'s'+Sselectid)[0].innerHTML;
   var content=$('#f'+Fselectid+'s'+Sselectid+'t'+Tselectid)[0].innerHTML;
    var str=(title+'.'+content);
    document.cookie="title="+ str;
     window.location.href = "content.html?number=" + number;
}

