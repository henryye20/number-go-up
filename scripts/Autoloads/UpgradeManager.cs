using Godot;
using System;
using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;

//Logic for upgrading shop upgrades and stage upgrade
public partial class UpgradeManager : Node
{	
	public static UpgradeManager Instance { get; private set; }
 	private List<ShopButton> ShopButtonList = new List<ShopButton>{};
	//index of list is which painting; Dictionary's contain ButtonID->Index conversions
	public Dictionary<string,int> ShopButtonIDX  = new Dictionary<string,int>
	{
		{ "tree", 0 }, { "pond", 1 }, { "cloud", 2 },
	}; 

	//Current level's stage and levelcap
	public int level=0;
	public int stage=0;
	public int level_cap=5;
	
	
	[Signal]
	public delegate void ButtonAddedEventHandler(ShopButton button);

		
	
	

	 
	public override void _Ready()
	{
		Instance = this;
		ButtonAdded+= OnButtonAdded;
		
		EventBus.Instance.StageUpgrade += OnStageUpgrade;
		EventBus.Instance.GameLoaded +=  OnGameLoaded;
		
	}
	public void OnGameLoaded(){
		stage = GameState.Instance.PaintingStages[level];
		level_cap = GameState.Instance.PaintingLevelCaps[level];
		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}
		
		
		GD.Print("UpgradeManager: Current painting and Buttons updated to LOAD UI");
	}
	private void UpgradeButton(ShopButton button)
	{
			
		int idx = ShopButtonIDX[button.ButtonID];

		
		if (GameState.Instance.ItemLevels[level][idx] < level_cap)
		{
			if (GameState.Instance.ItemLevels[level][idx] == -1)
			{
				button.FirstBuy();
			}
			GameState.Instance.ItemLevels[level][idx]++;
			//placeholder cost function
			GameState.Instance.ItemCosts[level][idx] =GameState.Instance.ItemCosts[level][idx]+10;
		}
		if(GameState.Instance.ItemLevels[level][idx]==level_cap)
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

		if(idx>=stage+1){
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
		
		if(idx<stage+1)
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
		GD.Print($"UI upgraded to {stage}" );

		for(int idx = 0; idx<stage+1; idx++)
		{
			GameState.Instance.ItemCapped[level][idx] =false;
		}
		stage++;
		level_cap+=5;
		GameState.Instance.PaintingStages[level]= stage;
		GameState.Instance.PaintingLevelCaps[level] = level_cap;
		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}
		
		
	}

	private void UpdateButtonUI(ShopButton button){
		int idx = ShopButtonIDX[button.ButtonID];
		if(idx<=stage){
			button.Visible = true;
		}
		else
		{
			button.Visible = false;
		}
		button.UpdateUI(GameState.Instance.ItemLevels[level][idx],GameState.Instance.ItemCosts[level][idx],GameState.Instance.ItemCapped[level][idx]);
	}

	public bool IsAllCapped()
	{
		for(int idx = 0; idx<stage+1; idx++)
		{
			GD.Print(idx+ ": " +GameState.Instance.ItemCapped[level][idx]);
			if(!GameState.Instance.ItemCapped[level][idx]){
				return false;
			}
		}
		return true;
	}
	public bool IsUIMaxed(){
		GD.Print("stage: " +stage);
		return stage == 2 && IsAllCapped();
	}
}
