using Application.Abstractions.Authentication;
using Application.Abstractions.Data.Repositories.Users;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Users.Addresses.GetWithPagination
{
    internal sealed class GetAddressesWithPaginationQueryHandler :
        IQueryHandler<GetAddressesWithPaginationQuery, PaginatedList<AddressResponse>>
    {
        private readonly IAddressUserRepository _addressUserRepository;
        private readonly IUserContext _userContext;

        public GetAddressesWithPaginationQueryHandler(
            IAddressUserRepository addressUserRepository,
            IUserContext userContext)
        {
            _addressUserRepository = addressUserRepository;
            _userContext = userContext;
        }

        public async Task<Result<PaginatedList<AddressResponse>>> Handle(GetAddressesWithPaginationQuery query, CancellationToken cancellationToken)
        {
            PaginatedList<AddressUser> addresses = await _addressUserRepository.GetByUserIdAsync(
                _userContext.UserId,
                query.PageNumber, 
                query.PageSize, 
                cancellationToken);

            PaginatedList<AddressResponse> paginatedAddressresponse = MapToAddressResponsePaginatedList(
                addresses, 
                query);

            return Result.Success(paginatedAddressresponse);
        }

        private PaginatedList<AddressResponse> MapToAddressResponsePaginatedList(
            PaginatedList<AddressUser> addressesPaginatedList,
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
