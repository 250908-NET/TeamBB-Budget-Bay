# Team-BB---Budget-Bay

## Synopsis

Budget Bay is an e-commerce web application designed for online auctions, enabling users to buy and sell items through bidding. Clients can easily create an account, log in securely, and explore available listings. They have the ability to post their own items for auction, place bids on other items, and track bidding activity in real time. Users can also manage their accounts, review their bidding history, and update personal details. Additionally, the platform features a powerful search function, allowing users to quickly find items by name or category, ensuring a smooth and intuitive shopping experience.


## Features

* As a user, I want to browse and filter auctions by category, price, and status(active/closed).
* As a seller, I want to create an auction listing by title, description, starting price, end date, and category, so that I can sell my items to the highest bidder.
* As a user, I want to sign up so I can have an account.
* As a buyer, I should be able to place a bid on a product
* As a user, I want to be able to login to view products and place bids

## Entity Relationship Diagram (ERD)

![alt text](Images/Screenshot_30-9-2025_142643_.jpeg)

## Wireframe Diagram

![alt text](Images/image.png)

## Unit Test Coverage

![alt text](Images/Coverage.png)

## Api Endpoints Documentation


All routes below are relative to this base.

---

## 🔐 Authentication

| Method | Endpoint | Description | Request Bodyy / Params | Return |
|:--|:--|:--|:--|:--|
| `POST` | `/Auth/register` | Register a new user | **RegisterUserDto**<br>`{ username, email, password }` | – |
| `POST` | `/Auth/login` | Log in a user | **LoginUserDto**<br>`{ email, password }` | "JWT Token" |

---

## 🧾 Products

| Method | Endpoint | Description | Request Body / Params | Return |
|:--|:--|:--|:--|:--|
| `GET` | `/Product` | Get all products | – | **List\<Product\>** |
| `POST` | `/Product` | Create a new product | **CreateProductDto**<br>`{ name, description, imageUrl, condition, endTime, sellerId, startingPrice, currentPrice? }` | **Product** |
| `GET` | `/Product/{id}` | Get a product by ID | `id` *(integer, path)* | **Product** |
| `DELETE` | `/Product/{id}` | Delete a product by ID | `id` *(integer, path)* | – |
| `PUT` | `/Product/{productId}` | Update a product | `productId` *(integer, path)*<br>**UpdateProductDto**<br> `{ name?, description?, imageUrl?, condition, endTime?, startingPrice?, currentPrice? }` | **Product** |
| `GET` | `/Product/search?q={query}` | Search for products by name or category | `q` *(string, query)* | **List\<Product\>** |

---

## 💰 Bids

| Method | Endpoint | Description | Request Body / Params | Return |
|:--|:--|:--|:--|:--|
| `GET` | `/bids` | Get all bids | – | **List\<Bid\>** |
| `GET` | `/Products/{productId}/bids` | Get all bid price and the related username for a product | `productId` *(integer, path)* | **List\<UserBidDto\>** *(username, amount)* |
| `POST` | `/Products/{productId}/bids` | Create a bid on a product | `productId` *(integer,path)*, **BidDto**<br>`{ Amount, BidderId }` | **BidDto**<br>`{Amount, ProductId,BidderId}` |
| `GET` | `/Products/{productId}/bids/highest` | Get the highest bid for a product | `productId` *(integer, path)* | **Bid** |
| `DELETE` | `/Products/{productId}/user/{userId}/bids` | Delete a user's bid for a product | `productId`, `userId` *(integer, path)* | – |

---

## 👤 Users

| Method | Endpoint | Description | Request Body / Params | Return | 
|:--|:--|:--|:--|:--|
| `GET` | `/Users/{id}` | Get user by ID | `id` *(integer, path)* | **UserDto** `{Username?,Email?,ProfilePictureUrl?}`|
| `PUT` | `/Users/{id}` | Update user info that has the select id | **UpdateUserDto**<br>`{Username?,Email?}` | **UserDto** `{Username?,Email?,ProfilePictureUrl?}` |
| `GET` | `/Users/{id}/bids` | Get all bids by a user | `id` *(integer, path)* | **List\<BidDto\>** `{Amount?,ProductId?,BidderId?}` |
| `GET` | `/Users/{id}/products` | Get all products listed by a user | `id` *(integer, path)* | **List\<UpdateProductDto\>** `{Name?,Description?,ImageUrl?,Condition,EndTime?,StartingPrice?,CurrentPrice?}` |
| `GET` | `/Users/{id}/won-auctions` | Get auctions won by a user | `id` *(integer, path)* | **List\<BidDto\>** `{Amount?,ProductId?,BidderId?}` |

---

## 🏠 Addresses

| Method | Endpoint | Description | Request Body / Params | Return |
|:--|:--|:--|:--|:--|
| `GET` | `/Users/{userId}/address` | Get a user's address | `userId` *(integer, path)* | **AddressDto** `{StreetNumber?,StreetName?,AptNumber?,City?,State,ZipCode?}` |
| `POST` | `/Users/{userId}/address` | Create a user's address | `userId` *(integer, path)* **AddressDto** `{StreetNumber?,StreetName?,AptNumber?,City?,State,ZipCode?,Country?}` | **AddressDto** `{StreetNumber?,StreetName?,AptNumber?,City?,State,ZipCode?,Country?}` |
| `PUT` | `/Users/{userId}/address` | Update a user's address | `userId` *(integer, path)* **AddressDto** `{StreetNumber?,StreetName?,AptNumber?,City?,State,ZipCode?,Country?}` | **AddressDto** `{StreetNumber?,StreetName?,AptNumber?,City?,State,ZipCode?,Country?}` |

---


