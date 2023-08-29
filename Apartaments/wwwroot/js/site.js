// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ByFilter()
{
    let v = document.getElementById("byRooms").value;
    location.href = "/Home/Index?byRooms=" + v;
}

/*google API/js*/
var data;
var chart;
var options = {
    title: 'Динамика стоимости жилья',
    curveType: 'function',
    legend: { position: 'bottom' }
};

google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(InitChart);

function InitChart() {
    chart = new google.visualization.LineChart(document.getElementById('curve_chart'));    
}

function DrawChart(byId) {
    $.ajax(
        {
            type: 'GET',
            url: '/Home/LineChart',
            dataType: 'json',
            data: { "id": byId },
            contentType: 'application/json; charset=utf-8',
            success: function (response)
            {
                let ar = [['месяц', 'цена']];

                response.forEach(function (x)
                {
                    ar.push([x.month, x.price]);
                });

                data = google.visualization.arrayToDataTable(ar);
                chart.draw(data, options);
            },
            error: function (xhr, status, error)
            {
                alert("error");
            }
        }
    );
}