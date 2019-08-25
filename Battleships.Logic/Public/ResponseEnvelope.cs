namespace Battleships.Logic.Public
{
    public class ResponseEnvelope<T>
    {
        public bool Success { get; set; }
        public T Content { get; set; }
        public string ErrorDescription { get; set; }
    }
}