using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Areas.Administrator.ViewModels.Home;
using XeonComputers.Areas.Administrator.ViewModels.UserRequests;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class UserRequestsController : AdministratorController
    {
        private readonly IUserRequestsService userRequestService;
        private readonly IMapper mapper;

        public UserRequestsController(IUserRequestsService userRequestService, IMapper mapper)
        {
            this.userRequestService = userRequestService;
            this.mapper = mapper;
        }

        public IActionResult Index(int id)
        {
            var userRequests = this.userRequestService.All().OrderByDescending(x => x.RequestDate).ToList();

            var currentUserRequest = this.userRequestService.GetRequestById(id);
            if (currentUserRequest == null)
            {
                currentUserRequest = userRequests.FirstOrDefault();
            }

            this.userRequestService.Seen(id);

            var userRequestsViewModel = mapper.Map<IList<UserRequestViewModel>>(userRequests);
            var currentUserRequestViewModel = mapper.Map<UserRequestViewModel>(currentUserRequest);

            var viewModel = new IndexUserRequestViewModel
            {
                UserRequestsViewModel = userRequestsViewModel,
                UserRequestViewModel = currentUserRequestViewModel
            };

            return View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            var userRequests = this.userRequestService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unseen(int id)
        {
            this.userRequestService.Unseen(id);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
