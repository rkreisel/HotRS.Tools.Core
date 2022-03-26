using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace RK.HotRS.ToolsCore.Exceptions
{
	/// <summary>
	/// Custom exception class for internal errors.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class RKToolsException : Exception
    {
		/// <summary>
		/// Holds the name of the method throwing the error
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// Holds a stirng list of additional details (preferrably user-friendly)
		/// </summary>
		public IList<string> ErrorDetails { get;  }

		/// <summary>
		/// Clone of Exception
		/// </summary>
		public RKToolsException()
			: base() { }

		/// <summary>
		/// Clone of Exception
		/// </summary>
		/// <param name="message"></param>
		public RKToolsException(string message)
			: base(message) { }

		/// <summary>
		/// Clone of Exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public RKToolsException(string message, Exception innerException)
			: base(message, innerException) { }

		/// <summary>
		/// Clone of Exception
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public RKToolsException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		/// <summary>
		/// Clone of Exception, adding Method
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="method">The name of the module where the error occured</param>
		/// <param name="innerException">The optional inner exception</param>
		public RKToolsException(string message, string method, Exception innerException = null)
			: base(message, innerException)
		{
			this.Method = method;
		}

		/// <summary>
		/// Clone of Exception, adding Method and ErrorDetails
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="method">The name of the module where the error occured</param>
		/// <param name="errorDetails">A list of strings with user supplied details</param>
		/// <param name="innerException">The optional inner exception</param>
		public RKToolsException(string message, string method, IList<string> errorDetails, Exception innerException = null)
			: base(message, innerException)
		{
			this.Method = method;
			this.ErrorDetails = errorDetails;
		}

		/// <summary>
		/// Clone of Exception, adding ErrorDetails
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="errorDetails">A list of strings with user supplied details</param>
		/// <param name="innerException">The optional inner exception</param>
		public RKToolsException(string message, IList<string> errorDetails, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorDetails = errorDetails;
		}
	}
}
