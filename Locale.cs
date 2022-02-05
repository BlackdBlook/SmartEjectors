namespace SmartEjectors
{
    public static class Locale
    {
        public static readonly LanguageText disabledEjector1 = new LanguageText("Disabled - Filled Sphere", "已禁用 - 戴森球已填满");
        public static readonly LanguageText disabledEjector2 = new LanguageText("Disabled", "已禁用");
        public static readonly LanguageText disabledEjector3 = new LanguageText("Disabled - Node Limit Reached", "已禁用 - 达到节点上限");
    }

    public class LanguageText
    {
        public readonly string enUS;
        public readonly string zhCN;

        public LanguageText(string enUS, string zhCN)
        {
            this.enUS = enUS;
            this.zhCN = zhCN;
        }

        public string this[Language language]
        {
            get
            {
                switch (language)
                {
                    case Language.enUS: return enUS;
                    case Language.zhCN: return zhCN;
                    default: return enUS;
                }
            }
        }
    }
}
