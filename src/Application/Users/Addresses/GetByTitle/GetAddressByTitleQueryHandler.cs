using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.GetByTitle
{
    internal sealed class GetAddressByTitleQueryHandler : IQueryHandler<GetAddressByTitleQuery, AddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserContext _userContext;

        public GetAddressByTitleQueryHandler(
            IAddressRepository addressRepository,
            IUserContext userContext)
        {
            _addressRepository = addressRepository;
            _userContext = userContext;
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByTitleQuery query, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByTitleAsync(query.Title, cancellationToken);

            if (address is null)
            {
                return Result.Failure<AddressResponse>(AddressErrors.NotFoundByTitle);
            }

            long userId = address.AddressUsers.Select(au => au.ApplicationUserId).FirstOrDefault();

            if (userId != _userContext.UserId)
            {
                return Result.Failure<AddressResponse>(UserErrors.Unauthorized());
            }

            AddressResponse response = AddressToAddressResponseMapper.Map(address);

            return Result.Success(response);
        }
    }
}
