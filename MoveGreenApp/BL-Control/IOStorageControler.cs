using System;
using PCLStorage;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace MoveGreenApp
{
	public class IOStorageControler
	{
		public IOStorageControler ()
		{

		}

		public async Task<bool> WriteFile(string fileName, string file)
		{
			bool didWrite = false;
			try {
				if (fileName != null && fileName.Trim () != "" && file != null && file.Trim () != "") 
				{
					IFolder rootFolder = FileSystem.Current.LocalStorage;
					IFolder folder = await rootFolder.CreateFolderAsync ("MoveGreen Data",
						CreationCollisionOption.OpenIfExists);
					IFile iFile = await folder.CreateFileAsync (fileName,
						CreationCollisionOption.ReplaceExisting);
					await iFile.WriteAllTextAsync (file);
					didWrite = true;
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Exception raised while writing file : " + ex);
			}
			return didWrite;
		}

		public async Task<T> ReadFile<T>(string fileName)
		{
			if (fileName != null && fileName.Trim () != "")
			{
				IFolder rootFolder = FileSystem.Current.LocalStorage;
				IFolder folder = await rootFolder.CreateFolderAsync ("MoveGreen Data",
					CreationCollisionOption.OpenIfExists);

				ExistenceCheckResult isFileExisting = await folder.CheckExistsAsync(fileName + ".txt");

				if (!isFileExisting.ToString().Equals("NotFound"))
				{
					try
					{
						IFile file = await folder.CreateFileAsync(fileName + ".txt",
							CreationCollisionOption.OpenIfExists);

						String languageString = await file.ReadAllTextAsync();

						XmlSerializer oXmlSerializer = new XmlSerializer(typeof(T));
						return (T)oXmlSerializer.Deserialize(new StringReader(languageString));
					}
					catch (Exception ex)
					{
						Debug.WriteLine("Exception : " + ex);
						return default(T);
					}
				}

				return default(T);
			}
			return default(T);
		}
	}
}