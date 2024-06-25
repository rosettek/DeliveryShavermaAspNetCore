﻿namespace OrderService.DataAccess.Entities;

public class CanceledOrderEntity : BaseOrderEntity
{
    public int LastStatus { get; set; }
    public string ReasonOfCanceled { get; set; } = string.Empty;
    public DateTime CanceledDate { get; set; } = DateTime.UtcNow;
}