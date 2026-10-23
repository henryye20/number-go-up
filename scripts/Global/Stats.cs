using Godot;
using System;
using System.Threading.Tasks.Dataflow;
public partial class Stats : Node
{	
	//index of list is which painting; inner Lists contain each button's values
	private List<List<bool>> ItemCapped  = new List<List<bool>>();
	private List<List<int>> ItemLevels = new List<List<int>>();
	private List<List<int>> ItemCosts = new List<List<int>>();
	int level = 0;
	//index of list is which painting; Dictionary's contain ButtonID->Index conversions
	private ShopButtonIDX List<Dictionary<string,int>> = new List<Dictionary<string,int>>
	{
		new Dictionary<string, int> { { "tree", 0 }, { "pond", 1 }, { "cloud", 2 } },
    	
	}; 

	[Signal]
    public delegate void NumberChangedEventHandler(int newnum);

	public static Stats Instance { get; private set; }
	public int number { get; private set; } = 0;
	
	
	public struct StageObj
	{
		public int stage;
		public int LevelCap;
		public StageObj()
		{
			stage = 1;
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
			LevelCap = stage * 5;
		}
		
	}
		
	public StageObj UIState = new StageObj(0,5);
	

	 
	public override void _Ready()
	{
		Instance = this;
		
		EventBus.Instance.ButtonUpgrade+= OnButtonUpgrade;
		EventBus.Instance.ButtonCreated+= OnButtonCreated;
		
	}
	private void OnButtonUpgrade(ShopButton button)
	{
			
		int idx = ShopButtonIDX[button.ButtonID];

		
		if (ItemLevels[level][idx] < UIState.LevelCap)
		{
			if (ItemLevels[level][idx] == -1)
			{
				button.FirstBuy();
			}
			ItemLevels[level][idx]++;
			//placeholder cost function
			ItemCosts[level][idx] += 10;
		}
		if(ItemLevels[level][idx]==UIState.LevelCap)
		{	
			ItemCapped[level][idx] = true;
		}

		if (IsAllCapped())
		{
			GD.Print("ALL CAPPED");
			UIUpgrade();
		}
		

	}
	private void OnButtonAdded(ShopButton button)
	{
		int idx = ShopButtonIDX[level][button.ButtonID];
		ItemCapped[level][idx] = false;
		ItemLevels[level][idx] = -1;
		ItemCosts[level][idx] = button.InitialCost;
	}

	public void UpdateNumber(int amt){
		number += amt;
		EmitSignal(SignalName.NumberChanged,number);
	}

	public bool TryBuy(ShopButton item)
	{
		int idx = ShopButtonIDX[button.ButtonID];
		
		if(!ItemCosts.ContainsKey(ButtonID))
		{
			if(item.InitialCost<=number){
				ItemCosts[level][idx] = item.InitialCost;
				ItemCapped[level][idx] = false;
				ItemLevels[level][idx] = -1;
				
			}
			else{
				return false;
			}
			
		}
		
		if (ItemCosts[level][idx] <= number && !ItemCapped[level][idx])
		{
			UpdateNumber(-ItemCosts[level][idx]);
			return true;
		}
		else
		{
			return false;

		}
	}

	public int GetLevel(string ButtonID)
	{
		int idx = ShopButtonIDX[ButtonID];
		if(ItemLevels[level].TryGetValue(idx,out int level))
		{
			return level;
		}

		else
		{
			GD.Print($"{ButtonID} does not exist in ItemLevels");
			return -1;
		}
	}

	public int GetCost(string ButtonID)
	{
		if(ItemCosts[level].TryGetValue(ShopButtonIDX[ButtonID],out int cost))
		{
			return cost;
		}

		else
		{
			GD.Print($"{ButtonID} does not exist in ItemCosts");
			return -1;
		}
	}

	public bool IsCapped(string ButtonID)
	{
		return ItemCapped[level][ShopButtonIDX[ButtonID]];
	}
	
	private void UIUpgrade()
	{
		UIState.UpgradeStage();

		// change shop texture to new texture
		GD.Print($"UI upgraded to {UIState.stage}" );

		for(int i = 0; i<UIState.stage+1; i++)
		{
			ItemCapped[level][key]=false;
		}

		EventBus.Instance.TriggerStageUpgrade();


	}

	private bool IsAllCapped()
	{
		bool FullyCapped = true;
		foreach(bool capped in ItemCapped.Values)
		{
			FullyCapped = FullyCapped&&capped;
		}
		return FullyCapped;
	}
	
}
