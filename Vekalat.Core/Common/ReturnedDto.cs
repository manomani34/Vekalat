namespace Vekalat.Application.Common
{
    public class ReturnedDto<T> : ReturnedDto where T : class
    {
        public T Result { get; set; }
    }

    public class ReturnedDto
    {
        public int Status { get; set; }
        public string Target { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

