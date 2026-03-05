using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxCore.Web.AspNetCore.Mvc
{
    public class HxMvcControllerBase : ControllerBase
    {
        public string Module { get; protected set; }

        public HxMvcControllerBase()
        {
            // 기본 생성자
        }
        public HxMvcControllerBase(string module)
        {
            // 모듈 이름을 설정하는 생성자
            Module = module;
        }

        protected virtual string GetDefaultViewName()
        {
            //return null;
            return "Index";
        }

        protected virtual string GetDefaultLayoutName()
        {
            //return null;
            return "MainLayout";
        }

        protected virtual string GetDefaultErrorViewName()
        {
            //return null;
            return "Error";
        }
        
        public static ActionResult<HxResponseResult> CreateResponseResult(HxResponseResult result)
        {

            if (result == null)
            {
                result = new HxResponseResult
                {
                    ResultType = HxResultType.Error,
                    Message = "Response result cannot be null.",
                    MessageType = HxMessageType.Error,
                    Timestamp = DateTime.Now,
                    UtcTimestamp = DateTime.UtcNow
                };
            }
            else if (result.ResultType == HxResultType.None)
            {
                result.ResultType = HxResultType.Success; // 기본적으로 성공으로 설정
            }
            else if (result.ResultType == HxResultType.Error && string.IsNullOrEmpty(result.Error))
            {
                result.Error = "An error occurred."; // 기본 에러 메시지 설���
            }
            else if (result.ResultType == HxResultType.Success && string.IsNullOrEmpty(result.Message))
            {
                result.Message = "Operation completed successfully."; // 기본 성공 메시지 설정
            }

            if (result.Timestamp == default)
            {
                result.Timestamp = DateTime.Now; // 현재 시각으로 설정
            }
            if (result.UtcTimestamp == default)
            {
                result.UtcTimestamp = DateTime.UtcNow; // 현재 UTC 시각으로 설정
            }

            // 결과 객체를 ActionResult로 래핑하여 반환
            return new ActionResult<HxResponseResult>(result);
        }

        public static ActionResult<HxResponseResult> CreateResponseResult(HxResultType resultType, object value = null, string message = null, HxMessageType messageType = HxMessageType.Info, string module = null)
        {
            var result = new HxResponseResult
            {
                ResultType = resultType,
                Value = value,
                Message = message,
                MessageType = messageType,
                Module = module,
                Timestamp = DateTime.Now,
                UtcTimestamp = DateTime.UtcNow
            };
            return CreateResponseResult(result);
        }
        public static ObjectResult CreateStatusCodeResult(HxResponseResult value, int statusCode = StatusCodes.Status200OK)
        {
            return new ObjectResult(value)
            {
                StatusCode = statusCode
            };
        }
        public static ObjectResult CreateStatusOkResult(HxResponseResult value)
        {
            return new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        public static ObjectResult CreateStatusServerErrorResult(HxResponseResult value)
        {
            return new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        public static ObjectResult CreateStatusServerNotFoundResult(HxResponseResult value)
        {
            return new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public ObjectResult CreateActionResult(object result)
        {
            return Ok(result);
        }
        public IActionResult CreateActionResult(HxResultType resultType, string message, HxMessageType messageType = HxMessageType.Info, string module = null)
        {
            try
            {
                ActionResult<HxResponseResult> result = CreateResponseResult(resultType, null, message, messageType, module);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // 예외 발생 시 HxResponseResult를 생성하여 반환
                var errorResult = new HxResponseResult
                {
                    ResultType = HxResultType.Error,
                    Message = "An error occurred while processing the request.",
                    Error = ex.Message,
                    Exception = ex,
                    MessageType = HxMessageType.Error,
                    Timestamp = DateTime.Now,
                    UtcTimestamp = DateTime.UtcNow
                };
                return CreateStatusCodeResult(errorResult, StatusCodes.Status500InternalServerError);
                //throw ex;
            }
        }
    }
}
