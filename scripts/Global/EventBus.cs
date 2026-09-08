using Godot;
using System;

public partial class EventBus : Node
{
	public static EventBus Instance { get; private set; }


	[Signal]
    public delegate void ButtonUpgradeEventHandler(Button button,int stage);
	[Signal]
    public delegate void ShopUnlockedEventHandler();
	

	public override void _Ready()
	{
		Instance = this;
	}
	public void TriggerButtonUpgrade(Button button, int stage){
		EmitSignal(SignalName.ButtonUpgrade,button,stage);
	}
	public void TriggerShopUnlocked(){
		EmitSignal(SignalName.ShopUnlocked);

	}
	
}
