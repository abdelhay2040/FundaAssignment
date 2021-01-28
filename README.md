# Funda Programming Assignment

In this repository there's the code of the implementation of the programming assignment for the Funda appliance.
It's a simple Command Line application created using .Net Core 2.1. 
The code is (hopefully) easy to follow and have some comments on the most important parts.
***
## General Usage

  - Build it (using .Net Core 2.1 SDK)
  - Go to the folder Funda.ProgrammingAssignment.Console bin folder (most probably bin/Debug/netcoreapp2.1/)
  - Execute the console app (dotnet .\Funda.ProgrammingAssignment.Console.dll --help to get the command detail) 

### Assignment Scenario
From the bin folder of the Console execute, in order:
- `dotnet .\Funda.ProgrammingAssignment.Console.dll -t Amsterdam` 
- `dotnet .\Funda.ProgrammingAssignment.Console.dll -t Amsterdam Tuin`

It will dump on the console the results of the search. Please note that, due to the restriction of the service, it will most probably incur in the temporary block of the server. The application will try to get the data after an increasing amount of time on each failure. It will just take some time but it will succeed.

### Unit Tests
The project contains a small set of unit tests created using xUnit. They can be runned from Vstudio or using console by running `dotnet test .\Funda.ProgrammingAssignment.UnitTests` from the root of the project folder
***
## Other features 
There are different things that can be tested just for "fun". I'll start by explaining the different options of the console application:
- `--fa` *Use Fake API integration* (default **false**) that will cause the application to load a fake module to return "dummy data". Usefull to test the dumpers and for educational purpose
- `--rn` *Results Number* (default **10**) Change the number of results returned from the execution. Please note that changing the number of results does not affect directly (small impact) the speed of execution of the method as the data needs to be retrieved completely anyway
- `--t` *Search Terms* (**Required**) The terms used to perform the search, as a list of space separated words


