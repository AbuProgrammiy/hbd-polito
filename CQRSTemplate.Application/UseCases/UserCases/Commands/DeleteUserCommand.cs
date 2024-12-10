using CQRSTemplate.Domain.Entities.Views;
using MediatR;

namespace CQRSTemplate.Application.UseCases.UserCases.Commands
{
    public class DeleteUserCommand:IRequest<ResponseModel>
    {
        public Guid Id { get; set; }
    }
}
