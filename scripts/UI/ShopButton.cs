using Godot;
using System;
using Godot.Collections;
public partial class ShopButton : Button
{	
	[Export] public string ButtonId { get; set; } = "button";
	[Export] public PackedScene spawnable;


	private Label cost_text;
	private Label level_text;
	
	[Export] public Array<Texture2D> num_imgs { get; set; } = new();
	public override void _Ready()
	{
		cost_text = GetNode<Label>("Cost");
		level_text = GetNode<Label>("Level");
	
		Pressed += OnPressed;
		UpdateUI();

	}
	private void OnPressed(){
		
		if(Stats.Instance.TryBuy(ButtonId)){
			EventBus.Instance.TriggerButtonUpgrade(this);
			UpdateUI();
		}
		
	}

	public void UpdateButton()
	{
		UpdateUI();
	}
	private void UpdateUI()
	{
		cost_text.Text = Stats.Instance.GetCost(ButtonId).ToString();
		level_text.Text = Stats.Instance.GetLevel(ButtonId).ToString();
		
	}
	private void FirstBuy()
	{
		
	}



	
}
