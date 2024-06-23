﻿using OrderService.Domain.Models;

namespace OrderService.Api.Contracts.Client;

public record ClientGetLast(
    Guid Id,
    List<BasketItem> Basket,
    int Price,
    string Comment,
    DateTime OrderDate,
    DateTime? DeliveryDate,
    string Cheque);