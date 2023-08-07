var EventId = 0;
var _typ = '1';
$(document).ready(function () {
    tab_css();
});

function viewDetail(id) {
    debugger
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
            debugger
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

function closeModal() {
    $("#partialModal").modal('hide');
}

function deleteDetail(id) {
    if (confirm("Are you want to delete this event?") == true) {
        $.ajax({
            url: "DeleteEvent",
            type: "POST",
            data: { id: id },
            success: function (dataResult) {
                window.location.href = "/event/UserEventList?type=" + 1;
            }
        });
    }
}

function getEventById(id) {
    $.ajax({
        url: '/home/GetEventById',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            if (data != undefined) {
                $('#eventtitle').text(data.title);
                $('#eventdescription').text(data.description);
                $('#eventauther').text(data.author);
                $('#eventdate').text(data.startDate);

            } else {

            }
        }
    });
}

function onselectionchangeEvent(type) {
    $.ajax({
        url: '/home/Index',
        type: 'GET',
        data: { type: type },
        success: function (data) {
            debugger
            if (data != undefined) {
                window.location.href = "/home/Index?type=" + type;
                tab_css();
            } 
        }
    });
}

function onPrivateselectionchangeEvent(type) {
    $.ajax({
        url: 'Index',
        type: 'GET',
        data: { type: type },
        success: function (data) {
            debugger
            if (data != undefined) {
                window.location.href = "/event/UserEventList?type=" + type;
                tab_css();
            }
        }
    });
}

function editEventDetails(id) {
    window.location.href = "/event/Edit?id=" + id;
}

function tab_css() {
    debugger
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