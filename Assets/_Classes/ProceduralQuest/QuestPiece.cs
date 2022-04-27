namespace JL.ProceduralQuestSystem
{
	// can be:
	// Requirement for the Player -> get Item, talk to Person
	// Consequences when the requirement is fulfilled or broken
	// Ending
	public class QuestPiece
	{
		string text;
		// wants specific context infos

		// requires: 2 persons with (positive) Relationship, 1 location, person1 located in village
		// generates: 1 quest-item

		// My Brother Alois needs three Swords to defend our Village. Last time I've seen him was by the tower to the north.
		// My <relationship> <name> needs <amount> <item> to defend our Village. Last time I've seen him was by the <location> to the <compass direction>.
	}

}

/*
Village
	Person
		Gender
		Name
		Position
		Home
		Relationships
		Job / Profession

	Monster
		Name
		Position
		Category
		Effects on Humans (petrify, hex, wound, ..)



 */