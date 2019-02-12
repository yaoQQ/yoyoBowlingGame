using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureUtil
{

   

    public static Texture2D FastBlur(Texture2D image, int radius, int iterations)
    {
        float avgR = 0;
        float avgG = 0;
        float avgB = 0;
        //float avgA = 0;
        float blurPixelCount = 0;
        Texture2D tex = image;

        for (var i = 0; i < iterations; i++)
        {

            tex = BlurImage(tex, radius, true, ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);
            tex = BlurImage(tex, radius, false,ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);
        }

        return tex;
    }

    static Texture2D BlurImage(Texture2D image, int blurSize, bool horizontal, ref float avgR, ref float avgG, ref float avgB, ref float blurPixelCount)
    {

        Texture2D blurred = new Texture2D(image.width, image.height);
        int _W = image.width;
        int _H = image.height;
        int xx, yy, x, y;

        if (horizontal)
        {
            for (yy = 0; yy < _H; yy++)
            {
                for (xx = 0; xx < _W; xx++)
                {
                    ResetPixel(ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);

                    //Right side of pixel

                    for (x = xx; (x < xx + blurSize && x < _W); x++)
                    {
                        AddPixel(image.GetPixel(x, yy), ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);
                    }

                    //Left side of pixel

                    for (x = xx; (x > xx - blurSize && x > 0); x--)
                    {
                        AddPixel(image.GetPixel(x, yy), ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);

                    }


                    CalcPixel(ref avgR, ref avgG, ref avgB, ref blurPixelCount);

                    for (x = xx; x < xx + blurSize && x < _W; x++)
                    {
                        blurred.SetPixel(x, yy, new Color(avgR, avgG, avgB, 1.0f));

                    }
                }
            }
        }

        else
        {
            for (xx = 0; xx < _W; xx++)
            {
                for (yy = 0; yy < _H; yy++)
                {
                    ResetPixel(ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);

                    //Over pixel

                    for (y = yy; (y < yy + blurSize && y < _H); y++)
                    {
                        AddPixel(image.GetPixel(xx, y), ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);
                    }
                    //Under pixel

                    for (y = yy; (y > yy - blurSize && y > 0); y--)
                    {
                        AddPixel(image.GetPixel(xx, y), ref  avgR, ref  avgG, ref  avgB, ref  blurPixelCount);
                    }
                    CalcPixel(ref avgR, ref avgG, ref avgB, ref blurPixelCount);
                    for (y = yy; y < yy + blurSize && y < _H; y++)
                    {
                        blurred.SetPixel(xx, y, new Color(avgR, avgG, avgB, 1.0f));

                    }
                }
            }
        }

        blurred.Apply();
        return blurred;
    }
    static void AddPixel(Color pixel,ref float avgR, ref float avgG, ref float avgB,ref float blurPixelCount)
    {
        avgR += pixel.r;
        avgG += pixel.g;
        avgB += pixel.b;
        blurPixelCount++;
    }

    static void ResetPixel(ref float avgR, ref float avgG, ref float avgB, ref float blurPixelCount)
    {
        avgR = 0.0f;
        avgG = 0.0f;
        avgB = 0.0f;
        blurPixelCount = 0;
    }

    static void CalcPixel(ref float avgR, ref float avgG, ref float avgB, ref float blurPixelCount)
    {
        avgR = avgR / blurPixelCount;
        avgG = avgG / blurPixelCount;
        avgB = avgB / blurPixelCount;
    }



}
