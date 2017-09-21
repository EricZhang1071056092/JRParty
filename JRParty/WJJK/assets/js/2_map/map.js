require(["leaflet", "jquery", "bootstrap-table", "bootstrap", "jquery.cookie", "subgroup-src", "realworld.388"], function (L) {

    // 百度地图API功能
    var map = new BMap.Map("l-map");
    var point = new BMap.Point(119.1975591274, 32.1753243935);
    var data = {
        "rows": [
            {
                "x": "119.198817",
                "y": "32.18159",
            },
            {
                "x": "119.195367",
                "y": "32.172849",
            },
               {
                   "x": "119.195008",
                   "y": "32.172604",
               },
            {
                "x": "119.197092",
                "y": "32.172849",
            },
            {
                "x": "119.202554",
                "y": "32.178228",
            },
            {
                "x": "119.200326",
                "y": "32.177265",
            },
            {
                "x": "119.20241",
                "y": "32.17725",
            },
            {
                "x": "119.194738",
                "y": "32.176455",
            }

        ],
    }
    map.centerAndZoom(point, 15);
    var opts = {
        width: 350,     // 信息窗口宽度
        height: 150,     // 信息窗口高度
        title: "信息窗口", // 信息窗口标题
        enableMessage: true//设置允许信息窗发送短息
    };
    var MAX = 100;
    var markers = [];
    var pt = null;
    var i = 0;
    for (var i = 0; i < data.rows.length; i++) {
        pt = new BMap.Point(data.rows[i].x, data.rows[i].y);
        var content = '<form class="form-inline"id="' + 'form' + i + '" style="margin-top:20px;margin-left:30px">' +
                            '<div class="form-group">' +
                                '<label for="disabledSelect">监控名称：&nbsp;</label>' +
                                '<input type="text" style="width:184px" class="form-control" id="' + 'type' + i + '" value="下蜀镇">' +
                            '</div> ' +
                            '<div class="form-group"style="margin-top:20px">' +
                                '<label for="content">标&nbsp;&nbsp;注&nbsp;人：&nbsp;</label>' +
                                '<input type="text"style="width:184px" class="form-control" id="' + 'context' + i + '" value="下蜀镇管理员" >' +
                            '</div>' +
                        '</form>';
        var myIcon = new BMap.Icon("../assets/js/common/leaflet/images/marker-icon.png", new BMap.Size(30, 45), {
            anchor: new BMap.Size(10, 30),
            infoWindowAnchor: new BMap.Size(10, 0)
        });

        var marker = new BMap.Marker(pt, { icon: myIcon });// 创建标注
        map.addOverlay(marker);             // 将标注添加到地图中
        marker.enableDragging();
        addClickHandler(content, marker);// 不可拖拽 
        var removeMarker = function (e, ee, marker) {
            map.removeOverlay(marker);
        }
        //创建右键菜单
        var markerMenu = new BMap.ContextMenu();
        markerMenu.addItem(new BMap.MenuItem('删除', removeMarker.bind(marker)));

        marker.addContextMenu(markerMenu);
    }
    //标注工具栏
    map.enableScrollWheelZoom(true);
    var polygon = new BMap.Polygon([
		new BMap.Point(119.1919701989, 32.1775294700),
		new BMap.Point(119.1918745299, 32.1673911425),
		new BMap.Point(119.1941490431, 32.1673548124),
		new BMap.Point(119.2052367187, 32.1777071983),
        new BMap.Point(119.1983127594, 32.1849836919)
    ], { strokeColor: "red", strokeWeight: 2, strokeOpacity: 0 });  //创建多边形
    map.addOverlay(polygon);   //增加多边形
    marker.enableDragging()//拖拽
    map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
    map.setCurrentCity("镇江");          // 设置地图显示的城市 此项是必须设置的
    map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
    var top_left_control = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_TOP_LEFT });// 左上角，添加比例尺
    var top_left_navigation = new BMap.NavigationControl();  //左上角，添加默认缩放平移控件  
    function addClickHandler(content, marker) {
        marker.addEventListener("click", function (e) {
            openInfo(content, e)
        }
		);
    }
    function openInfo(content, e) {
        var p = e.target;
        var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
        var infoWindow = new BMap.InfoWindow(content, opts);  // 创建信息窗口对象 
        map.openInfoWindow(infoWindow, point); //开启信息窗口
    }
    map.enableScrollWheelZoom();
    var overlays = [];
    var overlaycomplete = function (e) {
        overlays.push(e.overlay);
       
        var marker = e.overlay;
        marker.enableDragging();
        /*-----------------标注右键删除-------------------------*/
        var markerMenu = new BMap.ContextMenu();
        markerMenu.addItem(new BMap.MenuItem('删除标注 ', function () {
            map.removeOverlay(marker);
        }))
        marker.addContextMenu(markerMenu);
        /*-----------------标注点击弹窗-------------------------*/
        var pContent = '<form class="form-inline"id="' + 'form' + i + '" style="margin:20px">' +
                           '<div class="form-group">' +
                               '<label for="disabledSelect">监控名称：&nbsp;</label>' +
                               '<input type="text" style="width:184px" class="form-control" id="' + 'type' + i + '" placeholder="">' +
                           '</div> <br>' +
                           '<div class="form-group"style="margin-top:20px">' +
                               '<label for="content">标&nbsp;&nbsp;注&nbsp;人：&nbsp;</label>' +
                               '<input type="text"style="width:184px" class="form-control" id="' + 'context' + i + '"  >' +
                           '</div>' +
                       '</form>';
        var infoWindow = new BMap.InfoWindow(pContent);
        marker.addEventListener("click", function (e) {
                     var sContent = '<form class="form-inline"id="' + 'form' + i + '" style="margin:20px">' +
                            '<div class="form-group">' +
                                '<label for="disabledSelect">监控名称：&nbsp;</label>' +
                                '<input type="text" style="width:184px" class="form-control" id="' + 'type' + i + '" >' +
                            '</div> <br>' +
                            '<div class="form-group"style="margin-top:20px">' +
                                '<label for="content">标&nbsp;&nbsp;注&nbsp;人：&nbsp;</label>' +
                                '<input type="text"style="width:184px" class="form-control" id="' + 'context' + i + '" >' +
                            '</div>' +
                        '</form>';
            var opts = {
                enableMessage: true
            };
            var infoWindow = new BMap.InfoWindow(sContent);
            this.openInfoWindow(infoWindow); 
        }); 
        e.overlay.openInfoWindow(infoWindow, marker);
    };
    var styleOptions = {
        strokeColor: "red",    //边线颜色。
        fillColor: "red",      //填充颜色。当参数为空时，圆形将没有填充效果。
        strokeWeight: 3,       //边线的宽度，以像素为单位。
        strokeOpacity: 0.8,	   //边线透明度，取值范围0 - 1。
        fillOpacity: 0.6,      //填充的透明度，取值范围0 - 1。
        strokeStyle: 'solid' //边线的样式，solid或dashed。
    }
    //实例化鼠标绘制工具
    var drawingManager = new BMapLib.DrawingManager(map, {
        isOpen: false, //是否开启绘制模式
        enableDrawingTool: true, //是否显示工具栏
        drawingToolOptions: {
            anchor: BMAP_ANCHOR_TOP_LEFT, //位置
            offset: new BMap.Size(5, 5), //偏离值
        },
        circleOptions: styleOptions, //圆的样式
        polylineOptions: styleOptions, //线的样式
        polygonOptions: styleOptions, //多边形的样式
        rectangleOptions: styleOptions //矩形的样式
    });
    //添加鼠标绘制工具监听事件，用于获取绘制结果
    drawingManager.addEventListener('overlaycomplete', overlaycomplete);
    function clearAll() {
        for (var i = 0; i < overlays.length; i++) {
            map.removeOverlay(overlays[i]);
        }
        overlays.length = 0
    }
})

//查看详情
function populationDetailOpen(id) {
    console.log(id);
    window.open("detail.html?id=" + encodeURI(id));

}



