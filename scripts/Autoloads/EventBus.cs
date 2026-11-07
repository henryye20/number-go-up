using Godot;
using System;

public partial class EventBus : Node
{
	public static EventBus Instance { get; private set; }



	[Signal]
    public delegate void StageUpgradeEventHandler();
	
	[Signal]
	public delegate void First3DTransitionEventHandler();	

	[Signal]
	public delegate void PaintingEnteredEventHandler();	

	[Signal]
	public delegate void NumberChangedEventHandler(int newnum);

	[Signal]
	public delegate void SaveGameEventHandler(int slot=0);
	
	[Signal]
	public delegate void LoadGameEventHandler(int slot=0);
	
	[Signal]
	public delegate void GameLoadedEventHandler();
	[Signal]
	public delegate void UpgradePosChangedEventHandler(int idx,Vector2 pos);

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

	public void TriggerSaveGame(int slot = 0){
		EmitSignal(SignalName.SaveGame,slot);
	}
	public void TriggerLoadGame(int slot = 0){
		EmitSignal(SignalName.LoadGame,slot);
	}
	public void TriggerGameLoaded(){
		EmitSignal(SignalName.GameLoaded);
	}
	public void TriggerStageUpgrade(){
		EmitSignal(SignalName.StageUpgrade);
	}

	public void TriggerPaintingEntered(){
		EmitSignal(SignalName.PaintingEntered);
	}
	public void TriggerFirst3DTransition(){
		EmitSignal(SignalName.First3DTransition);
	}
	public void TriggerNumberChanged(int number){
		EmitSignal(SignalName.NumberChanged,number);
	}
	public void TriggerUpgradePosChanged(int idx,Vector2 pos){
		EmitSignal(SignalName.UpgradePosChanged, idx, pos);
	}
}
