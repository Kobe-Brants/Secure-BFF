# Secure BFF Repository 

This repository contains three projects to demonstrate a secure Backend for Frontend (BFF) architecture:

1. **API**: A .NET API that serves as the backend.
2. **Secure Token Provider**: A secure token provider implemented using Duende IdentityServer.
3. **React App**: A React application serving as the frontend.

## Table of Contents

- [Secure BFF Repository](#secure-bff-repository)
  - [Table of Contents](#table-of-contents)
  - [Project Structure](#project-structure)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Setup](#setup)
  - [API](#api)
    - [Running the API](#running-the-api)
  - [Secure Token Provider](#secure-token-provider)
    - [Running the Token Provider](#running-the-token-provider)
  - [React App](#react-app)
    - [Running the React App](#running-the-react-app)
  - [Extra](#extra)
  - [License](#license)

## Project Structure

```sh
secure-bff-repo/
├── api/
├── secure-token-provider/
├── react-app/
├── extra/
      └── Secure BFF.drawio
      └── bff duende/
└── README.md
```

## Getting Started

### Prerequisites

Ensure you have the following installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)

### Setup

Clone the repository:

```sh
git clone https://github.com/kobeo2/Secure-BFF
cd secure-bff-repo
```

## API

The .NET API serves as the backend for this project.

### Running the API
Navigate to the api directory and run the API:

```sh
cd api/
dotnet run
```

## Secure Token Provider
The secure token provider is implemented using Duende IdentityServer.

### Running the Token Provider
Navigate to the secure-token-provider directory and run the IdentityServer:

```sh
cd secure-token-provider/
dotnet run
```

## React App
The React application serves as the frontend for this project.

### Running the React App
Navigate to the react-app directory and start the development server:

```sh
cd react-app/
npm install
npm start
```

The React app will be available at https://localhost:4000.

## Extra

1. **Secure BFF.drawio**:
   - This file contains a visual diagram of the BFF architecture, showing the interactions between the .NET API, Duende IdentityServer, and the React app. Use [draw.io](https://app.diagrams.net/) to view or edit this diagram.

2. **BFF Duende**:
   - This folder contains a .NET application. It includes the frontend, backend api and the STS. [more information](https://docs.duendesoftware.com/identityserver/v7/bff/)
   - use `dotnet run`

   

## License
This repository is licensed under the MIT License. See the LICENSE file for more details.

