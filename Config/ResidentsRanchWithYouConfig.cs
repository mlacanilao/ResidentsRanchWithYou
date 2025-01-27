using System.IO;
using BepInEx.Configuration;

namespace ResidentsRanchWithYou
{
    internal static class ResidentsRanchWithYouConfig
    {
        internal static ConfigEntry<bool> EnableBrush;
        internal static ConfigEntry<bool> EnableRequirePasture;
        internal static ConfigEntry<bool> EnableFood;
        internal static ConfigEntry<bool> EnableLivestockLevel;
        internal static ConfigEntry<bool> EnableEgg;
        internal static ConfigEntry<int> EggChance;
        internal static ConfigEntry<bool> EnableMilk;
        internal static ConfigEntry<int> MilkChance;
        internal static ConfigEntry<bool> EnableFeedMilkToBaby;
        internal static ConfigEntry<bool> EnableFurLevel;
        internal static ConfigEntry<bool> EnableShear;
        internal static ConfigEntry<int> FurLevelThreshold;
        internal static ConfigEntry<bool> EnableNoMove;
        
        internal static string XmlPath { get; private set; }
        internal static string TranslationXlsxPath { get; private set; }

        internal static void LoadConfig(ConfigFile config)
        {
            EnableBrush = config.Bind(
                section: ModInfo.Name,
                key: "Enable Brush",
                defaultValue: true,
                description: "Enable or disable the ability to brush livestock.\n" +
                             "Set to 'true' to allow brushing, or 'false' to disable it.\n" +
                             "家畜をブラッシングする機能を有効または無効にします。\n" +
                             "'true' に設定するとブラッシングが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用刷洗牲畜的功能。\n" +
                             "设置为 'true' 以启用刷洗，设置为 'false' 以禁用。"
            );
            
            EnableRequirePasture = config.Bind(
                section: ModInfo.Name,
                key: "Enable Require Pasture",
                defaultValue: false,
                description: "Enable or disable requiring residents to use pasture for feeding livestock and generating milk and eggs.\n" +
                             "Set to 'true' to require pasture, or 'false' to allow feeding and generating resources without it.\n" +
                             "牧草を使用して家畜に餌を与え、牛乳や卵を生産する必要があるかどうかを設定します。\n" +
                             "'true' に設定すると牧草が必要になり、'false' に設定すると牧草なしで飼育や生産が可能になります。\n" +
                             "启用或禁用要求居民使用牧草喂养牲畜并生成牛奶和鸡蛋。\n" +
                             "设置为 'true' 以需要牧草，或设置为 'false' 允许无牧草喂养和生成资源。"
            );
            
            EnableFood = config.Bind(
                section: ModInfo.Name,
                key: "Enable Food",
                defaultValue: true,
                description: "Enable or disable feeding livestock.\n" +
                             "Set to 'true' to enable feeding, or 'false' to disable it.\n" +
                             "家畜に餌を与える機能を有効または無効にします。\n" +
                             "'true' に設定すると餌を与えることが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用喂养牲畜。\n" +
                             "设置为 'true' 以启用喂养，设置为 'false' 以禁用。"
            );
            
            EnableLivestockLevel = config.Bind(
                section: ModInfo.Name,
                key: "Enable Livestock Level",
                defaultValue: true,
                description: "Enable or disable increasing the level of livestock.\n" +
                             "Set to 'true' to enable livestock level gain, or 'false' to disable it.\n" +
                             "家畜のレベルを上げる機能を有効または無効にします。\n" +
                             "'true' に設定すると家畜のレベルを上げることが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用提高牲畜等级。\n" +
                             "设置为 'true' 以启用等级提升，设置为 'false' 以禁用。"
            );
            
            EnableEgg = config.Bind(
                section: ModInfo.Name,
                key: "Enable Egg",
                defaultValue: true,
                description: "Enable or disable generating eggs from livestock.\n" +
                             "Set to 'true' to enable egg generation, or 'false' to disable it.\n" +
                             "家畜から卵を生成する機能を有効または無効にします。\n" +
                             "'true' に設定すると卵の生成が可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用从牲畜生成鸡蛋。\n" +
                             "设置为 'true' 以启用生成鸡蛋，设置为 'false' 以禁用。"
            );
            
            EggChance = config.Bind(
                section: ModInfo.Name,
                key: "Egg Chance",
                defaultValue: 10,
                description: "Set the percentage chance (1-100) to generate eggs from livestock.\n" +
                             "家畜から卵を生成する確率 (1-100%) を設定します。\n" +
                             "设置从牲畜生成鸡蛋的概率 (1-100%)。"
            );
            
            EnableMilk = config.Bind(
                section: ModInfo.Name,
                key: "Enable Milk",
                defaultValue: true,
                description: "Enable or disable generating milk from livestock.\n" +
                             "Set to 'true' to enable milk generation, or 'false' to disable it.\n" +
                             "家畜から牛乳を生成する機能を有効または無効にします。\n" +
                             "'true' に設定すると牛乳の生成が可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用从牲畜生成牛奶。\n" +
                             "设置为 'true' 以启用生成牛奶，设置为 'false' 以禁用。"
            );
            
            MilkChance = config.Bind(
                section: ModInfo.Name,
                key: "Milk Chance",
                defaultValue: 10,
                description: "Set the percentage chance (1-100) to generate milk from livestock.\n" +
                             "家畜から牛乳を生成する確率 (1-100%) を設定します。\n" +
                             "设置从牲畜生成牛奶的概率 (1-100%)。"
            );
            
            EnableFeedMilkToBaby = config.Bind(
                section: ModInfo.Name,
                key: "Enable Feed Milk to Baby",
                defaultValue: true,
                description: "Enable or disable feeding milk to baby livestock.\n" +
                             "Set to 'true' to allow feeding milk to babies, or 'false' to disable it.\n" +
                             "乳児家畜に牛乳を与える機能を有効または無効にします。\n" +
                             "'true' に設定すると乳児家畜に牛乳を与えることが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用向幼畜喂奶。\n" +
                             "设置为 'true' 以启用喂奶，设置为 'false' 以禁用。"
            );
            
            EnableFurLevel = config.Bind(
                section: ModInfo.Name,
                key: "Enable Fur Level",
                defaultValue: true,
                description: "Enable or disable increasing the fur level of livestock.\n" +
                             "Set to 'true' to enable fur level gain, or 'false' to disable it.\n" +
                             "家畜の毛皮レベルを上げる機能を有効または無効にします。\n" +
                             "'true' に設定すると毛皮レベルを上げることが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用提高牲畜毛皮等级。\n" +
                             "设置为 'true' 以启用毛皮等级提升，设置为 'false' 以禁用。"
            );
            
            EnableShear = config.Bind(
                section: ModInfo.Name,
                key: "Enable Shear",
                defaultValue: true,
                description: "Enable or disable shearing livestock.\n" +
                             "Set to 'true' to allow shearing, or 'false' to disable it.\n" +
                             "家畜の毛を刈る機能を有効または無効にします。\n" +
                             "'true' に設定すると毛刈りが可能になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用剪毛功能。\n" +
                             "设置为 'true' 以启用剪毛，设置为 'false' 以禁用。"
            );

            FurLevelThreshold = config.Bind(
                section: ModInfo.Name,
                key: "Fur Level Threshold",
                defaultValue: 5,
                description: "Set the fur level threshold (1-5) required for shearing livestock.\n" +
                             "家畜の毛を刈るために必要な毛皮レベルのしきい値 (1-5) を設定します。\n" +
                             "设置剪毛所需的毛皮等级阈值 (1-5)。"
            );
            
            EnableNoMove = config.Bind(
                section: ModInfo.Name,
                key: "Enable No Move",
                defaultValue: true,
                description: "Enable or disable restricting livestock movement during chores.\n" +
                             "Set to 'true' to prevent livestock from moving, or 'false' to allow movement.\n" +
                             "雑用中に家畜の移動を制限するかどうかを設定します。\n" +
                             "'true' に設定すると移動を防止し、'false' に設定すると移動を許可します。\n" +
                             "启用或禁用在杂务期间限制牲畜移动。\n" +
                             "设置为 'true' 以防止移动，设置为 'false' 以允许移动。"
            );
        }
        
        internal static void InitializeXmlPath(string xmlPath)
        {
            if (File.Exists(path: xmlPath))
            {
                XmlPath = xmlPath;
            }
            else
            {
                XmlPath = string.Empty;
            }
        }
        
        internal static void InitializeTranslationXlsxPath(string xlsxPath)
        {
            if (File.Exists(path: xlsxPath))
            {
                TranslationXlsxPath = xlsxPath;
            }
            else
            {
                TranslationXlsxPath = string.Empty;
            }
        }
    }
}