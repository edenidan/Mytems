// sales per day graph
function categoryMoneyValueAdminGraph() {
    var jsonObject = JSON.parse($('#categoryMoneyValueScript').attr('data-chart-data'));
    var labelsContent = jsonObject.labels;
    var dataContent = jsonObject.data;
    var ctx = $('#categoryMoneyValueCanvas').get(0).getContext('2d');
    var chart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labelsContent,
            datasets: [{
                label: '# of Tomatoes',
                data: dataContent,
                backgroundColor: [
                    'rgba(255, 236, 33, 0.5)',
                    'rgba(55, 138, 255, 0.5)',
                    'rgba(255, 163, 47, 0.5)',
                    'rgba(245, 79, 82, 0.5)',
                    'rgba(147, 240, 59, 0.5)',
                    'rgba(149, 82, 234, 0.5)',
                    'rgba(160,82,45, 0.5)',
                    'rgba(255, 20, 147, 0.5)'
                ],
                borderColor: [
                    'rgba(255, 236, 33, 1)',
                    'rgba(55, 138, 255, 1)',
                    'rgba(255, 163, 47, 1)',
                    'rgba(245, 79, 82, 1)',
                    'rgba(147, 240, 59, 1)',
                    'rgba(149, 82, 234, 1)',
                    'rgba(160,82,45, 1)',
                    'rgba(255, 20, 147, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            cutoutPercentage: 45,
            responsive: false,

        }
    });
}





$(document).ready(function () {
    categoryMoneyValueAdminGraph();
})