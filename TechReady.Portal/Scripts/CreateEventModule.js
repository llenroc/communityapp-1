var app = angular.module('CreateEventModule', ["kendo.directives"]);

app.controller('CreateEventController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    "use strict"

    var dialogTrack;
    var dialogSessions;
    var dialogEventTechnoLogTags;
    var dialogEventAudienceTypeTags;
    var eventTrackDiv = $("#div_event_track");
    //var DxRoot = "/DXEvents_SAMPLE" ;
    var DxRoot = "" ;
    $scope.event = {};
    $scope.eventTrackList = [];
    $scope.eventTrack = {};
    $scope.eventTrack.TrackDisplayName = null;
    $scope.eventTechnologTags = {};
    $scope.eventAudienceTypeTags = {};
    $scope.showNewTrackView = false;
    $scope.numRegex = /^[0-9]*$/;
    $scope.formSubmitted = false;
    $scope.inactiveLink = false;
    $scope.AllFieldsValid = true;
    $scope.currentEventIndex = -1;
    $scope.editTrackAgendas = [];
    $scope.currentSessionIndex = -1;
    $scope.modifyEvent = false;

    //get existing event details.
    var getEvent = function () {
        if (EventID != null && EventID != "undefined" && EventID != 0) {        //if event id exist in global variable EventID(from ViewBag.EventID in view).
            $http.get(DxRoot + "/Event/GetEvent?id=" + EventID +"&rnd="+new Date().getTime())   //http-get request to get event details. 
       .success(function (data) {
           if (data != null && data != "undefined") {       //on request success.
               $scope.event = data[0];              //get event data from first element of response.

               if ($scope.event.EventTechnologTags.length != 0) {     //if event technolog tags of event exist.  
               $scope.eventTechnologTags = $scope.event.EventTechnologTags;     //move retrieved event technolog tags to scope variable for multi-select list.
               }

               if ($scope.event.EventAudienceTypeTags.length != 0) {     
                   $scope.eventAudienceTypeTags = $scope.event.EventAudienceTypeTags;
               }



               if($scope.event != null && $scope.event != "undefined"){$scope.modifyEvent = true;}      //if event is existing event then event modify mode.

               if (data[1] != null && data[1] != "undefined" && data[1].length>0) {         //if event tracks exist in existing event.
                   $scope.eventTrackList = data[1];                 //move event tracks to eventTrackList to show on page.
                   jQuery(".tracks-container").show();
               }
           }
       });
        }
    };
    getEvent();     //call- getEvent if EventID is defined.

    //return track index for given track display name.
    var setTracksList = function (tracks, trackDisplayName) {       //parameters: 1.track list 2.track display name to search for index.
        var indexes = tracks.map(function (obj, index) {            
            if (obj.TrackDisplayName == trackDisplayName) {     
                return index;                                       //return index of matching track display name passed. 
            }
        }).filter(isFinite);
        return indexes;
    };

    //get data for dropdown fields on view.
    var getDropdownLists = function () {
        $http.get(DxRoot + "/Services/getViewModelData?rnd="+new Date().getTime())      //get request to services controller.
       .success(function (data) {
           
           $scope.AvailableCities = data.AvailableCities;           //event cities dropdown.
           $scope.ScEligibility = data.ScEligibility;               //event score card eligibility dropdown.
           $scope.EventStatus = data.EventStatus;                   //event status dropdown.
           $scope.EventVisibility = data.EventVisibility;           //event visibility dropdown.
           $scope.EventType = data.EventType;                       //event type dropdown.
           $scope.EventTechnologTags = data.EventTechnologTags;     //event technolog tags dropdown.
          $scope.EventAudienceTypeTags = data.EventAudienceTypeTags;

           var eventTrackListLen = $scope.eventTrackList.length;
           $scope.Speakers = data.Speakers;                         //speakers dropdown for track agendas speaker.
           $scope.Tracks = data.Tracks;                             //tracks dropdown.
           if(eventTrackListLen != 0){                              //if event tracks length greater than 0.
               for (var i = 0; i < eventTrackListLen; i++) {        //loop tracks of event.
                   var resultTrackIndex = setTracksList($scope.Tracks, $scope.eventTrackList[i].TrackDisplayName);  //get index for this track display name.
                   var trackIndex = resultTrackIndex[0];
                   $scope.Tracks.splice(trackIndex, 1);             //remove the track from tracks if track is already selected in event track.
               }           
           }
       });
    };
    getDropdownLists();

    //hide session link.
    var hideSessions = function () {
        $scope.inactiveLink = false;
        jQuery("a.inactiveLink").removeClass("inactiveLink").addClass("active");
    };

    //show edit track view.
    $scope.setShowTrack = function () {
        $scope.showNewTrackView = true;
    };

    //add new and edit tracks.
    $scope.addTrack = function (trackId, index) {               //parameter: id of event track, index of event track from list.
    
        if (trackId != "undefined" && $scope.eventTrack.TrackVenue != null && $scope.eventTrack.TrackStartTime != null && $scope.eventTrack.TrackEndTime != null && $scope.eventTrack.TrackSeating != null && $scope.eventTrack.TrackOwner != null) {       //check track details if not null.
            
            if ($scope.eventTrack.editMode) {       //if event track in edit mode.
                $scope.eventTrackList[index] = $scope.eventTrack;
            }
            else {
                if (trackId != null && trackId != "undefined") {        //if new event track exist. 

                    for (var track in $scope.Tracks) {
                        if ($scope.Tracks[track].TrackID == trackId) {
                            $scope.Tracks.splice(track, 1);         //remove selected track from tracks dropdown.
                        }
                    }
                    $scope.eventTrackList.push($scope.eventTrack);
                    $scope.getSessions(($scope.eventTrackList.length-1), trackId);
                }
            }
                $scope.eventTrack.editMode = false;              //change track's edit mode to view mode.
                $scope.newEventTrackForm.$setPristine();
                $scope.eventTrack = {};
                jQuery(".tracks-container").show();
                dialogTrack.dialog("close");
        } else {
            $scope.AllFieldsValid = false;                      //if all fields of track are not valid.
        }
    };

    //turn edit mode of currently selected event track from list.
    //parameters: event track object to edit, index of event track from event list.
    $scope.eventTrackEdit = function (currentEventTrack,index) {
        $scope.eventTrack = currentEventTrack;
        $scope.eventTrack.editMode = true;
        $scope.currentEventIndex = index;                      //current event track index from event track's list.
        dialogTrack.dialog("open");
    };

    //deletes event track from event track list.
    //parameters: TrackID of event track.
    $scope.deleteTrack = function (trackId) {
        for (var track in $scope.eventTrackList) {
            if ($scope.eventTrackList[track].TrackID == trackId) {          //remove the event track from even track list having trackid passed.
                $scope.Tracks.push({ TrackID: $scope.eventTrackList[track].TrackID, TrackDisplayName: $scope.eventTrackList[track].TrackDisplayName  });;
                $scope.eventTrackList.splice(track, 1);
            }
        }
    };

    //sets event track display name of currently editing track, track display name taken from main tracks list selected option.
    //parameters: trackid of selected event track for edit.
    $scope.changeTrackDispName = function (trackId) {
        for (var track in $scope.Tracks) {
            if ($scope.Tracks[track].TrackID == trackId) {
                $scope.eventTrack.TrackDisplayName = $scope.Tracks[track].TrackDisplayName;
            }
        }
    };

    //cancel currently editing existing track/adding new track to event track list.
    $scope.cancelAddTrack = function () {
        $scope.eventTrack = {};                 //reset eventTrack variable to accomodate new track.
        $scope.AllFieldsValid = true;
        dialogTrack.dialog("close");

    };

    //create new event/save existing event details.
    $scope.createEvent = function () {
        $scope.formSubmitted = true;

        //var j = 0, i = 0;
        //for (i = 0; i < $scope.editTrackAgendas.length; i++) {
        //    if ($scope.editTrackAgendas[i].Speaker != "undefined" || $scope.editTrackAgendas[i].Speaker != null) {
        //        $scope.editTrackAgendas[i].SpeakerID = $scope.editTrackAgendas[i].Speaker.SpeakerID;
        //        $scope.editTrackAgendas[i].Speaker = null;
        //    }
        //}

        var j = 0, i = 0;
        //loop to event tracks to set speaker-id of all track agendas of all event tracks. 
        for (j = 0; j < $scope.eventTrackList.length; j++) {
            if (typeof ($scope.eventTrackList[j].TrackAgendas) != "undefined" && $scope.eventTrackList[j].TrackAgendas != null) {
                for (i = 0; i < $scope.eventTrackList[j].TrackAgendas.length; i++) {
                    if (typeof ($scope.eventTrackList[j].TrackAgendas[i].Speaker) != "undefined" || $scope.eventTrackList[j].TrackAgendas[i].Speaker != null) {
                        $scope.eventTrackList[j].TrackAgendas[i].SpeakerID = $scope.eventTrackList[j].TrackAgendas[i].Speaker.SpeakerID;
                        $scope.eventTrackList[j].TrackAgendas[i].Speaker = null;
                    }
                }
            }
        }
        //create object for event details if event form fields are valid.
        if (typeof ($scope.event) !== "undefined" && $scope.event != null && $scope.CreateEventForm.$valid) {
            var eventInfoModel = {
                Event: $scope.event,
                EventTracks: $scope.eventTrackList,
                EventTechnologTags: $scope.eventTechnologTags,
                EventAudienceTypeTags: $scope.eventAudienceTypeTags
            };


            //http post request with data from page view bundled into eventInfoModel object.
            $http.post('CreateEvent', eventInfoModel, null)
            .success(function (data) {
                if (data.responseCode == '1') {                 //if response success and response code 1, navigate to index page events.
                    $window.location.href = DxRoot + "/Events/CurrentEvents";
                } else {
                    if(data.responseCode == '003'){
                        alert("please add tracks or select any event technolog tag & audience type");
                        }   else {
                                alert("server error occured!!");
                        }
                }
                //$scope.eventTrack = null;
                //delete $scope.eventTrack;
            });
        }
    };

    //request for sessions/track agendas of event tracks.
    //if new event track then get sessions, if existing event track then gettrack agendas of the event track.
    //parameters: index of event track from list, trackId of event track.
    $scope.getSessions = function (index, trackId) {
        if (!$scope.inactiveLink) {
            if (trackId != null && trackId !== "undefined") {
                if (typeof($scope.eventTrackList[index].TrackAgendas) == "undefined" || $scope.eventTrackList[index].TrackAgendas == null) {
                $http.get(DxRoot + "/Services/getSessionValues?rnd="+new Date().getTime(), { params: { id: trackId, eventTrackId: $scope.eventTrackList[index].EventTrackID } })
                    .success(function (data) {
                        $scope.editTrackAgendas = data[0];                  //track agendas from server moved to editTrackAgendas for editing of track agendas.
                        var j = 0,i=0;
                        for (j = 0; j < $scope.editTrackAgendas.length;j++)
                        {
                            //pass speaker object having matched speaker id with track agendas obtained.
                            for (i = 0; i < $scope.Speakers.length;i++)
                            {
                                if ($scope.Speakers[i].SpeakerID === $scope.editTrackAgendas[j].SpeakerID) {
                                    $scope.editTrackAgendas[j].Speaker = $scope.Speakers[i];
                                }
                            }
                        }
                                        dialogSessions.dialog("open");

                });
            }
            else {      //existing track agendas in event track.
                    $scope.editTrackAgendas = $scope.eventTrackList[index].TrackAgendas;        //copy existing track agendas of evet track for edit. 
                    var j = 0, i = 0;

                    for (j = 0; j < $scope.editTrackAgendas.length; j++) {

                        for (i = 0; i < $scope.Speakers.length; i++) {
                            if ($scope.Speakers[i].SpeakerID == $scope.editTrackAgendas[j].SpeakerID) {
                                $scope.editTrackAgendas[j].Speaker = $scope.Speakers[i];
                            }
                        }
                    }
                    dialogSessions.dialog("open");

                }
                $scope.currentSessionIndex = index;             //index of currently selected event track.
                $scope.inactiveLink = true;
                jQuery("a.active").removeClass("active").addClass("inactiveLink");
            } 
        }
    };

    //saves track agendas of event track currently edited.
    //parameters: index of event track of track agendas.
    $scope.saveSessions = function (index) {
        var j = 0, i = 0;
        //loop to set speaker id and event track id to track agendas currently edited.
        for (i = 0; i < $scope.editTrackAgendas.length; i++) {
            if (typeof ($scope.editTrackAgendas[i].Speaker) != "undefined" || $scope.editTrackAgendas[i].Speaker != null) {
                $scope.editTrackAgendas[i].SpeakerID = $scope.editTrackAgendas[i].Speaker.SpeakerID;
            }
            if ($scope.editTrackAgendas[i].EventTrackID == "undefined" || $scope.editTrackAgendas[i].EventTrackID == null) {
                if($scope.eventTrackList[index].EventTrackID != "undefined" && $scope.eventTrackList[index].EventTrackID != null){
                    $scope.editTrackAgendas[i].EventTrackID = $scope.eventTrackList[index].EventTrackID;
                }
            }
        }
        var FLAG = 1;
        var TrackAgendasLen = $scope.editTrackAgendas.length;
        var TrackAgendasIndex = 0;
        //loop edited track agendas for validation check for empty values.
        while(TrackAgendasIndex < TrackAgendasLen){
            if ($scope.editTrackAgendas[TrackAgendasIndex].StartTime == null || $scope.editTrackAgendas[TrackAgendasIndex].EndTime == null || $scope.editTrackAgendas[TrackAgendasIndex].SpeakerID == null) {
                FLAG = 0;
                $scope.editTrackAgendas[TrackAgendasIndex].allFieldsValid = true;           //if track agenda fields are not valid. 
                break;
            }else{$scope.editTrackAgendas[TrackAgendasIndex].allFieldsValid = false;}
            TrackAgendasIndex++;
        }
        
        //all track agenda's fields are valid.
        if (FLAG == 1) {
            if ($scope.editTrackAgendas.length != 0 && ($scope.eventTrackList[index].EventTrackID != null && $scope.eventTrackList[index].EventTrackID != "undefined")) {
                //http post request for modifying event track's track agendas. payload track agendas list for saving.
                $http.post(DxRoot + "/Event/ModifyTrackAgendas", $scope.editTrackAgendas, null)
                    .success(function (data) {
                        if (data.responseCode == '001') {
                            $scope.eventTrackList[index].TrackAgendas = data.trackAgendasSaved;         //saved and retrieved track agendas in response data.
                            $scope.createSessionForm.$setPristine();
                            dialogSessions.dialog("close");
                        }
                        if (data.responseCode == '002') {
                            alert("server error track agendas not updated!!");
                        }
                    });
            }
            else {          //if event track is new.
                if ($scope.editTrackAgendas.length != 0) {
                    $scope.eventTrackList[index].TrackAgendas = $scope.editTrackAgendas;            //pass edited track agendas to track agendas object of event track.
                    $scope.createSessionForm.$setPristine();
                    dialogSessions.dialog("close");
                }
            }
        }
    };

    //cancels the save track agendas currently in edit mode.
    $scope.cancelSaveSessions = function () {
        $scope.editTrackAgendas = [];
        $scope.createSessionForm.$setPristine();
        dialogSessions.dialog("close");
    };

    //closes technolog tag dialog after save button click on dialog.
    $scope.saveTechnoLogTag = function(){
        dialogEventTechnoLogTags.dialog("close");
    };

    $scope.saveAudienceTypeTag = function () {
        dialogEventAudienceTypeTags.dialog("close");
    };

    //intialize dialog for editing and adding track.
    jQuery(function(){
        dialogTrack = jQuery("#dialog-form-newTrack").dialog({
      autoOpen: false,
      height: 545,
      width: 400,
      modal: true,
      
      dialogClass: 'dialog-title-background'
    });
    //register click event on add-track anchor to open dialog.
    jQuery("#add-track").button().on("click", function () {
        $scope.AllFieldsValid = true;
        dialogTrack.dialog("open");
    });

        //initialize session dialog for editing sessions/track agendas of event track.
    dialogSessions = jQuery("#dialog-form-sessions").dialog({
        autoOpen: false,
        height: 550,
        width: 852,
        modal: true,
        close: function () {
            hideSessions();
            dialogSessions.dialog("close");
        },
        dialogClass: 'dialog-title-background'
    });

    //initialize event techno log tags dialog to select from multi select list on dialog.
    dialogEventTechnoLogTags = jQuery("#dialog-form-eventTechnoLogTags").dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        modal: true,
        close: function () {
            dialogSessions.dialog("close");
        },
        dialogClass: 'dialog-title-background'
    });

    dialogEventAudienceTypeTags = jQuery("#dialog-form-eventAudienceTypeTags").dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        modal: true,
        close: function () {
            dialogSessions.dialog("close");
        },
        dialogClass: 'dialog-title-background'
    });

     //open eventtechnologtags dialog on event technolog tags anchor click from view page.
     $scope.openTechnoLogDialog = function() {
        dialogEventTechnoLogTags.dialog("open");
     };

     $scope.openAudienceTypeDialog = function () {
         dialogEventAudienceTypeTags.dialog("open");
     };
    });
}]);