"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadAccounts", function () {
    location.href = '/Account/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});