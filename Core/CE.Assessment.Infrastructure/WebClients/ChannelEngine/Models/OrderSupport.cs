/////////////////////////////////////////////////////////////////
//															   //
// This code is generated by a tool                            //
// https://github.com/StevenThuriot/dotnet-openapi-generator   //
//															   //
/////////////////////////////////////////////////////////////////

#nullable enable

#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.

namespace CE.Assessment.Infrastructure.WebClients.ChannelEngine.Models;

[System.CodeDom.Compiler.GeneratedCode("dotnet-openapi-generator", "8.0.0-preview.15+2dc8cfca012adb9b7e1a243f167db99da7b5cfe4")]
[System.Text.Json.Serialization.JsonConverter(typeof(OrderSupportEnumConverter))]
public enum OrderSupport
{
	NONE,
	ORDERS,
	SPLIT_ORDERS,
	SPLIT_ORDER_LINES,
}

public static class OrderSupportFastEnum
{
     public static string ToString(OrderSupport value) => value switch
     {
         OrderSupport.NONE => "NONE",
         OrderSupport.ORDERS => "ORDERS",
         OrderSupport.SPLIT_ORDERS => "SPLIT_ORDERS",
         OrderSupport.SPLIT_ORDER_LINES => "SPLIT_ORDER_LINES",
         _ => throw new System.NotSupportedException(value + " is not a supported Enum value")
     };

     public static OrderSupport FromString(string? value) => value switch
     {
         "NONE" => OrderSupport.NONE,
         "ORDERS" => OrderSupport.ORDERS,
         "SPLIT_ORDERS" => OrderSupport.SPLIT_ORDERS,
         "SPLIT_ORDER_LINES" => OrderSupport.SPLIT_ORDER_LINES,
         _ => throw new System.NotSupportedException((value ?? "NULL") + " is not a supported Enum value")
     };
}

public class OrderSupportEnumConverter : System.Text.Json.Serialization.JsonConverter<OrderSupport>
{
    public override OrderSupport Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        return OrderSupportFastEnum.FromString(reader.GetString());
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, OrderSupport value, System.Text.Json.JsonSerializerOptions options)
    {
        writer.WriteStringValue(OrderSupportFastEnum.ToString(value));
    }
}
