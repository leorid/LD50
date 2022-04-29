using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	[CreateAssetMenu]
	public class CodeGenSaveData : ScriptableObject
	{
		public List<CodeGenTemplate> templates = new();
	}
}
