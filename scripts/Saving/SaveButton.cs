using Godot;
using System;

public partial class SaveButton : Button
{
	public override void _Ready()
	{
		Pressed+= OnPressed;
	}

	public void OnPressed(){
		GD.Print("SaveButton: pressed");
		EventBus.Instance.TriggerSaveGame();

	}

	
}
