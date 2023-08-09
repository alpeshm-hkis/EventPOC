var EventId = 0;
var _typ = '1';
$(document).ready(function () {
    tab_css();
});

// Call when user want to see event full detail
function viewDetail(id) {    
    getEventById(id);
    EventId = id;
    $.ajax({
        url: "/home/GetEventComments",
        type: "POST",
        data: { id: id },
        success: function (dataResult) {
            $("#partialModal").find(".modal-body").html(dataResult);
            $("#partialModal").modal('show');
        }
    });

}
// Call When User add new comment in comment box
function addComment() {
    var comment = $('#newComment').val();
    if (comment.trim() == '') {
        alert("Please add comment");
        return false;
    }
    $.ajax({
        url: "/home/AddEventComments",
        type: "POST",
        data: { id: EventId, comment: comment },
        success: function (dataResult) {
            // After add new comment show comment in model box
            $('#ulCommentList').append(`<div class="row d-flex justify-content-center p-1 col-lg-12">
                <div class="col-md-8 col-lg-11"  style="border-bottom: 1px solid #c9c9c9;">
                        <div class="">
                            <div class="d-flex align-items-center justify-content-between">
                                <h4>you</h4>
                                <p class="m-0">Now</p>
                            </div>
                            <div class="my-2">
                                `+ comment +`
                            </div>
                           
                    </div>
                </div>
            </div>`);
            $('#newComment').val('');
        }
    });
}

// Call when event view model closed
function closeModal() {
    $("#partialModal").modal('hide');
}

// Call when User want to delete event
function deleteDetail(id) {
    debugger;
    $('#confirm').modal("show");
    $("#delete").on('click', function (e) {
        $.ajax({
            url: "DeleteEvent",
            type: "POST",
            data: { id: id },
            success: function (dataResult) {
                window.location.href = "/event/UserEventList?type=" + 1;
            }
        });
     });
    $("#cancel").on('click', function (e) {
        
        $('#confirm').modal('hide');
    });
    // Show confirm box for delete event
    
}
// Get Event detail By id
function getEventById(id) {
    $.ajax({
        url: '/home/GetEventById',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            
            if (data != undefined) {
                data.startDate = new Date(data.startDate);
                var date = data.startDate.toLocaleString().split(',');
                $('#eventtitle').text(data.title);
                $('#eventdescription').text(data.description);
                $('#eventauther').text(data.author);
                $('#eventdate').text(date[0] + ' ' + date[1]);

            } else {

            }
        }
    });
}

// event filtter by passed or uppcomming event

function onselectionchangeEvent(type) {
    $.ajax({
        url: '/home/Index',
        type: 'GET',
        data: { type: type },
        success: function (data) {
            
            if (data != undefined) {
                window.location.href = "/home/Index?type=" + type;
                tab_css();
            } 
        }
    });
}

// event filtter by passed or uppcomming event

function onPrivateselectionchangeEvent(type) {
    $.ajax({
        url: 'Index',
        type: 'GET',
        data: { type: type },
        success: function (data) {
            
            if (data != undefined) {
                window.location.href = "/event/UserEventList?type=" + type;
                tab_css();
            }
        }
    });
}

// Call when User click on edit event button
// Redirect to edit form view page

function editEventDetails(id) {
    window.location.href = "/event/Edit?id=" + id;
}

// tab css apply when click on tab upcomming and passed

function tab_css() {
    
    var path = window.location.href.split('=');
    if (path[1] == '1') {
        $('#profile-tab').removeClass('active');
        $('#home-tab').addClass('active');
    }
    if (path[1] == '2') {
        $('#profile-tab').addClass('active');
        $('#home-tab').removeClass('active');
    }
}

function checkOnlyDigits(e) {
    e = e ? e : window.event;
    var charCode = e.which ? e.which : e.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        return true;
    }
}