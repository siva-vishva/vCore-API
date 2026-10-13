using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vCore.Application.DTOs;
using vCore.Application.Services;
using vCore.Infrastructure.Authorization;

namespace vCore.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class TenantsController : ControllerBase
{
    private readonly ITenantApplicationService _tenantApplicationService;

    public TenantsController(ITenantApplicationService tenantApplicationService)
    {
        _tenantApplicationService = tenantApplicationService;
    }

    [HttpPost]
    [Authorize(Policy = TenantPolicies.TenantRegistration)]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TenantResponse>> Create([FromBody] CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { tenantId = response.TenantId }, response);
    }

    [HttpGet("{tenantId:guid}")]
    [Authorize(Policy = TenantPolicies.TenantProfileManagement)]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TenantResponse>> GetById(Guid tenantId, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.GetByIdAsync(tenantId, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = TenantPolicies.TenantProfileManagement)]
    [ProducesResponseType(typeof(PagedResult<TenantResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TenantResponse>>> List([FromQuery] TenantListRequest request, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.ListAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{tenantId:guid}")]
    [Authorize(Policy = TenantPolicies.TenantProfileManagement)]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TenantResponse>> Update(Guid tenantId, [FromBody] UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.UpdateAsync(tenantId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{tenantId:guid}/status")]
    [Authorize(Policy = TenantPolicies.TenantStatusManagement)]
    [ProducesResponseType(typeof(TenantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TenantResponse>> ChangeStatus(Guid tenantId, [FromBody] ChangeTenantStatusRequest request, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.ChangeStatusAsync(tenantId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{tenantId:guid}/provision")]
    [Authorize(Policy = TenantPolicies.TenantProvisioning)]
    [ProducesResponseType(typeof(ProvisionTenantResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProvisionTenantResponse>> Provision(Guid tenantId, CancellationToken cancellationToken)
    {
        var response = await _tenantApplicationService.ProvisionAsync(tenantId, cancellationToken);
        return Accepted(response);
    }
}
