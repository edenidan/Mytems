// Best sellers by total sales money
function bestSellersByTotalSalesMoney() {
    var jsonObject = JSON.parse($('#bestSellersByTotalSalesMoneyScript').attr('data-chart-data'));
    var labelsContent = jsonObject.labels;
    var dataContent = jsonObject.data;
    var ctx = $('#bestSellersByTotalSalesMoneyCanvas').get(0).getContext('2d');
    var chart = new Chart(ctx, {
        // The type of chart to create
        type: 'bar',

        // The data for the dataset
        data: {
            labels: labelsContent,
            datasets: [{
                label: 'Total sales',
                backgroundColor: [
                    'rgba(255, 236, 33, 1)',
                    'rgba(55, 138, 255, 1)',
                    'rgba(255, 163, 47, 1)',
                    'rgba(245, 79, 82, 1)',
                    'rgba(147, 240, 59, 1)',
                    'rgba(149, 82, 234, 1)',
                    'rgba(160,82,45, 1)',
                    'rgba(255, 20, 147, 1)',
                    'rgba(55,55,55, 1)',
                    'rgba(0, 191, 255, 1)'
                    
                ],
                data: dataContent
            }]
        },

        // Configuration options
        options: {
            legend: { display: false },
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




$(document).ready(function () {
    bestSellersByTotalSalesMoney();
})