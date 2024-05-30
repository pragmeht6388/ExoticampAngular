﻿using Exoticamp.UI.Models;
using Exoticamp.UI.Models.Campsite;
using Exoticamp.UI.Models.ContactUs;
using Exoticamp.UI.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Exoticamp.UI.Controllers
{
    public class CampsiteController : Controller
    {
        private readonly ICampsiteRepository _campsiteRepository;

        public CampsiteController(ICampsiteRepository campsiteRepository)
        {
            _campsiteRepository = campsiteRepository;
        }
        [HttpGet]
        public IActionResult AddCampsite()
        {
            return View();
        }


        public async Task<IActionResult> AddCampsite(CampsiteVM campsite)
        {

            var result = await _campsiteRepository.AddCampsite(campsite);

            if (result.Message != null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            else
            {

                ModelState.AddModelError(string.Empty, "An error occurred while creating the SysPrefCompany.");
            }


            return RedirectToAction("ShowCampsite");

        }

        public async Task<IActionResult> ShowCampsite()
        {
            var campsiteDetail = await _campsiteRepository.GetAllCampsites();
            return View(campsiteDetail);
        }

        [HttpGet]
        public async Task<IActionResult> EditCampsite(string id)
        {
            var eventObj = await _campsiteRepository.GetCampsiteById(id);
            return View(eventObj.Data);
        }
        [HttpPost]
        public async Task<IActionResult> EditCampsite(CampsiteVM model)
         { 
            if (ModelState.IsValid)
            {
                model.isActive = true;
                var response = await _campsiteRepository.EditCampsite(model);
                
                return RedirectToAction("ShowCampsite");
            }
            else
                ModelState.AddModelError("", "Oops! Some error occured.");

            return View(model);


        }

        public async Task<IActionResult> DeleteCampsite(string id)
        {
            var deleteResponse = await _campsiteRepository.DeleteCampsite(id);

            return RedirectToAction("ShowCampsite");
            
        }


    }
}
