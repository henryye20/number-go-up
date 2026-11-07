using Godot;
using System;

public partial class ShopUIManager : Control
{
	private Button SUButton;
	private Button DevButton;
	public override void _Ready()
	{
		SUButton = GetNode<Button>("StageUpgradeButton");
		SUButton.Pressed += OnSUButtonPressed;
		DevButton = GetNode<Button>("DevStageUpgrade");
		DevButton.Pressed += OnDevButtonPressed;
	}

	private void OnSUButtonPressed(){
		GD.Print("CHECKING MAXED");
		if(UpgradeManager.Instance.IsUIMaxed()){
			EventBus.Instance.TriggerFirst3DTransition();
			return;			
		}
		if(UpgradeManager.Instance.IsAllCapped()){
			
			
			EventBus.Instance.TriggerStageUpgrade();
			
			
		}
		
	}
	private void OnDevButtonPressed(){
		EventBus.Instance.TriggerFirst3DTransition();
	}
	
}
