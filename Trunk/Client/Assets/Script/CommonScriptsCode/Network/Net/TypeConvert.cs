using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TypeConvert
{
    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] CharsToBytes ( char[] data )
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes( data );
        return bytes;
    }

    public static byte[] Int16ToBytes ( int data )
    {
		byte[] bytes = BitConverter.GetBytes( (Int16)data );
//        if ( BitConverter.IsLittleEndian )
//        {
//            Array.Reverse( bytes );
//        }
        return bytes;
    }

	public static byte[] Int32ToBytes ( int data )
	{
		byte[] bytes = BitConverter.GetBytes(data );
		//        if ( BitConverter.IsLittleEndian )
		//        {
		//            Array.Reverse( bytes );
		//        }
		return bytes;
	}

	public static byte[] DatatimeToBytes (System.DateTime now)
	{
		byte[] bts = BitConverter.GetBytes(now.ToBinary());
		return bts;
		//DateTime rt = DateTime.FromBinary(BitConverter.ToInt64(bts, 0)); 
	}

	public static float[] Vector3ToFloatArray(Vector3 pos)
	{
		float[] posArray = new float[3]{pos.x,pos.y,pos.z};
		return posArray;
	}

	public static Vector3 FloatArrayToVector3(List<float> pos)
	{
		Vector3 vectorPos  = new Vector3(pos[0],pos[1],pos[2]);
		return vectorPos;
	}

	public static Vector3 FloatArrayToVector3(float[] pos)
	{
		Vector3 vectorPos  = new Vector3(pos[0],pos[1],pos[2]);
		return vectorPos;
	}
}

