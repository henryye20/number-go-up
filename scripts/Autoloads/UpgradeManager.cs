using Godot;
using System;
using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;

//Logic for upgrading shop upgrades and stage upgrade
public partial class UpgradeManager : Node
{	
	public static UpgradeManager Instance { get; private set; }
	public PaintingState CurrentPainting;
	private List<ShopButton> ShopButtonList = new List<ShopButton>{};
	
	//index of list is which painting; Dictionary's contain ButtonID->Index conversions
	public Dictionary<string,int> ShopButtonIDX  = new Dictionary<string,int>
	{
		 { "tree", 0 }, { "pond", 1 }, { "cloud", 2 },
	}; 
	
	
	[Signal]
	public delegate void ButtonAddedEventHandler(ShopButton button);
	[Signal]
	public delegate void PaintingAddedEventHandler(PaintingState button);
	
	
	public struct StageObj
	{
		public int stage;
		public int LevelCap;
		public StageObj()
		{
			stage = 0;
			LevelCap = 5;
		}
		public StageObj(int new_stage,int level_cap)
		{
			stage = new_stage;
			LevelCap = level_cap;
		}

		public void UpgradeStage()
		{
			
			stage++;
			LevelCap = LevelCap +  5;
			if(Instance.CurrentPainting!=null){
				Instance.CurrentPainting.stage= stage;
				Instance.CurrentPainting.levelcap = LevelCap;
			}
		}
		
	}
		
	public StageObj UIState = new StageObj(0,5);
	

	 
	public override void _Ready()
	{
		Instance = this;
		ButtonAdded+= OnButtonAdded;
		PaintingAdded += OnPaintingAdded;
		EventBus.Instance.StageUpgrade += OnStageUpgrade;
		EventBus.Instance.GameLoaded +=  OnGameLoaded;
	}
	public void OnGameLoaded(){
		
		if(CurrentPainting!=null){
			int level = CurrentPainting.level;
			
			CurrentPainting.stage = UIState.stage;
			CurrentPainting.levelcap = UIState.LevelCap;
			CurrentPainting.ItemCapped= GameState.Instance.ItemCapped[level];
			CurrentPainting.ItemLevels=GameState.Instance.ItemLevels[level];
			CurrentPainting.ItemCosts=GameState.Instance.ItemCosts[level];
		}
		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}
		GD.Print("UpgradeManager: Current painting and Buttons updated to LOAD UI");
	}
	private void UpgradeButton(ShopButton button)
	{
			
		int idx = ShopButtonIDX[button.ButtonID];

		
		if (CurrentPainting.ItemLevels[idx] < UIState.LevelCap)
		{
			if (CurrentPainting.ItemLevels[idx] == -1)
			{
				button.FirstBuy();
			}
			CurrentPainting.SetItemLevels(idx,CurrentPainting.ItemLevels[idx]+1);
			//placeholder cost function
			CurrentPainting.SetItemCosts(idx,CurrentPainting.ItemCosts[idx]+10);
		}
		if(CurrentPainting.ItemLevels[idx]==UIState.LevelCap)
		{	
			CurrentPainting.SetItemCapped(idx,true);
		}

		UpdateButtonUI(button);

	}
	private void OnStageUpgrade(){
		UIUpgrade();
	}
	private void OnButtonAdded(ShopButton button)
	{
		int idx = ShopButtonIDX[button.ButtonID];
		
		ShopButtonList.Add(button);

		if(idx>=UIState.stage+1){
			button.Visible = false;
		}
		
	}

	private void OnPaintingAdded(PaintingState painting_state){
		CurrentPainting = painting_state;
		CurrentPainting.stage = UIState.stage;
		CurrentPainting.levelcap = UIState.LevelCap;
		ShopButton button;
		for(int idx = 0; idx<ShopButtonList.Count; idx++){
			button = ShopButtonList[idx];
			CurrentPainting.ItemCapped[idx] = false;
			CurrentPainting.ItemLevels[idx] = -1;
			CurrentPainting.ItemCosts[idx] = button.InitialCost;
			UpdateButtonUI(button);
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
			if (CurrentPainting.ItemCosts[idx] <= GameState.Instance.GetNumber() && !CurrentPainting.ItemCapped[idx])
			{
				UpdateNumber(-CurrentPainting.ItemCosts[idx]);
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
			CurrentPainting.ItemCapped[idx]=false;
		}
		UIState.UpgradeStage();

		foreach(ShopButton button in ShopButtonList){
			UpdateButtonUI(button);
		}


	}

	private void UpdateButtonUI(ShopButton button){
		int idx = ShopButtonIDX[button.ButtonID];
		if(idx<UIState.stage+1){
			button.Visible = true;
		}
		button.UpdateUI(CurrentPainting.ItemLevels[idx],CurrentPainting.ItemCosts[idx],CurrentPainting.ItemCapped[idx]);
	}

	public bool IsAllCapped()
	{
		for(int idx = 0; idx<UIState.stage+1; idx++)
		{
			GD.Print(idx+ ": " +CurrentPainting.ItemCapped[idx]);
			if(!CurrentPainting.ItemCapped[idx]){
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
