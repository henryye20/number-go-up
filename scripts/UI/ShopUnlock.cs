using Godot;
using System;

public partial class ShopUnlock : Button
{
	
	private AnimationPlayer _shopanim;
	private VBoxContainer _upgrademenu;
	const int cost = 30;
	Tween tween;
	
	private float button_s = Constants.button_scale;
	private float button_t = Constants.button_time;
	
	public override void _Ready()
	{
		_shopanim = GetNode<AnimationPlayer>("ShopUnlockImg/ShockUnlockAnim");
		_upgrademenu = GetNode<VBoxContainer>("../UpgradeMenu");

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		Pressed += OnPressed;
	}

	

	private void OnMouseEntered(){
		_shopanim.Play("shop_imgs");
		
		tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector2.One * button_s, button_t)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);

	}
	private void OnMouseExited(){
		_shopanim.Pause();
		
		tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector2.One, button_t)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.In);

	}

	private void OnPressed(){
		if(Stats.Instance.number>= cost){
			this.Visible = false;
			_upgrademenu.Visible = true;
			Stats.Instance.UpdateNumber(-cost);
		}
		
		
	}
}
