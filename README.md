
# sws-ayush-demo

Here are the step by step instruction to run this project.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Cloning the Repository](#cloning-the-repository)
  - [Opening the Solution](#opening-the-solution)
  - [Restoring Dependencies](#restoring-dependencies)
  - [Running the Application](#running-the-application)
- [Accessing Swagger](#accessing-swagger)
- [API Usage](#api-usage)
- [Running Unit Tests](#running-unit-tests)

## Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- A code editor (e.g., [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/))

## Getting Started

### Cloning the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/ayushjoshi19/sws-ayush-demo.git
cd sws-ayush-demo\AyushDemo
```

### Opening the Solution

Open the solution file (`.sln`) in your preferred IDE or editor. If you're using Visual Studio, simply double-click the `.sln` file. If you prefer the command line, you can use the `dotnet` command:

```bash
dotnet sln AyushDemo.sln
```

### Restoring Dependencies

Before running the application, restore the necessary dependencies:

```bash
dotnet restore AyushDemo.sln
```

### Running the Application

You can run the application directly from the command line:

```bash
dotnet run --project Apis\ProductApi.csproj
```


## Accessing Swagger

you can access  Swagger for API documentation here:


<http://localhost:5214/swagger/index.html>


## API Usage

To interact with the API, you need to include the `X-SWS-Header` in your requests. The header should have the following name and value:

```
X-SWS-Header: 123
```

### Example cURL Command

Here's an example of how to make a request using `cURL`:

```bash
curl -X 'GET' 'http://localhost:5214/api/Product' -H 'accept: */*' -H 'X-SWS-Header: 123'
```
## Running Unit Tests

To run the unit tests in your solution, navigate to the test project folder or use the solution file:

```bash
dotnet test AyushDemo.sln
```


### Viewing Test Results

After running the tests, you will see the results in the command line, including any failed tests and their output.