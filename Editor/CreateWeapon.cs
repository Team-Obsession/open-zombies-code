using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateWeapon {
	[MenuItem("Assets/Create/Weapon")]
	
	public static void CreateMyWeapon()
	{
		Weapon asset = ScriptableObject.CreateInstance<Weapon>();

		AssetDatabase.CreateAsset (asset, "NewWeapon.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}
