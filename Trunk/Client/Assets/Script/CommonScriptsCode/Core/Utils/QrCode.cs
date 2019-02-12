using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QrCode
{
    public static Texture2D Encode(string text, int size)
    {
        if (string.IsNullOrEmpty(text))
            return null;
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = size,
                Width = size
            }
        };
        Color32[] color32 = writer.Write(text);
        Texture2D encoded = new Texture2D(size, size);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
}