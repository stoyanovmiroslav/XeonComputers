using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XeonComputers.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.ViewModels;
using AutoMapper;
using XeonComputers.Areas.Administrator.ViewModels.Home;

namespace XeonComputers.Areas.Administrator.Controllers
{
    public class HomeController : AdministratorController
    {
        private readonly IOrdersService ordersService;
        public readonly IPartnerRequestsService partnerRequestService;
        private readonly IMapper mapper;

        public HomeController(IOrdersService ordersService, IPartnerRequestsService partnerRequestService, IMapper mapper)
        {
            this.ordersService = ordersService;
            this.partnerRequestService = partnerRequestService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var unprocessedОrders = this.ordersService.GetUnprocessedOrders().OrderByDescending(x => x.OrderDate);
            var processedОrders = this.ordersService.GetProcessedOrders().OrderByDescending(x => x.DispatchDate);

            var unprocessedОrdersViewModel = mapper.Map<IList<IndexUnprocessedОrdersViewModels>>(unprocessedОrders);
            var processedОrdersViewModel = mapper.Map<IList<IndexProcessedОrdersViewModels>>(processedОrders);
            var partnerRequestsCount = this.partnerRequestService.GetPartnetsRequests().Count();

            var viewModel = new IndexViewModel
            {
                UnprocessedОrdersViewModel = unprocessedОrdersViewModel,
                ProcessedОrdersViewModel = processedОrdersViewModel,
                PartnerRequestsCount = partnerRequestsCount
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}