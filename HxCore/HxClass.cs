using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    using System.Linq;
    public class HxResponseResult
    {
        public string Result => ResultType.ToString(); // 결과 상태 코드 문자열 표현
        public HxResultType ResultType { get; set; } // 결과 상태 코드 (예: Success, Failed 등)
        

        public object Value { get; set; } // 결과 값 (선택적)
        public string ValueType => Value?.GetType().Name ?? "null"; // 결과 값의 타입 (선택적)

        public int? ValueInputCount { get; set; } // 결과 값의 개수 (선택적)
        public int ValueCount // 결과 값의 개수 (선택적)
        {
            get
            {
                if (ValueInputCount != null && ValueInputCount > 0)
                {
                    return ValueInputCount.ToIntEx(); // ValueCnt가 설정되어 있으면 해당 값을 반환
                }
                return ValueActualCount;
            }
        }
        public int ValueActualCount // 결과 값의 개수 (선택적)
        {
            get
            {
                if (Value is IEnumerable<object> enumerable)
                {
                    return enumerable.Count();
                }
                return Value != null ? 1 : 0; // Value가 null이 아니면 1, null이면 0
            }
        }
        public string Message { get; set; } // 결과 메시지 (선택적)
        public string Error { get; set; } // 에러 메시지 (선택적)
        public Exception Exception { protected get; set; } // 예외 정보 (선택적)
        public HxMessageType MessageType { get; set; } // 메시지 유형 (예: Info, Warning, Error 등)
        public string Remark { get; set; } // 추가적인 설명 (선택적)
        public string Module { get; set; } // 모듈 이름 (선택적)
        /*
        public string Code { get; set; } // 결과 코드 (선택적)
        public string DetailCode { get; set; } // 결과 상세 코드 (선택적)
        public string Detail { get; set; } // 결과 상세 정보 (선택적)
        public string Title { get; set; } // 결과 제목 (선택적)
        public string Description { get; set; } // 결과 설명 (선택적)
        public string ErrorCode { get; set; } // 오류 코드 (선택적)
        public string ErrorMessage { get; set; } // 오류 메시지 (선택적)
        //public object Data { get; set; } // 결과 데이터 (선택적)
        */
        public DateTime Timestamp { get; set; } // 결과 생성 시각
        public DateTime UtcTimestamp { get; set; } // 결과 생성 시각
        public HxResponseResult()
        {
            ResultType = HxResultType.None; // 기본값으로 None 설정
            Timestamp = DateTime.Now; // 기본값으로 현재 시각 설정
            UtcTimestamp = DateTime.UtcNow; // 기본값으로 현재 UTC 시각 설정
        }
#if DEBUG
        public Exception DeugException => Exception; // 디버그 모드에서 예외 정보 반환 (선택적)
        /*
        public string ResultTypeName => ResultType.ToString(); // 결과 상태 코드 이름 (예: Success, Failed 등)
        public string ResultTypeFullName => ResultType.GetType().FullName; // 결과 상태 코드의 전체 타입 이름

        public string ResultTypeFullNameWithAssembly => ResultType.GetType().FullName + ", " + ResultType.GetType().Assembly.GetName().Name; // 결과 상태 코드의 전체 타입 이름과 어셈블리 이름
        public string ResultTypeShortName => ResultType.GetType().Name; // 결과 상태 코드의 짧은 타입 이름 (예: HxResultType)
        public string ResultTypeShortNameWithAssembly => ResultType.GetType().Name + ", " + ResultType.GetType().Assembly.GetName().Name; // 결과 상태 코드의 짧은 타입 이름과 어셈블리 이름
        public string ResultTypeNamespace => ResultType.GetType().Namespace; // 결과 상태 코드의 네임스페이스 (예: HxCore)
        public string ResultTypeNamespaceWithAssembly => ResultType.GetType().Namespace + ", " + ResultType.GetType().Assembly.GetName().Name; // 결과 상태 코드의 네임스페이스와 어셈블리 이름
        public string ResultTypeAssemblyName => ResultType.GetType().Assembly.GetName().Name; // 결과 상태 코드의 어셈블리 이름 (예: HxCore)
        public string ResultTypeAssemblyFullName => ResultType.GetType().Assembly.FullName; // 결과 상태 코드의 어셈블리 전체 이름 (예: HxCore, Version=
        public string ResultTypeAssemblyVersion => ResultType.GetType().Assembly.GetName().Version.ToString(); // 결과 상태 코드의 어셈블리 버전 (예:
        public string ResultTypeAssemblyCulture => ResultType.GetType().Assembly.GetName().CultureInfo.Name; // 결과 상태 코드의 어셈블리 문화 정보 (예: "neutral")
        public string ResultTypeAssemblyCultureName => ResultType.GetType().Assembly.GetName().CultureInfo.Name; // 결과 상태 코드의 어셈블리 문화 정보 이름 (예: "neutral")
        public string ResultTypeAssemblyCultureDisplayName => ResultType.GetType().Assembly.GetName().CultureInfo.DisplayName; // 결과 상태 코드의 어셈블리 문화 정보 표시 이름 (예: "중립")
        public string ResultTypeAssemblyLocation => ResultType.GetType().Assembly.Location; // 결과 상태 코드의 어셈블리 위치 (예: "C:\path\to\HxCore.dll")
        public string ResultTypeAssemblyQualifiedName => ResultType.GetType().AssemblyQualifiedName; // 결과 상태 코드의 어셈블리 완전한 이름 (예: "HxCore.HxResultType, HxCore, Version=

        public string ValueTypeFullName => Value?.GetType().FullName ?? "null"; // 결과 값의 전체 타입 이름 (선택적)
        public string ValueTypeFullNameWithAssembly => Value?.GetType().FullName + ", " + Value?.GetType().Assembly.GetName().Name ?? "null"; // 결과 값의 전체 타입 이름과 어셈블리 이름 (선택적)
        public string ValueTypeShortName => Value?.GetType().Name ?? "null"; // 결과 값의 짧은 타입 이름 (선택적)
        */
#endif
    }
}
