using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ImportConfigExtensions;

public class AssetImporter : AssetPostprocessor
{
    void OnPostprocessTexture (Texture texture)
    {
        string path = Directory.GetParent(assetPath).FullName.RemoveSystemDirectory();
        TextureImportConfiguration config = GetConfigRecursive<TextureImportConfiguration>(path);
        
        if (config != null)
        {
            Debug.Log("We have a config");
        }
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