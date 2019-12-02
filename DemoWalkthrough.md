#Get Comfortable with .NET Core and the CLI  
#Demo Walkthrough 

By following this walkthrough, you can re-create the projects in this repository: [https://github.com/jeremybytes/core-cli-30](https://github.com/jeremybytes/core-cli-30).

**Level**: Introductory  

**Target**: C# developers who have been working with .NET Framework and are interested in trying out .NET Core. 

**Required Software**:  
* .NET Core SDK  
[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)  
* Visual Studio Code  
[https://code.visualstudio.com/download](https://code.visualstudio.com/download)  
(You can also use Visual Studio 2019 Community Edition)
* C# Extension for Visual Studio Code  
[https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)  
(This will give you code completion and lots of other help/refactoring in Visual Studio Code).

**Additional Files:**  
The following files will be required during the walkthrough. These files are in the [Starting Files folder](https://github.com/jeremybytes/core-cli-30/tree/master/StartingFiles) of the repository:

* [snippets.txt](https://github.com/jeremybytes/core-cli-30/blob/master/StartingFiles/snippets.txt)  
(code snippets to pasted into the project)
* [CSVPeopleProvider.cs](https://github.com/jeremybytes/core-cli-30/blob/master/StartingFiles/CSVPeopleProvider.cs)  
(data provider to show dependency injection)
* [People.txt](https://github.com/jeremybytes/core-cli-30/blob/master/StartingFiles/People.txt)  
(data for the CSVPeopleProvider)

At the end of the walkthrough, we will have a working web service, unit tests for the service, and a console application that calls the service. In addition, we will look at the built-in dependency injection container in ASP.NET Core.

You can use Visual Studio Code or Visual Studio to edit the code files (I will use a mixture of the two). And PowerShell or cmd.exe will work for the command line. The instructions here show PowerShell commands (which may be slightly different from cmd.exe).

Code to be typed at the command prompt will appear like this:

```
PS C:\CoreCLI> command
```

In addition, code blocks will be shown for the projects themselves.

Hello dotnet
--------------
We'll start by opening PowerShell to the root folder that we use for this project (the location and name of the root folder is up to you).

The "dotnet" command will be used throughout this walkthrough. This is available and automatically in your path when you install the .NET Core SDK.

```
PS C:\CoreCLI> dotnet
Usage: dotnet [options]
Usage: dotnet [path-to-application]

Options:
  -h|--help         Display help.
  --info            Display .NET Core information.
  --list-sdks       Display the installed SDKs.
  --list-runtimes   Display the installed runtimes.

path-to-application:
  The path to an application .dll file to execute.
PS C:\CoreCLI>
```

By typing a command, you get a list of options. To get further details on any command, just add "-h".

```
PS C:\CoreCLI> dotnet -h
.NET Core SDK (3.0.100)
Usage: dotnet [runtime-options] [path-to-application] [arguments]

Execute a .NET Core application.

runtime-options:
  --additionalprobingpath <path>   Path containing probing policy and assemblies to probe for.
  --additional-deps <path>         Path to additional deps.json file.
  --fx-version <version>           Version of the installed Shared Framework to use to run the application.
  --roll-forward <setting>         Roll forward to framework version  (LatestPatch, Minor, LatestMinor, Major, LatestMajor, Disable).
[...Plus a lot more...]
```

To see what version of .NET Core is installed, use "--version"

```
PS C:\CoreCLI> dotnet --version
3.0.100
PS C:\CoreCLI>
```

This walkthrough was built with version 3.0.100.

Web Service
--------------------
We'll start by creating a web service. 

### Creating the Project
First, create a new folder and navigate to it.

```
PS C:\CoreCLI> mkdir person-api
```
```
PS C:\CoreCLI> cd person-api
PS C:\CoreCLI\person-api>
```

Use "dotnet new" to see a list of installed templates:

```
PS C:\CoreCLI\person-api>dotnet new
Usage: new [options]

Options:
  -h, --help          Displays help for this command.
  -l, --list          Lists templates containing the specified name. If no name is specified, lists all templates.
  -n, --name          The name for the output being created. If no name is specified, the name of the current directory is used.
[...Plus a lot more, here are some templates...]
ASP.NET Core Empty                                web
ASP.NET Core Web App (Model-View-Controller)      mvc
ASP.NET Core Web App                              webapp
ASP.NET Core with Angular                         angular
ASP.NET Core with React.js                        react
ASP.NET Core Web API                              webapi
ASP.NET Core gRPC Service                         grpc
[...There are a lot more templates...]
```

Notice that many command arguments have 2 options: a single dash (-h) option and a double dash (--help) option. These are equivalent.

To create a web service, we will use the "webapi" template:

```
PS C:\CoreCLI\person-api>dotnet new webapi
The template "ASP.NET Core Web API" was created successfully.

Processing post-creation actions...
Running 'dotnet restore' on C:\CoreCLI\person-api\person-api.csproj...
  Restore completed in 71.96 ms for C:\CoreCLI\person-api\person-api.csproj.

Restore succeeded.

PS C:\CoreCLI\person-api>  
```

Here is a directory listing to show the files that are generated by the template:

```
PS C:\CoreCLI\person-api> dir                               
    Directory: C:\CoreCLI\person-api

Mode                LastWriteTime         Length Name
----                -------------         ------ ----
d-----       11/22/2019  11:39 AM                Controllers
d-----       11/22/2019  11:39 AM                Properties
-a----       11/22/2019  11:39 AM            146 appsettings.Development.json
-a----       11/22/2019  11:39 AM            192 appsettings.json
-a----       11/22/2019  11:39 AM            228 person-api.csproj
-a----       11/22/2019  11:39 AM            718 Program.cs
-a----       11/22/2019  11:39 AM           1462 Startup.cs
-a----       11/22/2019  11:39 AM            306 WeatherForecast.cs

PS C:\CoreCLI\person-api>   
```

Note that the project is named "person-api.csproj" (the same as the folder). We will see how to change this default value a bit later.

### Building and Running the Sample Service
From here, we can build the project.

```
PS C:\CoreCLI\person-api>dotnet build
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 17.39 ms for C:\CoreCLI\person-api\person-api.csproj.
  person-api -> C:\CoreCLI\person-api\bin\Debug\netcoreapp3.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:06.96
PS C:\CoreCLI\person-api> 
```

The service is self-hosting, so we can run the project directly.

```
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

Note that the host is listening on localhost port 5001 (for https) and port 5000 (for http).

Now we can navigate to the service in the browser.

[http://localhost:5000/WeatherForecast](http://localhost:5000/WeatherForecast)

Using the http endpoint will automatically redirect to the https endpoint [https://localhost:5001/WeatherForecast](https://localhost:5001/WeatherForecast). If you do not have a trusted certificate installed for localhost, then you will get a browser warning.

### Changing HTTPS Redirect
Instead of dealing with https and a trusted certificate, we will disable the redirect so we can continue developing easily on the local machine.

For this, we'll first stop the service by using "Ctrl+C" in the PowerShell window.

```
      Application is shutting down...
PS C:\CoreCLI\person-api>  
```

If you have Visual Studio Code installed, you can easily open the folder by using the "code ." command.

```
PS C:\CoreCLI\person-api> code .
```

With .NET Core, we do not need to open a solution file or a project file (right now, we do not even have a solution file). Instead, we can open a folder to show the contents.

Inside Visual Studio Code, you will be prompted to "Add required assets". If you choose "Yes", this will create a new ".vscode" folder that has some setting files in it. These are used for debugging and running from within Visual Studio code. You can say "Yes" to add them, but we will not use them during this walkthrough.

To remove the https redirection, open the "Startup.cs" file. Then comment out the following line:

```csharp
    // app.UseHttpsRedirection();
```

Back at the command prompt, re-run the service. 

*Note that you do not need to build first, using "dotnet run" will rebuild the project if necessary.*

```
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

Now navigate to the http endpoint.

[http://localhost:5000/WeatherForecast](http://localhost:5000/WeatherForecast)

This time we are not redirected, and the service supplies the data.

```json
[{"date":"2019-11-23T11:59:50.1563699-05:00","temperatureC":49,"temperatureF":120,"summary":"Mild"},
{"date":"2019-11-24T11:59:50.1566145-05:00","temperatureC":51,"temperatureF":123,"summary":"Scorching"},
{"date":"2019-11-25T11:59:50.1566224-05:00","temperatureC":-20,"temperatureF":-3,"summary":"Hot"},
{"date":"2019-11-26T11:59:50.1566229-05:00","temperatureC":54,"temperatureF":129,"summary":"Scorching"},
{"date":"2019-11-27T11:59:50.1566233-05:00","temperatureC":45,"temperatureF":112,"summary":"Hot"}]
```

This is dummy data that is part of the webapi template.

### Updating the Service Code
With a working service, we can now go in and change it to run our code.

Stop the service using "Ctrl+C".

```
      Application is shutting down...
PS C:\CoreCLI\person-api>  
```

In Visual Studio Code, add a new folder to the project called "Models". To add a folder, put your cursor in the Explorer window (this is the one that shows the files). In the folder header (where it says "PERSON-API"), you will see a set of icons. One of these is "New Folder".

The "Models" folder should be at the root of the project folder (as a sibling to "Controllers").

*As an alternate, you can add a new folder from the command prompt or File Explorer. The new folder will automatically show up as part of the project in Visual Studio Code.*

### Adding a Person Class
From here, add a new file to the Models folder called "Person.cs". To add a file, click on the Models folder, and then choose the "New File" icon.

Visual Studio Code creates an empty file. Start by adding the namespace

```csharp
namespace person_api
{
    
}
```

Note that the default namespace for the project is the same as the project name, except the dashes have been replaced by underscores.

Now locate the "snippets.txt" file and copy the "Person" class into the file. (Or just copy the code from this code block.)

```csharp
namespace person_api
{
    public class Person
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime StartDate { get; set; }
        public int Rating { get; set; }
        public string FormatString { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FormatString))
                FormatString = "{0} {1}";
            return string.Format(FormatString, GivenName, FamilyName);
        }
    }
}
```

*If the indentation is off when you paste in the code, use the keyboard shortcut "Shift+Alt+F" to format the file.*

The "DateTime" type will have red squigglies because the using statement is missing. Just click on "DateTime", and press "Ctrl+." to bring up a list of shortcuts. The top option will add "using System;" to the top of the file.

```csharp
using System;
```

### Adding a Data Provider
The Person class is the data type that for the service. To supply some data, we will add a data provider. Add a new file to the Models folder named "HardCodedPeopleProvider.cs".

As above, we will add the namespace plus the class declaration.

```csharp
namespace person_api
{
    public class HardCodedPeopleProvider
    {
        
    }
}
```

Then copy in the "GetPeople" and "GetPerson" methods from the "snippets.txt" file (or from the following code block).

Be sure to use "Ctrl+." to bring in the needed "using" statements.

```csharp
using System;
using System.Collections.Generic;

namespace person_api
{
    public class HardCodedPeopleProvider
    {
        public List<Person> GetPeople()
        {
            var p = new List<Person>()
            {
                new Person() { Id=1, GivenName="John", FamilyName="Koenig",
                    StartDate = new DateTime(1975, 10, 17), Rating=6 },
                new Person() { Id=2, GivenName="Dylan", FamilyName="Hunt",
                    StartDate = new DateTime(2000, 10, 2), Rating=8 },
                new Person() { Id=3, GivenName="Leela", FamilyName="Turanga",
                    StartDate = new DateTime(1999, 3, 28), Rating=8,
                    FormatString = "{1} {0}" },
                new Person() { Id=4, GivenName="John", FamilyName="Crichton",
                    StartDate = new DateTime(1999, 3, 19), Rating=7 },
                new Person() { Id=5, GivenName="Dave", FamilyName="Lister",
                    StartDate = new DateTime(1988, 2, 15), Rating=9 },
                new Person() { Id=6, GivenName="Laura", FamilyName="Roslin",
                    StartDate = new DateTime(2003, 12, 8), Rating=6},
                new Person() { Id=7, GivenName="John", FamilyName="Sheridan",
                    StartDate = new DateTime(1994, 1, 26), Rating=6 },
                new Person() { Id=8, GivenName="Dante", FamilyName="Montana",
                    StartDate = new DateTime(2000, 11, 1), Rating=5 },
                new Person() { Id=9, GivenName="Isaac", FamilyName="Gampu",
                    StartDate = new DateTime(1977, 9, 10), Rating=4 },
            };
            return p;
        }

        public Person GetPerson(int id)
        {
            return GetPeople().First(p => p.Id == id);
        }
    }
}
```

### Updating the Controller
The next step is to update the controller to use our new data.

Inside the "Controllers" folder is the "WeatherForecastController.cs" file. We'll rename this file. To do this, click on the file, press F2, and change the name to "PeopleController.cs".

Open the "PeopleController.cs" file. Here we will rename the class, remove unneeded code, and update the "Get" method.

Here is the result:

```csharp
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Person> Get()
        {

        }
    }
```

Be sure to update the class name (PeopleController) and the return type of the "Get" method ("Person" instead of "WeatherForeacast").

To fill in the functionality, we will use the HardCodedPeopleProvider that we created earlier. Here's the completed code:

```csharp
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        HardCodedPeopleProvider provider = new HardCodedPeopleProvider();

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return provider.GetPeople();
        }
    }
```

Finally, we will add a "Get" method that takes an ID parameter so that we can return a single item.

```csharp
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return provider.GetPerson(id);
        }
```

Save all the files.

Back at the command prompt, build the service. (We'll do an explicit build here just to see if there are any build problems we need to fix before running the service.)

```
PS C:\CoreCLI\person-api> dotnet build  
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 13.4 ms for C:\CoreCLI\person-api\person-api.csproj.
  person-api -> C:\CoreCLI\person-api\bin\Debug\netcoreapp3.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:01.20
PS C:\CoreCLI\person-api> 
```

Assuming you have a successful build, run the service.

```
Time Elapsed 00:00:01.20
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

Now open a browser and navigate to the service location. Note that the URL is different since we changed the name of the controller.

[http://localhost:5000/people](http://localhost:5000/people)

This will give us the entire collection of Person objects.

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":null},
{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":null},
{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},
{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":null},
{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":null},
{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":null},
{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":null},
{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":null},
{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":null}]
```

We can call the other "Get" method by adding a value to the URL.

[http://localhost:5000/people/2](http://localhost:5000/people/2)

```json
{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":null}
```

Now we have a working service.

### Changing the Port
One last change is that we will change the port that is used for the service. This is something that I do with my localhost projects to eliminate possible collisions if I have multiple services running at the same time.

To change the port, we'll go back to Visual Studio Code and open the "Program.cs" file.

In the "CreateHostBuilder" method, we'll add `.UseUrls("http://localhost:9874")`.

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>()
                .UseUrls("http://localhost:9874");
        });
```

Back in the PowerShell window, use "Ctrl+C" to stop the service, then "dotnet run" to restart it.

```
      Application is shutting down...
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

We now see that the host is listening on port 9874.

If we click "Refresh" in the browser (or navigate to the old address), we get a "connection refused" error.

[http://localhost:5000/people](http://localhost:5000/people)

If we update the URL to use the new port, then we have the data that we expect.

[http://localhost:9874/people](http://localhost:9874/people)

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":null},
{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":null},
{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},
{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":null},
{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":null},
{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":null},
{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":null},
{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":null},
{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":null}]
```

And that's our working service.


Unit Tests
------------------
Next we'll create a unit test project and add some tests for the Controller class in the web service.

### Creating the Project
Open a new PowerShell (or cmd.exe window) at the root of the project.

```
PS C:\CoreCLI>
```

Create a folder for the unit test project named "person-api-tests" and navigate to that folder.

```
PS C:\CoreCLI> mkdir person-api-tests

    Directory: C:\CoreCLI

Mode         LastWriteTime         Name
----         -------------         ----
d-----       11/22/2019   2:37 PM  person-api-tests

PS C:\CoreCLI> cd .\person-api-tests\
PS C:\CoreCLI\person-api-tests>
```

If we type "dotnet new", we can see several unit test project options in the list.

```
PS C:\CoreCLI\person-api-tests> dotnet new

Unit Test Project                       mstest
NUnit 3 Test Project                    nunit
NUnit 3 Test Item                       nunit-test
xUnit Test Project                      xunit
```

For our sample, we'll use NUnit.

```
PS C:\CoreCLI\person-api-tests> dotnet new nunit  
The template "NUnit 3 Test Project" was created successfully.

Processing post-creation actions...
Running 'dotnet restore' on C:\CoreCLI\person-api-tests\person-api-tests.csproj...
  Restore completed in 2.7 sec for C:\CoreCLI\person-api-tests\person-api-tests.csproj.

Restore succeeded.

PS C:\CoreCLI\person-api-tests>  
```

Open the project folder in Visual Studio Code.

```
PS C:\CoreCLI\person-api-tests> code .
```

The "UnitTest1.cs" file has a sample test.

```csharp
using NUnit.Framework;

namespace person_api_tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
```

### Running the Test
To run the test, we use "dotnet test" at the command prompt.

```
PS C:\CoreCLI\person-api-tests> dotnet test  
Test run for C:\CoreCLI\person-api-tests\bin\Debug\netcoreapp3.0\person-api-tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 1
     Passed: 1
 Total time: 1.2749 Seconds
PS C:\CoreCLI\person-api-tests>   
```

Just like with "dotnet run", we do not need to do an explicit build to run the tests. The project will be built automatically if required.

### Adding a Project Reference
Since we want to test the controller from the web service, we need to add a reference to that project. We can do this from the command line using "dotnet add reference ...".

```
PS C:\CoreCLI\person-api-tests> dotnet add reference ..\person-api\person-api.csproj  
Reference `..\person-api\person-api.csproj` added to the project.
PS C:\CoreCLI\person-api-tests>  
```

### Creating the First Test
Back in Visual Studio Code, we will update the placeholder test with our own.

First, rename the file from "UnitTest1.cs" to "PeopleControllerTests.cs". (As a reminder, click on the file and press F2 in Visual Studio Code).

Inside the file, we will rename the class from "Tests" to "PeopleControllerTests".

```csharp
public class PeopleControllerTests
```

We will use the setup method to create an instance of the controller that our tests will use. For this, we'll add a class-level field for the controller and "new" it up in the Setup method.

```csharp
public class PeopleControllerTests
{
    PeopleController controller;

    [SetUp]
    public void Setup()
    {
        controller = new PeopleController();
    }
    ...
}
```

Note: you will need to use "Ctrl+." to bring in the "using" statement for the PeopleController class.

Next, we'll remove "Test1" and add something a little more useful.

```csharp
[Test]
public void GetPeople_ReturnsAllItems()
{
    IEnumerable<Person> result = controller.Get();
    Assert.AreEqual(9, result.Count());
}
```

(You will need to bring in some using statements, but I'll assume that you are used to doing that now.)

Back at the command prompt, we'll run our new test.

```
PS C:\CoreCLI\person-api-tests> dotnet test  
Test run for C:\CoreCLI\person-api-tests\bin\Debug\netcoreapp3.0\person-api-tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 1
     Passed: 1
 Total time: 0.7540 Seconds
PS C:\CoreCLI\person-api-tests>  
```

The first test is passing. This test is not very useful because it is tied to the "HardCodedPeopleProvider" class that is part of the web service project. We'll separate this out later on when we look at dependency injection.

### Adding 2 More Tests
Next we will add a couple of tests for the "Get(int id)" method.

```csharp
[Test]
public void GetPerson_WithValidId_ReturnsPerson()
{
    Person result = controller.Get(2);
    Assert.AreEqual(2, result.Id);
}

[Test]
public void GetPerson_WithInvalidId_ReturnsNull()
{
    Person result = controller.Get(-10);
    Assert.IsNull(result);
}
```

Now run the tests.

```
PS C:\CoreCLI\person-api-tests> dotnet test  
Test run for C:\CoreCLI\person-api-tests\bin\Debug\netcoreapp3.0\person-api-tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.
  X GetPerson_WithInvalidId_ReturnsNull [18ms]
  Error Message:
   System.InvalidOperationException : Sequence contains no matching element
  Stack Trace:
     at System.Linq.ThrowHelper.ThrowNoMatchException()
   at System.Linq.Enumerable.First[TSource](IEnumerable`1 source, Func`2 predicate)
   at person_api.HardCodedPeopleProvider.GetPerson(Int32 id) in C:\CoreCLI\person-api\Models\HardCodedPeopleProvider.cs:line 38
   at person_api.Controllers.PeopleController.Get(Int32 id) in C:\CoreCLI\person-api\Controllers\PeopleController.cs:line 25
   at person_api_tests.PeopleControllerTests.GetPerson_WithInvalidId_ReturnsNull() in C:\CoreCLI\person-api-tests\PeopleControllerTests.cs:line 36

Test Run Failed.
Total tests: 3
     Passed: 2
     Failed: 1
 Total time: 0.7159 Seconds
PS C:\CoreCLI\person-api-tests>  
```

In this case, the last test fails. That is because the web service throws an exception (InvalidOperationException) rather than returning null.

*We won't talk about whether returning null from the service is a good idea or not (it probably isn't). Instead, we'll focus on creating the tests and making sure they pass.*

The test fails because of the way the HardCodedPeopleProvider is coded. As a reminder, here is the "GetPerson" method from that class:

```csharp
public Person GetPerson(int id)
{
    return GetPeople().First(p => p.Id == id);
}
```

The "First" method looks for the first match it can find. But if it finds no matches at all, it will throw an exception.

But we don't want to modify the provider. Instead we want to modify the controller. This will assure that the Get method on the controller will return "null" as appropriate regardless of how the underlying provider behaves.

Let's go to the "Get(int id)" method from the PeopleController class.

```csharp
[HttpGet("{id}")]
public Person Get(int id)
{
    return provider.GetPerson(id);
}
```

We will wrap the "GetPerson" call in a try/catch block. Then if the provider throws an exception, we can still return "null".

```csharp
[HttpGet("{id}")]
public Person Get(int id)
{
    try
    {
        return provider.GetPerson(id);
    }
    catch (Exception)
    {
        return null;
    }
}
```

Now if we re-run the tests, we see that they all pass.

```
PS C:\CoreCLI\person-api-tests> dotnet test  
Test run for C:\CoreCLI\person-api-tests\bin\Debug\netcoreapp3.0\person-api-tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 3
     Passed: 3
 Total time: 0.8103 Seconds
PS C:\CoreCLI\person-api-tests>
```

*Note: we do not have to explicitly re-build the web service project because it is referenced by the test project. So it will get re-built automatically as needed.*

Our tests still need better isolation so that we can have more control over the provider during testing. But this gets us started. We will make things a bit more robust later on.

Console Application
--------------------
As a third project, we will build a console application that calls the web service.

### Creating the Project
Open a new PowerShell (or cmd.exe window) at the root of the project.

```
PS C:\CoreCLI>
```

Create a folder for the unit test project named "person-console" and navigate to that folder.

```
PS C:\CoreCLI> mkdir person-console

Directory: C:\CoreCLI

Mode                LastWriteTime         Name
----                -------------         ----
d-----       11/22/2019   5:27 PM         person-console

PS C:\CoreCLI> cd person-console
PS C:\CoreCLI\person-console>
```

Next create a new console application with "dotnet new console".

```
PS C:\CoreCLI\person-console> dotnet new console
The template "Console Application" was created successfully.

Processing post-creation actions...
Running 'dotnet restore' on C:\CoreCLI\person-console\person-console.csproj...
  Restore completed in 75.19 ms for C:\CoreCLI\person-console\person-console.csproj.

Restore succeeded.

PS C:\CoreCLI\person-console>
```

### Running the Application
Run the application.

```
PS C:\CoreCLI\person-console> dotnet run
Hello World!
PS C:\CoreCLI\person-console>
```

We have a working console application. Unfortunately, it takes the fun out of creating a new application because "Hello, World!" has already been added.

Open the project folder in Visual Studio Code

```
PS C:\CoreCLI\person-console> code .
```

Open the "Program.cs" file.

```csharp
using System;

namespace person_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

This is what is included in the default template.

### Calling the Service
To call the service, we will add a copy of the "Person" class and create a class that knows how to talk to the service.

Using File Explorer (or the method of your choice), copy the "Person.cs" file from the "person-api" project into the root folder of the "person-console" project.

*One thing to notice when you do this: the file automatically shows up in Visual Studio Code.* 

.NET Core uses a different project system than .NET Framework. By default, the project includes any files that are in the project folder. There is no need to explicitly add them. (We can explicitly exclude them if we need to).

Open "Person.cs" in Visual Studio Code and change the namespace to match the console application: person_console.

```csharp
namespace person_console
{
    public class Person
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime StartDate { get; set; }
        public int Rating { get; set; }
        public string FormatString { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FormatString))
                FormatString = "{0} {1}";
            return string.Format(FormatString, GivenName, FamilyName);
        }
    }
}
```

Create a new file / class named "PersonReader.cs" / "PersonReader".

```csharp
namespace person_console
{
    public class PersonReader
    {
        
    }
}
```

Add a field of type "HttpClient".

```csharp
public class PersonReader
{
    private HttpClient client = new HttpClient();
}
```

Copy the constructor from the "snippets.txt" file.

```csharp
public class PersonReader
{
    private HttpClient client = new HttpClient();

    public PersonReader()
    {
        client.BaseAddress = new Uri("http://localhost:9874");
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
```

Don't forget to add the using statements using "Ctrl+."

Next, add the "GetAsync" method from the "snippets.txt" file.

```csharp
public class PersonReader
{
    private HttpClient client = new HttpClient();

    public PersonReader()
    {
        client.BaseAddress = new Uri("http://localhost:9874");
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<Person>> GetAsync()
    {
        await Task.Delay(3000);

        HttpResponseMessage response = await client.GetAsync("people");
        if (response.IsSuccessStatusCode)
        {
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Person>>(stringResult);
        }
        return new List<Person>();
    }
}
```

*Note: the "Task.Delay" call pauses operation for 3 seconds. This code was originally taken from a demo on Task and await. For more information, check [I'll Get Back to You: Task, Await, and Asynchronous Methods](http://www.jeremybytes.com/Demos.aspx#TaskAndAwait).*

If you try to add the using statement for "JsonConvert", you'll see that it does not resolve. This is because the object comes from a NuGet package that we have not yet added.

### Adding a NuGet Package
To add the package, we'll go back to the command prompt and use "dotnet add package".

```
PS C:\CoreCLI\person-console> dotnet add package Newtonsoft.Json  
   Writing C:\Users\jerem\AppData\Local\Temp\tmpE47F.tmp
info : Adding PackageReference for package 'Newtonsoft.Json' into project 'C:\CoreCLI\person-console\person-console.csproj'.
info : Restoring packages for C:\CoreCLI\person-console\person-console.csproj...
info :   GET https://api.nuget.org/v3-flatcontainer/newtonsoft.json/index.json
info :   OK https://api.nuget.org/v3-flatcontainer/newtonsoft.json/index.json 53ms
info : Package 'Newtonsoft.Json' is compatible with all the specified frameworks in project 'C:\CoreCLI\person-console\person-console.csproj'.
info : PackageReference for package 'Newtonsoft.Json' version '12.0.3' added to file 'C:\CoreCLI\person-console\person-console.csproj'.
info : Committing restore...
info : Writing assets file to disk. Path: C:\CoreCLI\person-console\obj\project.assets.json
log  : Restore completed in 932.82 ms for C:\CoreCLI\person-console\person-console.csproj.
PS C:\CoreCLI\person-console> 
```

Starting with .NET Core 3.0, a JSON serializer is included in the framework. We're using an external serializer here so we can see how to add a NuGet package.

If you use "Ctrl+." on "JsonConvert" in the code file, it will now resolve and add the correct using statement.

### Updating the Program
Now that we have the data object and data reader set up, we need to call them from the Program class.

Open "Program.cs" and update the Main method as follows.

```csharp
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("One moment please...");
        var reader = new PersonReader();
        var people = await reader.GetAsync();
        foreach(var person in people)
            Console.WriteLine(person);

        Console.WriteLine("===============");
    }
}
```

This creates a data reader, awaits the "GetAsync" method, then loops through the results to output to the console.

This code will not compile at this point. Since we are using "await", we need to make the method "async". Fortunately, .NET Core supports an "async Main" method in console applications (since C# 7.1).

```csharp
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("One moment please...");
        var reader = new PersonReader();
        var people = await reader.GetAsync();
        foreach(var person in people)
            Console.WriteLine(person);

        Console.WriteLine("===============");
    }
}
```

The Main method needs to be "async Task" rather than "async void". "async void" is not allowed here.

### Running the Application
With everything in place, we can now run the application. First, go back to the PowerShell window with the service folder and start the service using "dotnet run".

```
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

Then run the console application from the PowerShell window that is open to the console project.

First it displays the "One moment please..." message.

```
PS C:\CoreCLI\person-console> dotnet run  
One moment please...
```

Then after 3 seconds, it displays the data from the web service.

```
PS C:\CoreCLI\person-console> dotnet run  
One moment please...
John Koenig
Dylan Hunt
Turanga Leela
John Crichton
Dave Lister
Laura Roslin
John Sheridan
Dante Montana
Isaac Gampu
===============
PS C:\CoreCLI\person-console>
```

Everything works!

Solution
---------
Now that we have 3 projects, we will create a solution for them. We'll do this from the command line as well.

### Creating the Solution
Open a new PowerShell (or cmd.exe window) at the root of the project.

```
PS C:\CoreCLI>
```

Use "dotnet new sln" to create a solution.

```
PS C:\CoreCLI> dotnet new sln -n "CoreCLI"
The template "Solution File" was created successfully.
```

By using the "-n" argument, we can name the solution whatever we want. This way we can override the default naming if we want something different than the root folder.

```
PS C:\CoreCLI> dir  

Directory: C:\CoreCLI

Mode                LastWriteTime         Name
----                -------------         ----
d-----       11/22/2019  12:05 PM         person-api
d-----       11/22/2019   2:55 PM         person-api-tests
d-----       11/22/2019   5:39 PM         person-console
-a----       11/22/2019   6:00 PM         CoreCLI.sln

PS C:\CoreCLI>
```

### Adding Projects to the Solution
To add projects, we use "dotnet sln add".

The following adds the web service project to the solution.

```
PS C:\CoreCLI> dotnet sln add .\person-api\person-api.csproj
Project `person-api\person-api.csproj` added to the solution.
PS C:\CoreCLI>
```

This may seem like a lot of work, but because of auto-completion on the command line, it does not take very long to type. Just type the first few letters of the folder or file and hit "Tab".

We'll do the same for the other 2 projects.

```
PS C:\CoreCLI> dotnet sln add .\person-api-tests\person-api-tests.csproj
Project `person-api-tests\person-api-tests.csproj` added to the solution.
PS C:\CoreCLI> dotnet sln add .\person-console\person-console.csproj
Project `person-console\person-console.csproj` added to the solution.
PS C:\CoreCLI>
```

Now that we have a populated solution file, we can open the file in Visual Studio.

With Visual Studio installed, we can type the name of the file to automatically open it.

```
PS C:\CoreCLI> .\CoreCLI.sln
```

In Visual Studio, we see all 3 projects, and we can run the tests in the test explorer.

When using Visual Studio, I'll often create the projects and classes inside the IDE. But it is good to know that we can do this all from the command line in case we need to. Also, if we understand how to do things from the command line, we have a better understanding of what Visual Studio does behind the scenes.

Dependency Injection
---------------------
As a last stop, we will take a quick look at the built-in dependency injection container that comes with ASP.NET Core. This allows us to quickly swap out dependencies to make our code more flexible and easier to test and maintain.

For our application, we will inject the data provider into the controller for the service. Then our controller will not need to know about any specific data provider, and we can change it in the service startup. It will also allow us to mock out the data provider for our unit tests so that we have consistent data and behavior.

As a first step, stop the web service (if it is running).

```
      Application is shutting down...
PS C:\CoreCLI\person-api>  
```

### Creating an Abstraction
Before changing out the data provider, we will need to create an abstraction that we can use. In this case, it will be an interface. Then the controller will reference the interface methods instead of the methods on a concrete type.

Visual Studio Code has an "Extract Interface" shortcut, but it does not quite do what I would like it to for this application. So I'll use Visual Studio instead.

In Visual Studio, open the "HardCodedPeopleProvider.cs" file and click on the class name.

Then use "Ctrl+." to bring up the built-in refactoring tools. Select "Extract interface...".

In the popup box, change the name of the interface to "IPeopleProvider" and click "OK".

This will create a new file (IPeopleProvider.cs) with the following code.

```csharp
namespace person_api
{
    public interface IPeopleProvider
    {
        List<Person> GetPeople();
        Person GetPerson(int id);
    }
}
```

*If you do not have Visual Studio, you can create the "IPeopleProvider.cs" file manually and add the code listed above.*

This interface is an abstraction that represents any class that includes these 2 methods.

In addition to creating a new file, Visual Studio updated the "HardCodedPeopleProvider" class to denote that it implements the new interface.

```csharp
    public class HardCodedPeopleProvider : IPeopleProvider
    {
        ...
    }
```

*If you add the interface manually, you will need to add the ': IPeopleProvider' to the class yourself.*

### Updating the Controller
Now that we have the interface, we can update the controller to use the interface for the field.

Here is an update to the "PeopleController" class.

```csharp
public class PeopleController : ControllerBase
{
    IPeopleProvider provider = new HardCodedPeopleProvider();
    ...
}
```

The next step is to remove the step where we "new" up the HardCodedPeopleProvider. Instead, we will create a constructor with a parameter that will set the field.

```csharp
public class PeopleController : ControllerBase
{
    IPeopleProvider provider;

    public PeopleController(IPeopleProvider provider)
    {
        this.provider = provider;
    }
    ...
}
```

With this code in place, the controller is no longer responsible for the provider. Whoever creates the controller is responsible for providing an already-instantiated provider (through the constructor parameter).

The body of the constructor sets the private field based on the parameter coming in.

*If you are not familiar with dependency injection, you can take a look at the materials available here: [DI Why: Getting a Grip on Dependency Injection](http://www.jeremybytes.com/Demos.aspx#DI).*

### Running the Application
At this point, we can build and run the application. We'll go back to the command line for this.

When we run "dotnet build" we get a successful build.

```
PS C:\CoreCLI\person-api> dotnet build  
Microsoft (R) Build Engine version 16.3.0+0f4c62fea for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 42.84 ms for C:\CoreCLI\person-api\person-api.csproj.
  person-api -> C:\CoreCLI\person-api\bin\Debug\netcoreapp3.0\person-api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:13.40
PS C:\CoreCLI\person-api>  
```

And if we use "dotnet run" the service starts successfully.

```
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

But if we navigate to the service in the browser, we get a runtime error.

[http://localhost:9874/people](http://localhost:9874/people)

If we go back to PowerShell, it lists the error. Look for the "Fail" in the output.

```
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
System.InvalidOperationException: Unable to resolve service for type 'person_api.IPeopleProvider' while attempting to activate 'person_api.Controllers.PeopleController'.
   at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.GetService(IServiceProvider sp, Type type, Type requiredBy, Boolean isDefaultParameterRequired)
```

An exception is thrown when trying to create an instance of "PeopleController". The error tells us that the system was unable to resolve "IPeopleProvder".

Although we coded the controller to use the interface, we still need to tell the dependency injection container how to map that abstraction to a concrete type.

### Mapping the Interface in the Dependency Injection Container
We can set up this mapping in the "Startup.cs" file, specifically in the "ConfigureServices" method.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddSingleton<IPeopleProvider, HardCodedPeopleProvider>();
}
```

The "AddSingleton" method specifies that anywhere we need an "IPeopleProvider", the system should use a "HardCodedPeopleProvider".

"AddSingleton" will create a single instance of the HardCodedPeopleProvider regardless of how many times we need one. Other options include "AddTransient" and "AddScoped". A discussion of scopes is outside the scope of this walkthrough.

### Running the Service
Now if we stop and restart the service, we can navigate to the service successfully.

```
    Application is shutting down...
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

[http://localhost:9874/people](http://localhost:9874/people)

```json
[{"id":1,"givenName":"John","familyName":"Koenig","startDate":"1975-10-17T00:00:00","rating":6,"formatString":null},
{"id":2,"givenName":"Dylan","familyName":"Hunt","startDate":"2000-10-02T00:00:00","rating":8,"formatString":null},
{"id":3,"givenName":"Leela","familyName":"Turanga","startDate":"1999-03-28T00:00:00","rating":8,"formatString":"{1} {0}"},
{"id":4,"givenName":"John","familyName":"Crichton","startDate":"1999-03-19T00:00:00","rating":7,"formatString":null},
{"id":5,"givenName":"Dave","familyName":"Lister","startDate":"1988-02-15T00:00:00","rating":9,"formatString":null},
{"id":6,"givenName":"Laura","familyName":"Roslin","startDate":"2003-12-08T00:00:00","rating":6,"formatString":null},
{"id":7,"givenName":"John","familyName":"Sheridan","startDate":"1994-01-26T00:00:00","rating":6,"formatString":null},
{"id":8,"givenName":"Dante","familyName":"Montana","startDate":"2000-11-01T00:00:00","rating":5,"formatString":null},
{"id":9,"givenName":"Isaac","familyName":"Gampu","startDate":"1977-09-10T00:00:00","rating":4,"formatString":null}]
```

### Setting Up Another Provider
Let's set up another data provider so that we can see how this works.

We will use already-created code for this. In addition to the "snippets.txt" file, there are "CSVPeopleProvider.cs" and "People.txt" files in the [Starting Files folder](https://github.com/jeremybytes/core-cli-30/tree/master/StartingFiles) of the repository.

Copy "CSVPeopleProvider.cs" into the "Models" folder of the web service.

Copy "People.txt" into the root folder of the web service.

As noted above, after copying the files into the appropriate folders, they automatically show up as part of the web service project, whether we use Visual Studio or Visual Studio Code.

"CSVPeopleProvider" implements the "IPeopleProvider" interface.

```csharp
public class CSVPeopleProvider : IPeopleProvider
```

The class reads data from a text file on the file system (People.txt) and parses it into C# Person objects.

One more thing we need to do is set the "People.txt" file so that it is copied to the output folder of the project.

In Visual Studio, right-click on the "People.txt" file and select "Properties". Change the "Copy to Output Directory" setting to "Copy always".

If you are using Visual Studio Code, you can manually change the "people-api.csproj" file to add this setting. Here is the completed project file. Note the "ItemGroup" section:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <RootNamespace>person_api</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <None Update="People.txt">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

### Updating Configuration
Now that we have the code for the new data provider, we can change the configuration in the "Startup.cs" file.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddSingleton<IPeopleProvider, CSVPeopleProvider>();
}
```

### Running the Service
Next, stop and restart the service.

```
      Application is shutting down...
PS C:\CoreCLI\person-api> dotnet run  
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:9874
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\CoreCLI\person-api
```

To show the updated service in action, go back to the PowerShell window for the console application. Then run the application.

```
PS C:\CoreCLI\person-console> dotnet run  
One moment please...
John Koenig
Dylan Hunt
Turanga Leela
John Crichton
Dave Lister
Laura Roslin
John Sheridan
Dante Montana
Isaac Gampu
**Jeremy Awesome**
===============
PS C:\CoreCLI\person-console>  
```

The text file has an extra record: Jeremy Awesome. So we can tell by looking at the data that the service is now getting data from the text file instead of using the hard-coded data provider.

### Fixing Broken Unit Tests
Since we changed the constructor for the controller, our unit tests no longer build.

The problem is in the Setup method of the "PeopleControllerTests" class.

```csharp
public class PeopleControllerTests
{
    PeopleController controller;

    [SetUp]
    public void Setup()
    {
        controller = new PeopleController();
    }
    ...
}
```

We need a data provider to pass as a parameter to the PeopleController constructor. We could use a mocking framework. But to keep things easier for those who are not familiar with mocking, we will use a fake object.

### Adding a Fake Data Reader
To create a fake data reader for testing, we will start with the HardCodedDataReader that we already have. This will save us a lot of typing.

Copy the "HardCodedDataReader.cs" file from the web service project folder into the unit test project folder.

After copying the file, rename it to "FakePeopleProvider.cs".

Open the file in Visual Studio (or Visual Studio Code) and change the name of the class to "FakePeopleProvider". We will leave the rest of the class the same. In addition, we'll change the namespace to "person_api_tests".

```csharp
namespace person_api_tests
{
    public class FakePeopleProvider : IPeopleProvider
    {
        public List<Person> GetPeople() ...

        public Person GetPerson(int id) ...
    }
}
```

This gives us a separate class that we can use for testing. Now we have more control over the test behavior.

### Updating the Tests
To update the tests, update the "Setup" method to use the "FakePeopleProvider".

```csharp
[SetUp]
public void Setup()
{
    var provider = new FakePeopleProvider();
    controller = new PeopleController(provider);
}
```

Since the "Setup" method runs before each test, all of our tests are now using the fake data provider.

If we re-run the tests, we'll find that they are all passing.

```
PS C:\CoreCLI\person-api-tests> dotnet test
Test run for C:\CoreCLI\person-api-tests\bin\Debug\netcoreapp3.0\person-api-tests.dll(.NETCoreApp,Version=v3.0)
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 3
     Passed: 3
 Total time: 2.7987 Seconds
PS C:\CoreCLI\person-api-tests>
```

Wrap Up
--------
We've seen how to use .NET Core and the command-line interface (CLI) to build a web service, unit tests, and a console application.

In addition, we've seen how to use the built-in dependency injection container. This allows us to change out the data provider with a few small changes and also helps us isolate our code for better control over our unit tests.

For more information, visit the GitHub repo to view the code samples and links to relevant articles.

[GitHub: Get Comfortable with .NET Core and the CLI](https://github.com/jeremybytes/core-cli-30)

Happy Coding!
