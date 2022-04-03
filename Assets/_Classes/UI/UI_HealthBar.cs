using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace JL
{
	public class UI_HealthBar : MonoBehaviour
	{
		UIDocument uiDocument;

		VisualElement healthBar, damage;
		float lastHealth = 0;

		public float damageVisDuration = 0.5f;

		float lastDamageTime;

		void Awake()
		{
			uiDocument = GetComponentInParent<UIDocument>();
			VisualElement healthRoot = uiDocument.rootVisualElement.Q("Health");
			healthBar = healthRoot.Q("HealthBar");
			damage = healthRoot.Q("Damage");
		}

		void Update()
		{
			float delta = (lastDamageTime + damageVisDuration) - Time.time;
			delta /= damageVisDuration;

			damage.style.opacity = Mathf.Clamp01(delta);
		}

		public void HealthChanged(float normalized)
		{
			normalized = Mathf.Clamp01(normalized);
			if (lastHealth > normalized)
			{
				lastDamageTime = Time.time;
			}
			lastHealth = normalized;
			healthBar.style.width = new Length(normalized * 100, LengthUnit.Percent);
		}
	}
}
