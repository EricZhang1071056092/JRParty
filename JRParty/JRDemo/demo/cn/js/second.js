/**
 * Created by Administrator on 2017/6/6.
 */
var secondPosition=1;
var secondPreposition=0;
$('#s'+secondPosition).css('color','#ffffff');
$('#s'+secondPosition).css('backgroundColor','#004e92');
var PartyBranch=getParam('PartyBranch');
var number = getParam('number');
var size;
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
$(function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
        + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    var number = getParam('number');
    var content = getParam('content');

	$('#text')[0].innerText=content;
    console.log(number, content);
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
    if (content == '党员大会') {
        var Sselectid=1
    } else if (content == '组织生活会') {
        var Sselectid = 2
    } else if (content == '支委会') {
        var Sselectid = 3
    } else if (content == '专题党课') {
        var Sselectid = 4
    }
    $('#title').html(content);
    
    console.log(Sselectid);
    $.ajax({
        url: svc_sys + "/getClassContext?",
        type: "GET",
        data: {
            id: content,
        },
        success: function (data3) {
			size=data3.data.length;
            var third = '<ul>'
            for (var k = 0 in data3.data) {
            
                var s = parseInt(parseInt(k) + parseInt(1));
		
            //    third = third + '<li id="' + 's' + s + '" >' + data3.data[k].context +'<span style="display:none">'+','+data3.data[k].id+','+'</span></li>'
           third = third +"<li><img style='float: left;margin-right: 10px;position: absolute;top: 3px' src='img/dangbiao.png'><div  class='list' style='float: left' id='s"+s+"'>"+ data3.data[k].context+"<span style='display:none'>'+',"+data3.data[k].id+",'+'</span>"+"<span class='time'>"+data3.data[k].month+"</span></div></li>"
			} 
            third = third + '</ul> '
            console.log(third);
            $('#second-center').html(third);
            $('#s'+secondPosition).css('color','#ffffff');
            $('#s'+secondPosition).css('backgroundColor','#004e92');
            //$('#third' + Sselectid).slideToggle();
        }
    })
})
document.onirkeypress = keyDown;
document.onkeydown = keyDown;
    function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;

    switch (keyCode) {
        case KEY.DOWN:
		console.log(size);
            if(secondPosition<size){
            secondPreposition=secondPosition;
            secondPosition++;
            $('#s'+secondPosition).css('backgroundColor','#004e92');
            $('#s'+secondPreposition).css('backgroundColor','#ffffff');
            $('#s'+secondPosition).css('color','#ffffff');
            $('#s'+secondPreposition).css('color','#004e92');
    }
        break;
        case KEY.UP:
            if(secondPosition>1){
                secondPreposition=secondPosition;
                secondPosition--;
                $('#s'+secondPosition).css('backgroundColor','#004e92');
                $('#s'+secondPreposition).css('backgroundColor','#ffffff');
                $('#s'+secondPosition).css('color','#ffffff');
                $('#s'+secondPreposition).css('color','#004e92');
            }
            break;
        case KEY.LEFT:
            break;
        case KEY.RIGHT:
            break;
        case KEY.ENTER: 
            var content = $('#s' + secondPosition)[0].innerHTML;
			var arr=content.split(',');
			var arrStr=arr[0].split('<');
			
		  //  console.log(arr);
          //  alert(content);
		 // console.log(arrStr[0]);
		 // console.log(arr[1]);
           window.location.href = 'demo.html?number=' + number +'&PartyBranch='+PartyBranch+ '&StudyContent=' + arr[1]+'&content='+arrStr[0];
            break
        case KEY.EXIT:
            break
        case KEY.BACK:
            break
    }
return false;
}
