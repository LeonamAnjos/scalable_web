# ScalableWeb
WebApi .NET Core 2.1 Application with Integration Tests

### Introduction

ScalableWeb is a .Net Core 2.1 WebApi Application that has the objective to practice and serve as a prototype for new projects with .Net Core WebApi.

### Solution
The application three main endpoints:
- __POST /v1/diff/{id}/right__ - receives a JSON with base64 encoded binary on the request body. The data is persisted with the informed _id_.
- __POST /v1/diff/{id}/left__ - receives a JSON with base64 encoded binary on the request body. The data is persisted with the informed _id_.
- __GET /v1/diff/{id}__ - compare the right and left data with the same _id_. 

### Installation
Download _ScalableWeb_ project from [GitHub](https://github.com/LeonamAnjos/scalable_web) or clone it with following command:

```sh
$ git clone https://github.com/LeonamAnjos/scalable_web.git
$ cd scalable_web
```
Open with Visual Studio and build the solution (_CTRL+Shift+B_).

### Running
Run local in debug mode and use any REST client send the requests below:

Equal data content:
- __POST__ http://localhost:51643/v1/diff/1/right with _{"Data":"eyAibWVzc2FnZSI6ICJDT05URU5UIEZPUiBURVNUIg=="}_ in the body.
- __POST__ http://localhost:51643/v1/diff/1/left with _{"Data":"eyAibWVzc2FnZSI6ICJDT05URU5UIEZPUiBURVNUIg=="}_ in the body.
- __GET__ http://localhost:51643/v1/diff/1.


Different data content:
- __POST__ http://localhost:51643/v1/diff/2/right with _{"Data":"eyAibWVzc2FnZSI6ICIxKzIrMys0KzYi"}_ in the body.
- __POST__ http://localhost:51643/v1/diff/2/left with _{"Data":"eyAibWVzc2FnZSI6ICIxKzIrMys1KzYi"}_ in the body.
- __GET__ http://localhost:51643/v1/diff/2

Different data size content:
- __POST__ http://localhost:51643/v1/diff/3/right with _{"Data":"eyAibWVzc2FnZSI6ICIxKzIrMys0Ig=="}_ in the body.
- __POST__ http://localhost:51643/v1/diff/3/left with _{"Data":"eyAibWVzc2FnZSI6ICIxKzIrMyI="}_ in the body.
- __GET__ http://localhost:51643/v1/diff/3

### Todo next
- Configure a database connection on the main program. 
- Create a ClassLibrary and move the Entity Framework repository implementation to this project.
- Configure migration.
- Prepare the integration tests to run with In Memory database or with a real one.
