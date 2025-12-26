## WHAT IS THIS REPOSITORY?

This is an ASP.NET Core Backend application that is a platform for online learning with courses/lectures/tests, comfortable authorization and authentication with sending confirmation codes to the mail. The application has a clean architecture, for communication between microservices it uses gRPC and Kafka, also for separate services and methods ElasticSearch and Redis are integrated, and Prometheus is used for taking metrics in each service. There are also xunit tests for testing some services.

## HOW TO WORK WITH IT?

To send codes by mail, change the data in the appsettings.json Email Service to your own. Next, write "docker-compose up -d" in the containers folder, and now you can safely work with this repository!
