using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Shop.Models.DTOs;
public class ClothingDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string? ClothingName { get; set; }

    [Required]
    [MaxLength(40)]
    public string? Brand { get; set; }
    [Required]
    public double Price { get; set; }
    public string? Image { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public IFormFile? ImageFile { get; set; }
    public IEnumerable<SelectListItem>? CategoryList { get; set; }
}