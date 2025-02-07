using CodeLeapChallengeAPI_06022025.Data.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseAPIController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        [NonAction]
        protected IActionResult GetRes<T>(ResponseDto<T> src)
        {
            return src.RespnseStatus.StatusCode switch
            {
                100 => StatusCode(StatusCodes.Status100Continue, src),
                101 => StatusCode(StatusCodes.Status101SwitchingProtocols, src),
                102 => StatusCode(StatusCodes.Status102Processing, src),
                201 => StatusCode(StatusCodes.Status201Created, src),
                202 => StatusCode(StatusCodes.Status202Accepted, src),
                203 => StatusCode(StatusCodes.Status203NonAuthoritative, src),
                204 => StatusCode(StatusCodes.Status204NoContent, src),
                400 => StatusCode(StatusCodes.Status400BadRequest, src),
                401 => StatusCode(StatusCodes.Status401Unauthorized, src),
                402 => StatusCode(StatusCodes.Status402PaymentRequired, src),
                403 => StatusCode(StatusCodes.Status403Forbidden, src),
                404 => StatusCode(StatusCodes.Status404NotFound, src),
                405 => StatusCode(StatusCodes.Status405MethodNotAllowed, src),
                406 => StatusCode(StatusCodes.Status406NotAcceptable, src),
                407 => StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, src),
                408 => StatusCode(StatusCodes.Status408RequestTimeout, src),
                409 => StatusCode(StatusCodes.Status409Conflict, src),
                410 => StatusCode(StatusCodes.Status410Gone, src),
                422 => StatusCode(StatusCodes.Status422UnprocessableEntity, src),
                500 => StatusCode(StatusCodes.Status500InternalServerError, src),
                501 => StatusCode(StatusCodes.Status501NotImplemented, src),
                502 => StatusCode(StatusCodes.Status502BadGateway, src),
                503 => StatusCode(StatusCodes.Status503ServiceUnavailable, src),
                504 => StatusCode(StatusCodes.Status504GatewayTimeout, src),
                505 => StatusCode(StatusCodes.Status505HttpVersionNotsupported, src),
                506 => StatusCode(StatusCodes.Status506VariantAlsoNegotiates, src),
                507 => StatusCode(StatusCodes.Status507InsufficientStorage, src),
                508 => StatusCode(StatusCodes.Status508LoopDetected, src),
                510 => StatusCode(StatusCodes.Status510NotExtended, src),
                511 => StatusCode(StatusCodes.Status511NetworkAuthenticationRequired, src),
                _ => StatusCode(StatusCodes.Status200OK, src),
            };
        }
    }
}
