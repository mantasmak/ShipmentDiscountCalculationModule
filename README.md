## Prerequisites

This project requires the [.NET Core SDK](https://www.microsoft.com/net/download). 

## Build and run

To build and run the solution, change to the **/ShipmentDiscountCalculationModule/ShipmentDiscountCalculationModule.UI* directory and execute the following command:

```console
dotnet run
```
`dotnet run` builds the solution and runs the output executable with default file names. 
If you would like to specify input file names, execute the following command:

```console
dotnet run <input file name>.txt <shipping price details file name>.txt
```

To run the tests, change to the **/ShipmentDiscountCalculationModule* directory and execute the following command:

```console
dotnet test
```

`dotnet test` runs the configured tests.
