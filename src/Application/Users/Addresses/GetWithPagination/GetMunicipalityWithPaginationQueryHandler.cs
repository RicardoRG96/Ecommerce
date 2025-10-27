using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Addresses.GetWithPagination
{
    internal sealed class GetMunicipalityWithPaginationQueryHandler :
        IQueryHandler<GetAddressesWithPaginationQuery, PaginatedList<AddressResponse>>
    {
        private readonly IAddressRepository _addressRepository;

        public GetMunicipalityWithPaginationQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<PaginatedList<AddressResponse>>> Handle(GetAddressesWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<Address> addresses = await _addressRepository.GetAllAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<AddressResponse> paginatedAddressresponse = MapToAddressResponsePaginatedList(
                addresses, 
                query);

            return Result.Success(paginatedAddressresponse);
        }

        private PaginatedList<AddressResponse> MapToAddressResponsePaginatedList(
            PaginatedList<Address> addressesPaginatedList,
            GetAddressesWithPaginationQuery query)
        {
            List<AddressResponse> addressResponse = addressesPaginatedList.Items
                .Select(a => AddressToAddressResponseMapper.Map(a)).ToList();

            PaginatedList<AddressResponse> paginatedAddressResponse =
                PaginatedList<AddressResponse>.Create(
                    addressResponse,
                    addressesPaginatedList.TotalCount,
                    addressesPaginatedList.PageNumber,
                    query.PageSize);

            return paginatedAddressResponse;
        }
    }
}
