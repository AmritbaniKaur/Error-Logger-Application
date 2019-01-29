
# Error Logger Application

 - The application consists of 3 parts – a REST webservice, a website and a library for logging.
 - The web application implements strong authentication.
 - The web application supports two roles – admin and user.
 - The web application collects the following data about each user:
	First Name
	Last Name
	Email
	Password
	Active/Inactive
	Last login date
 - The web application allows an admin to create a new application.
 - The web application allows an admin to assign a user to an application.
 - The web application allows an admin to disable an existing application.
 - Admin shall be able to access all applications and all data for them.
 - Upon login, user is presented with a list of their applications.
 - User’s access is restricted to only active applications to which they are allowed access.
 - A user is able to select, from this list a applications to view.
 - The web application presents the user with error log data in two forms:
 - List of errors, displays all of the data
 - A graphical representation of the errors (by category, over time, or a suitable combination).
 - The web application allows the user to filter, search and order the logs, on all categories/columns.
 - The selection of the filtered data impacts the graphical display.
 - The webservice receives error logs and save them into the database.
 - Extra work on performance.
 - Extra work on stability.
 - The library collects all necessary data and log them into the database.
 - Extra work on performance.
 - Extra work on stability.
 - The library is configured at application startup w/ the ID of the application for which we are saving.
 - The web application uses the logger for its own logging.
 - The application has a look and feel of a polished final product.
 - Extra work on UI.
 - Used Visual Studio or a text editor of your choosing for the HTML and CSS editing
 - Web applications is hosted in IIS.
 - REST service is hosted in IIS.
 - Used MVC .Net 5.0+ w/ Razer.
 - Used .Net 4.5+, with C#.
 - Used HTML, JS and CSS.
 - Stored almost all of your CSS in separate files (vs. inline styles/scripts).
 - Stored almost all of your JS in separate files (vs. inline styles/scripts).
 - Used our class DB server.
 - The application’s code is properly organized, supports reuse and testing.
 - The project is stable (no uncaught exceptions).
 - The project is secure (no unauthorized access).
 - The project does not reveal any technical details to the user.
 - The project does not use any external libraries, with the exception of libraries available via Nuget.
 - The project does implement an error logger, following best practices described in class.
 
----------------------------------------------------------------------------------
