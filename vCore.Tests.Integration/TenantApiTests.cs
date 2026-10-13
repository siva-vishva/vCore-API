using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using vCore.Application.DTOs;

namespace vCore.Tests.Integration;

public class TenantApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TenantApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "integration-test");
    }

    [Fact]
    public async Task CreateTenant_ReturnsCreatedResponse()
    {
        var request = new CreateTenantRequest
        {
            TenantCode = "ACME",
            TenantName = "ACME Corporation",
            TenantType = "Customer",
            TimeZone = "Asia/Kolkata",
            Locale = "en-IN",
            CountryCode = "IN",
            ContactEmail = "admin@acme.example",
            ContactPhone = "+91XXXXXXXXXX"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/tenants", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<TenantResponse>();
        Assert.NotNull(body);
        Assert.Equal("ACME", body.TenantCode);
        Assert.Equal("Customer", body.TenantType);
        Assert.Equal("Active", body.Status);
        Assert.False(string.IsNullOrWhiteSpace(body.RowVersion));
    }

    [Fact]
    public async Task DuplicateTenantCode_ReturnsConflict()
    {
        var request = new CreateTenantRequest
        {
            TenantCode = "ACME-DUPLICATE",
            TenantName = "Duplicate Tenant",
            TenantType = "Customer",
            TimeZone = "Asia/Kolkata",
            Locale = "en-IN",
            CountryCode = "IN",
            ContactEmail = "admin@duplicate.example",
            ContactPhone = "+91XXXXXXXXXX"
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/v1/tenants", request);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondResponse = await _client.PostAsJsonAsync("/api/v1/tenants", request);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateTenant_WhenRowVersionMatches_UpdatesProfile()
    {
        var createRequest = new CreateTenantRequest
        {
            TenantCode = "ACME-UPDATE",
            TenantName = "ACME Corporation",
            TenantType = "Customer",
            TimeZone = "Asia/Kolkata",
            Locale = "en-IN",
            CountryCode = "IN",
            ContactEmail = "admin@acme.example",
            ContactPhone = "+91XXXXXXXXXX"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/tenants", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<TenantResponse>();

        var updateRequest = new UpdateTenantRequest
        {
            TenantName = "Updated ACME",
            TimeZone = "America/New_York",
            Locale = "en-US",
            CountryCode = "US",
            ContactEmail = "updated@acme.example",
            ContactPhone = "+1XXXXXXXXXX",
            RowVersion = created!.RowVersion
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/tenants/{created.TenantId}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<TenantResponse>();
        Assert.Equal("Updated ACME", updated!.TenantName);
        Assert.Equal("America/New_York", updated.TimeZone);
    }
}
