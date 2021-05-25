using Delpin.Application.Contracts.v1.ProductCategories;
using Delpin.Application.Contracts.v1.ProductGroups;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Delpin.API.Test.IntegrationTest
{
    public class ProductGroupsControllerTest : IntegrationTest
    {
        private const string Url = BaseUrl + "ProductGroups/";
        private Guid categoryId = Guid.Empty;

        [Fact]
        public async Task GetGroup_WithMatchingId_ReturnsProductGroup()
        {
            await AuthenticateAsync();
            var productGroup = await CreateProductGroupAndCategory();

            var response = await TestClient.GetAsync(Url + productGroup.Id);
            var groupResponse = await response.Content.ReadFromJsonAsync<ProductGroupDto>();

            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            Assert.Equal(productGroup.Id, groupResponse?.Id);
        }

        [Fact]
        public async Task GetGroup_WithoutMatchingId_ReturnsNotFound()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync(Url + Guid.NewGuid());

            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetGroup_WithoutAuthentication_ReturnsUnauthorized()
        {
            var response = await TestClient.GetAsync(Url + Guid.NewGuid());

            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetGroup_WithoutRouteParams_ReturnsAllGroups()
        {
            await AuthenticateAsync();
            await CreateProductGroupAndCategory();

            var response = await TestClient.GetAsync(Url);
            var groups = await response.Content.ReadFromJsonAsync<List<ProductGroupDto>>(ReferenceHandlerOptions);

            Assert.NotEmpty(groups!);
        }

        [Fact]
        public async Task CreateGroup_WhenGroupIsCreated_ShouldReturnGroup()
        {
            await AuthenticateAsync();

            var group = new CreateProductGroupDto
            {
                Name = "TestGroup",
                Image = new byte[50],
                ProductCategoryId = Guid.Empty
            };

            var response = await TestClient.PostAsJsonAsync(Url, group);
            var groupResponse = await response.Content.ReadFromJsonAsync<ProductGroupDto>();

            Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
            Assert.Equal(group.Name, groupResponse?.Name);
            Assert.Equal(group.Image, groupResponse?.Image);
            Assert.NotNull(groupResponse?.Id);
        }

        [Fact]
        public async Task CreateGroup_WithNoName_ReturnsBadRequest()
        {
            await AuthenticateAsync();
            var group = new CreateProductGroupDto
            {
                Image = new byte[50],
                ProductCategoryId = Guid.Empty
            };

            var response = await TestClient.PostAsJsonAsync(Url, group);

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateGroup_WhenNotSuperUserOrAdmin_ReturnsForbidden()
        {
            await AuthenticateAsync("lene@delpin.dk");
            var group = new CreateProductGroupDto
            {
                Image = new byte[50],
                ProductCategoryId = Guid.Empty
            };

            var response = await TestClient.PostAsJsonAsync(Url, group);

            Assert.Equal(StatusCodes.Status403Forbidden, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateGroup_WhenGroupExists_ReturnsNoContent()
        {
            await AuthenticateAsync();
            var groupToUpdate = await CreateProductGroupAndCategory();

            var updateGroupDto = new UpdateProductGroupDto
            {
                Name = "Updated Test Group",
                Image = new byte[30],
                ProductCategoryId = categoryId
            };

            var response = await TestClient.PutAsJsonAsync(Url + groupToUpdate.Id, updateGroupDto);

            var updateResponse = await TestClient.GetAsync(Url + groupToUpdate.Id);
            var updatedGroup = await updateResponse.Content.ReadFromJsonAsync<ProductGroupDto>();

            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            Assert.Equal(updateGroupDto.Name, updatedGroup?.Name);
            Assert.Equal(updateGroupDto.Image, updatedGroup?.Image);
        }

        [Fact]
        public async Task UpdateGroup_WhenGroupDoesNotExist_ReturnsNotFound()
        {
            await AuthenticateAsync();

            var updateGroupDto = new UpdateProductGroupDto
            {
                Name = "Updated Test Group",
                Image = new byte[30],
                ProductCategoryId = categoryId
            };

            var response = await TestClient.PutAsJsonAsync(Url + Guid.NewGuid(), updateGroupDto);

            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteGroup_WhenGroupExists_ReturnsNoContent()
        {
            await AuthenticateAsync();
            var groupToDelete = await CreateProductGroupAndCategory();

            var response = await TestClient.DeleteAsync(Url + groupToDelete.Id);
            var attemptToGetDeletedGroup = await TestClient.GetAsync(Url + groupToDelete.Id);

            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
            Assert.Equal(StatusCodes.Status404NotFound, (int)attemptToGetDeletedGroup.StatusCode);
        }

        [Fact]
        public async Task DeleteGroup_WhenGroupDoesNotExist_ReturnsNotFound()
        {
            await AuthenticateAsync();

            var response = await TestClient.DeleteAsync(Url + Guid.NewGuid());

            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        private async Task<ProductGroupDto> CreateProductGroupAndCategory()
        {
            var productCategory = new CreateProductCategoryDto
            {
                Name = "Test Category",
                Image = new byte[20]
            };

            var categoryResponse = await TestClient.PostAsJsonAsync(BaseUrl + "ProductCategories/", productCategory);
            var category = await categoryResponse.Content.ReadFromJsonAsync<ProductCategoryDto>();

            var productGroup = new CreateProductGroupDto
            {
                Image = new byte[20],
                Name = "Test Group",
                ProductCategoryId = category?.Id ?? Guid.NewGuid()
            };

            categoryId = category?.Id ?? Guid.Empty;

            var groupResponse = await TestClient.PostAsJsonAsync(Url, productGroup);

            return await groupResponse.Content.ReadFromJsonAsync<ProductGroupDto>();
        }
    }
}
