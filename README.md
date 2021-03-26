
# OGAME Development Projects

This repository contains several personal projects that I have written C# using .NET 3.5 Framework and which are part of a bigger project callled **OGameOneAdmin**.

I developed these projects back in 2012 purely by passion and interest in learning more about technologies and programming. I was the sole person involved in all dev aspects and I was only coding in my spare time. It was more of an experimental project that grew bigger than I had anticipated.

**All the projects are live for demo purposes only:**
- The database (hosted on Azure SQL Server)
- The backend website "[Celestos](https://celestos.azurewebsites.net/)" (hosted on Azure using the App Service)
- The soap webservice "[OGameServiceV1](https://ogameservicev1.azurewebsites.net/Service.asmx)" (also on Azure)
- The client application (also referred as a Tool in the ecosystem) "[OGameOneAdmin v1.4](https://celestos.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin+v1.4.exe)" (hosted on a S3 bucket)

In a few words, I built the back-end system (as a prototype) to become an 'Admin Management Platform' for the Game Industry. A system that can be used by a Game Company to manage their admin ecosystem (admin users, server management, customer support and more) using a centralized platform with a set of tools. 

The platform provides full rights to the game company to configure and manage the entire ecosystem for different Games and multiple Communities (a country running multiple game servers). In this scenario, the game company Gameforge who operates dozens of games in over 30 regions would benefits from this system to manage the administrative side of their games. Each game (like OGame) can be deployed in 30+ regions and involves thousands of volunteers who perform admin tasks on a day-to-day basis.

### Architecture

* Website: ASP.NET AJAX (Webforms + Telerik UI) (the backend platform)
* SOAP Web Services (hosted onto 3 separate servers/regions)
* SQL Server Database (1 centralized database)
* WinForms Application (Client)

### Repos
#### ASP.NET Website (the backend platform)

Celestos Website: Celestos is just the name of the backend platform

[https://github.com/StephaneChevassus/Celestos](https://github.com/StephaneChevassus/Celestos)

(Dependency to LibCommonUtil)

Database schema:

[https://github.com/StephaneChevassus/Celestos/tree/master/Database](https://github.com/StephaneChevassus/Celestos/tree/master/Database)

#### SOAP Web Services

OgameServiceV1:

[https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/OgameServiceV1](https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/OgameServiceV1)

(Dependency to LibCommonUtil)

The client "OGameOneAdmin" WinForms app uses this endpoint to communicate with the backend platform "Celestos Website".


#### WinForms Application

OGameOneAdmin:

[https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/OGameOneAdmin](https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/OGameOneAdmin)

(Dependency to LibCommonUtil)
(Dependency to OgameServiceV1)
(Dependency to GF.BrowserGame)

It is the core application, the one admin tool used by the game operators and all other game admin users.

#### GF.BrowserGame

GF.BrowserGame:

[https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/GF.BrowserGame](https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/GF.BrowserGame)

(Dependency to LibCommonUtil)

This is a shared library that I wrote from scratch and which encapsulate all communication with the OGame game server and ticket platform.
I created this library in order to re-use the code in the future if I wanted to build a different UI / client application.

#### LibCommonUtil

LibCommonUtil

[https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/LibCommonUtil](https://github.com/StephaneChevassus/OGameOneAdmin/tree/master/LibCommonUtil)

This is a shared library containing helpers (Encryption, Networking, WebBrowser and many more). It is very practical and is used by all my projects.

## What is OGame?

OGame is a browser-based online game produced by Gameforge which has enthralled over 70 million players in 32 languages on 500 servers worldwide. 

There are thoushands of volunteers (**Game Operators**) around the world that dedicate their time to provide game support to the community and players. Each game instance is hosted on a dedicated server and must be administered from the admin panel of that specific server using a *php web interface*. There are hundreds of servers, which result in managing and allocating thoushands of admin login access.

It usually requires serveral admins (**Game Operators**) per game server to manage the good operation of the gameplay and make sure that the rules are well respected.
Depending on your admin level (**Community Manager, Game Admin, Super Game Operator, Operator**), you may have access to several servers to perform your duties.

**Game Operators** (GOs) duties range from making sure that players don't cheat to providing general gameplay help. A **Game Operator** (GO) uses the admin panel of a game server to perform tasks using different set of tools which simply retrieve and display logs based on search criterias like:

* IP logins logs
* Logs for each type of interaction within the game (Player A is attacking Player B)


**GOs** are trained to understand and interpret these logs but there is so many logs that a human being is able to process.

>  This was the starting point of building third party tools to save **Game Operators** time

I then started to create various WinForms tools that allowed **GOs** to copy/paste logs into the application which would automatically parse and interpret the logs.

On top of accessing game servers, each **GO** has a login access to the ticket support platform that runs again independently. When a player needs to get in touch with a game operator, he must submit a ticket using the ticket support platform.

**All these administrative tasks result in having multiple web browser windows opened, login in to several servers, switching between the ticket platform and the game servers constantly to perform any kind of admin operations. It involves a lot of coping/pasting/searching/clicking while 90% could be done programmatically** 

> This is where the idea of **OGameOneAdmin** was born

## What is OGameOneAdmin?

**OGameOneAdmin** is complete end-to-end solution to integrate and perform all game administrative tasks (gameplay & support ticket) for ***OGame*** within one centralized admin tool as opposed to having to access hundreds of independent admin panels and a separate support ticket platform.

**OGameOneAdmin** needed to be fully managed and customizable via a backend platform in order to control every fine details. Each country being independent from each other, the backend platform had to allow each community to configure the eco system for the application to live and function.

Each community (country) must:
* Create list of servers (name, url, game details)
* Invite all staff to register on the platform
* Allocate staff to a role which has a set of permissions to access the platform
* Allocate staff to a list of servers based on their role and allocation
* Allocate tool features to roles in order to manage permission access
* Give users permission to use the tool (based on roles, tool version)
* Monitor tool usage

A staff can belong to one or more community and have several different roles.

#### Brief Description of the tool (WinForms client)

Once the platform is configured and a user download the client application. The user first authenticates with the platform so the tool can download his profile which contains everything based on this access level and allocation. At runtime, the tool shows only the features allocated to that user.

Within the tool, the user can save all his logins access to as many servers as he is allowed to access + the ticket support platform. The tool allows the user to automatically login to all his game admin servers and it maintains the session. He can also perform most of his duties within the tool without the need to open the web browser and go to the web interface of a game admin panel. All credentials are encrypted and stored on the cloud which means that if the user access the tool from another computer, he won't have to re-enter any credentials form any game servers.

If the user needs to access the real admin panel of a given server, the tool will automatically open the web browser, extract the cookies from the app and inject the cookies into the web browser using javascript (Tampermonkey) so the session is maintain without the need to re-login.

Tampermonkey script: [ooa.import.cookies.v1.user.js](https://github.com/StephaneChevassus/OGameOneAdmin/blob/master/OGameOneAdmin/Resources/ooa.import.cookies.v1.user.js)

## Screenshots

### Celestos ASP.NET Website (the backend platform)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/celestos.png "IMAGE NAME")

### Server Game Admin (PHP Interface)

Unfortunately I can't share screenshots of the official Game Admin interface, however you can see from the implementation that it required to create Web Crawler Agent to access & manipulate server data.

File of interests are:
- [Internal WebClient](https://github.com/StephaneChevassus/OGameOneAdmin/blob/master/GF.BrowserGame/Utility/OGameWebClient.cs)
- [Url mapping](https://github.com/StephaneChevassus/OGameOneAdmin/blob/master/GF.BrowserGame/GameUri.cs)
- [Post data generation](https://github.com/StephaneChevassus/OGameOneAdmin/blob/master/GF.BrowserGame/Static/GamePostData.cs)
- [Html Parsing](https://github.com/StephaneChevassus/OGameOneAdmin/blob/master/GF.BrowserGame/Static/ParseHtml.cs)

### OGameOneAdmin - latest version 1.4

After the beta version, I've redone all the UI components of the app. I needed to break down small parts of the UI into reusable components in order to remove duplicated UI/functionality across features.

**Dashboard**

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-dashboard.png "IMAGE NAME")

**Ticket Support**: tickets are submitted via a separate platform which has a different set of credentials than the admin panel from the game server. I integrated the ticket support within the tool to remove the need to use a web browser to view tickets and it allowed me to match player's directly to their game account automatically within the app. In reality, you would need to copy the player's email address, login to the corresponding game server and search for the player's profile.

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-ticket-view1.png "IMAGE NAME")

**Options** 

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-options.png "IMAGE NAME")

**Multi:** a feature to find and analyze potential accounts belonging to the same player (=cheating)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-multi.png "IMAGE NAME")

**Stats:** a feature for GameAdmin to generate performance stats based on audit logs for the staff. It helps to evaluate how much work the staff does on a given server. All admin clicks and actions are recorded for audit purposes and to deter admin from giving inside info to players.

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-stats.png "IMAGE NAME")

### OGameOneAdmin - Beta Version

Once the Celestos platform was deployed, I released the beta version.

I have a series of screenshots but they do not demonstrate many features as they were taken at an early stage.

Dashboard (offline)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard.png "IMAGE NAME")

Dashboard (Online)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-online.png "IMAGE NAME")

Dashboard Menu 

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-menu1.png "IMAGE NAME")

Dashboard Menu

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-menu2.png "IMAGE NAME")

Dashboard Login with Internet Explorer

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-IE-login.png "IMAGE NAME")

Dashboard Quick Login

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-quick-login.png "IMAGE NAME")

Dashboard URL Login

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/dashboard-URL-login.png "IMAGE NAME")

Unlock the Application - You can lock the application so if you are sharing your computer with family members they cannot use the app (10 years ago it was common to share a computer!)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/unlock-application.png "IMAGE NAME")

General Note 1

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/general-notes1.png "IMAGE NAME")

General Note 2

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/general-notes2.png "IMAGE NAME")

General Note 3

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/general-notes3.png "IMAGE NAME")

General Note 4

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/general-notes4.png "IMAGE NAME")

### OGameOneAdmin - Alpha Version

The Alpha version was designed as a stand alone app which was not connected to any backend server. I have never released this version, it was a quick implementation of all the core features that I built for my personal use therefore the UI is not polished at all.

Ticket Support: automatically looks up the player's details on the game server to verify credentials such as email address.

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-ticket-support-integration1.png "IMAGE NAME")

Ticket Support: answer a ticket from within the tool

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-ticket-support-integration2.png "IMAGE NAME")

Generate automatically notes about the ticket enquiry and other details and save it in the player's profile on the game server. (Tickets get archived and deleted)

![IMAGE NAME](https://mygithubstatic.s3-ap-southeast-2.amazonaws.com/OGameOneAdmin/ooa-ticket-support-integration3.png "IMAGE NAME")

