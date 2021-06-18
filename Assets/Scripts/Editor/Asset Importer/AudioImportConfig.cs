// <copyright file="AudioImportConfig.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Import configuration scriptable object for audio clips
    /// </summary>
    [CreateAssetMenu(fileName = "AudioImportConfig", menuName = "ImportConfigurations/AudioImportConfig")]
    public class AudioImportConfig : BaseImportConfig
    {
        /// <summary>
        /// Gets or sets load type
        /// </summary>
        public AudioClipLoadType LoadType { get; set; }

        /// <summary>
        /// Gets or sets compression format
        /// </summary>
        public AudioCompressionFormat CompressionFormat { get; set; }

        /// <summary>
        /// Gets or sets sample rate
        /// </summary>
        public AudioSampleRateSetting SampleRateSetting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Android platform should be override
        /// </summary>
        public bool OverrideForAndroid { get; set; } = false;
    }
}
#endif