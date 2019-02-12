using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MahjongEditor
{
	public class CFG
	{
		/// <summary>
		/// 单个麻将的参数(M)
		/// </summary>
		public const float TileLength = 0.4f;
		public const float TileWidth = 0.3f;
		public const float TileHeight = 0.22f;
	}

	#region 立牌编辑

	public class StandTileEditor : EditorWindow
	{
		const float OffsetWCompensations = 0.01f; // 水平方向偏移补偿量

		Vector3 standHome = new Vector3(-2.5f, 0f, -3.8f);
		Vector3 standRight = new Vector3(4f, 0f, -2f);
		Vector3 standOpposite = new Vector3(2f, 0f, 3.5f);
		Vector3 standLeft = new Vector3(-3.8f, 0f, 2f);

		Vector3 OffsetXW = Vector3.right * CFG.TileWidth;
		Vector3 OffsetXL = Vector3.right * CFG.TileLength;
		Vector3 OffsetYH = Vector3.up * CFG.TileHeight;
		Vector3 OffsetZW = Vector3.forward * CFG.TileWidth;
		Vector3 OffsetZL = Vector3.forward * CFG.TileLength;

		Vector3 stackHome = new Vector3(2.47f, CFG.TileHeight / 2, -3.1f);
		Vector3 stackRight = new Vector3(3.085f, CFG.TileHeight / 2, 2.4f);
		Vector3 stackOpposite = new Vector3(-2.47f, CFG.TileHeight / 2, 3.1f);
		Vector3 stackLeft = new Vector3(-3.085f, CFG.TileHeight / 2, -2.4f);

		Vector3 wallHome = new Vector3(-0.672f, -0.11f, -1.278f);
		Vector3 wallRight = new Vector3(1.26f, -0.11f, -0.82f);
		Vector3 wallOpposite = new Vector3(0.672f, -0.11f, 1.278f);
		Vector3 wallLeft = new Vector3(-1.26f, -0.11f, 0.82f);

		Vector3 WallHomeAngle = new Vector3(-90f, 180f, 0f);
		Vector3 WallRightAngle = new Vector3(-90f, 0f, 90f);
		Vector3 WallOppositeAngle = new Vector3(-90f, 0f, 0f);
		Vector3 WallLeftAngle = new Vector3(-90f, 0f, -90f);

		Vector3 StackHomeOppositeAngle = new Vector3(90f, 0f, 0f);
		Vector3 StackLeftRightAngle = new Vector3(90f, 0f, -90f);

		Vector3 StandOppositeAngle = new Vector3(0f, 0f, 0f);
		Vector3 StandRightAngle = new Vector3(0f, 90f, 0f);
		Vector3 StandHomeAngle = new Vector3(0, 180f, 0f);
		Vector3 StandLeftAngle = new Vector3(0, 270f, 0f);

		[MenuItem("Tool/Scene/MahjongTool/MahjongSceneEditor")]
		public static void Init()
		{
			StandTileEditor window = GetWindow<StandTileEditor>();
			window.minSize = new Vector2(320, 100);
			window.Show();
		}

		bool isStandTileEditor = false;
		bool isStackTileEditor = false;
		bool isWallTileEditor = false;
		bool isWinTileEditor = false;
		private void OnGUI()
		{
			isStandTileEditor = EditorGUILayout.Foldout(isStandTileEditor, "StandTileEditor");
			if (isStandTileEditor)
			{
				EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
				if (GUILayout.Button("Home", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stand", "Home");
				if (GUILayout.Button("Right", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stand", "Right");
				if (GUILayout.Button("Opposite", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stand", "Opposite");
				if (GUILayout.Button("Left", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stand", "Left");
				if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
				{
					var containerList = GameObject.FindObjectsOfType(typeof(SceneContainer)) as SceneContainer[];
					var root = containerList.Where(i => i.containerName.Contains("Stand"));
					if (root.Any() == false)
					{
						Debug.LogErrorFormat("根容器节点不存在: {0}", name);
						return;
					}
					foreach (var i in root)
					{
						Clear(i.transform);
					}
				}
			}

			isStackTileEditor = EditorGUILayout.Foldout(isStackTileEditor, "StackTileEditor");
			if (isStackTileEditor)
			{
				EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
				if (GUILayout.Button("Home", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stack", "Home");
				if (GUILayout.Button("Right", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stack", "Right");
				if (GUILayout.Button("Opposite", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stack", "Opposite");
				if (GUILayout.Button("Left", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Stack", "Left");
				if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
				{
					var containerList = GameObject.FindObjectsOfType(typeof(SceneContainer)) as SceneContainer[];
					var root = containerList.Where(i => i.containerName.Contains("Stack"));
					if (root.Any() == false)
					{
						Debug.LogErrorFormat("根容器节点不存在: {0}", name);
						return;
					}
					foreach (var i in root)
					{
						Clear(i.transform);
					}
				}
			}

			isWallTileEditor = EditorGUILayout.Foldout(isWallTileEditor, "WallTileEditor");
			if (isWallTileEditor)
			{
				EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
				if (GUILayout.Button("Home", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Wall", "Home");
				if (GUILayout.Button("Right", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Wall", "Right");
				if (GUILayout.Button("Opposite", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Wall", "Opposite");
				if (GUILayout.Button("Left", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
					this.Create("Wall", "Left");
				if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
				{
					var containerList = GameObject.FindObjectsOfType(typeof(SceneContainer)) as SceneContainer[];
					var root = containerList.Where(i => i.containerName.Contains("Wall"));
					if (root.Any() == false)
					{
						Debug.LogErrorFormat("根容器节点不存在: {0}", name);
						return;
					}
					foreach (var i in root)
					{
						Clear(i.transform);
					}
				}
			}
			isWinTileEditor = EditorGUILayout.Foldout(isWinTileEditor, "WinTileEditor");
			if (isWallTileEditor)
			{

			}

		}

		private void Create(string pre, string name)
		{
			Debug.Log(pre + name);

			var containerList = GameObject.FindObjectsOfType(typeof(SceneContainer)) as SceneContainer[];
			var root = containerList.FirstOrDefault(i => i.containerName == (pre + name));
			if (root == null)
			{
				Debug.LogErrorFormat("根容器节点不存在: {0}{1}", pre, name);
				return;
			}
			Clear(root.transform);
			var path = "Assets/Resources/mahjong/prefab/mahjong_tile.prefab";
			var res = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
			if (res == null)
			{
				Debug.LogErrorFormat("路径错误: {0}", path);
				return;
			}

			int maxStandCount = 0;
			if (pre == "Stand")
				maxStandCount = 14;
			else if (pre == "Stack")
				maxStandCount = 34;

			else
				maxStandCount = 24;

			switch (name)
			{
				case "Home":
					for (var i = 0; i < maxStandCount; i++)
					{
						var obj = PrefabUtility.InstantiatePrefab(res) as GameObject;
						obj.transform.SetParent(root.transform);
						foreach (Transform t in obj.transform)
						{
							t.gameObject.layer = root.gameObject.layer;
						}
						if (pre == "Stand")
						{
							obj.transform.localEulerAngles = this.StandHomeAngle;
							if (i < maxStandCount - 1)
								obj.transform.localPosition = standHome + i * (this.OffsetXW + Vector3.right * OffsetWCompensations);
							else
								obj.transform.localPosition = standHome + i * (this.OffsetXW + Vector3.right * OffsetWCompensations) + Vector3.right * 0.1f;
						}
						else if (pre == "Stack")
						{
							obj.transform.localEulerAngles = this.StackHomeOppositeAngle;
							var stackIndex = (i / 2);
							if (i % 2 == 0)
								obj.transform.localPosition = stackHome - stackIndex * this.OffsetXW;
							else
								obj.transform.localPosition = stackHome - this.OffsetYH - stackIndex * this.OffsetXW;
						}

						else if (pre == "Wall")
						{
							obj.transform.localEulerAngles = this.WallHomeAngle;
							var wallIndex = i / 6; // 所在排数
							obj.transform.localPosition = this.wallHome - wallIndex * this.OffsetZL - wallIndex * this.OffsetXW * 6 + i * this.OffsetXW;
						}

					}
					break;
				case "Right":
					for (var i = 0; i < maxStandCount; i++)
					{
						var obj = PrefabUtility.InstantiatePrefab(res) as GameObject;
						obj.transform.SetParent(root.transform);
						foreach (Transform t in obj.transform)
						{
							t.gameObject.layer = root.gameObject.layer;
						}
						if (pre == "Stand")
						{
							obj.transform.localEulerAngles = this.StandRightAngle;
							if (i < maxStandCount - 1)
								obj.transform.localPosition = standRight + i * (OffsetZW + Vector3.forward * OffsetWCompensations);
							else
								obj.transform.localPosition = standRight + i * (OffsetZW + Vector3.forward * OffsetWCompensations) + Vector3.forward * 0.1f;
						}
						else if (pre == "Stack")
						{
							obj.transform.localEulerAngles = StackLeftRightAngle;
							var stackIndex = (i / 2);
							if (i % 2 == 0)
								obj.transform.localPosition = stackRight - stackIndex * this.OffsetZW;
							else
								obj.transform.localPosition = stackRight - this.OffsetYH - stackIndex * this.OffsetZW;
						}
						else if (pre == "Wall")
						{
							obj.transform.localEulerAngles = this.WallRightAngle;
							var wallIndex = i / 6; // 所在排数
							obj.transform.localPosition = this.wallRight + wallIndex * this.OffsetXL - wallIndex * this.OffsetZW * 6 + i * this.OffsetZW;
						}

					}
					break;
				case "Opposite":
					for (var i = 0; i < maxStandCount; i++)
					{
						var obj = PrefabUtility.InstantiatePrefab(res) as GameObject;
						obj.transform.SetParent(root.transform);
						foreach (Transform t in obj.transform)
						{
							t.gameObject.layer = root.gameObject.layer;
						}
						if (pre == "Stand")
						{
							obj.transform.localEulerAngles = this.StandOppositeAngle;
							if (i < maxStandCount - 1)
								obj.transform.localPosition = standOpposite - i * (OffsetXW + Vector3.right * OffsetWCompensations);
							else
								obj.transform.localPosition = standOpposite - i * (OffsetXW + Vector3.right * OffsetWCompensations) - Vector3.right * 0.1f;
						}
						else if (pre == "Stack")
						{
							obj.transform.localEulerAngles = StackHomeOppositeAngle;
							var stackIndex = (i / 2);
							if (i % 2 == 0)
								obj.transform.localPosition = stackOpposite + stackIndex * this.OffsetXW;
							else
								obj.transform.localPosition = stackOpposite - this.OffsetYH + stackIndex * this.OffsetXW;
						}
						else if (pre == "Wall")
						{
							obj.transform.localEulerAngles = this.WallOppositeAngle;
							var wallIndex = i / 6; // 所在排数
							obj.transform.localPosition = this.wallOpposite + wallIndex * this.OffsetZL + wallIndex * this.OffsetXW * 6 - i * this.OffsetXW;
						}
					}
					break;
				case "Left":
					for (var i = 0; i < maxStandCount; i++)
					{
						var obj = PrefabUtility.InstantiatePrefab(res) as GameObject;
						obj.transform.SetParent(root.transform);
						foreach (Transform t in obj.transform)
						{
							t.gameObject.layer = root.gameObject.layer;
						}
						if (pre == "Stand")
						{
							obj.transform.localEulerAngles = this.StandLeftAngle;
							if (i < maxStandCount - 1)
								obj.transform.localPosition = standLeft - i * (OffsetZW + Vector3.forward * OffsetWCompensations);
							else
								obj.transform.localPosition = standLeft - i * (OffsetZW + Vector3.forward * OffsetWCompensations) - Vector3.forward * 0.1f;
						}
						else if (pre == "Stack")
						{
							obj.transform.localEulerAngles = StackLeftRightAngle;
							var stackIndex = (i / 2);
							if (i % 2 == 0)
								obj.transform.localPosition = stackLeft + stackIndex * this.OffsetZW;
							else
								obj.transform.localPosition = stackLeft - this.OffsetYH + stackIndex * this.OffsetZW;
						}
						else if (pre == "Wall")
						{
							obj.transform.localEulerAngles = this.WallLeftAngle;
							var wallIndex = i / 6; // 所在排数
							obj.transform.localPosition = this.wallLeft - wallIndex * this.OffsetXL + wallIndex * this.OffsetZW * 6 - i * this.OffsetZW;
						}
					}
					break;
				default:
					Debug.LogErrorFormat("方位错误: {0}", name);
					break;
			}
		}

		public static void Clear(Transform root)
		{
			if (root == null)
				return;
			List<Transform> deleteList = new List<Transform>();
			foreach (Transform i in root)
			{
				if (i == root)
					return;
				deleteList.Add(i);
			}

			foreach (var i in deleteList)
			{
				DestroyImmediate(i.gameObject);
			}
		}
	}

	#endregion

}