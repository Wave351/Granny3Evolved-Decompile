using System;
using System.Collections.Generic;

[Serializable]
public partial class PuzzleForbiddenCombo
{
	public string itemName;
	public string forbiddenItem;
	public List<string> forbiddenPuzzles;
}
