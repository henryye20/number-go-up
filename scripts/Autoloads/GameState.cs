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


	public Array<int> PaintingStages = new Array<int>{
		0
	};
	public Array<int> PaintingLevelCaps = new Array<int>{
		5
	};

	private int number = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}
	

	Godot.Collections.Dictionary DEFAULTS = new Godot.Collections.Dictionary{
		["number"]=0,
		["paiting_stages"]= new Array<int>{0},
		["paiting_levelcaps"]= new Array<int>{5},
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
			["painting_stages"]= PaintingStages,
			["painting_levelcaps"]= PaintingLevelCaps,
			["upgrade_positions"]=serializedPositions,
			["item_capped"]=ItemCapped,
			["item_levels"]=ItemLevels,
			["item_costs"]=ItemCosts,
			
		};
	}
	private static Array<int> ToIntArray(Variant v)
	{
		var result = new Array<int>();
		foreach (var item in v.AsGodotArray())
			result.Add(item.AsInt32());
		return result;
	}

	private static Array<Array<T>> ToNested<[MustBeVariant]T>(Variant v, Func<Variant, T> conv)
	{
		var result = new Array<Array<T>>();
		foreach (var inner in v.AsGodotArray())
		{
			var row = new Array<T>();
			foreach (var item in inner.AsGodotArray())
				row.Add(conv(item));
			result.Add(row);
		}
		return result;
	}
	public void FromDict(Godot.Collections.Dictionary data)
	{
		Variant Get(string key) => data.TryGetValue(key, out var v) ? v : DEFAULTS[key];

		number = Get("number").AsInt32();
		EventBus.Instance.TriggerNumberChanged(number);

		PaintingStages   = ToIntArray(Get("painting_stages"));
		PaintingLevelCaps = ToIntArray(Get("painting_levelcaps"));

		UpgradePositions = new Array<Array<Vector2>>();
		foreach (var inner in Get("upgrade_positions").AsGodotArray())
		{
			var row = new Array<Vector2>();
			foreach (var item in inner.AsGodotArray())
			{
				var d = item.AsGodotDictionary();
				row.Add(new Vector2(
					d.TryGetValue("x", out var x) ? x.AsSingle() : 0f,
					d.TryGetValue("y", out var y) ? y.AsSingle() : 0f));
			}
			UpgradePositions.Add(row);
		}

		ItemCapped = ToNested(Get("item_capped"), v => v.AsBool());
		ItemLevels = ToNested(Get("item_levels"), v => v.AsInt32());
		ItemCosts  = ToNested(Get("item_costs"),  v => v.AsInt32());
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
