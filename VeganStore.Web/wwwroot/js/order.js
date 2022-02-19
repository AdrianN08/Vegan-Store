var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/User/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "firstName", "width": "5%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "orderDate", "width": "15%" },
            { "data": "orderStatus", "width": "5%" },
            { "data": "orderTotal", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/User/OrderDetails?orderId=${data}"
                        class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Detaljer</a>                        
					</div>
                        `
                },
                "width": "10%"
            }
        ]
    });
}