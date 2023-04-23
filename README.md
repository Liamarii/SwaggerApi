# Contents
* API
* Unit tests
* Integration Tests

# API
It's a .NET Core Swagger API with endpoints to get and insert users. 

You can get users in a number of ways like by id, forename, surname or just grab all of them.

The data comes from an in memory database which is created on startup so the data is recreated on build.

# Unit tests
The unit tests are just against part of the controller, I've not unit tested the whole solution as it's an example.
The unit tests use XUnit and Fluent assertions as well as MOQ.

# Integration tests
These tests are split into two with a file of ones which call the "real" in memory database through the service and check the responses.
As part of making them threadsafe you'll see in the API it has a lock to ensure the database isnt populated more than once.

The fake tests are swapping out the file which calls the real database with fake calls which you can change from within the tests to test all outcomes / not cause actual damage if real data was behind it.
These are also all XUnit and using Fluent.
