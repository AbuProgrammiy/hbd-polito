using CQRSTemplate.Application.Abstractions;
using CQRSTemplate.Application.UseCases.UserCases.Queries;
using CQRSTemplate.Domain.Entities.Models.PrimaryModels;
using CQRSTemplate.Domain.Entities.Views;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSTemplate.Application.UseCases.UserCases.Handlers.QueryHandlers
{
    public class GetTodaysBirthdaysQueryHandler : IRequestHandler<GetTodaysBirthdaysQuery, ResponseModel>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetTodaysBirthdaysQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ResponseModel> Handle(GetTodaysBirthdaysQuery request, CancellationToken cancellationToken)
        {
            try
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                IEnumerable<User> users = await _applicationDbContext.Users.Where(u=>u.IsConfirmed==true&&u.BirthDate==today).ToListAsync(cancellationToken);

                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Response = users
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
