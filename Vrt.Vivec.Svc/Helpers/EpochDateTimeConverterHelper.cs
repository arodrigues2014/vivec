namespace Vrt.Vivec.Svc.Helpers;

public class EpochDateTimeConverterHelper : JsonConverter
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            // Manejar el caso en que el valor es nulo
            return null;
        }

        // Intentar convertir el valor a Int64
        if (Int64.TryParse(reader.Value.ToString(), out var epochTime))
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(epochTime).UtcDateTime;
        }
        else
        {
            // Manejar el caso en que no se puede convertir a Int64
            throw new JsonSerializationException($"No se pudo convertir {reader.Value} a Int64.");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            var epochTime = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            writer.WriteValue(epochTime);
        }
        else
        {
            throw new JsonSerializationException($"No se puede convertir el valor {value} a DateTime.");
        }
    }

    public override bool CanConvert(Type objectType)
    {
        // Indicar si este convertidor puede convertir objetos del tipo especificado
        return objectType == typeof(DateTime);
    }
}
