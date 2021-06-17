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
        /// Audio Clip Load Type
        /// </summary>
        [Tooltip("Set the Load Type for imported AudioClips to use")]
        [SerializeField]
        private AudioClipLoadType loadType;

        /// <summary>
        /// Audio Compression Format
        /// </summary>
        [Tooltip("Set the Compression Format for imported AudioClips to use")]
        [SerializeField]
        private AudioCompressionFormat compressionFormat;

        /// <summary>
        /// Audio Sample Rate Setting
        /// </summary>
        [Tooltip("Set the Sample Rate for imported AudioClips to use")]
        [SerializeField]
        private AudioSampleRateSetting sampleRateSetting;

        /// <summary>
        /// Override Android check
        /// </summary>
        [Tooltip("Enable if the Audio import settings should overriden for Android")]
        [SerializeField]
        private bool overrideForAndroid = false;

        /// <summary>
        /// Gets or sets load type
        /// </summary>
        public AudioClipLoadType LoadType
        {
            get { return this.loadType; }
            set { this.loadType = value; }
        }

        /// <summary>
        /// Gets or sets compression format
        /// </summary>
        public AudioCompressionFormat CompressionFormat
        {
            get { return this.compressionFormat; }
            set { this.compressionFormat = value; }
        }

        /// <summary>
        /// Gets or sets sample rate
        /// </summary>
        public AudioSampleRateSetting SampleRateSetting
        {
            get { return this.sampleRateSetting; }
            set { this.sampleRateSetting = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Android platform should be override
        /// </summary>
        public bool OverrideForAndroid
        {
            get { return this.overrideForAndroid; }
            set { this.overrideForAndroid = value; }
        }
    }
}
#endif