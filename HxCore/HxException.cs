using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HxCore
{
    public class HxException : Exception
    {
        //참조 : https://github.com/Pkcs11Interop/Pkcs11Interop/blob/3.2.0/src/Pkcs11Interop/Pkcs11Interop/Common/UnmanagedException.cs

        private int? FErrorCode = null;
        /// <summary>
        /// Error Code
        /// </summary>
        public int? ErrorCode
        {
            get { return FErrorCode; }
            protected set { FErrorCode = value; }
        }

        /// <summary>
        /// Initializes new instance of Exception class
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        public HxException(string message) : base(message)
        {
            this.ErrorCode = null;
        }

        /// <summary>
        /// Initializes new instance of Exception class
        /// </summary>
        /// <param name="message">Message that describes the error</param>
        /// <param name="errorCode">Error code returned by the last function</param>
        public HxException(string message, int errorCode) : base(message)
        {
            this.ErrorCode = errorCode;
        }
        /// <summary>
        /// Initializes new instance of Exception class
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference</param>
        public HxException(string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorCode = null;
        }

        protected HxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                bool bErrorCodeSet = info.GetBoolean("ErrorCodeSet");
                if(bErrorCodeSet == true)
                {
                    this.ErrorCode = info.GetInt32("ErrorCode");
                }
            }
        }
    }
}
