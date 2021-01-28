using System.Collections.Generic;
using Funda.ProgrammingAssignment.Domain.Common.Dto;

namespace Funda.ProgrammingAssignment.ServiceProxy.Model.ApiResults
{
    public class PagedApiResult<T> : ApiResult<T> where T : class
    {
        public int NumberOfPages { get; set; }
        public int ActualPage { get; set; }
        public string NextPage { get; set; }
        public bool HasNextPageInPagination => ActualPage < NumberOfPages;

        public int TotalNumberOfResults { get; set; }
        public new IEnumerable<T> Data { get; set; }

        public new static PagedApiResult<T> Failed(int resultCode, string responseStatusDescription, string errorMessage)
        {
            return new PagedApiResult<T>
            {
                WasSuccessfull = false,
                FailureReason = errorMessage,
                ResultCode = resultCode,
                ResultDescription = responseStatusDescription
            };
        }
    }
}