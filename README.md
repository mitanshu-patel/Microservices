# Basic E-Commerce app using Azure functions with Microservice Architecture

This Azure Function app is built with a microservice architecture, featuring separate modules for users, products, and orders. Each module is encapsulated within its own service:

- **UserService**: Manages CRUD operations for users and handles user authentication.
- **ProductService**: Handles CRUD operations for products.
- **OrderService**: Manages order-related operations such as creating new orders, retrieving order details, and fetching a list of orders for a specific user.

## Architecture

Each service is developed using a combination of Clean Architecture and Domain-Driven Design (DDD) principles. This approach emphasizes separation of concerns, maintainability, and scalability.

### Data Access

Entity Framework is utilized for data access within each service, ensuring efficient and reliable interaction with the underlying data storage.

### Mediator-CQRS Pattern

The services follow the Mediator pattern with Command Query Responsibility Segregation (CQRS). This separation of concerns facilitates better command and query handling, enhancing application performance and scalability.

### Event Messaging with Azure Service Bus

Since the services have dependencies on each other and require data propagation, Azure Service Bus is leveraged for communication. Event-driven architecture using topics enables seamless integration and decoupling between services.

## Service Communication

The following topics and subscriptions are utilized for inter-service communication:

1. **userserviceevents**
   - Publishes messages related to users, including user details and authentication events.
   - **Subscriptions**:
     - *userserviceproduct*: ProductService subscribes to this to manage authentication tokens.
     - *userserviceorder*: OrderService subscribes to this to manage user-related details and authentication tokens.

2. **productserviceevents**
   - Publishes messages related to products, such as adding or updating product details.
   - **Subscriptions**:
     - *productserviceorder*: OrderService subscribes to this to manage product-related details.

## Getting Started

To set up and run the microservice Azure Function app locally, follow these steps:

1. Clone this repository.
2. Navigate to the root directory of each service (UserService, ProductService, OrderService).
3. Install dependencies using `dotnet restore`.
4. Configure Azure Service Bus connection strings and other environment-specific settings.
5. Run each service locally using `dotnet run`.
6. Access the HTTP endpoints provided by each service for testing and integration.

## Contributors

- Mitanshu Patel
