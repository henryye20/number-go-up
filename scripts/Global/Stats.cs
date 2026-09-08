using Godot;
using System;

public partial class Stats : Node
{
	public static Stats Instance { get; private set; }
	public int number { get; private set; } = 0;
	public UIstage UIstate = UIstage.Basic;
	public enum UIstage{
		Basic,
		Color,
		Anim,
		Smooth,

	}

	[Signal]
    public delegate void NumberChangedEventHandler(int newnum);

	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public void UpdateNumber(int amt){
		number += amt;
		EmitSignal(SignalName.NumberChanged,number);
	}

	
}
