# User Management System

This is an ASP.NET Core web application backed by Entity Framework Core, which facilitates management of fictional users. The application uses an in-memory database, so changes will not be persisted between executions.

## Description

The User Management System provides functionalities for adding, updating, viewing, and managing users. It's designed for administrators who need to manage user accounts and logs. The application also captures log information regarding primary actions performed on each user, with the ability to search through log entries.

## Documentation

The code is documented using Doxygen. You can view documentation [here](https://greyarea-66.github.io/TechTest/index.html)
To generate the documentation:

1. Install Doxygen.
2. Run `doxygen` in the terminal from the root directory of the project.
3. Open `./html/index.html` in a web browser to view the documentation.

## Installation

1. Clone this repository to your local machine.
2. Open the solution in Visual Studio (Community Edition is sufficient).
3. Build the solution to restore NuGet packages and compile the project.

## Usage

Run the project from Visual Studio. Use the provided user interface to add, update, view, and manage users. The users page contains filters to show all users, active users, or non-active users.

## Features

- **User Filters**: Show all users, active users, or non-active users.
- **User Management**: Add, view, edit, and delete users. The user model includes a DateOfBirth property.
- **Data Logging**: View a list of all actions performed against a user. Access a Logs page containing a list of log entries across the application. Able to search log entries based on UserID, Action or Timestamp range.
- **Asynchronous**: Applied asynchronous operations to service layers.

## Additional Notes

- New unit tests added for each module. Run dotnet test.
