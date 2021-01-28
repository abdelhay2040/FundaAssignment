using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.ProgrammingAssignment.Domain.Common.Dto;
using Funda.ProgrammingAssignment.Domain.Common.Services;
using Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ApiConfigurationParser;
using Funda.ProgrammingAssignment.ServiceProxy.Services.ProxyService;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater;
using Microsoft.Extensions.Logging;
using ComposableAsync;
using RateLimiter;

namespace Funda.ProgrammingAssignment.ServiceProxy
{
    //The simplest way to get the data. This Repository will use the ApiService in order to get all the pages (do GET while HASNEXTPAGEINPAGINATION) of the result.
    //Not all code has been implemented (special case handling like too many pages or failure in retrieving the result of a page). A recovery strategy injection approach is what i would suggest in this scenario
    public class FundaApiBasedRepository : IPropertiesRepository
    {
        private readonly IFundaProxyApiService _fundaProxyApiService;
        private readonly ILogger _logger;

        public const int DefaultPageSize = 25;
        public const int MaxRetrievablePages = 4000;

        private readonly IRequestStatusUpdater _requestStatusUpdater;
        public FundaApiBasedRepository(IFundaProxyApiService fundaProxyApiService,
            IApiConfigurationParser configurationParser, ILogger logger, IRequestStatusUpdater requestStatusUpdater)
        {
            _fundaProxyApiService = fundaProxyApiService;
            _logger = logger;
            _requestStatusUpdater = requestStatusUpdater;
        }


        public async Task<IEnumerable<PropertyDto>> SearchPropertiesOnSale(IEnumerable<string> searchTerms)
        {
            ConcurrentQueue<PropertyDto> res = new ConcurrentQueue<PropertyDto>();

            //In order to know how many pages the service have to retrieve I have to first try to gather the initial page.
            //I have to start from page 1 as the page 0 
            var pagedResult = await ExecuteApiCall(searchTerms, 1, DefaultPageSize, res);
            _logger.LogTrace($"Number of pages to inquire: {pagedResult.NumberOfPages} ");

            ReturnExceptionIfRequestedTooManyPagesToRetrieve(pagedResult, MaxRetrievablePages);

            if (pagedResult.HasNextPageInPagination)
                //There are more pages to retrieve. Those will be retrieved in parallel
                await RetrieveAllPagesDataAsync(searchTerms, pagedResult, res);

            return res;
        }

        private static void ReturnExceptionIfRequestedTooManyPagesToRetrieve(PagedApiResult<PropertyDto> pagedResult,
            int maxRetrievablePages)
        {
            if (pagedResult.NumberOfPages >= maxRetrievablePages)
            {
                //TODO: Handle Exception too many pages in a more useful way 
                throw new Exception("The request contains too many entries to be processed!");
            }
        }

        private async Task RetrieveAllPagesDataAsync(IEnumerable<string> searchTerms, PagedApiResult<PropertyDto> pagedResult,
            ConcurrentQueue<PropertyDto> res)
        {

            _requestStatusUpdater.Initialize(pagedResult.NumberOfPages, "Retrieving properties data...");

            await ExecuteRequestsAsync(searchTerms, pagedResult.NumberOfPages, res);
        }


        private async Task ExecuteRequestsAsync(IEnumerable<string> searchTerms,
            int totalNumberOfPages, ConcurrentQueue<PropertyDto> res)
        {
            // limits for the request per minutes and per seconds
            var maxRequestNumPerMinuteConstraint = new CountByIntervalAwaitableConstraint(59, TimeSpan.FromMinutes(1));
            var maxRequestNumPerSecondConstraint = new CountByIntervalAwaitableConstraint(5, TimeSpan.FromSeconds(1));

            // Compose the two constraints
            var timeConstraint = TimeLimiter.Compose(maxRequestNumPerMinuteConstraint, maxRequestNumPerSecondConstraint);
            for (var i = 2; i < totalNumberOfPages; i++)
            {
                await timeConstraint;
               
                _requestStatusUpdater.Tick(); 

                await ExecuteApiCall(searchTerms, i, DefaultPageSize, res);
            }
        }

       
        private async Task<PagedApiResult<PropertyDto>> ExecuteApiCall(IEnumerable<string> searchTerms, int currentPage,
            int pageSize, ConcurrentQueue<PropertyDto> res)
        {
            var pagedResult = await _fundaProxyApiService.GetPropertiesOnSale(searchTerms, currentPage, pageSize);
            if (pagedResult.WasSuccessfull)
            {
                foreach (var data in pagedResult.Data)
                    res.Enqueue(data);
            }
            else
            {
                //TODO: In this case we can inject different strategies to handle the different errors. E.g.: In case of 429 response (Too Many Requests) we can choose to slow down the requests and retry or return an information to the user to inform that he should try again later. Right now I'll simply return an Exception
                throw new Exception(pagedResult.FailureReason);
            }

            return pagedResult;
        }

    }
}