namespace Server.WebAPI.Model
{
    public class APIResponseModel<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
