namespace NEvo.ValueObjects.PersonalData;

public interface IPersonalData<T> where T : IPersonalData<T>
{
    T Anonimize();
}