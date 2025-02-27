"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadCategories", function () {
    location.href = '/Categories/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});