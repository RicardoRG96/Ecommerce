using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Data.UnitOfWork;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using SharedKernel;

namespace Application.Products.ProductGalleries.AddMedia
{
    internal sealed class AddMediaCommandHandler : ICommandHandler<AddMediaCommand, long>
    {
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public AddMediaCommandHandler(
            IProductGalleryRepository productGalleryRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _productGalleryRepository = productGalleryRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<long>> Handle(AddMediaCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //Result<string> uploadResult = await _fileStorageService.UploadAsync(
            //    command.Stream,
            //    command.FileName,
            //    command.ContentType,
            //    cancellationToken);

            //if (uploadResult.IsFailure)
            //{
            //    return Result.Failure<long>(uploadResult.Error);
            //}
        }
    }
}
