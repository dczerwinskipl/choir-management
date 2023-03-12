using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NEvo.ValueObjects.PersonalData;

public record PESEL : IPersonalData<PESEL>
{
    [MinLength(11), MaxLength(11), Required]
    public string Number { get; init; }

    [JsonConstructor]
    public PESEL(string number)
    {
        Number = Validate(number);
    }

    private PESEL() { }

    private string Validate(string peselNumber)
    {
        if (!IsValidChecksum(peselNumber))
            throw new InvalidPESELChecksumException();

        return peselNumber;
    }

    public bool IsValidChecksum(string peselNumber)
    {
        if (peselNumber == null || peselNumber.Length != 11)
        {
            return false;
        }

        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            var c = peselNumber[i];
            if (c < '0' || c > '9')
            {
                return false;
            }

            int digit = c - '0';
            switch (i % 4)
            {
                case 0: sum += digit * 1; break;
                case 1: sum += digit * 3; break;
                case 2: sum += digit * 7; break;
                case 3: sum += digit * 9; break;
            }
        }

        int checksum = (10 - (sum % 10)) % 10;
        return checksum == peselNumber[10] - '0';
    }

    public DateOnly GetBirthDate()
    {
        var year = (Number[0] - '0') * 10 + (Number[1] - '0');
        var month = (Number[2] - '0') * 10 + (Number[3] - '0');
        var day = (Number[4] - '0') * 10 + (Number[5] - '0');

        switch (Number[2])
        {
            case '0':
            case '1':
                year += 1900;
                break;
            case '2':
            case '3':
                year += 2000;
                month -= 20;
                break;
            case '4':
            case '5':
                year += 2100;
                month -= 40;
                break;
            case '6':
            case '7':
                year += 2200;
                month -= 60;
                break;
            case '8':
            case '9':
                year += 1800;
                month -= 80;
                break;
        }

        return new DateOnly(year, month, day);
    }

    public PESEL Anonimize() => new PESEL(RandomPESEL()); //TODO: generate random

    private static string RandomPESEL()
    {
        Span<byte> pesel = stackalloc byte[11];

        int year = Random.Shared.Next(1900, 2099);
        int month = Random.Shared.Next(1, 13);
        int day = Random.Shared.Next(1, DateTime.DaysInMonth(year, month));

        if (year >= 2000)
            month += 20;

        pesel[1] = (byte)(year % 10);
        pesel[0] = (byte)(year / 10 % 10);

        pesel[3] = (byte)(month % 10);
        pesel[2] = (byte)(month / 10 % 10);

        pesel[5] = (byte)(day % 10);
        pesel[4] = (byte)(day / 10 % 10);

        pesel[6] = (byte)Random.Shared.Next(0, 10);
        pesel[7] = (byte)Random.Shared.Next(0, 10);
        pesel[8] = (byte)Random.Shared.Next(0, 10);
        pesel[9] = (byte)Random.Shared.Next(0, 10);

        int sum = 1 * pesel[0] + 3 * pesel[1] + 7 * pesel[2] + 9 * pesel[3] + 1 * pesel[4] + 3 * pesel[5] + 7 * pesel[6] + 9 * pesel[7] + 1 * pesel[8] + 3 * pesel[9] + 1 * pesel[10];
        pesel[10] = (byte)((10 - (sum % 10)) % 10);

        for (int i = 0; i < 11; i++)
            pesel[i] = (byte)(pesel[i] + 48);

        return Encoding.ASCII.GetString(pesel);
    }
}

public class InvalidPESELChecksumException : ArgumentException
{
    public InvalidPESELChecksumException() : base("Invalid PESEL number checksum") { }
}