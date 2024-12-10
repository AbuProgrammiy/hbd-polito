using CQRSTemplate.Domain.Entities.Views;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CQRSTemplate.Application.UseCases.UserCases.Commands
{
    public class CreateUserCommand:IRequest<ResponseModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? InstagramUsername { get; set; }
        public string Wishes { get; set; }
        public IFormFile Photo { get; set; }
    }
}
