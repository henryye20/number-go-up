using Godot;
using System;

public partial class LoadButton : Button
{
	public override void _Ready()
	{
		Pressed+= OnPressed;
	}

	public void OnPressed(){
		EventBus.Instance.TriggerLoadGame();	
	}
}
