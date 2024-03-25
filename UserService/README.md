## User Service Endpoints Overview

This section provides an overview of the main endpoints offered by the UserService API, detailing their functionalities and request/response formats.

1) **AddUser**
   - **Description:** This endpoint facilitates the addition of a new user.
   - **Endpoint:** `/api/userservice/users`
   - **Verb:** POST
   - **Request:**
     ```json
     {
       "name": "string",
       "email": "string",
       "password": "string",
       "mobile": "string"
     }
     ```
   - **Response:** 
     ```json
     {
       "userId": 0
     }
     ```

2) **GetUser**
   - **Description:** This endpoint retrieves details of an existing user.
   - **Endpoint:** `/api/userservice/users/{userId}`
   - **Parameters:** userId:int
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "userId": 0,
       "email": "string",
       "name": "string",
       "mobileNumber": "string"
     }
     ```

3) **GetUsers**
   - **Description:** This endpoint retrieves a list of users.
   - **Endpoint:** `/api/userservice/users`
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "users": [
         {
           "id": 0,
           "name": "string",
           "email": "string",
           "mobileNumber": "string"
         }
       ]
     }
     ```

4) **UpdateUser**
   - **Description:** This endpoint updates the details of an existing user.
   - **Endpoint:** `/api/userservice/users/{userId}`
   - **Parameters:** userId:int
   - **Verb:** PUT
   - **Request:**
     ```json
     {
       "id": 0,
       "name": "string",
       "email": "string",
       "mobileNumber": "string"
     }
     ```
   - **Response:** 
     ```json
     {}
     ```

5) **AuthenticateUser**
   - **Description:** This endpoint authenticates a user's login.
   - **Endpoint:** `/api/userservice/users/authenticate`
   - **Verb:** POST
   - **Request:**
     ```json
     {
       "email": "string",
       "password": "string"
     }
     ```
   - **Response:**
     ```json
     {
       "token": "string"
     }
     ```
