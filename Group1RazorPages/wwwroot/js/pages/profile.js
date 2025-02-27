"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadProfile", function (id) {
    location.href = '/Account/Details/' + id;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});