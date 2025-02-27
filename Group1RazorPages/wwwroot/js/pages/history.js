"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadHistory", function (id) {
    location.href = '/NewsArticles/IndexHistory/' + id;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});