using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace DynamicField
{
    public enum SandwichOptions
    {
        BLT, BlackForestHam, BuffaloChicken
    };
    public enum LengthOptions { SixInch, FootLong };

    public enum CheeseOptions { American, MontereyCheddar, Pepperjack };

    [Serializable]
    public class SandwichOrder
    {
        public SandwichOptions? Sandwich;
        public LengthOptions? Length;
        public CheeseOptions? Cheese;
        [Optional]
        [Template(TemplateUsage.NoPreference, "None")]
        public string Specials;

        public static IForm<SandwichOrder> BuildForm()
        {
            return new FormBuilder<SandwichOrder>()
                .Message("Welcome to the simple sandwich order bot!")
                .AddRemainingFields()
                .Field(new FieldReflector<SandwichOrder>(nameof(Specials))
                    .SetType(null)
                    .SetActive((state) => state.Length == LengthOptions.FootLong)
                    .SetDefine(async (state, field) =>
                    {
                        var dic = GetSpecialFree(state.Sandwich);
                        foreach (KeyValuePair<string, string[]> item in dic)
                        {
                            field
                                .AddDescription(item.Key, item.Value[0])
                                .AddTerms(item.Key, item.Value);
                        }

                        return true;
                    }))

                .Build();
        }

        private static Dictionary<string, string[]> GetSpecialFree(SandwichOptions? sandwichOptions)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            switch (sandwichOptions)
            {
                case SandwichOptions.BLT:
                    dic.Add("cookie", new string[] {"Free cookie", "cookie"});
                    dic.Add("drink", new string[] {"Free drink", "drink"});
                    break;
                case SandwichOptions.BlackForestHam:
                    dic.Add("cookie", new string[] {"Free cookie", "cookie"});
                    dic.Add("salard", new string[] {"Free salard", "salard"});
                    break;
                case SandwichOptions.BuffaloChicken:
                    dic.Add("drink", new string[] {"Free drink", "drink"});
                    dic.Add("salard", new string[] {"Free salard", "salard"});
                    break;
            }
            return dic;
        }

    };

}