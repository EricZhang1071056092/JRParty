//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', 'bootstrap-table', 'bootstrap-select',
    'bootstrap-datetimepicker', 'bootstrap-datetimepicker.zh-CN'], function ($) {
        accountCheck(); 
        $(function () {
            $.ajax({
                url: svc_system + "/getOrganTree",
                type: "GET",
                success: function (data) { 
                    var str1 = ''
                    var str2 = ''
                    for (var i in data.rows) {
                        console.log(data.rows[i].SubOrgan.length);
                        if (data.rows[i].SubOrgan.length == 0) {
                            str1 = '<li>' +
                          '<span><i class="icon-minus-sign"></i>' + data.rows[i].name + '</span>  ' +
                             '<ul>';
                        } else {
                            str1 = '<li>' +
                            '<span><i class="icon-plus-sign"></i>' + data.rows[i].name + '</span>  ' +
                               '<ul>';
                        }
                       
                        for (var j in data.rows[i].SubOrgan) {
                            console.log(data.rows[i].SubOrgan[j]);
                            str1 = str1 + '<li class="leaf">' + '<span><i class="icon-leaf"></i>' + data.rows[i].SubOrgan[j] + '</span> ' +
                                        '</li>'
                        }
                        str1 = str1 + '</ul>' + '</li>'
                        str2 += str1
                    }
                    $('#tree_two').html(str2);
                    $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');
                    $('#tree_two').find('.leaf').hide('fast');
                    console.log($(".sidebar").height(), $(".rightbar_tree").height());
                    if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                        $(".sidebar").css("height", $(".rightbar_tree").height())
                    }
                    $('.tree li.parent_li > span').on('click', function (e) {

                        var children = $(this).parent('li.parent_li').find(' > ul > li');

                        if (children.is(":visible")) {

                            children.hide('fast');

                            $(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
                            if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                                $(".sidebar").css("height", $(".rightbar_tree").height())
                            }

                        } else {

                            children.show('fast');

                            $(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
                            if ($(".rightbar_tree").height() > $(".sidebar").height()) {
                                $(".sidebar").css("height", $(".rightbar_tree").height())
                            }
                        }

                        e.stopPropagation();

                    });
                }
            });
          //  $('.tree li:has(ul)').addClass('parent_li').find(' > span').attr('title', 'Collapse this branch');

          

        });
        

    })