using Godot;
using System;
using Godot.Collections;
public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	//index of Array is which painting; inner Arrays contain each button's values
	public Array<Array<bool>> ItemCapped  = new Array<Array<bool>>{
		new Array<bool> { false,false,false,false,false,false },
		
	};
	public Array<Array<int>> ItemLevels = new Array<Array<int>>{
		new Array<int> {-1,-1,-1,-1,-1,-1}
	};
	public Array<Array<int>> ItemCosts = new Array<Array<int>>{
		new Array<int> {-1,-1,-1,-1,-1,-1}
	};
	Array<Array<Vector2>> UpgradePositions = new Array<Array<Vector2>>{
		new Array<Vector2> {Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero}
	};

	private int number = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}
	

	Godot.Collections.Dictionary DEFAULTS = new Godot.Collections.Dictionary{
		["number"]=0,
		["upgrade_positions"]=new Array<Array<Vector2>>{new Array<Vector2> {Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero}},
		["item_capped"] =new Array<Array<bool>>{new Array<bool> { false,false,false,false,false,false }},
		["item_levels"] =new Array<Array<int>>{new Array<int> {-1,-1,-1,-1,-1,-1}},
		["item_costs"] =new Array<Array<int>>{new Array<int> {-1,-1,-1,-1,-1,-1}},
	};

	public Godot.Collections.Dictionary ToDict(){
		var serializedPositions = new Array<Array<Godot.Collections.Dictionary>>();
		foreach (var innerArray in UpgradePositions)
		{
			var newInnerArray = new Array<Godot.Collections.Dictionary>();
			foreach (Vector2 vec in innerArray)
			{
				newInnerArray.Add(new Godot.Collections.Dictionary
				{
					["x"] = vec.X,
					["y"] = vec.Y
				});
			}
			serializedPositions.Add(newInnerArray);
		}
		return new Godot.Collections.Dictionary{
			["number"] =number,
			["upgrade_positions"]=serializedPositions,
			["item_capped"]=ItemCapped,
			["item_levels"]=ItemLevels,
			["item_costs"]=ItemCosts,
			
		};
	}
	public void FromDict(Godot.Collections.Dictionary data){
		
		if (data.TryGetValue("number", out var numVal))
			number = numVal.AsInt32();
		else
			number = DEFAULTS["number"].AsInt32();
		EventBus.Instance.TriggerNumberChanged(number);

		if (data.TryGetValue("upgrade_positions", out var posVal))
		{
			var rawPositions = posVal.As<Array<Array<Godot.Collections.Dictionary>>>();
			UpgradePositions = new Array<Array<Vector2>>();
			
			foreach (var innerArray in rawPositions)
			{
				var newInnerArray = new Array<Vector2>();
				foreach (var vecDict in innerArray)
				{
					float x = vecDict.TryGetValue("x", out var xVal) ? xVal.AsSingle() : 0f;
					float y = vecDict.TryGetValue("y", out var yVal) ? yVal.AsSingle() : 0f;
					newInnerArray.Add(new Vector2(x, y));
				}
				UpgradePositions.Add(newInnerArray);
			}
		}
		else
		{
			UpgradePositions = DEFAULTS["upgrade_positions"].As<Array<Array<Vector2>>>();
		}

		if (data.TryGetValue("item_capped", out var cappedVal))
			ItemCapped = cappedVal.As<Array<Array<bool>>>();
		else
			ItemCapped = DEFAULTS["item_capped"].As<Array<Array<bool>>>();

		if (data.TryGetValue("item_levels", out var levelsVal))
			ItemLevels = levelsVal.As<Array<Array<int>>>();
		else
			ItemLevels = DEFAULTS["item_levels"].As<Array<Array<int>>>();

		if (data.TryGetValue("item_costs", out var costsVal))
			ItemCosts = costsVal.As<Array<Array<int>>>();
		else
			ItemCosts = DEFAULTS["item_costs"].As<Array<Array<int>>>();
				
	}
	public void ChangeNumber(int amt){
		
		number =  Math.Max(0,number+amt);
		EventBus.Instance.TriggerNumberChanged(number);
		GD.Print("Number:" + number);
	}

	public int GetNumber(){
		return number;
	}

	
}
