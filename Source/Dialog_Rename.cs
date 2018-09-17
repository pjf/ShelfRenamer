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
			bool enter = false;

            // If the user hits enter, we should accept their new name
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				enter = true;
				Event.current.Use();
			}
           
            // Textfield to type in, populated with the old name.
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.inputText);

            // Don't accept names that are too long
			if (text.Length < this.maxLength)
			{
				this.inputText = text;
			}

            // If the user hits enter or OK, then stash the label.
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, false, true) || enter)
			{
				if (this.inputText.Length > 0)
				{
					// Label is read-only, so we need to be a little more tricksy by getting the backing variable and assigning to that.
					var label = typeof(Building_Storage).GetField("<Label>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
					label.SetValue(building, this.inputText);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}
	}
}
