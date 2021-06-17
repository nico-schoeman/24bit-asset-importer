using UnityEngine;

#if (UNITY_EDITOR)
namespace EditorTools.ImportConfigExtensions
{
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
#endif