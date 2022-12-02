# AdventOfCodeBase
A basic C# program for Advent of Code, retrieving puzzle inputs on the go and creating instances of solutions as they are created. It runs on .NET 7.0 and can be built easily in Visual Studio Code or Visual Studio. 

#### Build & Run
```
> dotnet build
> dotnet run
```

#### Configuration
The local appsettings will be used to configure what solutions will run.  
Make a copy of appsettings.json and rename it to `appsettings.local.json`.  
Any changes to apsettings.local.json will be ignored by git.
```json
{
  "Config": {
    // Simple int value representing year to pull puzzles from. Used for folder navigation and puzzle input retrieval. 
    "year": 2022,
    // int[] representing days of which to collect solutions. 0 resolves to "all". 
    "days": []
  }
}
```

#### Secrets
Because inputs are retrieved automatically, you must setup the setup the sensitive data using `dotnet user-secrets`.

The value you need to retreive is from the Advent of Code website. Using the developer tools, copy the `session` cookie value located in the "Storage" tab.

Run the following in the project directory
```
> dotnet user-secrets init
> dotnet user-secrets set "session" "<COOKIE_VALUE>"
```

You can also right click the project and select "Manage User Secrets".  
The secrets.json should look like this:

```json
{
  "session": "<COOKIE_VALUE>"
}
```
