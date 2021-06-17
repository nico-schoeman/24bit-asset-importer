// <copyright file="AssetImporter.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using System.Collections.Generic;
    using System.IO;
    using EditorTools.AssetImporter.ImportConfigExtensions;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Main class for the Asset Importer tool
    /// </summary>
    public class AssetImporter : AssetPostprocessor
    {
        /// <summary>
        /// Keep track of the instance for use in the static menu item action.
        /// </summary>
        private static AssetImporter instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetImporter" /> class.
        /// </summary>
        public AssetImporter()
        {
            instance = this;
        }

        /// <summary>
        /// This command is called by the toolbar menu item and right-click context menu.
        /// It kicks of the retroactive process to apply config settings to assets in the selected folders.
        /// Hotkey is Shift-A
        /// </summary>
        [MenuItem("24Bit Tools/Asset Importer - Reimport select folders #a")]
        public static void RunPostprocesses()
        {
            List<string> paths = new List<string>();

            // Get all the selected assets in the editor (this includes folders)
            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

            foreach (UnityEngine.Object asset in selectedAssets)
            {
                string path = AssetDatabase.GetAssetPath(asset);

                // If it is a file, we get the directory the file is in
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                paths.Add(path.InvertSlashWindows());
            }

            // Get all the texture assets guids in the selected paths
            string[] textureGuids = AssetDatabase.FindAssets("t:Texture", paths.ToArray());

            foreach (string guid in textureGuids)
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
            string[] audioGuids = AssetDatabase.FindAssets("t:AudioClip", paths.ToArray());

            foreach (string guid in audioGuids)
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

        /// <summary>
        /// Event hook for texture imports
        /// </summary>
        /// <param name="texture">the Texture for this event</param>
        private void OnPostprocessTexture(Texture texture)
        {
            this.DoPostprocessTexture(this.assetPath);
        }

        /// <summary>
        /// Event hook for audio imports
        /// </summary>
        /// <param name="audio">The audio clip for this event</param>
        private void OnPostprocessAudio(AudioClip audio)
        {
            this.DoPostprocessAudio(this.assetPath);
        }

        /// <summary>
        /// Find and apply the config settings for the texture asset at the provided path
        /// </summary>
        /// <param name="path">path to the texture asset</param>
        private void DoPostprocessTexture(string path)
        {
            // get a path with the system path removed
            string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

            // recursively get the nearest config
            TextureImportConfig config = this.GetConfigRecursive<TextureImportConfig>(directory);

            if (config != null)
            {
                TextureImporter importer = assetImporter as TextureImporter;

                // Importer can be null in the case where this is run from a menu item instead from a event.
                // In that case we get the importer for the asset manualy
                if (importer == null)
                {
                    importer = TextureImporter.GetAtPath(path) as TextureImporter;
                }

                // Set the default import setting values accoridng to the configuration
                importer.anisoLevel = config.AnisotropicLevel;
                importer.maxTextureSize = config.MaxTextureSize;

                // Override settings for android
                if (config.OverrideForAndroid)
                {
                    // Platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                    TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings() { name = "Android", overridden = true, maxTextureSize = config.MaxTextureSize };

                    importer.SetPlatformTextureSettings(platformSettings);
                }
                else
                {
                    importer.ClearPlatformTextureSettings("Android");
                }

                // Save and refresh the asset database
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Find and apply the config settings for the audio clip asset at the provided path
        /// </summary>
        /// <param name="path">path to the audio asset</param>
        private void DoPostprocessAudio(string path)
        {
            // Get a path with the system path removed
            string directory = Directory.GetParent(path).FullName.RemoveSystemDirectory();

            // Recursively get the nearest config
            AudioImportConfig config = this.GetConfigRecursive<AudioImportConfig>(directory);

            if (config != null)
            {
                AudioImporter importer = assetImporter as AudioImporter;

                // Importer can be null in the case where this is run from a menu item instead from a event.
                // In that case we get the importer for the asset manualy
                if (importer == null)
                {
                    importer = AudioImporter.GetAtPath(path) as AudioImporter;
                }

                // Set the default import setting values accoridng to the configuration
                AudioImporterSampleSettings settings = importer.defaultSampleSettings;
                settings.loadType = config.LoadType;
                settings.compressionFormat = config.CompressionFormat;
                settings.sampleRateSetting = config.SampleRateSetting;

                importer.defaultSampleSettings = settings;

                // Override settings for android
                if (config.OverrideForAndroid)
                {
                    // platform options: "Webplayer", "Standalone", "iOS", "Android", "WebGL", "PS4", "PSP2", "XBoxOne", "Samsung TV"
                    importer.SetOverrideSampleSettings("Android", settings);
                }
                else
                {
                    importer.ClearSampleSettingOverride("Android");
                }

                // Save and refresh the asset database
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Recursively find a config file of generic type T.
        /// Directories will only be traversed backwards to parents, configurations in child directories will be ignored.
        /// </summary>
        /// <typeparam name="T">Type of the config, inherited from BaseImportConfig</typeparam>
        /// <param name="path">The current path in the recursion chain</param>
        /// <returns>Returns the config asset if it finds one, otherwise null</returns>
        private T GetConfigRecursive<T>(string path) where T : ScriptableObject
        {
            // If we traversed to the point where we are no longer in the Assets folder, assume that there is no config file defined and exit
            if (string.IsNullOrWhiteSpace(path.RemoveSystemDirectory()))
            {
                return null;
            }

            // find all the guids of assets of the generic type
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new string[] { path });

            foreach (string guid in guids)
            {
                // Convert guid to path
                string configPath = AssetDatabase.GUIDToAssetPath(guid);

                // Check that config path is the path we are currently looking at
                if (path == Directory.GetParent(configPath.InvertSlashWindows()).FullName.RemoveSystemDirectory())
                {
                    // Found the nearest-non-child config asset, return it
                    return AssetDatabase.LoadAssetAtPath<T>(configPath);
                }
            }

            // Didn't find a appropriate config file, so we traverse upwards
            return this.GetConfigRecursive<T>(Directory.GetParent(path).FullName.RemoveSystemDirectory());
        }
    }
}
#endif