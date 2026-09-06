using Godot;
using System;

public partial class ShopButton : Button
{	
	private int cost;
	private int addamt;
	private int stage;
	private int level;
	// Called when the node enters the scene tree for the first time.
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
			if(level%5==0){
				stage++;
			}
			Stats.Instance.UpdateNumber(-cost);
			cost *= (stage+1);
			EventBus.Instance.TriggerButtonUpgrade(stage);
			addamt += stage+1;
		}
		
		
	}
	
}
