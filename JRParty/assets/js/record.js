/**
 * Created by ping on 2017/1/5.
 */
var  FOCUS="BUT_0";
var  LFOCUS="";
document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : ""); //兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;
    switch (keyCode){
        case KEY.ENTER:
 //           window.location.href="../index.html";
            break;
        case KEY.EXIT:
            break;
        case KEY.BACK:
            break;
    }}