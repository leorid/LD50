using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace JL
{
	public class UI_WeaponIcon : MonoBehaviour
	{
		UIDocument uiDocument;

		static List<VisualElement> weaponIcons = new List<VisualElement>();

		void Awake()
		{
			uiDocument = GetComponentInParent<UIDocument>();
			VisualElement weaponIconRoot = uiDocument.rootVisualElement.Q("WeaponIcon");
			weaponIcons.AddRange(weaponIconRoot.Children());
			Debug.Log("weaponIcons.Count " + weaponIcons.Count);
		}

		public static void SetWeaponIcon(int weaponIndex)
		{
			for (int i = 0; i < weaponIcons.Count; i++)
			{
				bool draw = i == weaponIndex;
				weaponIcons[i].style.display =
					draw ? DisplayStyle.Flex : DisplayStyle.None;
			}
		}
	}
}
