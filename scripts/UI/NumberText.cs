using Godot;
using System;

public partial class NumberText : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Stats.Instance.NumberChanged += OnNumberChanged;

	}

	private void OnNumberChanged(int newnum){
		this.Text = newnum.ToString();
	}


}
