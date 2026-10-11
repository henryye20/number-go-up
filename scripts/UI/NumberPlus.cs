using Godot;
using System;

public partial class NumberPlus : Control
{
	Label _NumberLabel;
	public override void _Ready()
	{
		_NumberLabel = GetNode<Label>("Number");
		Visible = false;
		
	}
	public void data(int amt)
	{
		_NumberLabel.Text = amt.ToString();
	}
	public void Run()
	{
		Visible = true;
		Tween tween = CreateTween();
		tween.TweenProperty(this,"modulate:a",0f,1.0f);
		tween.TweenCallback(Callable.From(QueueFree));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
}
