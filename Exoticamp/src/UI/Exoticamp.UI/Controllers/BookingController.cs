﻿using AspNetCore;
using Exoticamp.Domain.Entities;
using Exoticamp.UI.Models;
using Exoticamp.UI.Models.Booking;
using Exoticamp.UI.Services.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exoticamp.UI.Controllers
{
    public class BookingController : Controller
    {

        private readonly IBookingRepository _bookingRepository;
        public readonly ICampsiteDetailsRepository _campsiteDetailsRepository;
        public readonly ILocationRepository _locationRepository;

        public BookingController(IBookingRepository bookingRepository, ICampsiteDetailsRepository campsiteDetailsRepository, ILocationRepository locationRepository)
        {
            _bookingRepository = bookingRepository;
            _campsiteDetailsRepository = campsiteDetailsRepository;
            _locationRepository = locationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBookings()
        {
            var bookings=await _bookingRepository.GetAllBookings();
            var Campsites = await _campsiteDetailsRepository.GetAllCampsites();

            var Locations = await _locationRepository.GetAllLocations();
            if (Campsites == null || Locations == null)
            {
                return View("Error", new ErrorViewModel { });
            }
            var CampSiteList = new List<CampsiteDetails>();
            var LocationList = new List<Location>();
        
            return View(bookings);
        }
        [HttpGet]
        public async Task<ActionResult> AddBooking()
        {
            BookingVM model = new BookingVM();

            var Campsites = await _campsiteDetailsRepository.GetAllCampsites();
   
            var Locations = await _locationRepository.GetAllLocations();
            if (Campsites == null ||  Locations == null)
            {
                return View("Error", new ErrorViewModel { });
            }

            model.CampsitesList= new SelectList(Campsites, "Id", "Name");
            model.LocationsList = new SelectList(Locations, "Id", "Name");


            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddBooking(BookingVM model)
        {
            var response=await _bookingRepository.AddBooking(model);
            if (response.Succeeded)
            {
                return RedirectToAction("GetAllBookings", "Booking");
            }
            return View(model);

        }
        [HttpGet]
        public async Task<JsonResult> GetCampsitesByLocation(Guid locationId)
        {
            var locations = await _locationRepository.GetAllLocations();
            var loc = locations.FirstOrDefault(x => x.Id == locationId);



            if (loc == null)
            {
                return Json(new List<object>());
            }

            var campsites = await _campsiteDetailsRepository.GetAllCampsites();
            var filteredCampsites = campsites
                .Where(c => c.Location == loc.Name)
                .Select(c => new { value = c.Id, text = c.Name })
                .ToList();

            return Json(filteredCampsites);
        }

        [HttpGet]
        public async Task<ActionResult> UserBooking(string id)
        {
            var campsite = await _campsiteDetailsRepository.GetCampsiteById(id);
            var location = await _locationRepository.GetAllLocations();
            var loc = location.FirstOrDefault(x => x.Name == campsite.Data.Location);
            campsite.Data.LocationId = loc.Id;
            ViewBag.Campsite = campsite.Data;
            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UserBooking(BookingVM model)
        {
            var response = await _bookingRepository.AddBooking(model);
            if (response.Succeeded)
            {
                return RedirectToAction("GetAllBookings", "Booking");
            }
            return View(model);

        }

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            var bookingResponse = await _bookingRepository.GetBookingById(id);

            if (bookingResponse.Succeeded)
            {
                var booking = bookingResponse.Data; // Assuming Data contains the BookingVM
                return View(booking);
            }

            // Handle the case when booking is not found
            ViewBag.ErrorMessage = bookingResponse.Message;
            return View("GetAllBookings", "Booking");
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
           var booking=await _bookingRepository.GetBookingById(id);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(BookingVM model)
        {
            var updatedBooking = await _bookingRepository.UpdateBooking(model);

            if (updatedBooking.Succeeded)
            {
                return RedirectToAction("Details", new { id = model.BookingId });
            }

            ViewBag.ErrorMessage = updatedBooking.Message;
            return View(model);
        }




    }
}
