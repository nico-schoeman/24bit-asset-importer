using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    [CreateAssetMenu(fileName = "AudioImportConfig", menuName = "ImportConfigurations/AudioImportConfig")]
    public class AudioImportConfig : BaseImportConfig
    {
        [Tooltip("Set the Load Type for imported AudioClips to use")]
        [SerializeField]
        public AudioClipLoadType loadType;

        [Tooltip("Set the Compression Format for imported AudioClips to use")]
        [SerializeField]
        public AudioCompressionFormat compressionFormat;

        [Tooltip("Set the Sample Rate for imported AudioClips to use")]
        [SerializeField]
        public AudioSampleRateSetting sampleRateSetting;

        [Tooltip("Enable if the Audio import settings should overriden for Android")]
        [SerializeField]
        public bool overrideForAndroid = false;
    }
}
#endif