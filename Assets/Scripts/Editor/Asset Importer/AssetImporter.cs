using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using EditorTools.ImportConfigExtensions;

#if (UNITY_EDITOR)
namespace EditorTools
{
    public class AssetImporter : AssetPostprocessor
    {
        // Keep track of the instance for use in the static menu item action
        public static AssetImporter instance;
        public AssetImporter()
        {
            instance = this;
        }

        [MenuItem("24Bit Tools/Asset Importer - Reimport select folders #a")] //Hotkey is Shift-A
        public static void Run()
        {
            List<string> paths = new List<string>();
            // Get all the selected assets in the editor (this includes folders)
            foreach (var item in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(item);
                // If it is a file, we get the directory the file is in
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                paths.Add(path);
            }

            // Get all the texture assets guids in the selected paths
            foreach (var guid in AssetDatabase.FindAssets("t:Texture", paths.ToArray()))
            {
                // Convert guid to path
                string path = AssetDatabase.GUIDToAssetPath(guid);

                // Check that the assets is in one of the paths selected and not in a child directory for example
                if (paths.Contains(Path.GetDirectoryName(path)))
                {
                    instance.DoPostprocessTexture(path);
                }
            }

            // Get all the audio clips guids in the selected paths
            foreach (var guid in AssetDatabase.FindAssets("t:AudioClip", paths.ToArray()))
            {
                // Convert guid to path
                string path = AssetDatabase.GUIDToAssetPath(guid);

                // Check that the assets is in one of the paths selected and not in a child directory for example
                if (paths.Contains(Path.GetDirectoryName(path)))
                {
                    instance.DoPostprocessAudio(path);
                }
            }
        }

        // Event hook for texture imports
        void OnPostprocessTexture(Texture texture)
        {
            DoPostprocessTexture(assetPath);
        }

        // Event hook for audio imports
        void OnPostprocessAudio(AudioClip audio)
        {
            DoPostprocessAudio(assetPath);
        }

        /// <summary>
        /// Find and apply the config settings for the texture asset at the provided path
        /// </summary>
        private void DoPostprocessTexture(string path)
        {
            // get a path with the system path removed
            string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

            // recursively get the nearest config
            TextureImportConfig config = GetConfigRecursive<TextureImportConfig>(directory);

            if (config != null)
            {
                TextureImporter importer = assetImporter as TextureImporter;

                // Importer can be null in the case where this is run from a menu item instead from a event.
                // In that case we get the importer for the asset manualy
                if (importer == null)
                    importer = TextureImporter.GetAtPath(path) as TextureImporter;

                // Set the default import setting values accoridng to the configuration
                importer.anisoLevel = config.anisotropicLevel;
                importer.maxTextureSize = config.maxTextureSize;

                //Override settings for android
                if (config.overrideForAndroid)
                {
                    TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
                    //platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                    platformSettings.name = "Android";
                    platformSettings.overridden = true;
                    platformSettings.maxTextureSize = config.maxTextureSize;

                    importer.SetPlatformTextureSettings(platformSettings);
                }

                // Save and refresh the asset database
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Find and apply the config settings for the audio clip asset at the provided path
        /// </summary>
        private void DoPostprocessAudio(string path)
        {
            // get a path with the system path removed
            string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

            // recursively get the nearest config
            AudioImportConfig config = GetConfigRecursive<AudioImportConfig>(directory);

            if (config != null)
            {
                AudioImporter importer = assetImporter as AudioImporter;

                // Importer can be null in the case where this is run from a menu item instead from a event.
                // In that case we get the importer for the asset manualy
                if (importer == null)
                    importer = AudioImporter.GetAtPath(path) as AudioImporter;

                // Set the default import setting values accoridng to the configuration
                AudioImporterSampleSettings settings = importer.defaultSampleSettings;
                settings.loadType = config.loadType;
                settings.compressionFormat = config.compressionFormat;
                settings.sampleRateSetting = config.sampleRateSetting;

                importer.defaultSampleSettings = settings;

                //Override settings for android
                if (config.overrideForAndroid)
                {
                    //platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                    importer.SetOverrideSampleSettings("Android", settings);
                }

                // Save and refresh the asset database
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Recursivly find a config file of generic type T.
        /// Directories will only be traversed backwards to parents, configs in child directories will be ignored.
        /// </summary>
        private T GetConfigRecursive<T>(string path) where T : ScriptableObject
        {
            // If we traversed to the point where we are no longer in the Assets folder, assume that there is no config file defined and exit
            if (string.IsNullOrWhiteSpace(path.RemoveSystemDirectory())) return null;

            // find all the guids of assets of the generic type
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] { path });

            foreach (var guid in guids)
            {
                // Convert guid to path
                string configPath = AssetDatabase.GUIDToAssetPath(guid);

                // Check that config path is the path we are currently looking at
                if (path == Directory.GetParent(configPath.InvertSlash()).FullName.RemoveSystemDirectory())
                {
                    // Found the nearest-non-child config asset, return it
                    return AssetDatabase.LoadAssetAtPath<T>(configPath);
                }
            }

            // Didn't find a appropriate config file, so we traverse upwards
            return GetConfigRecursive<T>(Directory.GetParent(path).FullName.RemoveSystemDirectory());
        }
    }
}
#endif