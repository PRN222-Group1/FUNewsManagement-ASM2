"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadNewsArticles", function () {
    location.href = '/NewsArticles/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});