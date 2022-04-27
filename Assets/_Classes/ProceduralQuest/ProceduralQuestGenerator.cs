using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL.ProceduralQuestSystem
{

	// Idea1: have existing places and generate quests based on them
	// Idea2: make branching stories based on conditions, so the player
	//		  can kill a quest-relevant-person and the quest adapts to it or breaks

	public class QuestGenerator
	{

	}

	public class QuestContext
	{
		List<QuestContextPiece> pieces = new List<QuestContextPiece>();
	}

	public abstract class QuestContextPiece
	{
		public abstract string GetAsText();
	}
	public class Context_Person : QuestContextPiece
	{
		// examples: Luke, Louise
		public string personName;
		public override string GetAsText()
		{
			return personName;
		}
	}
	public class Context_Area : QuestContextPiece
	{
		// examples: Townhall, Cave
		public string areaName;
		public override string GetAsText()
		{
			return areaName;
		}
	}

}
