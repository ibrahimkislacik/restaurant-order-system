Restaurant Order System
=======================
The Restaurant Order System is a web application designed to streamline the process of managing orders for a restaurant. It allows restaurant staff to efficiently receive, process, and manage customer orders, enhancing overall productivity and customer satisfaction.

Features
--------
*   **Order Management**: Easily manage incoming orders, view order details.
*   **Menu Management**: Maintain a menu with customizable items, prices, and descriptions.
*   **User Authentication**: Secure user authentication system to control access and ensure data privacy.

Version Control
---------------
This project uses Git for version control. The repository is hosted on GitHub, allowing for collaboration and easy tracking of changes.

Branching Strategy
------------------
Followed the Gitflow branching model for managing our branches:

*   **Master Branch**: The master branch contains stable, production-ready code. It should always reflect the latest release.
*   **Development Branch**: The development branch is where ongoing development takes place. Feature branches are merged into this branch for testing.
*   **Feature Branches**: Feature branches are created for new features or fixes. Once the feature is complete, a pull request is made to merge it into the development branch.

Development
-----------------
Our development process emphasizes modularity, maintainability, and scalability, leveraging the repository pattern for MongoDB data access and RESTful API architecture with service patterns.
### RESTful API
*   **Controller Layer**: API endpoints are organized into controllers, each responsible for handling requests related to a specific resource or functionality.
*   **Service Layer**: Business logic is encapsulated within service classes, following the service pattern. Services orchestrate interactions between controllers and repositories, promoting code reusability and maintainability.
*   **DTOs (Data Transfer Objects)**: DTOs are used to transfer data between the client and the server, providing a structured and standardized format for communication.
### Repository Pattern for MongoDB
*   **Data Layer**: MongoDB serves as our database, and we employ the repository pattern to abstract database interactions. Each data entity has its repository interface and implementation, allowing for separation of concerns.
*   **Models**: Data models are defined to represent MongoDB documents, ensuring consistency and clarity in data structure.
*   **Repositories**: Repository interfaces define CRUD operations and other data access methods, while concrete repository implementations interact with the MongoDB database using a MongoDB client library.

Testing and Quality Assurance
-----------------
A strong emphasis was placed on maintaining high levels of unit test coverage throughout the development process.

Technologies Used
-----------------
*   **Frontend**: Angular 17, HTML, CSS, TypeScript
*   **Backend**: .NET 7, Swagger for API documentation, MongoDB
*   **Authentication**: JSON Web Tokens (JWT)

Testing Libraries
-----------------
*   **Backend**: Xunit, Moq, Fluent Assertion
