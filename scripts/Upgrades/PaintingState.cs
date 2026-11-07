using Godot;
using System;
using Godot.Collections;

//Keeps track of all painting variables for transitioning between 2D and 3D, and GameState(save states)

public partial class PaintingState : Control
{
	
	public override void _Ready()
	{
		UpgradeManager.Instance.EmitSignal(UpgradeManager.SignalName.PaintingAdded,this);
		EventBus.Instance.UpgradePosChanged += OnUpgradePosChanged;

	}
	//index of Array is which painting; inner Arrays contain each button's values
	public Array<bool> ItemCapped  = new Array<bool>{
		false,false,false,false,false,false 
	};
	public Array<int> ItemLevels = new Array<int>{
		-1,-1,-1,-1,-1,-1
	};
	public Array<int> ItemCosts = new Array<int>{
		-1,-1,-1,-1,-1,-1
	};
	Array<Vector2> UpgradePositions = new Array<Vector2>{
		Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero
	};


	public int level = 0;
	public int stage = 0;
	public int levelcap =5;

	public void SetItemCapped(int index, bool val){
		ItemCapped[index] = val;
		GameState.Instance.ItemCapped[level][index] = val;
		
	}
	public void SetItemLevels(int index, int val){
		ItemLevels[index] = val;
		GameState.Instance.ItemLevels[level][index]= val;
	}
	public void SetItemCosts(int index, int val){
		ItemCosts[index] = val;
		GameState.Instance.ItemCosts[level][index] = val;
	}
	public void OnUpgradePosChanged(int index, Vector2 val){
		UpgradePositions[index] = val;
		GD.Print("Upgrade " + index + ": " +UpgradePositions[index]);
	}	
	


	
}
