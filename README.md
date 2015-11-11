Todo App Web API
================

[![Build status](https://ci.appveyor.com/api/projects/status/80ijkun0t8v15dxw?svg=true)](https://ci.appveyor.com/project/chrismcbride76/todoapp-webapi)

This project aims to provide a RESTful back-end for a "TODO list" application.  A web or mobile client could use the service over HTTP to obtain data from the server.

Development
------------

To build and run the service:

1. Open Todo.sln in Visual Studio 2013
2. Ensure Todo.Api is selected as your startup project
3. Run the project
⋅⋅* Note: project dependencies should automatically get pulled in by nuget on first build

Making Requests
---------------

All API URLs start with `http://[domain]/api`, where `[domain]` is the domain name of the service.  When running through visual studio, this will be `localhost:10522'.  The port number may be different depending on your setup.

Any part of the API can be reached from the main entry point at `http://[domain]/api`.  The entry point lists the resources provided by the server, as well as how to access the rest of the api.

A GET request to `http://localhost:10522/api` would return:

```
{
  "_links": [
    {
      "href": "http://localhost:10522/api",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "http://localhost:10522/api/todos",
      "rel": "list",
      "method": "GET"
    }
  ]
}
```

