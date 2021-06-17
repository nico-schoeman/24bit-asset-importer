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
        /// The anisotropic level for imported textures to use
        /// </summary>
        [Tooltip("Set the anisotropic level for imported textures to use")]
        [Range(0, 16)]
        [SerializeField]
        private int anisotropicLevel = 1;

        /// <summary>
        /// The max texture size for imported textures to use
        /// </summary>
        [Tooltip("Set the max texture size for imported textures to use")]
        [SelectOption(new object[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 })]
        [SerializeField]
        private int maxTextureSize = 2048;

        /// <summary>
        /// Check for whether the setting should be override
        /// </summary>
        [Tooltip("Enable if the texture settings should be overriden for Android")]
        [SerializeField]
        private bool overrideForAndroid = false;

        /// <summary>
        /// Gets or sets the value of the Anisotropic Level
        /// </summary>
        public int AnisotropicLevel { get => this.anisotropicLevel; set => this.anisotropicLevel = value; }

        /// <summary>
        /// Gets or sets the value of the max texture size
        /// </summary>
        public int MaxTextureSize { get => this.maxTextureSize; set => this.maxTextureSize = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the settings should override for Android
        /// </summary>
        public bool OverrideForAndroid { get => this.overrideForAndroid; set => this.overrideForAndroid = value; }
    }
}
#endif