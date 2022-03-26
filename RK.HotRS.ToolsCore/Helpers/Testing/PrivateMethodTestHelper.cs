namespace RK.HotRS.ToolsCore.Helpers.Testing
{
    using RK.HotRS.ToolsCore.Extensions;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// A class to implement a helper for unit testing private methods
    /// </summary>
    public static class PrivateMethodTestHelper
    {

        /// <summary>
        /// Allows unit testing of a private method.
        /// </summary>
        /// <typeparam name="T">A Type</typeparam>
        /// <param name="source">An instance of a class</param>
        /// <param name="methodName">The method name to find.</param>
        /// Usage:
        /// var class2Test = // a instance of the class
        /// var privateMethod = GetPrivateMethod(class2Test, "method2test");
        /// var result = privateMethod.Invoke(class2Test, new object[] { /* array of objects matching the method signature of the method to test – or null if none*/ });
        /// <returns>A MethodInfo object that can be used to execute a private method.</returns>
        public static MethodInfo GetPrivateMethod<T>(T source, string methodName) where T : class
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException(Properties.Resources.METHODNAMECANNOTBENULL);
            }
            source.CheckForNull(nameof(source));

            var method = source.GetType()
                     .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (method == null)
            {
                throw new ArgumentOutOfRangeException($"{methodName} method not found");
            }
            return method;
        }

        /// <summary>
        /// Allows unit testing of a private async method.
        /// </summary>
        /// <typeparam name="T">A Type</typeparam>
        /// <param name="source">An instance of a class</param>
        /// <param name="methodName">The method name to find.</param>
        /// Usage:
        /// var class2Test = // a instance of the class
        /// var privateMethod = GetPrivateMethod(class2Test, "method2test");
        /// var result = privateMethod.Invoke(class2Test,  });
        /// var result = (dynamic) Task.Run(() => privateMethod.Invoke(this.SystemUnderTest, new object[] { /* array of objects matching the method signature of the method to test – or null if none*/)).Result;
        /// <returns>A MethodInfo object that can be used to execute an async private method.</returns>
        public static async Task<MethodInfo> GetPrivateMethodAsync<T>(T source, string methodName) where T : class
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException(Properties.Resources.METHODNAMECANNOTBENULL);
            }

            source.CheckForNull(nameof(source));

            var method = source.GetType()
                     .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (method == null)
            {
                throw new ArgumentOutOfRangeException($"{methodName} method not found");
            }
            return await Task.FromResult(method).ConfigureAwait(true);
        }

    }


}
