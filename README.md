# HotRS.Tools.Core
Various tools and extensions for .Net (6)

Follow/Contribute:
https://github.com/rkreisel/HotRS.Tools.Core

## Categories

1. Console Application Helper
   CloseIfNotAborted(int seconds = 60) - closes the application after the defined number of seconds if the user does not press a key.

2. Extensions

   1. Assembly

      ```c#
      GetTextFileFromAssembly(this Assembly asm, string filename)
      ```

   2. Collection

      ```c#
      IsNullOrEmpty<T>(this IEnumerable<T> source)
      ```

      

   3. Configuration
      Three extensions to manage instantiated Configuration objects:

      ```c#
      CleanUpJSONConfigs(this IConfiguration source, KeepWhich keepWhich = KeepWhich.First)
          
      CleanUpJSONConfigs(this IConfiguration source, IList<ConfigItem> items)
          
      PreferUserSecrets(this IConfiguration source)
      ```

   4. Enum
      Three extensions to simplify using attributes on Enums:

      ```c#
      GetEnumDescription<T>(this T value, bool useDisplayIfNoDesc = true, bool useDefaultIfNoDescOrDisplay = true) 
          
      GetDataType<T>(this T value)
          
      GetValueFromDescription<T>(this T value, string description)
      ```

   5. Exception
      Four extensions for Exceptions:

      ```C#
      /*
      Access the normally readonly Data property. Useful for passing detailed information to the "catcher" of the exception.
      */
      SetData<T>(this T source, IDictionary<string, string> data) 
      
      //Access the normally readonly HelpLink property
      SetHelpLink<T>(this T source, string helpLink) 
          
      /*
      Returns a simple List<Exception> with all the exceptions in the primary 
      exception.
      */    
      GetInnerExceptions(this Exception ex)
          
      //Returns a list of all the exception messages as a single string
      AllExceptionMessages<T>(this T source, bool withCR = true) 
      ```

      

   6. Object
      A new custom CheckForNull extension that allows the developer to throw a custom exception of <T> with a custom message

      ```c#
      CheckForNull<T>(this object o, string paramName, string message = "") 
      ```

   7. String
      A handful of string manipulation extensions:

      ```c#
      ToNullableDateTime(this string s)
      ToNullableInt(this string s) 
      AddCSVInjectionProtection(this string source)
      RemoveCSVInjectionProtection(this string source)
      DateStringFromExcelDateString(this string source, string format = null)
      AppendListToString<T>(this string source, List<T> list, string prefix = ", ")
      ```

      

   8. ValidationError
      An extensions that formats the error messages into a string using the specified delimiter and optional line feed.

      ```c#
      FormatErrors(this IList<ValidationResult> source, string delimiter = ", ", bool useLineFeed = false, bool includeMemberNames = false)
      ```

      

3. Helpers

   1. File Upload (to web site)
      Methods to facilitate uploading of large files

      ```c#
      UploadAsync(HttpContext context, FormOptions _defaultFormOptions, string fileStorePath)
      
      UploadSmallFileAsync(IFormFile file, string landingPath)
      
      GetEncoding(MultipartSection section)
      ```

      HttpRequest (builder)

   2. MIscellaneous

      1. Directory

         ```c#
         CleanUp(string folder, int hours)
         
         EnsurePathExists(string path)
         ```

      2. Reflection

         ```c#
         GetCurrentMethod()	
         ```

   3. ZipTools

      ```C#
      GetManifest(string fileName)
      
      ExtractFile(string zipFileName, string itemPathAndName)
      ```

      

   4. Office
      Excel

      ```c#
      /*
      Gets the "name" of a column from its ordinal number. Use this to get the alphabetic value of an integer coulmn number. For instance 27 will return "AA"
      */
      GetExcelColumnName(int columnNumber)
      ```

      Office File Property Helper

      ```c#
      /*
      Gets the extended file properties (Owner, LastUpdatedBy, whatever else is there)
      */
      GetProperties(string fileName)
      ```

      

   5. Testing
      Two methods that allow unit testing of private methods

      ```c#
      GetPrivateMethod<T>(T source, string methodName) where T : class
      
      GetPrivateMethodAsync<T>(T source, string methodName) where T : class
      ```

      

4. Middleware
   Swagger Tool to add a FileUpload element to the Swagger screen

5. Utilities

   A couple utility methods methods for JSON and XML files

   ```
   ObfuscatedPropertyResolver(IEnumerable<string> propNamesToIgnore)
   ObfuscatingConverter 
   PropertyRenameOrIgnoreSerializerContractResolver
   ```

   
