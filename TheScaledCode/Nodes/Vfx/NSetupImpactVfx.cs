using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx;

public partial class NSetupImpactVfx : Node2D
{
	
	private Vector2 CloudShowDuration = new Vector2(0.4f, 0.65f);
	public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_setup_impact");

	[Export]
	private Sprite2D[] _cloudImages = [];

	private CancellationTokenSource? _cts;

	private Sprite2D? _currentCloud;

	public static NSetupImpactVfx? Create(Creature creature)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(creature);
		if (nCreature != null)
		{
			return Create(nCreature.VfxSpawnPosition);
		}
		return null;
	}

	public static NSetupImpactVfx? Create(Vector2 targetCenter)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		NSetupImpactVfx nSetupImpactVfx = PreloadManager.Cache.GetScene(scenePath).Instantiate<NSetupImpactVfx>(PackedScene.GenEditState.Disabled);
		nSetupImpactVfx.GlobalPosition = targetCenter;
		return nSetupImpactVfx;
	}

	public override void _Ready()
	{
		_currentCloud = null;
		_cloudImages = FindChildren("*", "Sprite2D", recursive: true, owned: false)
			.Cast<Sprite2D>()
			.ToArray();
		ModLog.Info(this,$"Found {_cloudImages.Length} cloud images");
	}

	public override void _ExitTree()
	{
		_cts?.Cancel();
	}

	/// <summary>
	/// Toggles a random cloud from the list of clouds on.
	/// Does not choose the previously enabled cloud.
	/// </summary>
	/// <returns></returns>
	public async Task ShowClouds(int numSetups)
	{
		

		if (_cloudImages.Length <= 1)
		{
			ModLog.Warning(this, "Not enough cloud images to show");
			await FinishSequence();
			return;
		}

		int numCloudsToShow = 3 + numSetups;
		var availableClouds = _cloudImages
		.Where(cloud => cloud != _currentCloud)
		.ToArray();
		for(int i = 0; i < numCloudsToShow; i++)
		{
			availableClouds = _cloudImages
				.Where(cloud => cloud != _currentCloud)
				.ToArray();

			_currentCloud?.Hide();
			_currentCloud = availableClouds[GD.Randi() % availableClouds.Length];
			_currentCloud.Show();
			await Cmd.CustomScaledWait(CloudShowDuration.X, CloudShowDuration.Y);
		}
		await FinishSequence();
		return;
	}


	public async Task FinishSequence()
	{
		_cts = new CancellationTokenSource();

		if (_currentCloud != null && IsInstanceValid(_currentCloud))
		{
			// Create a Tween to fade the alpha from current to 0 over 0.6 seconds
			var tween = CreateTween();
			tween.TweenProperty(_currentCloud, "modulate:a", 0.0f, 0.6f);

			using var _ = _cts.Token.Register(() => {
				if (tween.IsValid()) tween.Kill();
			});

			try
			{
				await ToSignal(tween, Tween.SignalName.Finished);
			}
			catch (Exception)
			{
			}
		}

		this.QueueFreeSafely();
	}

}
