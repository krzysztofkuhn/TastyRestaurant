## TastyRestaurant API

This is an online Restaurant API application.
It was created using ASP.NET Core Web API.

## Prerequisites

### .NET
1. [Install .NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
2. [* Install cURL]- for Windows] (https://curl.se/download.html)

### Running the application
1. clone or download the repository
2. go to project root folder (TastyRestaurant.sln file is located here)
3. run following command from your default terminal to start Web API
```
    dotnet run -lp https --project TastyRestaurant.WebApi
```
This will run the application with the `https` profile.
4. during first application startup it will create a new Sqlite file for application data (TastyRestaurant.db)
5. after successful build the console should contain following result:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7258
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5196
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\TastyRestaurant\TastyRestaurant.WebAp
```
6. now the API should be available on https://localhost:7258 and http://localhost:5196 addresses

### Using the API standalone
The TastyRestaurant REST API can run standalone. 
You can run the WebAPI project and make requests to various endpoints using the Swagger UI (or a client of your choice).
Go to https://localhost:7258/swagger/

<img width="1200" alt="image" src="https://user-images.githubusercontent.com/95136/204315486-86d25a5f-1164-467a-9891-827343b9f0e8.png">

Before executing any requests, you need to create a user and get an auth token.

1. To create a new user, run the application and POST a JSON to endpoint:
`https://localhost:7258/api/v1/users/register`

    ```json
    {
        "email": "user@example.com",
        "password": "somePass123",
        "firstName": "John",
        "lastName": "Doe",
        "phoneNumber": "+48100100100",
        "dateOfBirth": "1980-09-16"
    }
    ```
cURL:
```
curl -X 'POST' \
  'https://localhost:7258/api/v1/users/register' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '    {
        "email": "user@example.com",
        "password": "somePass123",
        "firstName": "John",
        "lastName": "Doe",
        "phoneNumber": "+48100100100",
        "dateOfBirth": "1980-09-16"
    }'
```

2. Then we need to login to application - send POST request to:
`https://localhost:7258/api/v1/users/login`
    ```json
    {
        "email": "user@example.com",
        "password": "somePass123"
    }
    ```

cURL:
```
curl -X 'POST' \
  'https://localhost:7258/api/v1/users/login' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "user@example.com",
  "password": "somePass123"
}'
```

As a result you should get a JWT Token:
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzMjViYTM1OS0wOTcxLTQ3MWItOTYzYi0zMTg0YmY1NmY1YzgiLCJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJleHAiOjE2OTQ5NTYxMzN9.ILsKRhF6jEX8_CEtFAhTx17AAqiSqhezx-qDO3IhLZM",
  "expiration": "2023-09-17T13:08:53Z"
}
```

3. In order to use order management endpoints the user needs to be authorized. 
-For swagger use the "Authorize" button in upper right corner above endpoints.
In the "Value" input put token received after Login, prefixed by the "Bearer" keyword:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzMjViYTM1OS0wOTcxLTQ3MWItOTYzYi0zMTg0YmY1NmY1YzgiLCJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJleHAiOjE2OTQ5NTYxMzN9.ILsKRhF6jEX8_CEtFAhTx17AAqiSqhezx-qDO3IhLZM
```

For cURL you need to include the token as a request header:
```
curl -X 'GET' \
  'https://localhost:7258/api/v1/menuitems?searchNamePhrase=steak' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzMjViYTM1OS0wOTcxLTQ3MWItOTYzYi0zMTg0YmY1NmY1YzgiLCJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJleHAiOjE2OTQ5NTYxMzN9.ILsKRhF6jEX8_CEtFAhTx17AAqiSqhezx-qDO3IhLZM'
```