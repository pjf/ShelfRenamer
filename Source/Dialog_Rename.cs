using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;

// Huge thanks to RimFridge, from which much of this code was lifted.

namespace ShelfRenamer
{
	public class Dialog_Rename : Window
    {
		private string inputText = "";
		private Building_Storage building;
		private readonly int maxLength = 28;

		public override Vector2 InitialSize => new Vector2(280f, 175f);

		public Dialog_Rename(Building_Storage building)
        {
			this.forcePause = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.building = building;
			this.inputText = building.Label;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;

            // If the user hits enter, we should accept their new name
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				SetName(this.inputText);
				Event.current.Use();
			}
           
            // Textfield to type in, populated with the old name.
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.inputText);

            // Don't accept names that are too long
			if (text.Length < this.maxLength)
			{
				this.inputText = text;
			}

            // OK Button
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, false, true))
			{
				SetName(this.inputText);
			}
		}

		void SetName(string newName)
		{
			if (newName.Length > 0)
            {
				ShelfRenamer.Instance.SetName(this.building, this.inputText);
                Find.WindowStack.TryRemove(this, true);
            }
		}
	}
}
