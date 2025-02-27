"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadTags", function () {
    location.href = '/Tags/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});