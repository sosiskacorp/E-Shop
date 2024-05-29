using E_Shop.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Shop.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ClothingController : Controller
{
    private readonly IClothingRepository _clothingRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IFileService _fileService;

    public ClothingController(IClothingRepository clothingRepo, ICategoryRepository categoryRepo, IFileService fileService)
    {
        _clothingRepo = clothingRepo;
        _categoryRepo = categoryRepo;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var clothings = await _clothingRepo.GetClothings();
        return View(clothings);
    }

    public async Task<IActionResult> AddClothing()
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
        });
        ClothingDTO clothingToAdd = new() { CategoryList = categorySelectList };
        return View(clothingToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddClothing(ClothingDTO clothingToAdd)
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
        });
        clothingToAdd.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(clothingToAdd);

        try
        {
            if (clothingToAdd.ImageFile != null)
            {
                if(clothingToAdd.ImageFile.Length> 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg",".jpg",".png"];
                string imageName=await _fileService.SaveFile(clothingToAdd.ImageFile, allowedExtensions);
                clothingToAdd.Image = imageName;
            }
            // manual mapping of ClothingDTO -> Clothing
            Clothing clothing = new()
            {
                Id = clothingToAdd.Id,
                Name = clothingToAdd.ClothingName,
                Brand = clothingToAdd.Brand,
                Image = clothingToAdd.Image,
                CategoryId = clothingToAdd.CategoryId,
                Price = clothingToAdd.Price
            };
            await _clothingRepo.AddClothing(clothing);
            TempData["successMessage"] = "Clothing is added successfully";
            return RedirectToAction(nameof(AddClothing));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"]= ex.Message;
            return View(clothingToAdd);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(clothingToAdd);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(clothingToAdd);
        }
    }

    public async Task<IActionResult> UpdateClothing(int id)
    {
        var clothing = await _clothingRepo.GetClothingById(id);
        if(clothing==null)
        {
            TempData["errorMessage"] = $"Clothing with the id: {id} does not found";
            return RedirectToAction(nameof(Index));
        }
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
            Selected=category.Id==clothing.CategoryId
        });
        ClothingDTO clothingToUpdate = new() 
        { 
            CategoryList = categorySelectList,
            ClothingName=clothing.Name,
            Brand=clothing.Brand,
            CategoryId=clothing.CategoryId,
            Price=clothing.Price,
            Image=clothing.Image 
        };
        return View(clothingToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateClothing(ClothingDTO clothingToUpdate)
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
            Selected=category.Id==clothingToUpdate.CategoryId
        });
        clothingToUpdate.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(clothingToUpdate);

        try
        {
            string oldImage = "";
            if (clothingToUpdate.ImageFile != null)
            {
                if (clothingToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                string imageName = await _fileService.SaveFile(clothingToUpdate.ImageFile, allowedExtensions);
                // hold the old image name. Because we will delete this image after updating the new
                oldImage = clothingToUpdate.Image;
                clothingToUpdate.Image = imageName;
            }
            // manual mapping of ClothingDTO -> Clothing
            Clothing clothing = new()
            {
                Id=clothingToUpdate.Id,
                Name = clothingToUpdate.ClothingName,
                Brand = clothingToUpdate.Brand,
                CategoryId = clothingToUpdate.CategoryId,
                Price = clothingToUpdate.Price,
                Image = clothingToUpdate.Image
            };
            await _clothingRepo.UpdateClothing(clothing);
            // if image is updated, then delete it from the folder too
            if(!string.IsNullOrWhiteSpace(oldImage))
            {
                _fileService.DeleteFile(oldImage);
            }
            TempData["successMessage"] = "Clothing is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(clothingToUpdate);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(clothingToUpdate);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(clothingToUpdate);
        }
    }

    public async Task<IActionResult> DeleteClothing(int id)
    {
        try
        {
            var clothing = await _clothingRepo.GetClothingById(id);
            if (clothing == null)
            {
                TempData["errorMessage"] = $"Clothing with the id: {id} does not found";
            }
            else
            {
                await _clothingRepo.DeleteClothing(clothing);
                if (!string.IsNullOrWhiteSpace(clothing.Image))
                {
                    _fileService.DeleteFile(clothing.Image);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on deleting the data";
        }
        return RedirectToAction(nameof(Index));
    }

}