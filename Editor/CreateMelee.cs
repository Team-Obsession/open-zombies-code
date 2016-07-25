using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMelee {

	[MenuItem("Assets/Create/Weapon/Melee")]
	public static void CreateMyWeapon()
	{
		Weapon asset = ScriptableObject.CreateInstance<Melee>();
		asset.weaponType = WeaponType.Melee;

		AssetDatabase.CreateAsset (asset, "Assets/open-zombies-code/Weapons/Guns/New Melee.asset");
		AssetDatabase.SaveAssets();
		asset.weaponName = asset.name;

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}
