using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;
using UnityEngine;

namespace ResidentsRanchWithYou
{
    public class UIController
    {
        public static void RegisterUI()
        {
            foreach (var obj in ModManager.ListPluginObject)
            {
                if (obj is BaseUnityPlugin plugin && plugin.Info.Metadata.GUID == ModInfo.ModOptionsGuid)
                {
                    var controller = ModOptionController.Register(guid: ModInfo.Guid, tooptipId: "mod.tooltip");
                    
                    var assemblyLocation = Path.GetDirectoryName(path: Assembly.GetExecutingAssembly().Location);
                    var xmlPath = Path.Combine(path1: assemblyLocation, path2: "ResidentsRanchWithYouConfig.xml");
                    ResidentsRanchWithYouConfig.InitializeXmlPath(xmlPath: xmlPath);
            
                    var xlsxPath = Path.Combine(path1: assemblyLocation, path2: "translations.xlsx");
                    ResidentsRanchWithYouConfig.InitializeTranslationXlsxPath(xlsxPath: xlsxPath);
                    
                    if (File.Exists(path: ResidentsRanchWithYouConfig.XmlPath))
                    {
                        using (StreamReader sr = new StreamReader(path: ResidentsRanchWithYouConfig.XmlPath))
                            controller.SetPreBuildWithXml(xml: sr.ReadToEnd());
                    }
                    
                    if (File.Exists(path: ResidentsRanchWithYouConfig.TranslationXlsxPath))
                    {
                        controller.SetTranslationsFromXslx(path: ResidentsRanchWithYouConfig.TranslationXlsxPath);
                    }
                    
                    RegisterEvents(controller: controller);
                }
            }
        }

        private static void RegisterEvents(ModOptionController controller)
        {
            controller.OnBuildUI += builder =>
            {
                var enableBrushToggle = builder.GetPreBuild<OptToggle>(id: "enableBrushToggle");
                enableBrushToggle.Checked = ResidentsRanchWithYouConfig.EnableBrush.Value;
                enableBrushToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableBrush.Value = isChecked;
                };
                
                var enableRequirePastureToggle = builder.GetPreBuild<OptToggle>(id: "enableRequirePastureToggle");
                enableRequirePastureToggle.Checked = ResidentsRanchWithYouConfig.EnableRequirePasture.Value;
                enableRequirePastureToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableRequirePasture.Value = isChecked;
                };
                
                var enableFoodToggle = builder.GetPreBuild<OptToggle>(id: "enableFoodToggle");
                enableFoodToggle.Checked = ResidentsRanchWithYouConfig.EnableFood.Value;
                enableFoodToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableFood.Value = isChecked;
                };
                
                var enableLivestockLevelToggle = builder.GetPreBuild<OptToggle>(id: "enableLivestockLevelToggle");
                enableLivestockLevelToggle.Checked = ResidentsRanchWithYouConfig.EnableLivestockLevel.Value;
                enableLivestockLevelToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableLivestockLevel.Value = isChecked;
                };
                
                var enableEggToggle = builder.GetPreBuild<OptToggle>(id: "enableEggToggle");
                enableEggToggle.Checked = ResidentsRanchWithYouConfig.EnableEgg.Value;
                enableEggToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableEgg.Value = isChecked;
                };
                
                var eggChanceSlider = builder.GetPreBuild<OptSlider>(id: "eggChanceSlider");
                string eggTitle = eggChanceSlider.Title;
                eggChanceSlider.Title = $"{eggTitle} {ResidentsRanchWithYouConfig.EggChance.Value.ToString()}%";
                eggChanceSlider.Value = ResidentsRanchWithYouConfig.EggChance.Value;
                eggChanceSlider.Step = 1;
                eggChanceSlider.OnValueChanged += value =>
                {
                    eggChanceSlider.Title = $"{eggTitle} {value.ToString()}%";
                    ResidentsRanchWithYouConfig.EggChance.Value = (int)value;
                };
                
                var enableMilkToggle = builder.GetPreBuild<OptToggle>(id: "enableMilkToggle");
                enableMilkToggle.Checked = ResidentsRanchWithYouConfig.EnableMilk.Value;
                enableMilkToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableMilk.Value = isChecked;
                };
                
                var milkChanceSlider = builder.GetPreBuild<OptSlider>(id: "milkChanceSlider");
                string milkTitle = milkChanceSlider.Title;
                milkChanceSlider.Title = $"{milkTitle} {ResidentsRanchWithYouConfig.MilkChance.Value.ToString()}%";
                milkChanceSlider.Value = ResidentsRanchWithYouConfig.MilkChance.Value;
                milkChanceSlider.Step = 1;
                milkChanceSlider.OnValueChanged += value =>
                {
                    milkChanceSlider.Title = $"{milkTitle} {value.ToString()}%";
                    ResidentsRanchWithYouConfig.MilkChance.Value = (int)value;
                };
                
                var enableFeedMilkToBabyToggle = builder.GetPreBuild<OptToggle>(id: "enableFeedMilkToBabyToggle");
                enableFeedMilkToBabyToggle.Checked = ResidentsRanchWithYouConfig.EnableFeedMilkToBaby.Value;
                enableFeedMilkToBabyToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableFeedMilkToBaby.Value = isChecked;
                };
                
                var enableFurLevelToggle = builder.GetPreBuild<OptToggle>(id: "enableFurLevelToggle");
                enableFurLevelToggle.Checked = ResidentsRanchWithYouConfig.EnableFurLevel.Value;
                enableFurLevelToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableFurLevel.Value = isChecked;
                };
                
                var enableNoMoveToggle = builder.GetPreBuild<OptToggle>(id: "enableNoMoveToggle");
                enableNoMoveToggle.Checked = ResidentsRanchWithYouConfig.EnableNoMove.Value;
                enableNoMoveToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableNoMove.Value = isChecked;
                };
                
                var enableShearToggle = builder.GetPreBuild<OptToggle>(id: "enableShearToggle");
                enableShearToggle.Checked = ResidentsRanchWithYouConfig.EnableShear.Value;
                enableShearToggle.OnValueChanged += isChecked =>
                {
                    ResidentsRanchWithYouConfig.EnableShear.Value = isChecked;
                };
                
                var furLevelThresholdSlider = builder.GetPreBuild<OptSlider>(id: "furLevelThresholdSlider");
                string furTitle = furLevelThresholdSlider.Title;
                furLevelThresholdSlider.Title = $"{furTitle} {ResidentsRanchWithYouConfig.FurLevelThreshold.Value.ToString()}";
                furLevelThresholdSlider.Value = ResidentsRanchWithYouConfig.FurLevelThreshold.Value;
                furLevelThresholdSlider.Step = 1;
                furLevelThresholdSlider.OnValueChanged += value =>
                {
                    furLevelThresholdSlider.Title = $"{furTitle} {value.ToString()}";
                    ResidentsRanchWithYouConfig.FurLevelThreshold.Value = (int)value;
                };
            };
        }
    }
}