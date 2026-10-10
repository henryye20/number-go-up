using Godot;
using System;
using Godot.Collections;
using System.Threading.Tasks.Dataflow;
public partial class Stats : Node
{
	private Dictionary<string,bool> ItemCapped = new()
	{
		{ "tree", false },
        { "pond", false },
		{ "cloud", false },
	};
	private Dictionary<string, int> ItemLevels = new()
    {
        { "tree", 0 },
        { "pond", -1 },
		{ "cloud", -1 },
    };
	private Dictionary<string, int> ItemCosts = new()
    {
        { "tree", 10 },
        { "pond", 30 },
		{ "cloud", 60 },
        
    };
	public static Stats Instance { get; private set; }
	public int number { get; private set; } = 0;
	
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
		GD.Print(ItemLevels[ButtonId]);
		
		if (ItemLevels[ButtonId] == -1)
		{
			GD.Print("FIRSTBUY");
			button.FirstBuy();
		}

		if (++ItemLevels[ButtonId] >= UIState.LevelCap)
		{
			ItemCapped[ButtonId] = true;
		}
		
		//placeholder cost function
		ItemCosts[ButtonId] += 10;

		button.UpdateButton();
		if (IsAllCapped())
		{
			UIState.stage++;
			UIUpgrade();
		}
		

	}
	
	public void UpdateNumber(int amt){
		number += amt;
		EmitSignal(SignalName.NumberChanged,number);
	}

	public bool TryBuy(string id)
	{
		
		if (ItemCosts[id] <= number && !ItemCapped[id])
		{
			
			UpdateNumber(-ItemCosts[id]);
			return true;
		}
		else
		{
			return false;

		}
	}

	public int GetLevel(string id)
	{
		if(ItemLevels.TryGetValue(id,out int level))
		{
			return level;
		}


		else
		{
			GD.Print($"{id} does not exist in ItemLevels");
			return -1;
		}
	}

	public int GetCost(string id)
	{
		if(ItemCosts.TryGetValue(id,out int cost))
		{
			return cost;
		}


		else
		{
			GD.Print($"{id} does not exist in ItemCosts");
			return -1;
		}
	}

	private void UIUpgrade()
	{
		// change shop texture to new texture
		GD.Print($"UI upgraded to {UIState.stage}" );

		return;
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
