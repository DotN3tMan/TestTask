namespace Apartaments.Infrastructure
{
    public static partial class SqlLiteManager
    {
        private static string ExistsTableQuery = File.ReadAllText(@"./Infrastructure/SqlQuerys/ExistsTableQuery.sql");
        private static string CreateTable_ApartamentsQuery = File.ReadAllText(@"./Infrastructure/SqlQuerys/CreateTable_Apartaments.sql");
        private static string CreateTable_HistoryApartmentPricesQuery = File.ReadAllText(@"./Infrastructure/SqlQuerys/CreateTable_HistoryApartmentPrices.sql");
        private static string SeedDataQuery = File.ReadAllText(@"./Infrastructure/SqlQuerys/SeedDataQuery.sql");
    }
}
