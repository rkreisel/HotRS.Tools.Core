using System.IO;
using System.Threading.Tasks;

namespace RK.HotRS.ToolsCore.Helpers.Azure
{
	/// <summary>
	/// An interface for the Azure Blob Storage Helper (to enable mocking in unit tests)
	/// </summary>
	public interface IAzureBlobStorageHelper
	{
		/// <summary>
		/// Retrieves a file from storage
		/// </summary>
		/// <param name="accountName">The name of the blob storage account</param>
		/// <param name="accountKey">he key for the blob storage account</param>
		/// <param name="containerName">The name for the container in which to place the content</param>
		/// <param name="fileName">The name of the item which will be retrieved from storage</param>
		/// <param name="filePath">An optional "sub folder" in which the content should be found</param>
		/// <returns></returns>
		Task<FileDownload> GetDownloadableFileFromStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "");
		/// <summary>
		/// Places a file into storage
		/// </summary>
		/// <param name="accountName">The name of the blob storage account</param>
		/// <param name="accountKey">he key for the blob storage account</param>
		/// <param name="containerName">The name for the container in which to place the content</param>
		/// <param name="fileStream">The content to be stored</param>
		/// <param name="fileName">The name of the item which will be placed into storage</param>
		/// <param name="filePath">An optional "sub folder" in which to place he content</param>
		/// <param name="returnEmptyStringOnFailure">Optional (default = true). If true an empty string is returned if an exception is encountered. 
		/// Otherwise, the exception is rethrown. </param>
		/// <returns></returns>
		Task<string> UploadFileToStorage(string accountName, string accountKey, string containerName, Stream fileStream, string fileName, string filePath = "", bool returnEmptyStringOnFailure = true);
		/// <summary>
		/// Adds a generic object to Blob storage
		/// </summary>
		/// <param name="accountName">The name of the blob storage account</param>
		/// <param name="accountKey">he key for the blob storage account</param>
		/// <param name="containerName">The name for the container in which to place the content</param>
		/// <param name="content">The conent to add</param>
		/// <param name="fileName">The name of the item which will be placed into storage</param>
		/// <param name="filePath">An optional "sub folder" in which to place he content</param>
		/// <returns></returns>
		Task<string> AddToBlobStorage(string accountName, string accountKey, string containerName, string content, string fileName, string filePath = "");
		/// <summary>
		/// Retrieves a blob from storage.
		/// </summary>
		/// <param name="accountName">The name of the blob storage account</param>
		/// <param name="accountKey">he key for the blob storage account</param>
		/// <param name="containerName">The name for the container in which to place the content</param>
		/// <param name="blobKey">The key of the blob to retreive"</param>
		/// <param name="filePath">The hierarchical location where the object should be found. (Optional: defaults to an empy string.</param>
		/// <returns>An object</returns>
		Task<string> GetFromBlobStorage(string accountName, string accountKey, string containerName, string blobKey, string filePath = "");
		/// <summary>
		/// Removes a blob from storage
		/// </summary>
		/// <param name="accountName">The name of the blob storage account</param>
		/// <param name="accountKey">he key for the blob storage account</param>
		/// <param name="containerName">The name for the container in which to place the content</param>
		/// <param name="blobKey">The key of the object to delete</param>
		/// <param name="filePath">The hierarchical location where the object should be found. (Optional: defaults to an empy string.</param>
		/// <returns></returns>
		Task DeleteFromBlobStorage(string accountName, string accountKey, string containerName, string blobKey, string filePath = "");

		/// <summary>
		/// Delete a file from storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="fileName"></param>
		/// <param name="filePath"></param>
		/// <returns>The key of the deleted blob.</returns>
		Task DeleteFileFromStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "");

		/// <summary>
		/// Determines if a file exists in blob storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="fileName"></param>
		/// <param name="filePath"></param>
		/// <returns>True or False</returns>
		Task<bool> FileExistsInStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "");
	}
}