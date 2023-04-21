# Contents
* API
* Unit tests
* Integration Tests

# API
It's a .NET Core Swagger API has endpoints to get and insert users, you can get users in a number of ways like by id, forename, surname or just grab all of them.
It uses an in memory database which is created on startup so the data is rebuilt whenever its run.

# Unit tests
The unit tests are just against part of the controller, I've not unit tested the whole solution.
The unit tests are in XUnit and use FluentAssertions as well as MOQ.

# Integration tests
These tests are split into two with a file of ones which call the "real" in memory database and check the responses.
The real tests have a lot of extra code around the file to make them threadsafe as they can run in parallel which would call the populate database method at the same time.
As part of making them threadsafe you'll see in the API it has a lock to ensure the database isnt populated more than once.
The fake tests are swapping out the file which calls the real database with fake calls which you can change from within the tests to test all outcomes / not cause actual damage if real data was behind it.
These are also all XUnit and using Fluent.
