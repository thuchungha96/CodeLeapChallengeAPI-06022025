namespace CodeLeapChallengeAPI_06022025.Data.Class
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorLog
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StackTrace { get; set; }
    }

}
