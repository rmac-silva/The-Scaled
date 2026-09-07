using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheScaled.TheScaledCode.Enchantments;

namespace TheScaled.TheScaledCode.Relics.Ancient;

[Pool(typeof(EventRelicPool))]
public class SaltBandages : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromEnchantment<Adhesive>().First(),HoverTipFactory.FromKeyword(CardKeyword.Retain)];

    public override async Task AfterObtained()
	{
        EnchantmentModel adhesive = ModelDb.Enchantment<Adhesive>();

		List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where(adhesive.CanEnchant).ToList();
		CardModel? cardModel = (await CardSelectCmd.FromDeckForEnchantment(prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), cards: list.UnstableShuffle(base.Owner.RunState.Rng.Niche).ToList(), enchantment: adhesive, amount: 1)).FirstOrDefault();
	
        if(cardModel != null)
        {
            CardCmd.Enchant<Adhesive>(cardModel,1);

            NCardEnchantVfx? nCardEnchantVfx = NCardEnchantVfx.Create(cardModel);
			if (nCardEnchantVfx != null)
			{
				NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
			}
        }
    }
}