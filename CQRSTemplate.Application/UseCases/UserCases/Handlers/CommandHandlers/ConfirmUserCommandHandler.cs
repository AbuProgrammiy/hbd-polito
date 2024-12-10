using CQRSTemplate.Application.Abstractions;
using CQRSTemplate.Application.UseCases.UserCases.Commands;
using CQRSTemplate.Domain.Entities.Models.PrimaryModels;
using CQRSTemplate.Domain.Entities.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSTemplate.Application.UseCases.UserCases.Handlers.CommandHandlers
{
    public class ConfirmUserCommandHandler : IRequestHandler<ConfirmUserCommand, ResponseModel>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ConfirmUserCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ResponseModel> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
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

                user.IsConfirmed = true;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Response = "User successflly confirmed!"
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
