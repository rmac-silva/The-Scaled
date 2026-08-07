using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

public abstract class TemporaryThornsPower : CustomPowerModel, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance;

	public override PowerType Type
	{
		get
		{
			if (!IsPositive)
			{
				return PowerType.Debuff;
			}
			return PowerType.Buff;
		}
	}

	public override PowerStackType StackType => PowerStackType.Counter;

	/// <summary>
	/// The canonical model that applies this power. For example, <see cref="T:MegaCrit.Sts2.Core.Models.Potions.FlexPotion" />.
	/// </summary>
	public abstract AbstractModel OriginModel { get; }

	public PowerModel InternallyAppliedPower => ModelDb.Power<ThornsPower>();

	/// <summary>
	/// If this power is supposed to apply negative strength, make this false
	/// </summary>
	protected virtual bool IsPositive => true;

	/// <summary>
	/// Shorthand indicating the sign of the amount to apply
	/// </summary>
	private int Sign
	{
		get
		{
			if (!IsPositive)
			{
				return -1;
			}
			return 1;
		}
	}

	public override LocString Title
	{
		get
		{
			AbstractModel originModel = OriginModel;
			if (!(originModel is CardModel cardModel))
			{
				if (!(originModel is PotionModel potionModel))
				{
					if (originModel is RelicModel relicModel)
					{
						return relicModel.Title;
					}
					throw new InvalidOperationException();
				}
				return potionModel.Title;
			}
			return cardModel.TitleLocString;
		}
	}

	public override LocString Description => new LocString("powers", IsPositive ? "THESCALED-TEMPORARY_THORNS.description" : "THESCALED-TEMPORARY_THORNS_DOWN.description");

	protected override string SmartDescriptionLocKey
	{
		get
		{
			if (!IsPositive)
			{
				return "TEMPORARY_STRENGTH_DOWN.smartDescription";
			}
			return "TEMPORARY_STRENGTH_POWER.smartDescription";
		}
	}

	protected override IEnumerable<IHoverTip> ExtraHoverTips
	{
		get
		{
			List<IHoverTip> list = new List<IHoverTip>();
			List<IHoverTip> list2 = list;
			AbstractModel originModel = OriginModel;
			IEnumerable<IHoverTip> collection;
			if (!(originModel is CardModel card))
			{
				if (!(originModel is PotionModel model))
				{
					if (!(originModel is RelicModel relic))
					{
						throw new InvalidOperationException();
					}
					collection = HoverTipFactory.FromRelic(relic);
				}
				else
				{
					collection = [HoverTipFactory.FromPotion(model)];
				}
			}
			else
			{
				collection =[HoverTipFactory.FromCard(card)];
			}
			list2.AddRange(collection);
			list.Add(HoverTipFactory.FromPower<ThornsPower>());
			return new List<IHoverTip>(list);
		}
	}

	public void IgnoreNextInstance()
	{
		_shouldIgnoreNextInstance = true;
	}

	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (_shouldIgnoreNextInstance)
		{
			_shouldIgnoreNextInstance = false;
		}
		else
		{
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, (decimal)Sign * amount, applier, cardSource, silent: true);
		}
	}

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (!(amount == (decimal)base.Amount) && power == this)
		{
			if (_shouldIgnoreNextInstance)
			{
				_shouldIgnoreNextInstance = false;
			}
			else
			{
				await PowerCmd.Apply<ThornsPower>(choiceContext, base.Owner, (decimal)Sign * amount, applier, cardSource, silent: true);
			}
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(base.Owner))
		{
			Flash();
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, -Sign * base.Amount, base.Owner, null);
		}
	}
}