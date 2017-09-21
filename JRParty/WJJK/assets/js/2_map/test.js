$(function () {
    // 百度地图API功能
    var map = new BMap.Map("l-map");
    var point = new BMap.Point(116.400244, 39.92556);
    map.centerAndZoom(point, 12);
    var marker = new BMap.Marker(point);// 创建标注
    map.addOverlay(marker);             // 将标注添加到地图中
    marker.disableDragging();           // 不可拖拽



})