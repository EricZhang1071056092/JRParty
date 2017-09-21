$(function () {
    /*---------------接口地址----------------*/
    //脚本里用到的所有的转发连接都放在这里
    var svcHeader = (window.location.protocol ? (window.location.protocol + "//")
            : "")
            + window.location.host;
    var svc_sys = svcHeader + "/JRPartyService/Party.svc";
    //获取党支部
    $(function () {
        $.ajax({
            type: "GET",
            url: svc_sys + "/getPictureList?",
            data: {
                offset: 0,
                limit: 1,
                name: '全部',
                StudyContent: '全部',
                time: '全部',
            },
            //offset=2&limit=5&name=全部&StudyContent=全部&time=全部
            success: function (data) {
                console.log(data);
                console.log(data.total);
                $.jqPaginator('#pagination1', {
                    //totalPages: 5,
                    totalCounts: 1,
                    pageSize: 5,
                    currentPage: 1,
                    prev: '<li class="prev"><a href="javascript:;">上一页</a></li>',
                    next: '<li class="next"><a href="javascript:;">下一页</a></li>',
                    first: '<li class="first"><a href="javascript:;">首页</a></li>',
                    last: '<li class="last"><a href="javascript:;">末页</a></li>',
                    page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
                })
            }
        })
        $.ajax({
            url: svc_sys + "/getParty",
            type: "GET",
            success: function (data) {

                $('#partybranch_top').html('');
                var str = '<select id="partybranch" class="form-control selectpicker" title="党支部"><option >全部</option>'
                for (var i in data.data) {
                    str = str + '<option id="' + i + '">' + data.data[i] + '</option>';
                }
                str = str + '</select>'
                $('#partybranch_top').html(str);
                $('.selectpicker').selectpicker({
                    style: 'btn-default',
                    size: 10
                });

                //通过党支部获取内容
                $('#partybranch').change(function () {
                    $("#content_top").html('');
                    console.log($('#partybranch').val())
                    $.ajax({
                        url: svc_sys + "/getContent",
                        type: "GET",
                        data: {
                            name: $('#partybranch').val()
                        },
                        success: function (data) {
                            console.log(data);
                            var str1 = '<select id="content" class="form-control selectpicker" title="内容"><option >全部</option>'
                            for (var i in data.data) {
                                console.log(data.data[i]);
                                str1 = str1 + '<option >' + data.data[i] + '</option>';
                            }

                            console.log(str1);
                            $('#content_top').html(str1 + '</select>');
                            $('.selectpicker').selectpicker({
                                style: 'btn-default',
                                size: 10
                            });
                            //通过党支部\内容获取时间
                            $('#content').change(function () {
                                console.log($('#content').val())
                                $.ajax({
                                    url: svc_sys + "/getTime",
                                    type: "GET",
                                    data: {
                                        name: $('#partybranch').val(),
                                        StudyContent: $('#content').val()
                                    },
                                    success: function (data) {
                                        console.log(data);
                                        var bg = 'assets/img /combo_box_bg.png'
                                        var str1 = '<select id="time" class="form-control selectpicker"style="background: url("' + bg + '"); " title="时间"><option >全部</option>'
                                        for (var i in data.data) {
                                            console.log(data.data[i]);
                                            str1 = str1 + '<option >' + data.data[i] + '</option>';
                                        }
                                        console.log(str1 + '</select>');
                                        $('#time_top').html('');
                                        $('#time_top').html(str1 + '</select>');
                                        $('.selectpicker').selectpicker({
                                            style: 'btn-default',
                                            size: 10
                                        });
                                    }
                                });
                            })
                        }
                    });
                })

            }
        });
    })

    $("#submit").click(function () {

        //$.ajax({
        //    type: "GET",
        //    url: svc_sys + "/getPictureList?",
        //    data: {
        //        offset: 0,
        //        limit: 50000,
        //        name: '全部',
        //        StudyContent: '全部',
        //        time: '全部',
        //    },
        //    //offset=2&limit=5&name=全部&StudyContent=全部&time=全部
        //    success: function (data) {
        //        console.log(data);
        //        console.log(data.total);
        //        var totalCounts = data.total
        //        if (totalCounts == '0') {
        //            totalCounts=1
        //        }
        //        $.jqPaginator('#pagination1', {
        //            //totalPages: 5,
        //            totalCounts: totalCounts,
        //            pageSize: 5,
        //            currentPage: 1,
        //            prev: '<li class="prev"><a href="javascript:;">上一页</a></li>',
        //            next: '<li class="next"><a href="javascript:;">下一页</a></li>',
        //            first: '<li class="first"><a href="javascript:;">首页</a></li>',
        //            last: '<li class="last"><a href="javascript:;">末页</a></li>',
        //            page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
        //            onPageChange: function (num, type) {
        //                var num = (num - 1) * 5

        //                $.ajax({
        //                    type: "GET",
        //                    url: svc_sys + "/getPictureList?",
        //                    url: svc_sys + "/getPictureList?",
        //                    data: {
        //                        offset: num,
        //                        limit: 10,
        //                        name: '全部',
        //                        StudyContent: '全部',
        //                        time: '全部',
        //                    },
        //                    success: function (data) {
        //                        $("#content").html('');
        //                        $.each(data.rows, function (i, rows) {
        //                            $("#content").append('<li class="am-g am-list-item-dated"id="' + rows.id + '">' + '<a class="am-list-item-hd ">' + '<img src="' + rows.cover + '"style="height:40px"/>' + rows.title + '</a>' + '<span class="am-list-date">' + rows.createTime + '</span>' + '</li>');
        //                            $("#content").append('<a href="new1.html">' + '<img src="../resource/party/ppp.jpg" style="height:40px;margin-bottom:10px;border-bottom-color:brown"/>' + rows.Name + '<span style="margin-left:5%;color:black">' + rows.Name + '</span>' + '</a>' + '<span style="float:right">' + '2016-03-18' + '</span>' + '<br>');

        //                            $("#" + rows.id + "").click(function () {
        //                                $("#pagination1").html('');
        //                                $("#content").empty();
        //                                $("#page_navigation").empty();
        //                                $("#content").append('<div class="cont_box"><h2 style="text-align:center"id="news1-title"> </h2><hr data-am-widget="divider"class="am-divider am-divider-default" /><div style="text-align:center;margin-bottom:15px;" id="news-time"><!--<span class="hong" style="color:#e84949;">发表时间：</span>2016-04-11&nbsp;&nbsp;&nbsp;&nbsp;<span class="time Main_print"> </span>--></div><div class="xl_box" id="new1_content"></div></div>');
        //                                $("#news-time").append('<span class="hong" style="color:#e84949;">' + '发表时间：' + '</span>' + rows.createTime + '&nbsp;&nbsp;&nbsp;&nbsp;' + '<span class="time Main_print">' + '</span>');
        //                                $("#news1-title").append(rows.title);
        //                                $("#new1_content").append(rows.htmlContent);
        //                            });
        //                        });
        //                    }
        //                })
        //            }
        //        })
        //    }
        //})
        console.log($('#partybranch').val(), $('#content').val(), $('#time').val());
        $.ajax({
            type: "GET",
            url: svc_sys + "/getPictureList",
            data: {
                offset: 0,
                limit: 50000,
                name: $('#partybranch').val(),
                StudyContent: $('#content').val(),
                time: $('#time').val(),
            },
            success: function (data) {//成功后需要刷新一下？前台已经删了，不用刷新的
                console.log(data);
                $("#baguetteBox_image").html('');
                if (data.data[0] != null) {
                    for (var i in data.data) {
                        $("#baguetteBox_image").append('<span id="' + i + '" style="margin-left:20px;margin-top:5px"><a href="' + 'http://localhost:8097/JRPartyService/picture/' + data.data[i] + '" id="' + i + '"><img src="' + 'http://localhost:8097/JRPartyService/picture/' + data.data[i] + '" width="220"style="margin-top:30px"/></a></span>');
                    }
                    $(".srcId").css({ "position": "absolute", "float": "right", "margin-left": "-20px", "z-index": "99", "width": "20", "height": "20" });
                    baguetteBox.run('.baguetteBoxOne', {
                        // Custom options
                    });

                } else {
                    $("#baguetteBox_image").html('暂无上传图片');
                }
                var totalCounts2 = data.total
                if (totalCounts2 == '0') {
                    totalCounts2 = 1
                }
                $.jqPaginator('#pagination1', {
                    //totalPages: 5,
                    totalCounts: totalCounts2,
                    pageSize: 10,
                    currentPage: 1,
                    prev: '<li class="prev"><a href="javascript:;">上一页</a></li>',
                    next: '<li class="next"><a href="javascript:;">下一页</a></li>',
                    first: '<li class="first"><a href="javascript:;">首页</a></li>',
                    last: '<li class="last"><a href="javascript:;">末页</a></li>',
                    page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
                    onPageChange: function (num, type) {
                        var num = (num - 1) * 10

                        $.ajax({
                            type: "GET",
                            url: svc_sys + "/getPictureList?",
                            data: {
                                offset: num,
                                limit: 10,
                                name: $('#partybranch').val(),
                                StudyContent: $('#content').val(),
                                time: $('#time').val(),
                            },
                            success: function (data) {
                                console.log(data);
                                //$("#content").html('');
                                $("#baguetteBox_image").html('');
                                if (data.data[0] != null) {
                                    for (var i in data.data) {
                                        $("#baguetteBox_image").append('<span id="' + i + '" style="margin-left:20px;margin-top:5px"><a href="' + 'http://localhost:8097/JRPartyService/picture/' + data.data[i] + '" id="' + i + '"><img src="' + 'http://localhost:8097/JRPartyService/picture/' + data.data[i] + '" width="220"style="margin-top:30px"/></a></span>');
                                    }
                                    $(".srcId").css({ "position": "absolute", "float": "right", "margin-left": "-20px", "z-index": "99", "width": "20", "height": "20" });
                                    baguetteBox.run('.baguetteBoxOne', {
                                        // Custom options
                                    });

                                } else {
                                    $("#baguetteBox_image").html('暂无上传图片');
                                }
                            }
                        })
                    }
                })
            },
            error: function (data) {
                console.log(data);
            }
        })
    })
})