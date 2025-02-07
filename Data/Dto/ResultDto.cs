namespace CodeLeapChallengeAPI_06022025.Data.Dto
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseDto<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string ResponseDateTime { get; set; } = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        /// <summary>
        /// 
        /// </summary>
        public T? ResponseData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ResponseSta? RespnseStatus { get; set; } = new ResponseSta();
    }
    /// <summary>
    /// 
    /// </summary>
    public class ResponseSta
    {
        /// <summary>
        /// 
        /// </summary>
        public string? ResponseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ResponseMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
    }
}
