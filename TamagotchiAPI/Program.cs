using RestSharp;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

var options = new RestClientOptions("https://pokeapi.co") {
    MaxTimeout = -1,
};
var client = new RestClient(options);
var request = new RestRequest("/api/v2/pokemon/", Method.Get);
var body = @"";
request.AddParameter("text/plain", body, ParameterType.RequestBody);
RestResponse response = await client.ExecuteAsync(request);

var pokemon = Pokemon.FromJson(response.Content);

Console.WriteLine("Lista de Pokemons:");

foreach(var poke in pokemon.Results) {
    Console.WriteLine($"Nome: {poke.Name}");
    //Console.WriteLine($"Url: {poke.Url}");

}


// https://app.quicktype.io/
#nullable enable
#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8603
public partial class Pokemon {
    [JsonPropertyName("count")]
    public long Count { get; set; }

    [JsonPropertyName("next")]
    public Uri Next { get; set; }

    [JsonPropertyName("previous")]
    public Uri Previous { get; set; }

    [JsonPropertyName("results")]
    public Result[] Results { get; set; }
}

public partial class Result {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }
}

public partial class Pokemon {
    public static Pokemon FromJson(string json) => JsonSerializer.Deserialize<Pokemon>(json, Converter.Settings);
}

public static class Serialize {
    public static string ToJson(this Pokemon self) => JsonSerializer.Serialize(self, Converter.Settings);
}

internal static class Converter {
    public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General) {
        Converters =
        {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            },
    };
}

public class DateOnlyConverter : JsonConverter<DateOnly> {
    private readonly string serializationFormat;
    public DateOnlyConverter() : this(null) { }

    public DateOnlyConverter(string? serializationFormat) {
        this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
    }

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString();
        return DateOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}

public class TimeOnlyConverter : JsonConverter<TimeOnly> {
    private readonly string serializationFormat;

    public TimeOnlyConverter() : this(null) { }

    public TimeOnlyConverter(string? serializationFormat) {
        this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
    }

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString();
        return TimeOnly.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(serializationFormat));
}

internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset> {
    public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

    private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

    private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
    private string? _dateTimeFormat;
    private CultureInfo? _culture;

    public DateTimeStyles DateTimeStyles {
        get => _dateTimeStyles;
        set => _dateTimeStyles = value;
    }

    public string? DateTimeFormat {
        get => _dateTimeFormat ?? string.Empty;
        set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
    }

    public CultureInfo Culture {
        get => _culture ?? CultureInfo.CurrentCulture;
        set => _culture = value;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) {
        string text;


        if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
            || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal) {
            value = value.ToUniversalTime();
        }

        text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

        writer.WriteStringValue(text);
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string? dateText = reader.GetString();

        if (string.IsNullOrEmpty(dateText) == false) {
            if (!string.IsNullOrEmpty(_dateTimeFormat)) {
                return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
            }
            else {
                return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
            }
        }
        else {
            return default(DateTimeOffset);
        }
    }


    public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
}

#pragma warning restore CS8618
#pragma warning restore CS8601
#pragma warning restore CS8603



/*
 * https://pokeapi.co/api/v2
 * https://pokeapi.co/api/v2/pokemon/{id or name}/
 * Hoje eu te desafio a desenvolver uma funcionalidade onde o jogador acessa uma lista de opções de espécies de mascotes e visualizar suas características para facilitar sua escolha antes da adoção.
 *
 * 
 * https://viacep.com.br/ws/88063254/json/
 */