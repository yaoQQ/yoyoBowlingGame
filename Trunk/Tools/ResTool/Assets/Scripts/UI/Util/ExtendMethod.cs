using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtendMethod
{
	/// <summary>
    /// 字符串转换浮点 如果转换失败 返回0;
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static float ToFloat (this string self)
	{
		float f = 0;
		float.TryParse (self, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out f);
		return f;
	}
	/// <summary>
    /// 字符串转整形 如果转换失败 返回0;
	/// </summary>
	/// <param name="self"></param>
	/// <returns></returns>
	public static int ToInt (this string self)
	{
		int i = 0;
		int.TryParse (self, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out i);
		return i;
	}
}
