using Godot;
using System;

public partial class ShopUI : Control
{
	
	

	public override void _Ready()
	{
		this.Visible = false;
		EventBus.Instance.ShopUnlocked += OnShopUnlocked;

		
	}

	private void OnShopUnlocked(){
		this.Visible = true;
	}

}
