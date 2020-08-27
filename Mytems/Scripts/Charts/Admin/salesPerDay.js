// sales per day graph
function salesPerDayAdminGraph() {
    var jsonObject = JSON.parse($('#salesPerDayScript').attr('data-graph-data'));
    var labelsContent = jsonObject.label;
    var dataContent = jsonObject.data;
    var ctx = $('#salesPerDayCanvas').get(0).getContext('2d');
    var chart = new Chart(ctx, {
        // The type of chart to create
        type: 'line',

        // The data for the dataset
        data: {
            labels: labelsContent,
            datasets: [{
                label: 'Number of Sales Per Day',
                backgroundColor: 'rgb(255, 99, 132)',
                borderColor: 'rgb(255, 99, 132)',
                data: dataContent
            }]
        },

        // Configuration options
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            },
            responsive: false,
            maintainAspectRatio: false
        }
    });
}



// sells per day graph tabs
function salesPerDayAdminGraphTabs() {
    $('.salesPerDayAdminTab').click(function () {
        $.ajax({
            url: "/Admins/numberOfSalesPerDay?days=" + $(this).attr('data-value'),
            success: function (data) {
                $('#salesPerDayScript').attr('data-graph-data', data);
                let canvasParent = $('#salesPerDayCanvas').parent();
                let canvasHTML = canvasParent.html();
                $('#salesPerDayCanvas').remove();
                canvasParent.append(canvasHTML);
                salesPerDayAdminGraph();
            }
        });
    });
}




$(document).ready(function () {
    salesPerDayAdminGraph();
    salesPerDayAdminGraphTabs();
})