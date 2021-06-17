using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ImportConfigExtensions;

public class AssetImporter : AssetPostprocessor
{
    // Keep track of the instance for use in the static menu item action
    public static AssetImporter instance;
    public AssetImporter()
    {
        instance = this;
    }

    [MenuItem("24Bit Tools/Asset Importer - Reimport select folders #a")] //Hotkey is Shift-A
    public static void Run ()
    {
        List<string> paths = new List<string>();
        foreach (var item in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            string path = AssetDatabase.GetAssetPath(item);
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            paths.Add(path);
        }

        foreach (var guid in AssetDatabase.FindAssets("t:Texture", paths.ToArray()))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (paths.Contains(Path.GetDirectoryName(path)))
            {
                instance.DoPostprocessTexture(path);
            }
        }

        foreach (var guid in AssetDatabase.FindAssets("t:AudioClip", paths.ToArray()))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (paths.Contains(Path.GetDirectoryName(path)))
            {
                instance.DoPostprocessAudio(path);
            }
        }
    }

    void OnPostprocessTexture (Texture texture)
    {
        DoPostprocessTexture(assetPath);
    }

    void OnPostprocessAudio (AudioClip audio)
    {
        DoPostprocessAudio(assetPath);
    }

    private void DoPostprocessTexture (string path)
    {
        string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

        TextureImportConfig config = GetConfigRecursive<TextureImportConfig>(directory);

        if (config != null)
        {
            TextureImporter importer = assetImporter as TextureImporter;

            if (importer == null)
                importer = TextureImporter.GetAtPath(path) as TextureImporter;

            importer.anisoLevel = config.anisotropicLevel;
            importer.maxTextureSize = config.maxTextureSize;

            if (config.overrideForAndroid)
            {
                TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
                //platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                platformSettings.name = "Android";
                platformSettings.overridden = true;
                platformSettings.maxTextureSize = config.maxTextureSize;

                importer.SetPlatformTextureSettings(platformSettings);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DoPostprocessAudio (string path)
    {
        string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

        AudioImportConfig config = GetConfigRecursive<AudioImportConfig>(directory);

        if (config != null)
        {
            AudioImporter importer = assetImporter as AudioImporter;

            if (importer == null)
                importer = AudioImporter.GetAtPath(path) as AudioImporter;

            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            settings.loadType = config.loadType;
            settings.compressionFormat = config.compressionFormat;
            settings.sampleRateSetting = config.sampleRateSetting;

            importer.defaultSampleSettings = settings;

            if (config.overrideForAndroid)
            {
                //platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                importer.SetOverrideSampleSettings("Android", settings);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Recursivly find a config file of generic type T.
    /// Directories will only be traversed backwards to parents, configs in child directories will be ignored.
    /// </summary>
    private T GetConfigRecursive<T> (string path) where T : ScriptableObject
    {
        if (string.IsNullOrWhiteSpace(path.RemoveSystemDirectory())) return null;

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] { path });

        foreach (var guid in guids)
        {
            string configPath = AssetDatabase.GUIDToAssetPath(guid);

            if (path == Directory.GetParent(configPath.InvertSlash()).FullName.RemoveSystemDirectory())
            {
                return AssetDatabase.LoadAssetAtPath<T>(configPath);
            }
        }

        return GetConfigRecursive<T>(Directory.GetParent(path).FullName.RemoveSystemDirectory());
    }
}


namespace ImportConfigExtensions {
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Change "/" to "\" in path strings
        /// </summary>
        public static string InvertSlash(this string path)
        {
            // invert the slashes from "/" to "\" since windows likes "\" and unity likes "/" because it uses URI paths
            return path.Replace('/', '\\');
        }

        /// <summary>
        /// Remove the system path from the front of a path string if it exists
        /// </summary>
        public static string RemoveSystemDirectory(this string path)
        {
            // Check if the path is a full path containing the Assets directory, in that case we remove the data path but keep the Assets Directory
            if (path.Contains("\\Assets")) return path.Replace(InvertSlash(Application.dataPath), "Assets");
            // Otherwise if the path is not a Assets path we remove the system path (data path - the assets directory)
            return path.Replace(InvertSlash(Application.dataPath.Replace("/Assets", "")), "");
        }
    }
}