using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateGun {

	[MenuItem("Assets/Create/Weapon/Gun")]
	public static void CreateMyWeapon()
	{
		Weapon asset = ScriptableObject.CreateInstance<Gun>();
		asset.weaponType = WeaponType.Gun;

		AssetDatabase.CreateAsset (asset, "Assets/open-zombies-code/Weapons/Guns/New Gun.asset");
		asset.weaponName = asset.name;
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}
