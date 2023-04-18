class createOrEditEReservationDto {
    constructor(id, title, equipmentId, memberId, reservedDate, returnDate, description) {
        this.id = id;
        this.title = title;
        this.equipmentId = equipmentId;
        this.memberId = memberId;
        this.reservedDate = reservedDate;
        this.returnDate = returnDate;
        this.description = description;
    }
}

! function ($) {
    "use strict";

    var CalendarApp = function () {
        this.$body = $("body")
        this.$calendar = $('#calendar'),
            this.$event = ('#calendar-events div.calendar-events'),
            this.$categoryForm = $('#add-new-event form'),
            this.$extEvents = $('#calendar-events'),
            this.$modal = $('#my-event'),
            this.$saveCategoryBtn = $('.save-category'),
            this.$calendarObj = null
    };

        CalendarApp.prototype.onDrop = function (eventObj, date) {

        var $this = this;
        // retrieve the dropped element's stored Event Object
        var originalEventObject = eventObj.data('eventObject');
        var $categoryClass = eventObj.attr('data-class');
        // we need to copy it, so that multiple events don't have a reference to the same object
        var copiedEventObject = $.extend({}, originalEventObject);
        // assign it the date that was reported
        copiedEventObject.start = date;
        if ($categoryClass)
            copiedEventObject['className'] = [$categoryClass];
        // render the event on the calendar
        $this.$calendar.fullCalendar('renderEvent', copiedEventObject, true);
        // is the "remove after drop" checkbox checked?
        if ($('#drop-remove').is(':checked')) {
            // if so, remove the element from the "Draggable Events" list
            eventObj.remove();
        }
    },
        CalendarApp.prototype.onEventClick = function (calEvent, jsEvent, view) {
            ShowEvent(calEvent.id);
            //Delete(calEvent.id);
        },
        CalendarApp.prototype.onSelect = function (start, end, allDay) {
            let startDate = moment(start).format('Y-MM-DD HH:mm:ss');
            let endDate = moment(end).format('Y-MM-DD HH:mm:ss');
            var item = new createOrEditEReservationDto(0, "", 0, 0, startDate, endDate, '')
            CreateEvent(item);
        },
        CalendarApp.prototype.enableDrag = function () {
            $(this.$event).each(function () {
                // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
                // it doesn't need to have a start or end
                var eventObject = {
                    title: $.trim($(this).text()) // use the element's text as the event title
                };
                // store the Event Object in the DOM element so we can get to it later
                $(this).data('eventObject', eventObject);
                // make the event draggable using jQuery UI
                $(this).draggable({
                    zIndex: 999,
                    revert: true, // will cause the event to go back to its
                    revertDuration: 0 //  original position after the drag
                });
            });
        },
        CalendarApp.prototype.applayEvent = function (item) {
            let result = item;
            var $this = this;

            $this.$calendarObj.fullCalendar('renderEvent', {
                id: result.id,
                title: result.title,
                start: result.reservedDate,
                end: result.returnDate,
                allDay: false,
                className: 'bg-success',
                description: result.description
            }, true);
            $this.$calendarObj.fullCalendar('unselect');
            loadReservation();
        },
        CalendarApp.prototype.applayDelete = function (itemId) {

            var $this = this;
            $this.$calendarObj.fullCalendar('removeEvents', function (ev) {
                return (ev.id == itemId);
            });
        }


    CalendarApp.prototype.init = function () {
        this.enableDrag();

        var $this = this;

        $.ajax({
            method: 'POST',
            url: '/admin/EquipmentReservation/CalenderInitData',
            success: function (response) {
                var result = response;
                if (result.status === 200) {
                    let items = JSON.parse(result.result.jsonData);
                    var defaultEvents = items.map(val => {
                        let test = {
                            id: val.Id,
                            title: val.Title,
                            start: val.ReservedDate,
                            end: val.ReturnDate,
                            className: 'bg-success',
                            description: val.Description
                        };
                        return test;
                    });
                    $this.$calendarObj = $this.$calendar.fullCalendar({
                        slotDuration: '00:15:00',
                        /* If we want to split day time each 15minutes */
                        minTime: '08:00:00',
                        maxTime: '19:00:00',
                        defaultView: 'month',
                        handleWindowResize: true,

                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        events: defaultEvents,
                        editable: true,
                        droppable: true, // this allows things to be dropped onto the calendar !!!
                        eventLimit: true, // allow "more" link when too many events
                        selectable: true,
                        drop: function (date) { $this.onDrop($(this), date); },
                        select: function (start, end, allDay) { $this.onSelect(start, end, allDay); },
                        eventClick: function (calEvent, jsEvent, view) { $this.onEventClick(calEvent, jsEvent, view); }

                    });
                    $this.$calendarObj.fullCalendar('unselect')

                }
            },
            error: function () {
                toastr.error(result.message, 'Error');
            },
        });
    },
        $.CalendarApp = new CalendarApp, $.CalendarApp.Constructor = CalendarApp

}(window.jQuery),

    $(window).on('load', function () {

        $.CalendarApp.init()
    });