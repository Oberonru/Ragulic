namespace System.Interfaces
{
    public interface IConstructable<T> where T : UnityEngine.Component
    {
        void Construct(T t);
    }
}