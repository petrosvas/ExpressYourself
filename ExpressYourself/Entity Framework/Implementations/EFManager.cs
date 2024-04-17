using ExpressYourself.Entity_Framework.DBContext;
using ExpressYourself.Entity_Framework.Interfaces;
using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Entity_Framework.Implementations
{
    public class EFManager : IEFManager
    {
        private readonly AppDbContext _appDbContext;

        public EFManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Countries> GetIpDetails(string IP)
        {
            var found = await _appDbContext.IPAddresses.FirstOrDefaultAsync(iPAddresses => iPAddresses.IP == IP);
            if (found != null)
            {
                return await _appDbContext.Countries.FirstAsync(country => country.Id == found.CountryId);
            }
            return null;
        }

        public async Task SetNewIP(Countries countries, string IP)
        {
            var country = await _appDbContext.Countries.FirstOrDefaultAsync(country => country.Name == countries.Name);
            if (country == null)
            {
                await _appDbContext.Countries.AddAsync(new Countries
                {
                    CreatedAt = DateTime.Now,
                    Name = countries.Name,
                    ThreeLetterCode = countries.ThreeLetterCode,
                    TwoLetterCode = countries.TwoLetterCode
                });
                await _appDbContext.SaveChangesAsync();
            }

            var newCountry = await _appDbContext.Countries.FirstAsync(country => country.Name == countries.Name);
            await _appDbContext.IPAddresses.AddAsync(new IPAddresses
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CountryId = newCountry.Id,
                IP = IP
            });
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<SQLReport>> GetSqlReport()
        {
            string query = "SELECT [Countries].Name AS CountryName, COUNT([IPAddresses].CountryId) AS AddressesCount, MAX([IPAddresses].UpdatedAt) AS LastAddressUpdated FROM [Countries], [IPAddresses] WHERE [Countries].Id = [IPAddresses].CountryId GROUP BY [Countries].Name";

            return await _appDbContext.SQLReport.FromSqlRaw(query).ToListAsync();
        }

        public async Task<List<IPToUpdate>> GetIPs(int rowsOffset)
        {
            List<IPToUpdate> list = new List<IPToUpdate>();
            var ipAddresses = await _appDbContext.IPAddresses.OrderBy(ipAddress => ipAddress.Id).Skip(rowsOffset).Take(100).ToListAsync();
            foreach (var ipAddress in ipAddresses)
            {
                Countries country = await _appDbContext.Countries.FirstAsync(country => country.Id == ipAddress.CountryId);
                list.Add(new IPToUpdate
                {
                    IP = ipAddress.IP,
                    Name = country.Name,
                    ThreeLetterCode = country.ThreeLetterCode,
                    TwoLetterCode = country.TwoLetterCode
                });
            };
            return list;
        }

        public async Task<bool> UpdateIPs(IPToUpdate row, string[] IP2CArray)
        {
            if (row.Name == IP2CArray.Name())
                return false;

            var country = await _appDbContext.Countries.FirstOrDefaultAsync(country => country.Name == IP2CArray.Name());
            if (country == null)
            {
                await _appDbContext.Countries.AddAsync(new Countries
                {
                    Name = IP2CArray.Name(),
                    ThreeLetterCode = IP2CArray.ThreeLetterCode(),
                    TwoLetterCode = IP2CArray.TwoLetterCode(),
                    CreatedAt = DateTime.Now,
                });
                await _appDbContext.SaveChangesAsync();
            }
            var newId = await _appDbContext.Countries.FirstAsync(country => country.Name == IP2CArray.Name());
            var ipAddress = await _appDbContext.IPAddresses.FirstAsync(ipAddress => ipAddress.IP == row.IP);
            ipAddress.CountryId = newId.Id;
            ipAddress.UpdatedAt = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
