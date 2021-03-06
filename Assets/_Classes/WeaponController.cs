using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class WeaponController : MonoBehaviour
	{
		public LayerMask mask;
		public WeaponBase currentWeapon;
		public bool isNoWeapon => currentWeapon is NoWeapon;

		public int weaponIndex;
		public List<WeaponBase> weapons = new List<WeaponBase>();
		public List<bool> weaponUnlocks = new List<bool>();

		private void Awake()
		{
			currentWeapon = weapons[weaponIndex];
			for (int i = 0; i < weapons.Count; i++)
			{
				WeaponBase weapon = weapons[i];
				weapon.Init();
				weapon.gameObject.SetActive(i == weaponIndex);
			}
		}

		public void GetInput(WeaponInput weaponInput)
		{
			if (currentWeapon) currentWeapon.GetInputs(weaponInput);
		}

		public void NextWeapon()
		{
			weaponIndex++;

			int whileBreaker = weapons.Count * 2;
			while (!SetWeapon(weaponIndex))
			{
				weaponIndex++;
				if (weaponIndex >= weapons.Count)
				{
					weaponIndex = 0;
				}

				whileBreaker--;
				if (whileBreaker < 0)
				{
					Debug.LogError("While Breaker Activated", this);
					throw new System.Exception("While Breaker");
				}
			}
		}
		public void PreviousWeapon()
		{
			weaponIndex--;

			int whileBreaker = weapons.Count * 2;
			while (!SetWeapon(weaponIndex))
			{
				weaponIndex--;
				if (weaponIndex < 0)
				{
					weaponIndex = weapons.Count - 1;
				}

				whileBreaker--;
				if (whileBreaker < 0)
				{
					Debug.LogError("While Breaker Activated", this);
					throw new System.Exception("While Breaker");
				}
			}
		}
		public bool SetWeapon(int index)
		{
			if (index < 0 || index >= weapons.Count) return false;
			if (!weaponUnlocks[index]) return false;
			currentWeapon.gameObject.SetActive(false);
			currentWeapon = weapons[index];
			currentWeapon.gameObject.SetActive(true);
			UI_WeaponIcon.SetWeaponIcon(index);
			return true;
		}

		public void SetWeaponLocked(int index)
		{
			weaponUnlocks[index] = false;
			if (index == weaponIndex)
			{
				NextWeapon();
			}
		}
		public void SetWeaponUnlocked(int index)
		{
			weaponUnlocks[index] = true;
			SetWeapon(index);

			if (index != 0) SetWeaponLocked(0);
		}

		public void LockAll()
		{
			for (int i = 0; i < weaponUnlocks.Count; i++)
			{
				weaponUnlocks[i] = false;
			}
			weaponUnlocks[0] = true;
			SetWeapon(0);
		}
	}
}