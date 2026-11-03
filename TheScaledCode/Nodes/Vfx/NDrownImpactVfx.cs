using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx;

public partial class NDrownImpactVfx : Node2D
{
	public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_drown_impact");

	[Export]
	private GpuParticles2D[] _impactParticles = [];

	private CancellationTokenSource? _cts;

	public static NDrownImpactVfx? Create(Creature creature)
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

	public static NDrownImpactVfx? Create(Vector2 targetCenter)
	{
		if (TestMode.IsOn)
		{
			return null;
		}
		NDrownImpactVfx nDrownImpactVfx = PreloadManager.Cache.GetScene(scenePath).Instantiate<NDrownImpactVfx>(PackedScene.GenEditState.Disabled);
		nDrownImpactVfx.GlobalPosition = targetCenter;
		return nDrownImpactVfx;
	}

	public override void _Ready()
	{
		_impactParticles = FindChildren("*", "GPUParticles2D", recursive: true, owned: false)
			.Cast<GpuParticles2D>()
			.ToArray();
		ModLog.Info(this,$"Found {_impactParticles.Length} impact particles");
		TaskHelper.RunSafely(PlaySequence());
	}

	public override void _ExitTree()
	{
		_cts?.Cancel();
	}

	private async Task PlaySequence()
	{
		_cts = new CancellationTokenSource();
		for (int i = 0; i < _impactParticles.Length; i++)
		{
			_impactParticles[i].Restart();
		}
		await Cmd.Wait(2f, _cts.Token);
		this.QueueFreeSafely();
	}
}
