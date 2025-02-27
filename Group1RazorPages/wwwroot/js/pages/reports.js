"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadReports", function () {
    location.href = '/Reports/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});