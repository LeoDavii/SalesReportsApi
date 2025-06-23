namespace SalesReports.Tests.Integration.Controllers;

public static class SalesReportControllerTestsHelpers
{

    public static string CreateMalformedCsvContent()
    {
        return @"Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit
Middle East and North Africa,Azerbaijan,Snacks,Online,C,10/8/2014,535113847,10/23/2014,INVALID_NUMBER,152.58,97.44,142509.72,91008.96,51500.76
Europe,Germany,Beverages,Offline,H,INVALID_DATE,123456789,12/20/2015,500,INVALID_PRICE,45.00,37500.00,22500.00,15000.00";
    }

    public static string CreateValidCsvContent()
    {
        return @"Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit
Middle East and North Africa,Azerbaijan,Snacks,Online,C,10/8/2014,535113847,10/23/2014,934,152.58,97.44,142509.72,91008.96,51500.76
Europe,Germany,Beverages,Offline,H,12/15/2015,123456789,12/20/2015,500,75.00,45.00,37500.00,22500.00,15000.00
North America,United States,Electronics,Online,M,5/10/2016,987654321,5/15/2016,250,199.99,120.00,49997.50,30000.00,19997.50";
    }
}