using Godot;
using System;

public partial class Stats : Node
{
	public static Stats Instance { get; private set; }
	public int number { get; set; } = 0;
	public int UIstate = 0;

	[Signal]
    public delegate void NumberChangedEventHandler(int newnum);
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public void UpdateNumber(int amt){
		number += amt;
		EmitSignal(SignalName.NumberChanged,number);
	}

	
}
