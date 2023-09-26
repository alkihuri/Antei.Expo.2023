using UnityEngine;
using System.Collections;
using System.IO;
using System;
using TMPro;
using System.Collections.Generic;
public class PhotoMaker : MonoBehaviour
{
	public List<String> PhotoPath;


	public void MakePhoto()
	{
		MakePhoto(false);
	}

	/// <summary>
	/// Saves the screen image as png picture, and optionally opens the saved file.
	/// </summary>
	/// <returns>The file name.</returns>
	/// <param name="openIt">If set to <c>true</c> opens the saved file.</param>
	public string MakePhoto(bool openIt)
	{
		int resWidth = Screen.width;
		int resHeight = Screen.height;

		Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); //Create new texture
		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);

		// hide the info-text, if any
		/*if (infoText)
		{
			infoText.text = string.Empty;
		}*/
		
		Camera.main.targetTexture = rt;
		Camera.main.Render();
		Camera.main.targetTexture = null;
		
		// get the screenshot
		RenderTexture prevActiveTex = RenderTexture.active;
		RenderTexture.active = rt;

		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

		// clean-up
		RenderTexture.active = prevActiveTex;
		Destroy(rt);

		byte[] btScreenShot = screenShot.EncodeToJPG();
		Destroy(screenShot);

#if !UNITY_WSA
		// save the screenshot as jpeg file
		string sDirName = Application.persistentDataPath + "/Screenshots";
		if (!Directory.Exists(sDirName))
			Directory.CreateDirectory(sDirName);

		string sFileName = sDirName + "/" + string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
		File.WriteAllBytes(sFileName, btScreenShot);
		PhotoPath.Add(sFileName);
		Debug.Log("Photo saved to: " + sFileName);
		/*
		if (infoText)
		{
			infoText.text = "Saved to: " + sFileName;
		}
		*/
		// open file
		if (openIt)
		{
			System.Diagnostics.Process.Start(sFileName);
		}

		return sFileName;
#elif NETFX_CORE
        System.Threading.Tasks.Task<string> task = null;

        string sFileName = string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
        string sFileUrl = string.Empty; // "ms-appdata:///local/" + sFileName;

		UnityEngine.WSA.Application.InvokeOnUIThread(() =>
		{
        	task = SaveImageFileAsync(sFileName, btScreenShot, openIt);
		}, true);

        while (task != null && !task.IsCompleted && !task.IsFaulted)
        {
            task.Wait(100);
        }

        if (task != null)
        {
            if (task == null)
                throw new Exception("Could not create task for SaveImageFileAsync()");
            else if (task.IsFaulted)
                throw task.Exception;

            sFileUrl = task.Result;
            Debug.Log(sFileUrl);
        }

        return sFileUrl;
#else
		return string.Empty;
#endif
	}

#if NETFX_CORE
    private async System.Threading.Tasks.Task<string> SaveImageFileAsync(string imageFileName, byte[] btImageContent, bool openIt)
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile imageFile = await storageFolder.CreateFileAsync(imageFileName,
            Windows.Storage.CreationCollisionOption.ReplaceExisting);

        await Windows.Storage.FileIO.WriteBytesAsync(imageFile, btImageContent);

        if(openIt)
        {
            await Windows.System.Launcher.LaunchFileAsync(imageFile);
        }

        return imageFile.Path;
    }
#endif

}