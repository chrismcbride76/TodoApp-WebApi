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
  * Note: project dependencies should automatically get pulled in by nuget on first build
  * Note: the project uses an in-memory database, meaning the todo list does not persist between app restarts
  
Making Requests
---------------

All API URLs start with `http://[domain]/api`, where `[domain]` is the domain name of the service.  When running through visual studio, this will be `localhost:10522'.  The port number may be different depending on your setup.

Any part of the API can be reached starting from the main entry point at `http://[domain]/api`.  The entry point lists the resources provided by the server, as well as how to access the api.

## Api Description

URI: `http://localhost:10522/api`

#### Request

```
GET /api HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```

#### Response

```json
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
    },
    {
      "href": "http://localhost:10522/api/todos",
      "rel": "create",
      "method": "POST"
    }
  ]
}
```

## Todo Item

The api provides operations using the /todos resource.  Use todos resource for creating, updating, deleting, and listing todo related details.

URI: `http://localhost:10522/api/todos`

### Add a todo to the list

Operation: `POST /api/todos`

#### Request

```json
POST /api/todos HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
{
  "task": "Mow the lawn",
  "deadlineUtc": "2015-11-11T20:05:32.773Z",
  "completed": false,
  "moreDetails": "Make sure to mow the front and back yards"
}
```

#### Response:

```json
{
  "id": 1,
  "task": "Mow the lawn",
  "deadlineUtc": "2015-11-11T20:05:32.773Z",
  "completed": false,
  "moreDetails": "Make sure to mow the front and back yards",
  "_links": [
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "edit",
      "method": "PUT"
    },
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "delete",
      "method": "DELETE"
    }
  ]
}
```

### View more information about a todo

Operation: `GET /api/todos/{id}`

#### Request
```
GET /api/todos/1 HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```
#### Response
```json
{
  "id": 1,
  "task": "Mow the lawn",
  "deadlineUtc": "2015-11-11T20:05:32.773Z",
  "completed": false,
  "moreDetails": "Make sure to mow the front and back yards",
  "_links": [
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "edit",
      "method": "PUT"
    },
    {
      "href": "http://localhost:10522/api/todos/1",
      "rel": "delete",
      "method": "DELETE"
    }
  ]
}
```

### Update a todo (mark completed, etc.)

Operation: `PUT /api/todos/{id}`

#### Request
```json
PUT /api/todos/1 HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache

{
  "task": "Mow the front and back yard",
  "deadlineUtc": "2015-11-11T20:05:32.773Z",
  "completed": true,
  "moreDetails": "Make sure to pick up the grass trimmings"
}
```

#### Response

`200 OK`

### Delete a todo

Operation: `DELETE /api/todos/{id}`

#### Request
```
DELETE /api/todos/1 HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```

#### Response

```
204 No Content
```

### List all todos

```
GET /api/todos HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```
### Response

```json
[
  {
    "id": 2,
    "task": "Mow the lawn",
    "deadlineUtc": "2015-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "Make sure to mow the front and back yards",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  },
  {
    "id": 3,
    "task": "Take out the trash",
    "deadlineUtc": "2016-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  },
  {
    "id": 4,
    "task": "Sweep the floor",
    "deadlineUtc": "2016-11-11T20:05:32.773Z",
    "completed": true,
    "moreDetails": "",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/4",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/4",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/4",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  },
  {
    "id": 5,
    "task": "mop the floor",
    "deadlineUtc": "2016-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  }
]
```

The API supports OData query options, allowing the client to specify filtering, ordering, paging, etc.

### See only todos that have not been completed

Operation: `GET /api/todos?$filter=completed eq false`

#### Request

```
GET /api/todos?$filter=completed eq false HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```
#### Response

```
[
  {
    "id": 2,
    "task": "Mow the lawn",
    "deadlineUtc": "2015-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "Make sure to mow the front and back yards",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  },
  {
    "id": 3,
    "task": "Take out the trash",
    "deadlineUtc": "2016-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/3",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  },
  {
    "id": 5,
    "task": "mop the floor",
    "deadlineUtc": "2016-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/5",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  }
]
  ```

### See overdue todos (todos that have not been completed and where todays date is past the deadline)

Operation: `GET /api/todos?overdue=true`

#### Request

```
GET /api/todos?overdue=true HTTP/1.1
Host: localhost:10522
Content-Type: application/json
Cache-Control: no-cache
```

#### Response

```json
[
  {
    "id": 2,
    "task": "Mow the lawn",
    "deadlineUtc": "2015-11-11T20:05:32.773Z",
    "completed": false,
    "moreDetails": "Make sure to mow the front and back yards",
    "_links": [
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "self",
        "method": "GET"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "edit",
        "method": "PUT"
      },
      {
        "href": "http://localhost:10522/api/todos/2",
        "rel": "delete",
        "method": "DELETE"
      }
    ]
  }
]
```
