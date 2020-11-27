using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Office.Core.Entities;
using Office.Core.Services;
using Office.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Web.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly ISpotService _spotService;

        public PositionController(ISpotService spotService)
        {
            _spotService = spotService;
        }

        public async Task<IActionResult> Index(int page = 1, string name = null)
        {

           var spots = await _spotService.GetSpotsByNameAsync(name, page, 3);
           
            var model = new PositionsViewModel()
            {
                positions = spots
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(PositionsViewModel searchModel, int page =1)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (searchModel?.name != null)
            {
                var spots = await _spotService.GetSpotsByNameAsync(searchModel?.name, page, 3);
                var model = new PositionsViewModel()
                {
                    positions = spots
                };
                return View("Index", model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            PositionViewModel model = new PositionViewModel()
            {
                Managers = await GetManagersList()
            };
            return View(model);
        }
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PositionViewModel positionVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            Spot spot = new Spot
            {
                Name = positionVM.Name,
                Title = positionVM.Title,
                Bonus = positionVM.Bonus,
                ManagerId = positionVM.ManagerId
            };
            using (var memoryStream = new MemoryStream())
            {
                await positionVM.FormFile.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    spot.Image = memoryStream.ToArray();                        
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            var successful = await _spotService.AddSpotAsync(spot);
            if (!successful)
            {
                return BadRequest("Could not add a new Position.");
            }
            TempData["info"] = $"Adding position {positionVM.Title} for {positionVM.Name}.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _spotService.GetSpotByIdAsync((Guid)id);
            if (spot == null)
            {
                return NotFound();
            }

            EditPositionViewModel model = new EditPositionViewModel()
            {
                Bonus = spot.Bonus,
                Created = spot.Created,
                Title = spot.Title,
                Name = spot.Name,
                IsActive = spot.IsActive,
                DirectReports = spot.DirectReports,
                ManagerId = spot.ManagerId,
                Managers = await GetManagersList(),
                Image = spot.Image
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPositionViewModel positionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(positionVM);
            }

            Spot spot = new Spot
            {
                Id = (Guid)positionVM.Id,
                Name = positionVM.Name,
                Title = positionVM.Title,
                Bonus = positionVM.Bonus,
                ManagerId = positionVM.ManagerId
            };
            if (positionVM.FormFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await positionVM.FormFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        spot.Image = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            var successful = await _spotService.EditSpotAsync(spot);
            if (!successful)
            {
                return BadRequest("Could not edit Position.");
            }

            TempData["info"] = $"Edting position {positionVM.Title} for {positionVM.Name}.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _spotService.GetSpotByIdAsync((Guid)id);
            if (spot == null)
            {
                return NotFound();
            }

            PositionViewModel model = new PositionViewModel()
            {
                Bonus = spot.Bonus,
                Created = spot.Created,
                Title = spot.Title,
                Name = spot.Name,
                IsActive = spot.IsActive,
                DirectReports = spot.DirectReports,
                ManagerId = spot.ManagerId,
                Image = spot.Image
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var spot = await _spotService.DeleteSpotAsnc(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetManagersList()
        {
            var spots = await _spotService.GetManagersAsync();
            List<SelectListItem> managerList = spots
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();
            return new SelectList(managerList, "Value", "Text");
        }
    }
}