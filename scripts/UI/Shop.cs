using Godot;
using System;

public partial class Shop : Control
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
