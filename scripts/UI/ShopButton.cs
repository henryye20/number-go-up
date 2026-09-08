using Godot;
using System;

public partial class ShopButton : Button
{	
	private int cost;
	private int addamt;
	private int stage;
	private int level;
	

	public override void _Ready()
	{
		stage = 0;
		level = 0;
		cost = 15;
		Pressed += OnPressed;
		addamt = 2;
		
		
	}
	private void OnPressed(){
		
		if(Stats.Instance.number>= cost){
			level++;
			if(level%Constants.levels_per_stage==0){
				stage++;
			}
			Stats.Instance.UpdateNumber(-cost);
			cost *= stage+1;
			EventBus.Instance.TriggerButtonUpgrade(this,stage);
			addamt += stage+1;
		}
		
		
	}
	
}
