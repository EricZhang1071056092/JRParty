//$(function () {
require(['jquery', 'jquery.cookie', 'bootstrap', "jqueryui", "fullcalendar"], function ($) {
        loadCommonModule('system'); 
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        var calendar = $('#calendar').fullCalendar({
            header: {
                left: ' ',
                center: '',
                right: ' '
            },
            defaultView: 'agendaDay',
            selectable: true,
            selectHelper: true,
            select: function (start, end, allDay) {
                var title = prompt('Event Title:');
                if (title) {
                    calendar.fullCalendar('renderEvent',
                        {
                            title: title,
                            start: start,
                            end: end,
                            allDay: allDay
                        },
                        true // make the event "stick"
                    );
                }

                calendar.fullCalendar('unselect');
                console.log(title, start, end, allDay);
            },
            editable: true,
            //拖动事件
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                console.log(event.id, event.title, event.start, event.end, allDay);
            },
            //单击事件项时触发
            eventClick: function (calEvent, jsEvent, view) {
                console.log(calEvent.id, calEvent.title, calEvent.start, calEvent.end, calEvent.allDay);
                $('#address').val('http://172.16.0.31:6713/mag/hls/0efc00cd2dbd4e84ab571aea39375c5f/0/live.m3u8');
                $('#common-alert').modal();
                $('.cancel').unbind("click");
                $(".cancel").click(function () {
                    $('#calendar').fullCalendar('removeEvents', calEvent.id)
                })
                $('.confirm').unbind("click");
                $(".confirm").click(function () {
                    var schdata = { start: calEvent.start, end: calEvent.end, title: $('#address').val(), allDay: false };
                    $('#calendar').fullCalendar('removeEvents', calEvent.id)
                    $('#calendar').fullCalendar('renderEvent', schdata, true);
                })
            },
            events: [
                {
                    id: 1,
                    title: 'http://172.16.0.31:6713/mag/hls/0efc00cd2dbd4e84ab571aea39375c5f/0/live.m3u8',
                    start: new Date(y, m, 1)
                },
                {
                    id: 2,
                    title: 'http://172.16.0.31:6713/mag/hls/0efc00cd2dbd4e84ab571aea39375c5f/0/live.m3u8',
                    start: new Date(y, m, d - 5),
                    end: new Date(y, m, d - 2)
                },
                {
                    id: 3,
                    title: 'Repeating Event',
                    start: new Date(y, m, d - 3, 16, 0),
                    allDay: false
                },
                {
                    id: 4,
                    title: 'Repeating Event',
                    start: new Date(y, m, d + 4, 16, 0),
                    allDay: false
                },
                {
                    id: 5,
                    title: 'http://172.16.0.31:6713/mag/hls/0efc00cd2dbd4e84ab571aea39375c5f/0/live.m3u8',
                    start: new Date(y, m, d, 10, 30),
                    allDay: false
                },
                {
                    id: 6,
                    title: 'http://172.16.0.31:6713/mag/hls/0efc00cd2dbd4e84ab571aea39375c5f/0/live.m3u8',
                    start: new Date(y, m, d, 12, 0),
                    end: new Date(y, m, d, 14, 0),
                    allDay: false
                },
                {
                    id: 7,
                    title: 'Birthday Party',
                    start: new Date(y, m, d + 1, 19, 0),
                    end: new Date(y, m, d + 1, 22, 30),
                    allDay: false
                },
                {
                    id: 8,
                    title: 'Click for Google',
                    start: new Date(y, m, 28),
                    end: new Date(y, m, 29),
                    url: 'http://google.com/'
                }
            ]
        });
    })