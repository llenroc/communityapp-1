$(document).ready(function(){
"use strict"

$("form").submit(function (e) {
        var FLAG = 0;
        var multiSelectTechTagNames  = $("#TechTagNames").val();
        var multiSelectAudienceTypeNames = $("#AudienceTypeNames").val();
        var notificationTypeValue = $("#TypeOfNotification option:selected").text();
        var actionLinkValue = $("#ActionLink").val();
        //show error when notificationType is selected Announcement/Link and actionLink is emtpy.
        if((notificationTypeValue == "Announcement" || notificationTypeValue == "Link") && (actionLinkValue == null || actionLinkValue == "")){
        $("#ActionLinkError").show();
            FLAG = 1;
        }else {
            $("#ActionLinkError").hide();
        }
        //show validation error if tech tag names not selected.
        if(multiSelectTechTagNames == null){
            $("#TechTagNamesError").show();
            FLAG = 1;
        } else {
            $("#TechTagNamesError").hide();
        }
    //show validation error if audience type names not selected.
        if (multiSelectAudienceTypeNames == null) {
            $("#AudienceTypeNamesError").show();
            FLAG = 1;
        } else {
            $("#AudienceTypeNamesError").hide();
        }

        if (FLAG == 1) {
            e.preventDefault();
        }
});

    //show and hide notification message field and action link field when type of notification selected Announcement or Link respectively.
$("#TypeOfNotification").change(function () {
    var notificationTypeValue = $("#TypeOfNotification option:selected").text();
    if (notificationTypeValue == "Announcement") {
        $("#ActionLinkDiv").hide();
        $("#NotificationMessageDiv").show();
    }
        if (notificationTypeValue == "Link") {
            $("#NotificationMessageDiv").hide();
            $("#ActionLinkDiv").show();
        }
        else if (notificationTypeValue != "Announcement" && notificationTypeValue != "Link" ) {
             $("#NotificationMessageDiv").show();
             $("#ActionLinkDiv").show();
        } 
});

$("#ActionLink").change(function(){
    $("#ActionLinkError").hide();
});

//$("#NotificationMessage").change(function(){
//    $("#NotificationMessageError").hide();
//});

    $("#AudienceTypeNames").change(function(){
        $("#AudienceTypeNamesError").hide();
    });

    $("#TechTagNames").change(function(){
        $("#TechTagNamesError").hide();
    });
});