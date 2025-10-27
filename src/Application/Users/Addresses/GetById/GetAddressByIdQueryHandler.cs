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

        public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByIdAsync(query.AddressId, cancellationToken);

            if (address is null)
            {
                return Result.Failure<AddressResponse>(AddressErrors.NotFound(query.AddressId));
            }

            AddressResponse response = AddressToAddressResponseMapper.Map(address);

            return Result.Success(response);
        }
    }
}
