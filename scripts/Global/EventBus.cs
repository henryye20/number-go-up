using Godot;
using System;

public partial class EventBus : Node
{
	public static EventBus Instance { get; private set; }


	[Signal]
    public delegate void ButtonUpgradeEventHandler(ShopButton button);
	[Signal]
    public delegate void StageUpgradeEventHandler();
	public override void _EnterTree()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree(); // Prevent duplicate instances
        }
    }
	// public override void _Ready()
	// {
	// 	Instance = this;
	// }
	public void TriggerButtonUpgrade(ShopButton button){
		EmitSignal(SignalName.ButtonUpgrade,button);
	}

	public void TriggerStageUpgrade(){
		EmitSignal(SignalName.StageUpgrade);
	}
	
	
}
