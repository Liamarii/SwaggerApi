namespace WebApi.Exceptions
{
    public class UnsuccessfulResponseException : Exception, ISerializable
    {
        public UnsuccessfulResponseException(string message) : base(message)
        {
        }

        public UnsuccessfulResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsuccessfulResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
