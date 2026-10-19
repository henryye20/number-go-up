using Godot;
using System;
using Godot.Collections;
using System.Threading.Tasks.Dataflow;
public partial class Stats : Node
{
	private Godot.Collections.Dictionary<string,bool> ItemCapped = new()
	{
		{ "tree", false },
        { "pond", false },
		
	};
	private Godot.Collections.Dictionary<string, int> ItemLevels = new()
    {
        { "tree", 0 },
        { "pond", -1 },
		
    };
	private Godot.Collections.Dictionary<string, int> ItemCosts = new()
    {
        { "tree", 10 },
        { "pond", 30 },
		
        
    };
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

	[Signal]
    public delegate void NumberChangedEventHandler(int newnum);
	

	
	public override void _Ready()
	{
		Instance = this;
		
		EventBus.Instance.ButtonUpgrade+= OnButtonUpgrade;
		
	}
	private void OnButtonUpgrade(ShopButton button)
	{
		
		string ButtonId = button.ButtonId;

		
		if (ItemLevels[ButtonId] < UIState.LevelCap)
		{
			if (ItemLevels[ButtonId] == -1)
			{
				button.FirstBuy();
			}
			ItemLevels[ButtonId]++;
			//placeholder cost function
			ItemCosts[ButtonId] += 10;
		}
		if(ItemLevels[ButtonId]==UIState.LevelCap)
		{
			
			ItemCapped[ButtonId] = true;
		}
		

		if (IsAllCapped())
		{
			GD.Print("ALL CAPPED");
			UIUpgrade();
		}
		

	}
	
	public void UpdateNumber(int amt){
		number += amt;
		EmitSignal(SignalName.NumberChanged,number);
	}

	public bool TryBuy(ShopButton item)
	{
		string ButtonId = item.ButtonId;
		
		if(!ItemCosts.ContainsKey(ButtonId))
		{
			if(item.InitialCost<=number){
				ItemCosts[ButtonId] = item.InitialCost;
				ItemCapped[ButtonId] = false;
				ItemLevels[ButtonId] = -1;
			}
			else{
				return false;
			}
			
		}
		
		if (ItemCosts[ButtonId] <= number && !ItemCapped[ButtonId])
		{
			UpdateNumber(-ItemCosts[ButtonId]);
			return true;
		}
		else
		{
			return false;

		}
	}

	public int GetLevel(string ButtonId)
	{
		if(ItemLevels.TryGetValue(ButtonId,out int level))
		{
			return level;
		}

		else
		{
			GD.Print($"{ButtonId} does not exist in ItemLevels");
			return -1;
		}
	}

	public int GetCost(string ButtonId)
	{
		if(ItemCosts.TryGetValue(ButtonId,out int cost))
		{
			return cost;
		}

		else
		{
			GD.Print($"{ButtonId} does not exist in ItemCosts");
			return -1;
		}
	}

	public bool IsCapped(string ButtonId)
	{
		return ItemCapped[ButtonId];
	}

	private void UIUpgrade()
	{
		UIState.UpgradeStage();

		// change shop texture to new texture
		GD.Print($"UI upgraded to {UIState.stage}" );

		foreach(string key in ItemCapped.Keys)
		{
			ItemCapped[key]=false;
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
