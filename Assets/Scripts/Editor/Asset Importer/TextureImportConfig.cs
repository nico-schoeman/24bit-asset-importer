using UnityEngine;

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    [CreateAssetMenu(fileName = "TextureImportConfig", menuName = "ImportConfigurations/TextureImportConfig")]
    public class TextureImportConfig : BaseImportConfig
    {
        [Tooltip("Set the anisotropic level for imported textures to use")]
        [Range(0, 16)]
        [SerializeField]
        public int anisotropicLevel = 1;

        [Tooltip("Set the max texture size for imported textures to use")]
        [SelectOption(new object[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 })]
        [SerializeField]
        public int maxTextureSize = 2048;

        [Tooltip("Enable if the texture settings should be overriden for Android")]
        [SerializeField]
        public bool overrideForAndroid = false;
    }
}
#endif