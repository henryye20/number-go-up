using Godot;
using System;
using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;

//Logic for upgrading shop upgrades and stage upgrade
public partial class UpgradeManager : Node
{	
	public static UpgradeManager Instance { get; private set; }
 	private List<ShopButton> ShopButtonList = new List<ShopButton>{};
	public int level;
	
	//index of list is which painting; Dictionary's contain ButtonID->Index conversions
	public Dictionary<string,int> ShopButtonIDX  = new Dictionary<string,int>
	{
		{ "tree", 0 }, { "pond", 1 }, { "cloud", 2 },
	}; 
	
	
	[Signal]
	public delegate void ButtonAddedEventHandler(ShopButton button);

		
	public Constants.StageObj UIState = new Constants.StageObj(0,5);
	

	 
	public override void _Ready()
	{
		Instance = this;
		ButtonAdded+= OnButtonAdded;
		
		EventBus.Instance.StageUpgrade += OnStageUpgrade;
		EventBus.Instance.GameLoaded +=  OnGameLoaded;
	}
	public void OnGameLoaded(){
		
		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}
		GD.Print("UpgradeManager: Current painting and Buttons updated to LOAD UI");
	}
	private void UpgradeButton(ShopButton button)
	{
			
		int idx = ShopButtonIDX[button.ButtonID];

		
		if (GameState.Instance.ItemLevels[level][idx] < UIState.LevelCap)
		{
			if (GameState.Instance.ItemLevels[level][idx] == -1)
			{
				button.FirstBuy();
			}
			GameState.Instance.ItemLevels[level][idx]++;
			//placeholder cost function
			GameState.Instance.ItemCosts[level][idx] =GameState.Instance.ItemCosts[level][idx]+10;
		}
		if(GameState.Instance.ItemLevels[level][idx]==UIState.LevelCap)
		{	
			GameState.Instance.ItemCapped[level][idx] =true;
		}

		UpdateButtonUI(button);

	}
	private void OnStageUpgrade(){
		UIUpgrade();
	}
	private void InitButtonState(ShopButton button)
	{
		if (GameState.Instance == null) return;
		int idx = ShopButtonIDX[button.ButtonID];
		GameState.Instance.ItemCapped[level][idx] = false;
		GameState.Instance.ItemLevels[level][idx] = -1;
		GameState.Instance.ItemCosts[level][idx] = button.InitialCost;
		UpdateButtonUI(button);
	}
	private void OnButtonAdded(ShopButton button)
	{
		int idx = ShopButtonIDX[button.ButtonID];
		
		ShopButtonList.Add(button);

		if(idx>=UIState.stage+1){
			button.Visible = false;
		}
		if(GameState.Instance!=null){
			InitButtonState(button);
		}
		
		
	}

	public void UpdateNumber(int amt){
		GameState.Instance.ChangeNumber(amt);
	}

	public void TryBuy(ShopButton button)
	{
		int idx = ShopButtonIDX[button.ButtonID];
		
		if(idx<UIState.stage+1)
		{
			if (GameState.Instance.ItemCosts[level][idx] <= GameState.Instance.GetNumber() && !GameState.Instance.ItemCapped[level][idx])
			{
				UpdateNumber(-GameState.Instance.ItemCosts[level][idx]);
				UpgradeButton(button);
			}
		}
		
		
		
	}

	
	private void UIUpgrade()
	{

		// change shop texture to new texture
		GD.Print($"UI upgraded to {UIState.stage}" );

		for(int idx = 0; idx<UIState.stage+1; idx++)
		{
			GameState.Instance.ItemCapped[level][idx] =false;
		}
		UIState.UpgradeStage();

		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}
		
		GameState.Instance.UIStates[level]= UIState;
		
	

	}

	private void UpdateButtonUI(ShopButton button){
		int idx = ShopButtonIDX[button.ButtonID];
		if(idx<UIState.stage+1){
			button.Visible = true;
		}
		button.UpdateUI(GameState.Instance.ItemLevels[level][idx],GameState.Instance.ItemCosts[level][idx],GameState.Instance.ItemCapped[level][idx]);
	}

	public bool IsAllCapped()
	{
		for(int idx = 0; idx<UIState.stage+1; idx++)
		{
			GD.Print(idx+ ": " +GameState.Instance.ItemCapped[level][idx]);
			if(!GameState.Instance.ItemCapped[level][idx]){
				return false;
			}
		}
		return true;
	}
	public bool IsUIMaxed(){
		GD.Print("stage: " +UIState.stage);
		return UIState.stage == 2 && IsAllCapped();
	}
}


/*
HAPPY ZONE:
1231231231203197410417341
4
2
34
2
342
34
234
2
342
34
23
4
23
42
34
234
23
42
34
2
342
34
234
2
34

23
42




*/