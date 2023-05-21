# SBOM API

This is a minimal API for managing Software Bill of Materials (SBOM) data. It uses the ExRam.Gremlinq library to interact with a Cosmos DB graph database.

## Setup

This API is built with .NET 6.0 and uses the new minimal API feature. To run the API, you'll need to have .NET 6.0 installed on your machine.

You'll also need access to a Cosmos DB instance. The API expects the Cosmos DB URI, database name, graph name, and authentication key to be provided as environment variables.

## Running the API

To run the API, use the `dotnet run` command in the root directory of the project.

## Endpoints

The API has one endpoint:

- `POST /spbx`: Uploads a new SBOM document. The document should be in the SPDX 2.2 format and should be sent in the request body.

## Authentication

The API uses JWT bearer authentication. To access the `POST /spbx` endpoint, you'll need to include a valid JWT in the `Authorization` header of your request.

## Authorization

The `POST /spbx` endpoint requires the `security.sbom.api.uploadsbom` scope. You'll need to include this scope in your JWT.

## Error Handling

If an error occurs while processing a request, the API will return a 400 Bad Request status code along with the error message.

## Logging

The API uses console logging to log information and error messages. You can view the logs in the console where you're running the API.
