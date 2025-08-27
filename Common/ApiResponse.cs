using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;


namespace TodoApi.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }       // Indicates overall success
        public string? Message { get; set; }    // Message for client
        public T? Data { get; set; }            // The actual payload
        public List<string>? Errors { get; set; } // Validation or error messages
        public bool IsPaginated { get; set; }   // Optional pagination flag

        public ApiResponse() { }

        public ApiResponse(T? data, bool success = true, string message = "", bool isPaginated = false)
        {
            Data = data;
            Success = success;
            Message = message;
            IsPaginated = isPaginated;
        }

        public ApiResponse(List<FluentValidation.Results.ValidationFailure> validationErrors)
        {
            Success = false;
            Errors = validationErrors.Select(e => e.ErrorMessage).ToList();
            Message = "Validation failed";
        }
    }



}
