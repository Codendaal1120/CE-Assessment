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
[System.Text.Json.Serialization.JsonConverter(typeof(OrderStatusViewEnumConverter))]
public enum OrderStatusView
{
	IN_PROGRESS,
	SHIPPED,
	IN_BACKORDER,
	MANCO,
	CANCELED,
	IN_COMBI,
	CLOSED,
	@NEW,
	RETURNED,
	REQUIRES_CORRECTION,
	AWAITING_PAYMENT,
}

public static class OrderStatusViewFastEnum
{
     public static string ToString(OrderStatusView value) => value switch
     {
         OrderStatusView.IN_PROGRESS => "IN_PROGRESS",
         OrderStatusView.SHIPPED => "SHIPPED",
         OrderStatusView.IN_BACKORDER => "IN_BACKORDER",
         OrderStatusView.MANCO => "MANCO",
         OrderStatusView.CANCELED => "CANCELED",
         OrderStatusView.IN_COMBI => "IN_COMBI",
         OrderStatusView.CLOSED => "CLOSED",
         OrderStatusView.@NEW => "NEW",
         OrderStatusView.RETURNED => "RETURNED",
         OrderStatusView.REQUIRES_CORRECTION => "REQUIRES_CORRECTION",
         OrderStatusView.AWAITING_PAYMENT => "AWAITING_PAYMENT",
         _ => throw new System.NotSupportedException(value + " is not a supported Enum value")
     };

     public static OrderStatusView FromString(string? value) => value switch
     {
         "IN_PROGRESS" => OrderStatusView.IN_PROGRESS,
         "SHIPPED" => OrderStatusView.SHIPPED,
         "IN_BACKORDER" => OrderStatusView.IN_BACKORDER,
         "MANCO" => OrderStatusView.MANCO,
         "CANCELED" => OrderStatusView.CANCELED,
         "IN_COMBI" => OrderStatusView.IN_COMBI,
         "CLOSED" => OrderStatusView.CLOSED,
         "NEW" => OrderStatusView.@NEW,
         "RETURNED" => OrderStatusView.RETURNED,
         "REQUIRES_CORRECTION" => OrderStatusView.REQUIRES_CORRECTION,
         "AWAITING_PAYMENT" => OrderStatusView.AWAITING_PAYMENT,
         _ => throw new System.NotSupportedException((value ?? "NULL") + " is not a supported Enum value")
     };
}

public class OrderStatusViewEnumConverter : System.Text.Json.Serialization.JsonConverter<OrderStatusView>
{
    public override OrderStatusView Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        return OrderStatusViewFastEnum.FromString(reader.GetString());
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, OrderStatusView value, System.Text.Json.JsonSerializerOptions options)
    {
        writer.WriteStringValue(OrderStatusViewFastEnum.ToString(value));
    }
}
