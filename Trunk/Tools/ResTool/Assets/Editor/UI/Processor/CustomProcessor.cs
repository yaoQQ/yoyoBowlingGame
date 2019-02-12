using UnityEngine;
using UnityEditor;

public class CustomProcessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        string[] pathArr = textureImporter.assetPath.Split('/');

        if(pathArr[pathArr.Length - 1].Split('.')[0].Split('_')[0] == "frame")
        {
            PreprocessFramePng(textureImporter);
            return;
        }

        string[] nameArr = pathArr[pathArr.Length - 1].Split('.')[0].Split('@');

        if (nameArr.Length < 2) return;
        string packageID = nameArr[0];

        textureImporter.textureType = TextureImporterType.Sprite;
        //textureImporter.grayscaleToAlpha = false;
        textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
        textureImporter.spritePackingTag = packageID;
        if (nameArr.Length >= 3)
        {
            textureImporter.spriteBorder = GetBorder(nameArr[2]);//LBRT
        }

        textureImporter.assetBundleName = "png/" + "packer_" + nameArr[0] + ".unity3d";
        textureImporter.mipmapEnabled = false;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.maxTextureSize = 1024;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        //textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;

        TextureImporterPlatformSettings androidSettings = new TextureImporterPlatformSettings();
        androidSettings.name = "Android";
        androidSettings.overridden = true;
        androidSettings.maxTextureSize = 1024;
        androidSettings.format = TextureImporterFormat.ETC_RGB4;
        androidSettings.compressionQuality = (int)TextureCompressionQuality.Normal;
        androidSettings.allowsAlphaSplitting = true;
        androidSettings.crunchedCompression = true;
        textureImporter.SetPlatformTextureSettings(androidSettings);

        TextureImporterPlatformSettings iPhoneSettings = new TextureImporterPlatformSettings();
        iPhoneSettings.name = "iPhone";
        iPhoneSettings.overridden = true;
        iPhoneSettings.maxTextureSize = 1024;
        iPhoneSettings.format = TextureImporterFormat.RGBA32;//
        iPhoneSettings.compressionQuality = (int)TextureCompressionQuality.Normal;// (int)TextureCompressionQuality.Best;
        iPhoneSettings.allowsAlphaSplitting = true;
        iPhoneSettings.crunchedCompression = true;
        textureImporter.SetPlatformTextureSettings(iPhoneSettings);


        textureImporter.SetAllowsAlphaSplitting(true);
    }
    void OnPostprocessTexture(Texture2D texture)
    {
        string[] nameArr = texture.name.Split('@');
        if (nameArr.Length < 2) return;
        texture.name = texture.name.ToLower();
        Debug.Log(texture.name);
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        foreach (var str in importedAssets)
        {

            if (str.EndsWith(".png"))
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(str);
                if (texture != null)
                {
                    string[] nameArr = texture.name.Split('@');
                    if (nameArr.Length < 2) continue;
                    texture.name = texture.name.ToLower();
                }
            }

        }

    }

    Vector4 GetBorder(string sliceStr)
    {
        if (!string.IsNullOrEmpty(sliceStr))
        {
            var values = sliceStr.Split('x');
            var v4 = new Vector4();
            {
                v4.x = values[0].ToFloat();
                v4.y = values.Length < 2 ? v4.x : values[1].ToFloat();
                v4.z = values.Length < 3 ? v4.y : values[2].ToFloat();
                v4.w = values.Length < 4 ? v4.z : values[3].ToFloat();

            }
            return v4;
        }
        return new Vector4();
    }

    void PreprocessFramePng(TextureImporter textureImporter)
    {
        int SIZEW = 128;
        int SIZEH = 128;
        int COLUMN = 8;
        int ROW =8;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        textureImporter.mipmapEnabled = false;


        var blocks = new SpriteMetaData[COLUMN * ROW];
        string[] pathArr = textureImporter.assetPath.Split('/');
        string name=pathArr[pathArr.Length - 1].Split('.')[0];
        int id = -1;
        for (int i = ROW-1; i >=0; --i)
        {
            for (int j = 0; j < COLUMN; ++j)
            {
                id++;
                SpriteMetaData tmp = new SpriteMetaData();
                tmp.name = name + "_" + id;
                tmp.rect = new Rect(j * SIZEW, SIZEH * i, SIZEW, SIZEH);
                blocks[id] = tmp;
               
            }
        }
        textureImporter.spritesheet = blocks;
    }

}
