// sales per day graph
function salesPerDayAdminGraph() {
    var jsonObject = JSON.parse($('#salesPerDayScript').attr('data-chart-data'));
    var labelsContent = jsonObject.labels;
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
                backgroundColor: 'rgb(60,186,159, 0.2)',
                borderColor: 'rgb(60,186,159, 1)',
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
    $('.salesPerDayTab').click(function () {
        $.ajax({
            url: "/Sellers/NumberOfSalesPerDay?days=" + $(this).attr('data-value'),
            success: function (data) {
                $('#salesPerDayScript').attr('data-chart-data', data);
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