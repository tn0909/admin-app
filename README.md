# Administration Applicaton

This project is a full-stack application designed to create and search companies and their users. It leverages ASP.NET Web API for the backend, Angular for the frontend, and Elasticsearch for persistence and efficient searching capabilities.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Folder Structure](#folder-structure)
  - [Setting up the Backend](#setting-up-the-backend)
  - [Setting up the Frontend](#setting-up-the-frontend)
- [Elasticsearch Mapping](#elasticsearch-mapping)
- [API Endpoints](#api-endpoints)

## Features

- Create companies
- Create users associated with a company
- Search companies by name, description, or address
- Search users by email, associated company, or both

### Screenshots

1. Create a new company
![Create a new company](https://github.com/tn0909/tn0909/blob/master/Screenshots/AdminApp/Create%20Company.png)

2. Create a new user
![Create user](https://github.com/tn0909/tn0909/blob/master/Screenshots/AdminApp/Create%20User.png)

3. Search companies
![Search companies](https://github.com/tn0909/tn0909/blob/master/Screenshots/AdminApp/Search%20Companies.png)

4. Search companies and include users in the result
![Search companies with users](https://github.com/tn0909/tn0909/blob/master/Screenshots/AdminApp/Search%20Companies%20including%20Users.png)

5. Search users
![Search users](https://github.com/tn0909/tn0909/blob/master/Screenshots/AdminApp/Search%20Users%20by%20company.png)

## Prerequisites

- .NET SDK
- Node.js and npm
- Docker
- Visual Studio Code

## Getting Started

### Folder Structure

```plaintext
admin-app/
├── AdminApp/        --> API project
│   ├── Controllers/
│   ├── Dtos/
│   ├── Models/
│   ├── Services/
│   └── ...
└── AdminAppUi/      --> UI project
    ├── src/
    │   ├── app/
    │   │   ├── components/
    │   │   ├── models/
    │   │   ├── services/
    │   │   └── ...
    └── ...
 ```

### Setting up the Backend

1. Start Elasticsearch container:
   ```bash
   docker run -d --name elasticsearch \
    -p 9200:9200 -p 9300:9300 \
    -e "discovery.type=single-node" \
    docker.elastic.co/elasticsearch/elasticsearch:7.17.0
   ```
3. Clone the repository:

    ```bash
    git clone https://github.com/tn0909/admin-app.git
    ```

4. Navigate to the ASP.NET Web API project:

    ```bash
    cd admin-app/AdminApp
    ```

5. Restore the NuGet packages:

    ```bash
    dotnet restore
    ```

6. Run the API:

    ```bash
    dotnet run
    ```

7. The API will be accessible at `http://localhost:5076`.

### Setting up the Frontend

1. Navigate to the Angular UI project:

    ```bash
    cd admin-app/AdminAppUI
    ```

2. Install the dependencies:

    ```bash
    npm install
    ```

3. Run the Angular application:

    ```bash
    ng serve
    ```

4. The app will be accessible at `http://localhost:4200`.

## Elasticsearch Mapping

Parent-child relation between company and user documents.

```
    {
        "mappings": {
            "_routing": {
                "required": true
            },
            "properties": {
                "id": {...},

                // company properties
                "name": {...},
                "website": {...},
                "address": {...},
                "description": {...},

                // user properties
                "email": {...},
                "firstName": {...},
                "lastName": {...},
                "title": {...},

                // parent-child relation
                 "joinField": {
                    "type": "join",
                    "eager_global_ordinals": true,
                    "relations": {
                        "parent": "user"
                    }
                }
            }
        }
    }
```

## API Endpoints

The API documentation is available at `http://localhost:5076/swagger/index.html`. Use this interface to explore available endpoints and test API functionality.

1. Create a new company
```
POST /api/companies
{
  "name": "string",
  "description": "string",
  "address": "string",
  "website": "string"
}
```

2. Search companies by name, address or description (full-text), return all if no search term is specified
```
// Request
POST /api/companies/search
{
  "searchTerm": "string",
  "limit": 0,
  "includeUsers": true,
  "usersLimit": 0
}

// Response
[
  {
    "id": "string",
    "name": "string",
    "description": "string",
    "address": "string",
    "website": "string",
    "users": [
      {
        "id": "string",
        "email": "string",
        "firstName": "string",
        "lastName": "string",
        "title": "string",
        "companyId": "string"
      }
    ]
  }
]
```

3. Create a user
```
POST /api/users
{
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "title": "string",
  "companyId": "string"
}
```

4. Search users by company or email, return all if no search params is specified
```
// Request
POST /api/users/search
{
  "company": "string",
  "email": "string",
  "limit": 0
}

// Response
[
  {
    "id": "string",
    "email": "string",
    "firstName": "string",
    "lastName": "string",
    "title": "string",
    "companyId": "string"
  }
]
```

## Contributing
Contributions are welcome! If you have suggestions for improving the API or adding new features, please open an issue to discuss or submit a pull request with your changes.

## License
Please credit the author if you use this project or any of its components in your own work.

## Acknowledgments
Libraries and frameworks used
