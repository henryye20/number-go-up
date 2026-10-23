using Godot;
using System;

public partial class ButtonAnimation : Button
{
	
	int stage = 0;
	// Called when the node enters the scene tree for the first time.
	private AnimationPlayer _buttonanim;
	Tween tween;
	
	private float button_s = Constants.button_scale;
	private float button_t = Constants.button_time;
	
	bool isSpin = false;
	private float r_rad;
	private float r_time = 0.045f;


	public override void _Ready()
	{
		
		_buttonanim = GetNode<AnimationPlayer>("ButtonImg/ButtonAnim");
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		Pressed += OnPressed;
	}

	
	
	private void PressAnim(float button_r, bool spin){
		
		if(spin){
			float rot = 360f*2.5f;
			isSpin = true;
			if(GD.RandRange(0,1)==1){
				rot = rot*-1f;
			}
			tween = GetTree().CreateTween().SetParallel(true);
			
			tween.TweenProperty(this, "scale", Vector2.One *(button_s+.4f), 0.1f)
				.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.In);

			tween.TweenProperty(this, "rotation", Mathf.DegToRad(rot), .8f)
				.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);

			
			tween.TweenProperty(this, "scale", Vector2.One * button_s, 0.1f)
				.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out)
				.SetDelay(0.5f);
			tween.Chain().TweenCallback(Callable.From(() => {
				Rotation = 0f;
				isSpin = false;			
			}));
			return;
		}

		
		
		r_rad = Mathf.DegToRad(button_r);
	
		tween = GetTree().CreateTween();
		
		tween.TweenProperty(this, "scale", Vector2.One *(0.975f), button_t+.03f)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);

		tween.Parallel().TweenProperty(this, "rotation", r_rad, r_time);
		tween.TweenProperty(this, "rotation", -r_rad, r_time);
		tween.TweenProperty(this, "rotation", 0.0, r_time);

		tween.Parallel().TweenProperty(this, "scale", Vector2.One *button_s, button_t+.03f)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.In).SetDelay(button_t);

	}

	

	private void OnMouseEntered(){
		_buttonanim.Play("button_imgs");
		
		if(isSpin) return;
		tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector2.One * button_s, button_t)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);

	}
	private void OnMouseExited(){
		_buttonanim.Pause();
		
		if(isSpin) return;
		tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector2.One, button_t)
		.SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.In);

	}
	private void OnPressed(){

		if(isSpin) return;
		bool spin= false;
		
		float button_r = (float)GD.RandRange(3.0, 8.0);
		
		
		if(GD.RandRange(1,500)==1){
			spin= true;
		}
		
		PressAnim(button_r,spin);

	}
	private void OnButtonUpgrade(Button button,int addamt){
		//TODO: Change Img and other animation stuff 
		return;
	}
}
