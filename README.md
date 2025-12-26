## WHAT IS THIS REPOSITORY?

This is an ASP.NET Core server application that provides an online learning platform with courses/lectures/tests, easy authorization and authentication with email confirmation codes. The application has a clean architecture, using gRPC and Kafka for communication between microservices, HTTP endpoints, and integrating ElasticSearch and Redis for individual services and methods. Prometheus is used for metrics in each service. There are also xunit tests for testing certain services.

## HOW TO WORK WITH IT?

To send codes by mail, change the data in the appsettings.json Email Service to your own. Next, write "docker-compose up -d" in the containers folder, and now you can safely work with this repository!
