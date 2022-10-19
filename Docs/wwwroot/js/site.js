// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder().withUrl("/fileChangeHub").build();

connection.on("FileChanged", function (file)
{
    var content = $(".pageContent");
    var id = content.attr("data-id");
    if (file == id) {
        GetPartial(content);
    }
});

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});

$(function () {
    AfterLoad($(this));
});

function AfterLoad(loadedObj) {

    //This is used to dynamically load content on the pages. 
    //See /ticket/details for a few examples.
    loadedObj.find("*.GetPartial").each(function (index, value) {
        var obj = $(this);
        GetPartial(obj);
    });
}

function GetPartial(obj) {
    var uri = obj.attr('data-uri');
    var id = obj.attr('data-id');
    
    $.ajax({
        type: "GET",
        url: uri,
        contentType: "application/json; charset=utf-8",
        data: { "Id": id },
        datatype: "json",
        success: function (data) {
            obj.html(data);
            AfterLoad(obj);
        },
        error: function (response) {
            obj.html("Error code: " + response.status);
        }
    });
}