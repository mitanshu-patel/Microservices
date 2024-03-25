## Product Service Endpoints Overview

This section outlines the main endpoints provided by the ProductService API, including functionalities and usage instructions.

1) **AddProduct**
   - **Description:** This endpoint allows the addition of a new product.
   - **Endpoint:** `/api/productservice/products`
   - **Verb:** POST
   - **Request:**
     ```json
     {
       "name": "string",
       "price": 0,
       "description": "string"
     }
     ```
   - **Response:** 
     ```json
     {
       "productId": 0
     }
     ```

2) **GetProduct**
   - **Description:** This endpoint retrieves details of an existing product.
   - **Endpoint:** `/api/productservice/products/{productId}`
   - **Parameters:** productId:int
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "productDetail": {
         "productId": 0,
         "name": "string",
         "description": "string",
         "price": 0
       }
     }
     ```

3) **GetProducts**
   - **Description:** This endpoint retrieves a list of products.
   - **Endpoint:** `/api/productservice/products`
   - **Verb:** GET
   - **Request:** NA
   - **Response:** 
     ```json
     {
       "products": [
         {
           "productId": 0,
           "name": "string",
           "description": "string",
           "price": 0
         }
       ]
     }
     ```

4) **UpdateProduct**
   - **Description:** This endpoint updates the details of an existing product.
   - **Endpoint:** `/api/productservice/products`
   - **Verb:** PUT
   - **Request:**
     ```json
     {
       "productId": 0,
       "name": "string",
       "description": "string",
       "price": 0
     }
     ```
   - **Response:** 
     ```json
     {}
     ```
