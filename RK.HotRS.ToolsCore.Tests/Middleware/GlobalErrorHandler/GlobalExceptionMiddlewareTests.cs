using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace RK.HotRS.ToolsCore.Middleware.GlobalErrorHandler.Tests
{
    [TestFixture()]
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionMiddlewareTests
    {
        [Test()]
        public void BuildOptionsSucceeds()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.That(actual.ContentType.Equals("text/json", StringComparison.OrdinalIgnoreCase));
            Assert.That(actual.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test()]
        public void FullDetail()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.FullDetail);
        }

        [Test()]
        public void IncludeData()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeData);
        }

        [Test()]
        public void IncludeHResult()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeHResult);
        }

        [Test()]
        public void IncludeHelpLink()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeHelpLink);
        }

        [Test()]
        public void IncludeInnerexception()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeInnerException);
        }

        [Test()]
        public void IncludeSource()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeSource);
        }

        [Test()]
        public void IncludeStackTrace()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeStackTrace);
        }

        [Test()]
        public void GetStatusCode()
        {
            var actual = new GlobalExceptionHandlerOptions();
            Assert.IsFalse(actual.IncludeStackTrace);
        }
    }
}