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

        public GetAddressByTitleQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByTitleQuery query, CancellationToken cancellationToken)
        {
            Address? address = await _addressRepository.GetByTitleAsync(query.Title, cancellationToken);

            if (address is null)
            {
                return Result.Failure<AddressResponse>(AddressErrors.NotFound);
            }
        }
    }
}
