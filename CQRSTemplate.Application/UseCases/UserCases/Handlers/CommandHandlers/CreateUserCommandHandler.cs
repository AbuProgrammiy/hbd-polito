using CQRSTemplate.Application.Abstractions;
using CQRSTemplate.Application.UseCases.UserCases.Commands;
using CQRSTemplate.Domain.Entities.Models.PrimaryModels;
using CQRSTemplate.Domain.Entities.Views;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CQRSTemplate.Application.UseCases.UserCases.Handlers.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public CreateUserCommandHandler(IApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<ResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = request.Adapt<User>();

                user.IsConfirmed = false;

                string fileName = $"/UsersPhoto/{Guid.NewGuid()}---{request.Photo.FileName}";

                using (FileStream stream = new FileStream($"{_webHostEnvironment.WebRootPath}{fileName}", FileMode.Create))
                {
                    await request.Photo.CopyToAsync(stream);
                }

                user.PhotoUrl = $"{_configuration.GetValue<string>("DNS")}{fileName}";

                await _applicationDbContext.Users.AddAsync(user);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Response = "User created successflly!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Response = ex.Message
                };
            }
        }
    }
}
