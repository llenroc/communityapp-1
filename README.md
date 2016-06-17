# communityapp
contains code for the community app (India Tech Community on store)

# To Run this app
-> Please update configuration details in app.config under the projects
-> Update Azure Mobile Service Client Connection String in Mobile App Projects. Android: AuthenticationHelper.cs -> Connection string for Azure Mobile Services. Change Connection String for Notifications Hub -> Techready.Portal -> Helpers -> NotificationsHubHelper.cs  

# App Features

# User Registration

-	A user can register by using either a Microsoft Account, a Facebook Account or a Twitter Account. 
-	After authentication, App determines the current location of the user using device geolocation. 
-	By using current locations latitude and longitude app find out the nearest location for the user in backend. In application backend there are some Master Locations created with their co-ordinates and app finds out nearest location from backend based on current location of user. 
-	Following details are captured as part of registration process:
o	Full Name
o	Email Address
o	Nearest Location (Region)
o	Current Location Co-ordinates
o	User Type: One of Following: Developer, IT Pro, Student, Key Decision Maker, Architect/Consultant
o	User Organization: One of Following values: Consulting Firm, ISV, SI, Large Enterprise, SME, Startup, IT Services Provider
o	Technologies a user is interested in: This can be multiple values; List of Technologies is shown to user from a backend table. 
-	Once a user is saves / confirms the user profile. User is registered with Push Notifications system in backend with Following Tags:
o	Technologies
o	UserId
o	UserType
-	User will now get notifications and inbox messages whenever a new event or news update is created for any of technology or user type tags. 

# Events (Attend Section)

-	Post registration and upon subsequent launches of app, User is taken to the Hub Screen, with Events (Attend) as the first section (tab)
-	Here a user is shown events recommended for him based on following sort logic. 
o	Featured Event/ Global Event
o	All Events – Same City – Based on relevant Tech and Role
o	1st Party
o	Community
o	3rd Party/ Industry
o	All Event – Nearby Cities – Based on relevant Tech and Role
o	Webinar
o	Student doesn’t see any other profile’s 1st party Event. But see’s community and 3rd party event based on relevant tech. Also sees webinars 
-	If there are more than 5 events in system a View All Button is shown to the user in the end. 
-	On Clicking view all, user can see all Upcoming events and Filter Events option is enabled. 
-	When a user clicks on filter events, App shows another screen from where user can filter events based on : User Type, Technology or Location. 
-	User also gets an option of resetting the screen to view only the recommended events. 

# Event Details

-	From event list, App allows a user to view details for any event. 
-	Following details are shown when a user taps on event. 
o	Event Name
o	Venue
o	Location
o	Start and End Time
o	Event Description
o	Tracks (If the event is having multiple tracks). App provides a Drop Down / Combo Box to select a track for viewing details.  
o	Track’s Agenda, which includes
	Date
	Session Name 
	Session Start and End Time
o	Track Speakers, which includes
	Speaker Name
	Speaker Title
	Speaker Profile Abstract
-	A user can tap on track session to view details about the session. Following details are shown for a session
o	Session Name
o	Session Abstract
o	Start and End Time
o	Venue / Hall 
o	Speaker Name, Tile and Profile Abstract
-	A user can tap on speaker from either the event details screen or session details screen to view speaker details
-	From event details app allows a user to register for the event by tapping on register button which is displayed prominently on the screen. 
-	App allows a user to follow an event. When a user follows an event, he will get notifications regarding event. 

# Register for Event

-	From event details app allows a user to register for the event by tapping on register button which is displayed prominently on the screen. 
-	When a user presses register for event, App takes the user to a page via an in App browser to the event registration link provided in backend when event was created. User can register for the event using the page available via this link. 
-	When user presses the back, App shown a message to user: “ Do you want to follow this event to receive notifications”. If user replies in confirmation, event is marked as being followed by the user. 

# Learning Resources (Learn Section)

-	On Backend Administrators can provide Channel9 or MVA RSS Feed links for Technology and Audience Types. 
-	App has a backend component which runs every day once and pulls out all resources provided via those RSS Links. These resources are saved in the app backend as Learning resources. Following details are saved in backed for each learning resource. 
o	LearningResourceID (Unique No to identify a resource)
o	Title
o	Description
o	Thumbnail Uri
o	User Type
o	Technology
o	Published Date & Time 
o	Link
-	When a user navigates to Learn Section, All recommended learning resources are fetched from backed and show to user based on his technology and user type tags. 
-	If there are more than 5 resources, app provides a View All Button, which allows a user to view all learning resources relevant for him.
-	App provides a Filter button when user is in view all mode. On pressing filter App shows a screen from where a user can filter resources based on : User Type, Learning resource type(MVA./Channel9) or Technology. 
-	App provides a reset button to user for reverting the view state to recommended learning resources only.
-	A user can bookmark a learning resource. 
-	On tapping on learning resource, App takes the user to learning resource page by using learning resource link Uri. 


# Notifications (Inbox Section)

-	Backend allows an administrator to create notification messages for users. 
-	An administrator can provide: A Title, 4 Line Description and details link when he creates a push messages via backend. 
-	When a user navigates to Inbox, apps shows all the messages pushed to the user.
-	Initially when a message is pushed it is shown as Un-Read. 
-	User can tap on the message to read it or dismiss it. 
-	App saves in the backend a flag regarding message being read by user.  
-	When a user reads a message, it is marked as read and user is navigated to a page which shows message details in an in app browser via the link. If there was a link attached to the message. 

# Speakers

-	When a user navigates to speakers section, User gets to view all speakers added in the backed. 
-	The List Show Speaker Name, Title, and Profile Abstract
-	A user can search for a speaker based on his name. 
-	App allows a user to view speaker details by tapping on a speaker tile. 
-	Following details are shown for every speaker
o	Profile Pic
o	Name
o	Title 
o	Speaker Profile Description
o	Twitter Handle
o	LinkedIn Profile Link
o	Location
-	When in speaker details screen user can navigate to speaker’s blog. Which shows the blogs of a speaker by using the RSS link (provided in backend). 
-	Following information is shown in blog list: Title, Description, Publish Time. 
-	User can tap on blog and app navigates user to blog page from where he can read the blog. 
-	When in speaker details screen user can navigate to a page which show upcoming events for this speaker
-	If a user is interested in receiving notifications for the speaker (Notifications about new events for speaker), he can also follow the speaker by pressing follow speaker button from speaker details page. 

# Followed Speakers
-	This page list all the Speakers user is following and works like speakers list page. 
-	User can view speaker details for a speaker by tapping on it. 

# Followed Events
-	This page list all events followed by user.  (All Upcoming events and events which have been completed in last week)
-	User can view events details page for a event by tapping on it

# Bookmarked Learning Resources (Fav learning resources0
-	This page lists all learning resources which are marked as favorite by user.
-	User can view the learning resource page (via link) by tapping on it
-	User can remove learning resource from favorites. 

# Settings

-	User can enable to disable push notifications from here. 
-	User can navigate to about app page from here. 
-	User can navigate to Profile Page from here. 


# Profile
-	This works like user registration page. 
-	User can update his registered profile from here:
o	Full Name
o	Email Address
o	Nearest Location (Region)
o	Current Location Co-ordinates
o	User Type: One of Following: Developer, IT Pro, Student, Key Decision Maker, Architect/Consultant
o	User Organization: One of Following values: Consulting Firm, ISV, SI, Large Enterprise, SME, Startup, IT Services Provider
o	Technologies a user is interested in: This can be multiple values; List of Technologies is shown to user from a backend table. 



