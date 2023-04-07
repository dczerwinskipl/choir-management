using System.ComponentModel.DataAnnotations;
using NEvo.Core;

namespace NEvo.ValueObjects.PersonalData;

public record Address : IPersonalData<Address>
{
    [MaxLength(64), Required]
    public string Street { get; init; }

    [MaxLength(16), Required]
    public string HouseNumber { get; init; }

    [MaxLength(16)]
    public string? ApartmentNumber { get; init; }

    [MaxLength(6), Required]
    public string PostalCode { get; init; }

    [MaxLength(32), Required]
    public string City { get; init; }

    [MaxLength(32)]
    public string? Commune { get; init; }

    [MaxLength(32)]
    public string? County { get; init; }

    [MaxLength(32)]
    public string? Voivodeship { get; init; }

    public Address(string street, string houseNumber, string? apartmentNumber, string postalCode, string city, string? commune = null, string? county = null, string? voivodeship = null)
    {
        Street = Check.NullOrEmpty(street);
        HouseNumber = Check.NullOrEmpty(houseNumber);
        ApartmentNumber = apartmentNumber;
        PostalCode = Check.NullOrEmpty(postalCode);
        City = Check.NullOrEmpty(city);
        Commune = commune;
        County = county;
        Voivodeship = voivodeship;
        Check.Annotations(this);
    }

    public Address Anonimize() => new Address(
        "anonymized",
        "anonymized",
        null,
        "00-000",
        "anonymized",
        null,
        null,
        null
        );
}