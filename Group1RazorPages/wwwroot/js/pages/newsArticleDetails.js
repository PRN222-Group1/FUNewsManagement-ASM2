"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadNewsArticleDetails", function (id) {
    location.href = '/NewsArticles/Details/' + id;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});