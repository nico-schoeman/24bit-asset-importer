using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureImportConfig", menuName = "ImportConfigurations/TextureImportConfig")]
public class TextureImportConfig : BaseImportConfig
{
    [Tooltip("Set the anisotropic level for imported textures to use")]
    [Range(0, 16)]
    [SerializeField]
    public int anisotropicLevel = 1;

    [SelectOption(new object[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 })]
    public int maxTextureSize = 2048;

    public bool overrideForAndroid = false;
}
