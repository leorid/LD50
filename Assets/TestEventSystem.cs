using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class TestEventSystem : MonoBehaviour
	{
		void Start()
		{
			Debug.Log("TestEventSystem starting");

			EventHub.Global.Subscribe<int>(this, "GetNr", GetNr);
			EventHub.Global.Execute<int>(this, "GetNr", 9);
			EventHub.Global.UnSubscribe<int>(this, "GetNr", GetNr);
			EventHub.Global.Execute<int>(this, "GetNr", 3);
		
			Debug.Log("TestEventSystem finished");
		}

		void GetNr(int nr)
		{
			Debug.Log("GetNr(int nr) = " + nr);
		}
	}
}
