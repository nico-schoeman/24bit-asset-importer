// <copyright file="AssetImporterExtensions.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter.ImportConfigExtensions
{
    using UnityEngine;

    /// <summary>
    /// Extensions to use when dealing with directory paths
    /// </summary>
    public static class AssetImporterExtensions
    {
        /// <summary>
        /// Change "/" to "\" in path strings
        /// </summary>
        /// <param name="path">The path to modify</param>
        /// <returns>The modified path</returns>
        public static string InvertSlashWindows(this string path)
        {
            // Invert the slashes from "/" to "\" since windows likes "\" and unity likes "/" because it uses URI paths
            return path.Replace('/', '\\');
        }

        /// <summary>
        /// Change "\" to "/" in path strings
        /// </summary>
        /// <param name="path">The path to modify</param>
        /// <returns>The modified path</returns>
        public static string InvertSlashURI(this string path)
        {
            // Invert the slashes from "\" to "/" since windows likes "\" and unity likes "/" because it uses URI paths
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// Remove the system path from the front of a path string if it exists
        /// </summary>
        /// <param name="path">The path to modify</param>
        /// <returns>The modified path</returns>
        public static string RemoveSystemDirectory(this string path)
        {
            // Check if the path is a full path containing the Assets directory, in that case we remove the data path but keep the Assets Directory
            if (path.Contains("\\Assets"))
            {
                return path.Replace(InvertSlashWindows(Application.dataPath), "Assets");
            }

            // Otherwise if the path is not a Assets path we remove the system path (data path - the assets directory)
            return path.Replace(InvertSlashWindows(Application.dataPath.Replace("/Assets", string.Empty)), string.Empty);
        }
    }
}
#endif