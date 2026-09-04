using Godot;
using System;

public partial class ShopButton : Button
{	
	private int cost;
	private int addamt;
	private int stage;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		stage = 0;
		cost = 15;
		Pressed += OnPressed;
		addamt = 2;
		
		
	}
	private void OnPressed(){
		
		if(Stats.Instance.number>= cost){
			stage++;
			
			Stats.Instance.UpdateNumber(-cost);
			cost *= (stage+1);
			EventBus.Instance.TriggerButtonUpgrade(stage);
			addamt += stage+1;
		}
		
		
	}
	
}
