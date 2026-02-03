using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Application.Abstractions.Data.Repositories.Products;
using Application.Abstractions.Search;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Search
{
    public sealed class AlgoliaSearchService : ISearchService
    {
        private readonly ISearchClient _searchClient;
        private readonly IAttributeRepository _attributeRepository;
        private readonly string _indexName;

        public AlgoliaSearchService(
            ISearchClient searchClient,
            IAttributeRepository attributeRepository,
            IConfiguration configuration)
        {
            _searchClient = searchClient;
            _attributeRepository = attributeRepository;
            _indexName = configuration["Algolia:IndexName"] ?? "products";
        }

        public async Task InitializeIndexAsync(CancellationToken cancellationToken = default)
        {
            await _searchClient.SetSettingsAsync(_indexName, new IndexSettings
            {
                SearchableAttributes = new List<string>
                {
                    "name",
                    "description",
                    "brand",
                    "category",
                    "skuCode"
                },
                AttributesForFaceting = await CreateDynamicFacets(cancellationToken),

                AttributesToRetrieve = new List<string>
                {
                    "objectID",
                    "productId",
                    "name",
                    "slug",
                    "description",
                    "skuCode",
                    "brand",
                    "category",
                    "price",
                    "imageUrl",
                    "attributes"
                },
                CustomRanking = new List<string>
                {
                    "desc(price)"
                },
                QueryLanguages = new List<SupportedLanguage>
                {
                    SupportedLanguage.Es
                },
                RemoveStopWords = new RemoveStopWords(true),
                IgnorePlurals = new IgnorePlurals(true),
                AttributeForDistinct = "productId",
            }, null, null, cancellationToken);
        }

        private async Task<List<string>> CreateDynamicFacets(CancellationToken cancellationToken)
        {
            var attributes = await _attributeRepository.GetAllAsync(cancellationToken);

            List<string> facets = ["brand", "category", "filterOnly(price)"];

            List<string> formatedAttributes = attributes.Select(a => $"attributes.{a.Name}").ToList();

            facets.AddRange(formatedAttributes);

            return facets;
        }

        public async Task IndexProductsAsync(
            IEnumerable<ProductSearchModel> products, 
            CancellationToken cancellationToken = default)
        {
            IEnumerable<Dictionary<string, object>> records = products.Select(p => new Dictionary<string, object>
            {
                ["objectID"] = p.ObjectID,
                ["productId"] = p.ProductId,
                ["name"] = p.Name,
                ["slug"] = p.Slug,
                ["description"] = p.Description,
                ["skuCode"] = p.SkuCode ?? string.Empty,
                ["brand"] = p.Brand ?? string.Empty,
                ["category"] = p.Category ?? string.Empty,
                ["price"] = p.Price,
                ["imageUrl"] = p.ImageUrl ?? string.Empty,
                ["attributes"] = p.Attributes ?? new Dictionary<string, object>()
            });

            await _searchClient.SaveObjectsAsync(_indexName, records, null, cancellationToken);
        }

        public async Task<IEnumerable<ProductSearchModel>> SearchProductsAsync(
            string query, 
            CancellationToken cancellationToken = default)
        {
            SearchParamsObject param = new()
            {
                Query = query
            };

            SearchResponse<ProductSearchModel> searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(param),
                null,
                cancellationToken);

            return searchResult.Hits;
        }

        public async Task<IEnumerable<ProductSearchModel>> FilterByBrandAsync(
            List<string> brands, 
            CancellationToken cancellationToken = default)
        {
            IEnumerable<SearchResponse<ProductSearchModel>> results = await Task.WhenAll(brands.Select(async b =>
                await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                        _indexName,
                        new SearchParams(
                            new SearchParamsObject
                            {
                                Query = b,
                                Filters = $"brand:'{b}'"
                            }),
                        null,
                        cancellationToken)).ToList());
            
            return results.SelectMany(r => r.Hits);
        }

        public async Task<IEnumerable<ProductSearchModel>> FilterByCategoryAsync(
            List<string> categories, 
            CancellationToken cancellationToken = default)
        {
            IEnumerable<SearchResponse<ProductSearchModel>> results = await Task.WhenAll(categories.Select(async c =>
                await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                        _indexName,
                        new SearchParams(
                            new SearchParamsObject
                            {
                                Query = c,
                                Filters = $"category:'{c}'"
                            }),
                        null,
                        cancellationToken)).ToList());

            return results.SelectMany(r => r.Hits);
        }

        public async Task<IEnumerable<ProductSearchModel>> FilterByPriceRange(
            decimal minPrice, 
            decimal maxPrice, 
            CancellationToken cancellationToken = default)
        {
            SearchResponse<ProductSearchModel> searchResult = await _searchClient.SearchSingleIndexAsync<ProductSearchModel>(
                _indexName,
                new SearchParams(
                    new SearchParamsObject
                    {
                        Filters = $"price: {minPrice} TO {maxPrice}"
                    }),
                null,
                cancellationToken);

            return searchResult.Hits;
        }
    }
}
