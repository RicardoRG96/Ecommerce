using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors.Users;
using SharedKernel;

namespace Application.Users.Addresses.GetById
{
    internal sealed class GetAddressByIdQueryHandler : IQueryHandler<GetAddressByIdQuery, AddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserContext _userContext;

        public GetAddressByIdQueryHandler(
            IAddressRepository addressRepository,
            IUserContext userContext)
        {
            _addressRepository = addressRepository;
            _userContext = userContext;
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByIdIncludingAddressUserAsync(query.AddressId, cancellationToken);

            if (address is null)
            {
                return Result.Failure<AddressResponse>(AddressErrors.NotFound(query.AddressId));
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
