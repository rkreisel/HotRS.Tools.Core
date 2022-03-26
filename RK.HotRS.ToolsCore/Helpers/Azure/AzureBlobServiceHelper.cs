using Microsoft.AspNetCore.StaticFiles;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using RK.HotRS.ToolsCore.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace RK.HotRS.ToolsCore.Helpers.Azure
{
	/// <summary>
	/// A set of helper methods for managing Azure blob storage
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class AzureBlobStorageHelper : IAzureBlobStorageHelper
	{
		/// <summary>
		/// This gets a file with all the needed information to create a FileResult stream in the controller.  
		/// That is: the file memory stream, the file mime type, the file name.
		/// </summary>
		/// <param name="accountName">Name of the storage account.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container the file exist in.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="filePath">The path to the file.</param>
		/// <returns>The file memory stream, the file mime type, the file name packaged in a FileDownload object.</returns>
		public async Task<FileDownload> GetDownloadableFileFromStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			ValidateAzureSettings(accountName, accountKey, containerName);
			if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(filePath));

			var stream = new MemoryStream();
			var mimeProvider = new FileExtensionContentTypeProvider();
			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{fileName}");
			await block.DownloadToStreamAsync(stream).ConfigureAwait(false);

			stream.Position = 0;

            string mimeType;
            try
			{
				mimeType = mimeProvider.Mappings[Path.GetExtension(fileName)];
			}
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
				mimeType = "application/octet-stream";
			}
			
			return new FileDownload
			{
				MemoryStream = stream,
				FileName = Path.GetFileNameWithoutExtension(fileName),
				FileExtension = Path.GetExtension(fileName),
				MimeType = mimeType
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountName">Name of the storage account.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container the file exist in.</param>
		/// <param name="fileStream">A Stream containing the file to store.</param>
		/// <param name="fileName">A name for the file.</param>
		/// <param name="filePath">The optional path in which to store the file.</param>
		/// <param name="returnEmptyStringOnFailure">True or False</param>
		/// <returns>The unique key for the stored blob.</returns>
		public async Task<string> UploadFileToStorage(string accountName, string accountKey, string containerName, Stream fileStream, string fileName, string filePath = "", bool returnEmptyStringOnFailure = true)
		{
			filePath.CheckForNull(nameof(filePath));
			ValidateAzureSettings(accountName, accountKey, containerName);
			if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(filePath));

			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{fileName}");

			//TODO: Determine if this is the desired result. It swallows any exceptions rather than allowing the caller
			//handle them and try to recover.
			try
			{
				await block.UploadFromStreamAsync(fileStream).ConfigureAwait(false);
				return await Task.FromResult(block.Name).ConfigureAwait(false);
			}
			catch
			{
				if (returnEmptyStringOnFailure)
				{
					return await Task.FromResult(string.Empty).ConfigureAwait(false);
				}
				else
				{
					throw;
				}
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		/// <summary>
		/// This method accepts a string and places it into Blob storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="content">The value in insert.</param>
		/// <param name="fileName">The name of the "file" to add.</param>
		/// <param name="filePath">The optional additional "path" for the "file".</param>
		/// <returns>The unique key of he blob that was stored.</returns>
		public async Task<string> AddToBlobStorage(string accountName, string accountKey, string containerName, string content, string fileName, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			ValidateAzureSettings(accountName, accountKey, containerName);
			if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(filePath));

			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{fileName}");

			await block.UploadTextAsync(content).ConfigureAwait(false);
			return await Task.FromResult(block.Name).ConfigureAwait(false);
		}

		/// <summary>
		/// This method pulls a string from blob storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="blobKey">The key identifying the blob to pull.</param>
		/// <param name="filePath">The hierarchical location where the object should be found. (Optional: defaults to an empy string.)</param>
		/// <returns>A string</returns>
		public async Task<string> GetFromBlobStorage(string accountName, string accountKey, string containerName, string blobKey, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			ValidateAzureSettings(accountName, accountKey, containerName);
			if (string.IsNullOrWhiteSpace(blobKey)) throw new ArgumentNullException(nameof(blobKey));

			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{blobKey}");

			var blob = await block.DownloadTextAsync().ConfigureAwait(false);
			return await Task.FromResult(blob).ConfigureAwait(false);
		}

		/// <summary>
		/// Deletes a blob from storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="blobKey">The key identifying the blob to delete.</param>
		/// <param name="filePath">The hierarchical location where the object should be found. (Optional: defaults to an empy string.</param>
		/// <returns></returns>
		public async Task DeleteFromBlobStorage(string accountName, string accountKey, string containerName, string blobKey, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			ValidateAzureSettings(accountName, accountKey, containerName);
			if (string.IsNullOrWhiteSpace(blobKey)) throw new ArgumentNullException(nameof(blobKey));

			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var blob = container.GetBlockBlobReference($"{filePath}{blobKey}");
			await blob.DeleteIfExistsAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Delete a file from storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="fileName"></param>
		/// <param name="filePath"></param>
		/// <returns>The key of the deleted blob.</returns>
		public async Task DeleteFileFromStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{fileName}");
			if (block == null || string.IsNullOrWhiteSpace(block.Name))
			{
				throw new InvalidDataException($"The blob named {filePath}{fileName} could not be found.");
			}
				await block.DeleteAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Determines if a file exists in blob storage
		/// </summary>
		/// <param name="accountName">The name of the storage account used.</param>
		/// <param name="accountKey">The key on the storage account.</param>
		/// <param name="containerName">The container to use.</param>
		/// <param name="fileName"></param>
		/// <param name="filePath"></param>
		/// <returns>True or False</returns>
		public async Task<bool> FileExistsInStorage(string accountName, string accountKey, string containerName, string fileName, string filePath = "")
		{
			filePath.CheckForNull(nameof(filePath));
			var container = GetContainer(accountName, accountKey, containerName);

			filePath = filePath.EndsWith('/') || string.IsNullOrEmpty(filePath) ? filePath : filePath += "/";

			var block = container.GetBlockBlobReference($"{filePath}{fileName}");

			return await block.ExistsAsync().ConfigureAwait(false);
		}

		//TODO: Consider the possible value of exposing these methods publicly.
		private static CloudBlobContainer GetContainer(string accountName, string accountKey, string containerName) =>
			GetClient(accountName, accountKey).GetContainerReference(containerName);

		private static CloudBlobClient GetClient(string accountName, string accountKey)
		{
			var credentials = new StorageCredentials(accountName, accountKey);
			var account = new CloudStorageAccount(credentials, true);
			return account.CreateCloudBlobClient();
		}

		private static void ValidateAzureSettings(string accountName, string accountKey, string containerName)
		{
			var msg = string.Empty;
			if (string.IsNullOrWhiteSpace(accountName)) msg += "accountName parameter was null or empty ";
			if (string.IsNullOrWhiteSpace(accountKey)) msg += "accountKey parameter was null or empty ";
			if (string.IsNullOrWhiteSpace(containerName)) msg += "containerName parameter was null or empty ";
			if (msg.Length > 0) throw new ArgumentNullException(msg);
		}
	}
}

