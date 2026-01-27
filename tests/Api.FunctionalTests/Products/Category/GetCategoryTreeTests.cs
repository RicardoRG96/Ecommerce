using Api.FunctionalTests.Abstractions;
using Api.FunctionalTests.Common;
using Application.Products.Categories.GetCategoryTree;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Api.FunctionalTests.Products.Category
{
    public class GetCategoryTreeTests : BaseFunctionalTest
    {
        public GetCategoryTreeTests(FunctionalTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_ReturnOk_WhenCategoriesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnListOfCategories_WhenCategoriesExist()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            categories.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_ReturnOnlyRootCategories_AtTopLevel()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            // Root categories from seed data: Electronics (1), Computers (2), Smartphones (3), Home Appliances (4), Sports & Outdoors (5)
            // Fashion (6) is inactive so should not appear
            categories.Should().HaveCount(5);
        }

        [Fact]
        public async Task Should_ReturnOnlyActiveAndVisibleCategories()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            // Fashion (6) is inactive and Phone Accessories (17) is inactive, should not appear in tree
            categories.Should().NotContain(c => c.Name == "Fashion");
        }

        [Fact]
        public async Task Should_BuildHierarchicalStructure_WithChildren()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            
            // Find Electronics category (ID 1)
            CategoryTreeResponse? electronics = categories!.FirstOrDefault(c => c.Name == "Electronics");
            electronics.Should().NotBeNull();
            
            // Electronics should have children (Televisions, Audio Equipment, Cameras)
            electronics!.Children.Should().NotBeEmpty();
            electronics.Children.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Should_BuildThreeLevelHierarchy_WithGrandchildren()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            
            // Find Electronics -> Cameras
            CategoryTreeResponse? electronics = categories!.FirstOrDefault(c => c.Name == "Electronics");
            CategoryTreeResponse? cameras = electronics?.Children.FirstOrDefault(c => c.Name == "Cameras");
            
            cameras.Should().NotBeNull();
            // Cameras should have children (DSLR Cameras, Mirrorless Cameras)
            cameras!.Children.Should().NotBeEmpty();
            cameras.Children.Should().Contain(c => c.Name == "DSLR Cameras");
            cameras.Children.Should().Contain(c => c.Name == "Mirrorless Cameras");
        }

        [Fact]
        public async Task Should_OrderCategoriesByDisplayOrder()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            categories.Should().NotBeEmpty();
            
            // Verify that categories are ordered by DisplayOrder
            for (int i = 0; i < categories!.Count - 1; i++)
            {
                categories[i].DisplayOrder.Should().BeLessThanOrEqualTo(categories[i + 1].DisplayOrder);
            }
        }

        [Fact]
        public async Task Should_IncludeAllCategoryProperties()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            categories.Should().NotBeEmpty();

            CategoryTreeResponse firstCategory = categories!.First();
            firstCategory.Id.Should().BeGreaterThan(0);
            firstCategory.Name.Should().NotBeNullOrEmpty();
            firstCategory.Slug.Should().NotBeNullOrEmpty();
            firstCategory.DisplayOrder.Should().BeGreaterThan(0);
            firstCategory.Children.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_NotIncludeInactiveCategories()
        {
            // Arrange
            SetAdminAuthentication();

            // Act
            List<CategoryTreeResponse>? categories = 
                await HttpClient.GetFromJsonAsync<List<CategoryTreeResponse>>($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            categories.Should().NotBeNull();
            
            // Fashion (6) is inactive - should not appear at root level
            categories!.Should().NotContain(c => c.Name == "Fashion");
            
            // Phone Accessories (17) is inactive - should not appear as child of Smartphones
            CategoryTreeResponse? smartphones = categories.FirstOrDefault(c => c.Name == "Smartphones");
            if (smartphones != null)
            {
                smartphones.Children.Should().NotContain(c => c.Name == "Phone Accessories");
            }
        }

        [Fact]
        public async Task Should_ReturnUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "");

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserHasNotPermission()
        {
            // Arrange
            SetCustomerUserAuthentication();

            // Act
            HttpResponseMessage response = await HttpClient.GetAsync($"{ApiRoutes.Categories.Base}/tree");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
