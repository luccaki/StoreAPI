# Store API

# Web API Project

 - CRUD for Company, Store and Product table
 - EntityFramework as ORM
 - Swagger for OpenAPI documentation
 
#### Running:
 Can run locally using Docker, ```docker build . --tag webapi``` and ```docker run -p 1357:8080 webapi```.
 
 API will be available at ```localhost:1357```

#### Postman Collection:
  For testing: ```StoreAPI.postman_collection.json```
 
# Test Project
 - TDD, tests for the creation of Service layer inside Web API

# Functions Project
- Azure Functions for CRUD only in Product table
- 
#### Runing:
Unfortunatelly Azure Functions is not available in the Free Tier, just ran locally.

Need to install ```Azure Functions Core Tools```, and run command ```func start``` inside the project directory

#### Documentation:
Can be found at: https://documenter.getpostman.com/view/14465268/2sA3kYjfRD

# Azure

 - AppServices: Created two web app, one for staging and one for production
 - SQL databases: Created two with distinct SQL servers, staging and production
 - API Management: Could be implemented as gateway, but its not available in the Free Tier

# Github Actions

- Build and deploy a container to Azure Web App: Workflow to build container image and deploy it to staging App Service, triggered when changes in ```master``` brach is made.
- Deploy to Production: Manually triggered workflow, to get last image and deploy it to production
