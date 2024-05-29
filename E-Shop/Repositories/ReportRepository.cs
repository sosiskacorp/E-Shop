using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace E_Shop.Repositories;

[Authorize(Roles = nameof(Roles.Admin))]
public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;
    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TopNSoldClothingModel>> GetTopNSellingClothingsByDate(DateTime startDate, DateTime endDate)
    {
        var startDateParam = new SqlParameter("@startDate", startDate);
        var endDateParam = new SqlParameter("@endDate", endDate);
        var topFiveSoldClothings = await _context.Database.SqlQueryRaw<TopNSoldClothingModel>("exec Usp_GetTopNSellingClothingsByDate @startDate,@endDate", startDateParam, endDateParam).ToListAsync();
        return topFiveSoldClothings;
    }

}

public interface IReportRepository
{
    Task<IEnumerable<TopNSoldClothingModel>> GetTopNSellingClothingsByDate(DateTime startDate, DateTime endDate);
}