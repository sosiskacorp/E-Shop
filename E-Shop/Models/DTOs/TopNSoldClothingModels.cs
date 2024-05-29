namespace E_Shop.Models.DTOs;

public record TopNSoldClothingModel(string ClothingName, string Brand, int TotalUnitSold); 
public record TopNSoldClothingsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldClothingModel> TopNSoldClothings);