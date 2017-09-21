/**
 * Created by ping on 2017/1/4.
 */
var  FOCUS="BUT_0";
var  LFOCUS="";
document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;
    switch (keyCode){
        case KEY.DOWN:
            switch (FOCUS){
                case  "BUT_0":
                    $("#StartRealPlay").hide();
                    $("#StartRealPlayFocus").show();
                    LFOCUS=FOCUS;
                    FOCUS="BUT_00";
                    break
            }
            break;
        case KEY.UP:

            break;
        case KEY.LEFT:
            switch (FOCUS){
                case "BUT_0":
                    $("#StartRealPlay").hide();
                    $("#StartRealPlayFocus").show();
                    FOCUS="BUT_00";
                    break;
                case "BUT_1":
                    $("#StartRealPlay").hide();
                    $("#StartRealPlayFocus").show();
                    $("#EndRealPlayFocus").hide();
                    $("#EndRealPlay").show();
                    FOCUS = "BUT_00";
                    break;
             
            }
            break;
        case KEY.RIGHT:
            switch (FOCUS){
                case "BUT_0":
                    $("#StartRealPlay").hide();
                    $("#StartRealPlayFocus").show();
                    FOCUS="BUT_00";
                    break;
                case "BUT_00":
                    $("#EndRealPlayFocus").show();
                    $("#EndRealPlay").hide();
                    $("#StartRealPlay").show();
                    $("#StartRealPlayFocus").hide();
                    FOCUS="BUT_1";
                    break;
             
            }
            break;
        case KEY.ENTER:
             switch (FOCUS){
                 case  "BUT_0":
                     $("#StartRealPlay").hide();
                     $("#StartRealPlayFocus").show();
                     FOCUS="BUT_00";
                     break;
                 case "BUT_00":
                     clickStartRealPlay();
                     break;
                 case "BUT_01":
                     clickStopRealPlay();
                     break;
                 
             }
            break
    }
}