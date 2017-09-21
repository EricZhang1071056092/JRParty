/**
 * Created by ping on 2017/1/4.
 */
var Fselectid = 1;  //上方列表选中项
var Sselectid = 1;  //下方列表选中项
var preFselectid = 0;  //上方列表上一次的选中项
var preSselectid = 0;  //下方列表上一次的选中项
var type = 0;     //判断是哪一级列表 0：上方；1：下方。
var showIndex = 0;
var topSize;        //上方列表的页下方数
var bottomSize;    //下方列表的页数
var showTop=1;      //当前显示上方的页码
var showBottom=1;     //当前显示下方的页码
var topLength;        //上方显示的数量
var bottomLength;     //下方显示的数量
var text;
$('#td'+Fselectid).css('color','#f6ff00');
var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
        + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    $.ajax({
        url: svc_sys + "/getClass?",
        type: "GET",
        data: {
            PageIndex: showTop,
        },
        success: function (data) {
			topSize=data.total;
			topLength=data.data.length;
         for(var i=0;i<data.data.length;i++){
			 $('#td'+(i+1)).html(data.data[i].title);
			 $('#in'+(i+1)).val(data.data[i].id);
			 	showIndex=$('#in1').val();
		 }
		 if(topSize>1){
	/**	 if(showTop!=topSize){
			 $('#top_right').css('display','block');
	         $('#top_left').css('display','block');
         } else if(showTop=TopSize){
			 $('#top_left').css('display','block');
			 $('#top_right').css('display','none');
		 } */
		  $('#top_right').css('display','block');
	   
		 }
	     
		 $.ajax({
        url: svc_sys + "/getClassContext?",
        type: "GET",
        data: {
			id: showIndex,
            PageIndex: showBottom,
        },
        success: function (data) {
			console.log(data);
			bottomSize=data.total;  
			bottomLength=data.data.length;
              for(var j=0;j<data.data.length;j++){
				  if(data.data[j].type.length<=7){
					   $('#input'+(j+1)).val(data.data[j].type);
					  $('#s'+(j+1)).html(data.data[j].type);
				  }else{
					   $('#input'+(j+1)).val(data.data[j].type);
					   $('#s'+(j+1)).html(data.data[j].type.substring(0,6)+'..');
				  }
			 
			 $('#bottomin'+(j+1)).val(data.data[j].id);
		 }   
		  if(bottomSize>1){
	       $('#bottom_right').css('display','block');
         }
        }
    })
        }
    })


document.onirkeypress = keyDown;
document.onkeydown = keyDown;
function keyDown(evt) {
    evt = (evt) ? evt : ((window.event) ? window.event : "");//兼容IE和Firefox获得keyBoardEvent对象
    var keyCode = evt.keyCode ? evt.keyCode : evt.which;
    switch (keyCode) {
        case KEY.DOWN:
            if (type == 0) {
                type = 1;
                Sselectid=1;
                $('#s'+Sselectid).css('color','#f6ff00');
				text=$('#input'+Sselectid).val();
				
				// $('#s'+Sselectid)[0].liMarquee();
		//		$('#s'+Sselectid).html('<marquee width=200px align="left"'+'scrollamount=3 id="text">'+text+'</marquee>');
            }
            break;
        case KEY.UP:
            if (type == 1) {
                type = 0;
                $('#s'+Sselectid).css('color','#ffffff');
            }
            break;
        case KEY.LEFT:
            if (type == 0) {
                if (Fselectid >= 1 && Fselectid <= topLength) {
                    if (Fselectid == 1) {					
						if(showTop>1){
		                    showTop--;
							showBottom=1;
							getTopData();
						}
                    } else {
                        preFselectid=Fselectid;
                        Fselectid--;
                        changeLeftColor();
						showBottom=1;
						getBottomData();
                    }
                }
            } else if (type == 1) {
                if (Sselectid >= 1 && Sselectid <= bottomLength) {
                    if (Sselectid == 1) {
						if(showBottom>1){
							showBottom--;
                             getData();
						}
                    } else {
                        preSselectid=Sselectid;
                        Sselectid--;
                        changeLeftColor();
                    }
                }
            }
            break;
        case KEY.RIGHT:
            if (type == 0) {
				if(Fselectid >= 1 && Fselectid < topLength){
					
               if (Fselectid == 5) {				 
				   if(showTop<topSize){   
					   showTop++;
					   showBottom=1;
					   getTopData();
				   }
                      
                } else {
                    preFselectid=Fselectid;
                    Fselectid++;
                    changeRightColor();
					showBottom=1;
					getBottomData();
                }
				}
            } else if (type == 1) {
				if(Sselectid >= 1 && Sselectid <= bottomLength){
                if (Sselectid == 4) {

					if(showBottom<bottomSize){
						showBottom++;
						getData();
					}
                } else {
                    preSselectid=Sselectid;
                    Sselectid++;
                    changeRightColor();
                }
            }
			}
            break;
        case KEY.ENTER:
            if(type==1){
                setCookie(Sselectid);
            }
            break
    }
	return false;
}
function getBottomData(){  //获取下方数据
	    
		for(var i=0;i<4;i++){
			 $('#bottomin'+(i+1)).val("");
			 $('#s'+(i+1))[0].innerHTML="";
			 $('#input'+(i+1)).val("");
		}
	    var id=$('#in'+Fselectid).val();
		
		 $.ajax({
        url: svc_sys + "/getClassContext?",
        type: "GET",
        data: {
			id: id,
            PageIndex: showBottom,
        },
        success: function (data) {
			 bottomLength=data.data.length;
              for(var j=0;j<data.data.length;j++){
			 if(data.data[j].type.length<=7){
					  $('#input'+(j+1)).val(data.data[j].type);
					  $('#s'+(j+1)).html(data.data[j].type);
				  }else{
					   $('#input'+(j+1)).val(data.data[j].type);
					   $('#s'+(j+1)).html(data.data[j].type.substring(0,6)+'..');
				  };
				 $('#bottomin'+(j+1)).val(data.data[j].id);  
		 }   
		  if(bottomSize>1){
	       $('#bottom_right').css('display','block');
         }
        }
    })
}
function getData() {  
      
   //移动左右获取下方数据
		for(var i=0;i<4;i++){
			 $('#s'+(i+1)).html("");
			 $('#bottomin'+(i+1)).val("");
		}
		var id=$('#in'+Fselectid).val();
		 $.ajax({
        url: svc_sys + "/getClassContext?",
        type: "GET",
        data: {
			id: id,
            PageIndex: showBottom,
        },
        success: function (data) {
		
			bottomSize=data.total;  
			bottomLength=data.data.length;
              for(var j=0;j<data.data.length;j++){
				  if(data.data[j].type.length<=7){
					   $('#input'+(j+1)).val(data.data[j].type);
					  $('#s'+(j+1)).html(data.data[j].type);
				  }else{
					   $('#input'+(j+1)).val(data.data[j].type);
					   $('#s'+(j+1)).html(data.data[j].type.substring(0,6)+'..');
				  }
				 
			 $('#bottomin'+(j+1)).val(data.data[j].id);
		 }   
		   if(showBottom<=bottomSize){			   
		 if(showBottom!=bottomSize){
			 if(showBottom==1){
			 $('#bottom_right').css('display','block');
	         $('#bottom_left').css('display','none');
			 }else{
		     $('#bottom_right').css('display','block');
	         $('#bottom_left').css('display','block'); 
			 }
         } else if(showBottom==bottomSize){
			 $('#bottom_left').css('display','block');
			 $('#bottom_right').css('display','none');
		 }
		   Sselectid=1;
           $('#s'+Sselectid).css('color','#f6ff00');
		   text=$('#input'+Sselectid).val();
		   $('#s'+4).css('color','#ffffff');
		//  $('#s'+Sselectid).html('<marquee width=200px align="left"'+'scrollamount=3 id="text">'+text+'</marquee>');
		}
	}		
    })
}
function changeLeftColor() {
    if(type==0){
        $('#td'+preFselectid).css('color','#ffffff');
        $('#td'+Fselectid).css('color','#f6ff00');
    }else if(type==1){
        $('#s'+preSselectid).css('color','#ffffff');
        $('#s'+Sselectid).css('color','#f6ff00');
		
		//text=$('#s'+Sselectid)[0].innerHTML;
		if($('#input'+preSselectid).val().length>=7){
			var ss=$('#input'+preSselectid).val().substring(0,6);
			$('#s'+preSselectid).html(ss+'...');
		}else{
			$('#s'+preSselectid).html($('#input'+preSselectid).val());
		}
	//	$('#s'+Sselectid).html('<marquee width=200px  scrollamount=3 id="text">'+$('#input'+Sselectid).val()+'</marquee>');
		
    }
}
function changeRightColor() {
        if(type==0){
            $('#td'+preFselectid).css('color','#ffffff');
            $('#td'+Fselectid).css('color','#f6ff00');
        }else if(type==1){
            $('#s'+preSselectid).css('color','#ffffff');
            $('#s'+Sselectid).css('color','#f6ff00');
		
		//	text=$('#s'+Sselectid)[0].innerHTML;
	
             if($('#input'+preSselectid).val().length>=7){
			  var ss=$('#input'+preSselectid).val().substring(0,6);
			$('#s'+preSselectid).html(ss+'...');
		   }else{
			$('#s'+preSselectid).html($('#input'+preSselectid).val());
		    }
	//		$('#s'+Sselectid).html('<marquee width=200px  scrollamount=3 id="text">'+ $('#input'+Sselectid).val()+'</marquee>');
        }
}
function setCookie(position) {

    var number = getParam('number');
   // alert(number);
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
			console.log(position);
			var studycontent=$('#bottomin'+position).val();
			console.log(studycontent);
		//	var content=$('#text')[0].innerHTML;
		    var content=$('#input'+position).val();
		    //  window.location.href='http://172.16.0.221:8006/tvdemo/index.htm';
			  window.location.href = 'demo.html?number=' + number +'&PartyBranch='+data.data+ '&StudyContent=' +studycontent+'&content='+content;
            //    window.location.href = "second.html?number=" + number + "&PartyBranch=" + data.data;
        }
    })
    // window.location.href = "second.html?number=" + number + "&PartyBranch=" +'句容党支部';
}
function getTopData(){    //获取上方数据
	    
	for(var i=0;id<5;i++){
		$('#td'+(i+1)).html("");
		$('#in'+(i+1)).val("");
	}
	var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    $.ajax({
        url: svc_sys + "/getClass?",
        type: "GET",
        data: {
            PageIndex: showTop,
        },
        success: function (data) {
          topLength=data.data.length;
         for(var i=0;i<data.data.length;i++){
			 $('#td'+(i+1)).html(data.data[i].title);
			 $('#in'+(i+1)).val(data.data[i].id);
			 	showIndex=$('#in1').val();
		 }
		 if(showTop<topSize){
		 if(showTop!=topSize){
			 if(showTop==1){
			 $('#top_right').css('display','block');
	         $('#top_left').css('display','none');
			 }else{
		     $('#top_right').css('display','block');
	         $('#top_left').css('display','block'); 
			 }
         } else if(showTop=TopSize){
			 $('#top_left').css('display','block');
			 $('#top_right').css('display','none');
		 }
		 $.ajax({
        url: svc_sys + "/getClassContext?",
        type: "GET",
        data: {
			id: showIndex,
            PageIndex: showBottom,
        },
        success: function (data) {
			bottomSize=data.total;  
              for(var j=0;j<data.data.length;j++){
			 $('#s'+(j+1)).html(data.data[j].type);
			 $('#bottomin'+(j+1)).val(data.data[j].id);
		 }   
		  if(bottomSize>1){
	       $('#bottom_right').css('display','block');
         }
        }
    })
		 }
        }
    })
}
