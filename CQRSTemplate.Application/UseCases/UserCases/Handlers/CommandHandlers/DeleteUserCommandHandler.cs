using CQRSTemplate.Application.Abstractions;
using CQRSTemplate.Application.UseCases.UserCases.Commands;
using CQRSTemplate.Domain.Entities.Models.PrimaryModels;
using CQRSTemplate.Domain.Entities.Views;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CQRSTemplate.Application.UseCases.UserCases.Handlers.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseModel>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteUserCommandHandler(IApplicationDbContext applicationDbContext, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

                if (user == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        StatusCode = 404,
                        Response = "User not found!"
                    };
                }

                _applicationDbContext.Users.Remove(user);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                string filePath = user.PhotoUrl.Replace(_configuration.GetValue<string>("DNS")!, _webHostEnvironment.WebRootPath);

                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        StatusCode = 202,
                        Response = $"User successflly deleted but not it's photo! Don't worry it's not a big problem. Exception: {ex.Message}"
                    };
                }

                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 204,
                    Response = "User successflly deleted!"
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
