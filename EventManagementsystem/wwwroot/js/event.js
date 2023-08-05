var EventId = 0;
$(document).ready(function () {

});

function viewDetail(id) {
    debugger
    getEventById(id);
    EventId = id;
    $.ajax({
        url: "home/GetEventComments",
        type: "POST",
        data: { id: id },
        success: function (dataResult) {
            $("#partialModal").find(".modal-body").html(dataResult);
            $("#partialModal").modal('show');
        }
    });

}

function addComment() {
    debugger
    var comment = $('#newComment').val();
    EventId;
    $.ajax({
        url: "home/AddEventComments",
        type: "POST",
        data: { id: EventId, comment:comment },
        success: function (dataResult) {
            debugger
            $('#ulCommentList').append(`<div class="row d-flex justify-content-center p-1 col-lg-12">
                <div class="col-md-8 col-lg-10">
                    <div class="card shadow-0 border" style="background-color: #d8e9f1;">
                        <div class="card-body p-4">
                            <div class="card mb-2">
                                <div class="card-body">
                                    `+ comment +`
                                </div>
                            </div>
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
    $.ajax({
        url: "DeleteEvent",
        type: "POST",
        data: { id: id },
        success: function (dataResult) {
        }
    });
}

function getEventById(id) {
    $.ajax({
        url: 'event/GetEventById',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            debugger
            if (data != undefined) {
                $('#eventtitle').text(data.title);
                $('#eventdescription').val();
                $('#eventauther').val();
                $('#eventdate').val();

            } else {

            }
        }
    });
}