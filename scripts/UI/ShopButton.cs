using Godot;
using System;
using Godot.Collections;
public partial class ShopButton : Button
{	
	[Export] public string ButtonId { get; set; }
	[Export] public PackedScene spawnable;


	private TextureRect lock_img;
	private Label cost_text;
	private Label level_text;
	
	public override void _Ready()
	{
		cost_text = GetNode<Label>("Cost");
		level_text = GetNode<Label>("Level");
		lock_img = GetNode<TextureRect>("Lock");
		Pressed += OnPressed;
		
		EventBus.Instance.StageUpgrade += OnStageUpgrade;
		UpdateUI();

	}
	private void OnPressed(){
		
		if(Stats.Instance.TryBuy(ButtonId)){
			EventBus.Instance.TriggerButtonUpgrade(this);
			UpdateUI();
		}
		
	}

	private void UpdateUI()
	{
		cost_text.Text = Stats.Instance.GetCost(ButtonId).ToString();

		int level = Stats.Instance.GetLevel(ButtonId);
		
		if(Stats.Instance.IsCapped(ButtonId)){
			
			
		}
		else
		{
			//not unlocked
			if (level == -1)
			{
				level_text.Text = "";
				lock_img.Visible = true;
			}

			//level up
			else
			{
				if (level == 0)
				{
					level_text.Text = "";
				}
				else
				{
					level_text.Text = level.ToString();
				}
				lock_img.Visible = false;
			}

		}
		
		

		
	}
	
	//called in Stats OnButtonUpgrade
	public void FirstBuy()
	{
		Node spawnedNode = spawnable.Instantiate();
		GetTree().CurrentScene.AddChild(spawnedNode);
		
	}

	public void OnStageUpgrade()
	{
		UpdateUI();

	}



	
}
