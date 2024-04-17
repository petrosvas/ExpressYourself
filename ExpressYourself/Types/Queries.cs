namespace ExpressYourself.Types
{
    public static class Queries
    {
        public const string GetCountryByIP =
            "SELECT [Countries].[Name], [Countries].[TwoLetterCode], [Countries].[ThreeLetterCode]" +
            " FROM [dbo].[Countries], [dbo].[IPAddresses]" +
            " WHERE [IPAddresses].[CountryId] = [Countries].[Id]" +
            " AND [IPAddresses].[IP] = @ip";

        public const string InsertIntoIpAddresses =
            "SET IDENTITY_INSERT [dbo].[IPAddresses] ON" +
            " INSERT [dbo].[IPAddresses] ([Id], [CountryId], [IP], [CreatedAt], [UpdatedAt]) VALUES" +
            " ((SELECT COALESCE(MAX(Id), 0) + 1 FROM [IPAddresses]), @countryId, @ip, CAST(CURRENT_TIMESTAMP AS DateTime2)," +
            " CAST(CURRENT_TIMESTAMP AS DateTime2))" +
            " SET IDENTITY_INSERT [dbo].[IPAddresses] OFF";

        public const string GetCountryByName =
            "SELECT [Countries].[Id] FROM [master].[dbo].[Countries]" +
            " WHERE Name = @name";

        public const string InsertIntoCountries =
            "SET IDENTITY_INSERT [dbo].[Countries] ON" +
            " INSERT [dbo].[Countries] ([Id], [Name], [TwoLetterCode], [ThreeLetterCode], [CreatedAt]) VALUES (@id," +
            " @name, @twoLetterCode, @threeLetterCode, CAST(CURRENT_TIMESTAMP AS DateTime2))" +
            " SET IDENTITY_INSERT [dbo].[Countries] OFF";

        public const string GetCountriesMaxID =
            "SELECT COALESCE(MAX(Id), 0) as MAXID from [master].[dbo].[Countries]";

        public const string GetAddressesReport =
            "SELECT [Countries].Name AS CountryName, COUNT([IPAddresses].CountryId) AS AddressesCount, MAX([IPAddresses].UpdatedAt) AS LastAddressUpdated" +
            " FROM [Countries], [IPAddresses]" +
            " WHERE [Countries].Id = [IPAddresses].CountryId" +
            " GROUP BY [Countries].Name";

        public const string GetIPsToUpdate =
            "SELECT [IPAddresses].[IP], [Countries].[Name], [Countries].[TwoLetterCode], [Countries].[ThreeLetterCode]" +
            " FROM [IPAddresses], [Countries]" +
            " WHERE [IPAddresses].CountryId = [Countries].Id" +
            " ORDER BY [IPAddresses].[Id]" +
            " OFFSET @offset ROWS FETCH NEXT 100 ROWS ONLY";

        public const string SetIPAdressCountryId =
            "UPDATE [IPAddresses]" +
            " SET CountryId = @countryId" +
            ", UpdatedAt = CAST(CURRENT_TIMESTAMP AS DateTime2)" +
            " WHERE IP = @ip";
    }
}
