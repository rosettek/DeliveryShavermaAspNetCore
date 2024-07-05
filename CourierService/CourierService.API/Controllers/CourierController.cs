﻿using BarsGroupProjectN1.Core.Contracts;
using BarsGroupProjectN1.Core.Exceptions;
using CourierService.API.Contracts;
using CourierService.API.Extensions;
using CourierService.API.Models;
using CourierService.Core.Abstractions;
using CourierService.Core.Exceptions;
using CourierService.Core.Models;
using CourierService.Core.Models.Code;
using Microsoft.AspNetCore.Mvc;

namespace CourierService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourierController : Controller
{
    private readonly ICourierService _courierService;

    private readonly IOrdersApiClient _ordersApiClient;

    private readonly ILogger<CourierController> _logger;

    public CourierController(
        ICourierService courierService,
        IOrdersApiClient ordersApiClient,
        ILogger<CourierController> logger
    )
    {
        _courierService = courierService;
        _ordersApiClient = ordersApiClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCouriers()
    {
        var couriers = await _courierService.GetAllCouriers();

        var response = couriers.Select(c => new CourierResponse(c.Id, c.Status));

        return Ok(response);
    }

    [HttpGet("getcourierbyid")]
    public async Task<IActionResult> GetCourierById(Guid id)
    {
        var courier = await _courierService.GetCourierById(id);

        return Ok(courier);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourier([FromBody] CourierRequest request)
    {
        var (courier, error) = Courier.Create(
            Guid.NewGuid(),
            status: default
        );

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        await _courierService.CreateCourier(courier);

        return Ok(courier);
    }

    [HttpPost("status/{id:guid}")]
    public async Task<IActionResult> UpdateCourierStatus(Guid id, [FromForm] CourierStatusCode status)
    {
        await _courierService.UpdateCourier(id, status);

        return RedirectToAction(nameof(CourierProfile));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourier(Guid id)
    {
        return Ok(await _courierService.DeleteCourier(id));
    }

    [HttpGet("orders/courier/last")]
    public async Task<IActionResult> GetLastOrder()
    {
        try
        {
            var latestOrder = _ordersApiClient.GetLatestOrderAsync();
            return Ok(latestOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("orders/order_id/status")]
    public async Task<IActionResult> GetOrderStatus()
    {
        try
        {
            var orders = await _ordersApiClient.GetCurrentOrdersAsync();

            var firstOrderStatus = orders.FirstOrDefault()?.Status;

            return Ok(firstOrderStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
        }
    }

    [HttpGet("profile")]
    public async Task<IActionResult> CourierProfile()
    {
        var courierId = User.UserId();

        try
        {
            var courier = await _courierService.GetCourierById(courierId);

            return View(new CourierViewModel
            {
                Id = courier.Id,
                Status = courier.Status
            });
        }

        catch (EntityNotFound e)
        {
            var (newCourier, error) = Courier.Create(
                courierId,
                status: default
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            await _courierService.CreateCourier(newCourier);

            return RedirectToAction("CourierProfile", new { id = newCourier.Id });
        }
        catch (ArgumentException e)
        {
            BadRequest(e.Message);
            _logger.LogError(e, e.Message);
        }
        catch (RepositoryException e)
        {
            BadRequest(e.Message);
            _logger.LogError(e, e.Message);
        }

        return BadRequest();
    }

    [HttpGet("getactivecourier")]
    public async Task<IActionResult> GetActiveCourier()
    {
        var couriers = await _courierService.GetAllCouriers();

        var activeCourier = couriers.FirstOrDefault(c => c.Status == CourierStatusCode.Active);

        if (activeCourier is null)
        {
            return NotFound("Courier not found");
        }

        return Ok(
            new OrderTaskExecution<Courier>
            {
                Executor = activeCourier,
                Time = TimeSpan.FromMinutes(15)
            }
        );
    }
}