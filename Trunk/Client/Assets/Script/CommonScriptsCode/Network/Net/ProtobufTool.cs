
using UnityEngine;
using System.Collections;
using System.IO;
using ProtoBuf;

public class ProtobufTool {


	public static byte[] PSerializer(object entity)
	{
		//Serialize
		byte[] buffer = null;
		using (MemoryStream m = new MemoryStream())
		{
			Serializer.Serialize(m, entity);
			m.Position = 0;
			int length = (int)m.Length;
			buffer = new byte[length];
			m.Read(buffer, 0, length);
		}
		return buffer;
	}
	public static T PDeserialize<T>(byte[] buffer)where T : class
	{
		T t = default(T);
		using (MemoryStream m = new MemoryStream(buffer))
		{
			t= Serializer.Deserialize<T>(m);
		}
		return t;
	}
}
