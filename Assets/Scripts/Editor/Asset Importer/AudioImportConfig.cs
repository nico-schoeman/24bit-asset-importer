using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "AudioImportConfig", menuName = "ImportConfigurations/AudioImportConfig")]
public class AudioImportConfig : BaseImportConfig
{
    public AudioClipLoadType loadType;
    public AudioCompressionFormat compressionFormat;
    public AudioSampleRateSetting sampleRateSetting;

    public bool overrideForAndroid = false;
}
