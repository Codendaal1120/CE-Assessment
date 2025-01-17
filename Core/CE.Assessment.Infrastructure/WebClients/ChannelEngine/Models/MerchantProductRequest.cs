﻿/////////////////////////////////////////////////////////////////
//															   //
// This code is generated by a tool                            //
// https://github.com/StevenThuriot/dotnet-openapi-generator   //
//															   //
/////////////////////////////////////////////////////////////////

#nullable enable

#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.

namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;

[System.CodeDom.Compiler.GeneratedCode("dotnet-openapi-generator", "8.0.0-preview.15+2dc8cfca012adb9b7e1a243f167db99da7b5cfe4")]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public sealed class MerchantProductRequest : __ICanIterate
{
    public MerchantProductRequest() { }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    [System.Text.Json.Serialization.JsonConstructor]
    public MerchantProductRequest(string merchantProductNo, int stock, double price, VatRateType vatRateType, bool isFrozen)
    {
        MerchantProductNo = merchantProductNo;
        Stock = stock;
        Price = price;
        VatRateType = vatRateType;
        IsFrozen = isFrozen;
    }

    public string? ParentMerchantProductNo { get; set; }
    public string? ParentMerchantProductNo2 { get; set; }
    public System.Collections.Generic.List<MerchantProductExtraDataItemRequest>? ExtraData { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Brand { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public string? Ean { get; set; }
    public string? ManufacturerProductNumber { get; set; }
    public required string MerchantProductNo { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public double? MSRP { get; set; }
    public double? PurchasePrice { get; set; }
    public VatRateType VatRateType { get; set; }
    public double? ShippingCost { get; set; }
    public string? ShippingTime { get; set; }
    public string? Url { get; set; }
    public string? ImageUrl { get; set; }
    public string? ExtraImageUrl1 { get; set; }
    public string? ExtraImageUrl2 { get; set; }
    public string? ExtraImageUrl3 { get; set; }
    public string? ExtraImageUrl4 { get; set; }
    public string? ExtraImageUrl5 { get; set; }
    public string? ExtraImageUrl6 { get; set; }
    public string? ExtraImageUrl7 { get; set; }
    public string? ExtraImageUrl8 { get; set; }
    public string? ExtraImageUrl9 { get; set; }
    public bool IsFrozen { get; set; }
    public string? CategoryTrail { get; set; }

    System.Collections.Generic.IEnumerable<(string name, object? value)> __ICanIterate.IterateProperties()
    {
        yield return ("ParentMerchantProductNo", ParentMerchantProductNo);
        yield return ("ParentMerchantProductNo2", ParentMerchantProductNo2);
        yield return ("ExtraData", ExtraData);
        yield return ("Name", Name);
        yield return ("Description", Description);
        yield return ("Brand", Brand);
        yield return ("Size", Size);
        yield return ("Color", Color);
        yield return ("Ean", Ean);
        yield return ("ManufacturerProductNumber", ManufacturerProductNumber);
        yield return ("MerchantProductNo", MerchantProductNo);
        yield return ("Stock", Stock);
        yield return ("Price", Price);
        yield return ("MinPrice", MinPrice);
        yield return ("MaxPrice", MaxPrice);
        yield return ("MSRP", MSRP);
        yield return ("PurchasePrice", PurchasePrice);
        yield return ("VatRateType", VatRateType switch
        {
            VatRateType.STANDARD => "STANDARD",
            VatRateType.REDUCED => "REDUCED",
            VatRateType.SUPER_REDUCED => "SUPER_REDUCED",
            VatRateType.EXEMPT => "EXEMPT",
            _ => null
        });
        yield return ("ShippingCost", ShippingCost);
        yield return ("ShippingTime", ShippingTime);
        yield return ("Url", Url);
        yield return ("ImageUrl", ImageUrl);
        yield return ("ExtraImageUrl1", ExtraImageUrl1);
        yield return ("ExtraImageUrl2", ExtraImageUrl2);
        yield return ("ExtraImageUrl3", ExtraImageUrl3);
        yield return ("ExtraImageUrl4", ExtraImageUrl4);
        yield return ("ExtraImageUrl5", ExtraImageUrl5);
        yield return ("ExtraImageUrl6", ExtraImageUrl6);
        yield return ("ExtraImageUrl7", ExtraImageUrl7);
        yield return ("ExtraImageUrl8", ExtraImageUrl8);
        yield return ("ExtraImageUrl9", ExtraImageUrl9);
        yield return ("IsFrozen", IsFrozen);
        yield return ("CategoryTrail", CategoryTrail);
    }
}
