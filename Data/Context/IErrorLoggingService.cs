using CodeLeapChallengeAPI_06022025.Data.Class;

namespace CodeLeapChallengeAPI_06022025.Data.Context
{
    public interface IErrorLoggingService
    {
        Task LogErrorAsync(HttpContext context, Exception ex);
    }
    /// <summary>
    /// 
    /// </summary>
    public class ErrorLoggingService : IErrorLoggingService
    {
        private readonly CodeDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ErrorLoggingService(CodeDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public async Task LogErrorAsync(HttpContext context, Exception ex)
        {
            var log = new ErrorLog
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                Level = "Error",
                Message = ex.Message,
                Exception = ex.InnerException?.Message,
                StackTrace = ex.StackTrace
            };

            _context.ErrorLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
