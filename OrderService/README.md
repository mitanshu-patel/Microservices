1) **AddOrder**
   - **Description:** This endpoint adds a new order.
   - **Endpoint:** `/api/orderservice/orders`
   - **Verb:** POST
   - **Request:**
     ```json
     {
       "productOrders": [
         {
           "productId": 0,
           "quantity": 0
         }
       ],
       "userId": 0
     }
     ```
   - **Response:** 
     ```json
     {
       "orderId": 0
     }
     ```

2) **GetOrder**
   - **Description:** This endpoint retrieves details of an existing order.
   - **Endpoint:** `/api/orderservice/orders/{orderId}`
   - **Parameters:** orderId:int
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "orderDetails": {
         "customerName": "string",
         "orderId": 0,
         "totalPrice": 0,
         "orderDate": "2024-03-25T11:49:53.353Z",
         "productOrders": [
           {
             "productId": 0,
             "price": 0,
             "name": "string",
             "quantity": 0
           }
         ]
       }
     }
     ```

3) **GetUserOrders**
   - **Description:** This endpoint retrieves a list of orders placed by a specific user.
   - **Endpoint:** `/api/orderservice/users/{userId}/orders`
   - **Parameters:** userId:int
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "userOrders": [
         {
           "orderId": 0,
           "totalPrice": 0,
           "orderDate": "2024-03-25T11:50:35.994Z",
           "productOrders": [
             {
               "productId": 0,
               "price": 0,
               "name": "string",
               "quantity": 0
             }
           ]
         }
       ]
     }
     ```
