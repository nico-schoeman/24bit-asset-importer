// <copyright file="BaseImportConfig.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Base class for all import config scriptable objects
    /// </summary>
    public class BaseImportConfig : ScriptableObject
    {
        /// <summary>
        /// Add a right-click context menu to run the retroactive process on/in the select folders (can also select a asset)
        /// </summary>
        [MenuItem("Assets/Reimport Assets in selected folders")]
        public static void ReimportAssetsWithConfigurations()
        {
            AssetImporter.RunPostprocesses();
        }
    }
}
#endif