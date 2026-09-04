using Godot;
using System;

public partial class EventBus : Node
{
	public static EventBus Instance { get; private set; }


	[Signal]
    public delegate void ButtonUpgradeEventHandler(int stage);

	public override void _Ready()
	{
		Instance = this;
	}
	public void TriggerButtonUpgrade(int stage){
		EmitSignal(SignalName.ButtonUpgrade,stage);
	}
	
}
