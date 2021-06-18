// <copyright file="TextureImportConfig.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using UnityEngine;

    /// <summary>
    /// Import configuration scriptable object for textures
    /// </summary>
    [CreateAssetMenu(fileName = "TextureImportConfig", menuName = "ImportConfigurations/TextureImportConfig")]
    public class TextureImportConfig : BaseImportConfig
    {
        /// <summary>
        /// Gets or sets the value of the Anisotropic Level
        /// </summary>
        public int AnisotropicLevel { get; set; } = 1;

        /// <summary>
        /// Gets or sets the value of the max texture size
        /// </summary>
        public int MaxTextureSize { get; set; } = 2048;

        /// <summary>
        /// Gets or sets a value indicating whether the settings should override for Android
        /// </summary>
        public bool OverrideForAndroid { get; set; } = false;
    }
}
#endif