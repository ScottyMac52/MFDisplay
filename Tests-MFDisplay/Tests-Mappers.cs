using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using MFDSettingsManager.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Tests_MFDisplay.Mocks;

namespace Tests_MFDisplay
{
    /// <summary>
    /// Test fixture
    /// </summary>
    [TestClass]
    public class TestsMappers
    {
        private readonly int left = 2975;
        private readonly int top = 615;
        private readonly int width = 850;
        private readonly int height = 720;
        private readonly int xOffsetStart = 101;
        private readonly int xOffsetFinish = 785;
        private readonly int yOffsetStart = 132;
        private readonly int yOffsetFinish = 901;
        private readonly float opacity = 1.0F;
        private readonly string Name1 = "LMFD";
        private readonly string Name2 = "RMFD";

        /// <summary>
        /// TestToEnsureThatDefaultsFunctionCorrectly
        /// </summary>
        [TestMethod]
        public void TestToEnsureThatDefaultsFunctionCorrectly()
        {
            // ARRANGE
            var moduleName = "DefaultsWork";
            var displayName = "Defaults Work";
            var fileName = "DefaultsWork.jpg";
            var configurationCount = 2;

            //ACT and ASSERT
            TestNamedModuleConfiguredCorrectly("DefaultsWork.config", moduleName, configurationCount, (module) =>
            {
                Assert.AreEqual(displayName, module.DisplayName);
                Assert.AreEqual(true, module.Configurations
                    .All(cf =>
                        cf.Height == height &&
                        cf.Top == top &&
                        cf.Width == width &&
                        cf.Left == left &&
                        cf.XOffsetStart == xOffsetStart &&
                        cf.XOffsetFinish == xOffsetFinish &&
                        cf.YOffsetStart == yOffsetStart &&
                        cf.YOffsetFinish == yOffsetFinish &&
                        cf.Opacity == opacity));

                var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == "LMFD").FirstOrDefault();
                var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == "RMFD").FirstOrDefault();
                Assert.AreEqual(Name1, mfdConfiguration1.Name);
                Assert.AreEqual(Name2, mfdConfiguration2.Name);
                Assert.AreEqual(fileName, mfdConfiguration1.FileName);
                Assert.AreEqual(fileName, mfdConfiguration2.FileName);
            });
        }

        /// <summary>
        /// TestToEnsureThatPartialOverridesFunctionCorrectly
        /// </summary>
        [TestMethod]
        public void TestToEnsureThatExtraConfigurationsFunctionCorrectly()
        {
            // ARRANGE
            var moduleName = "PartialOverridesWork";
            var displayName = "PartialOverrides Work";
            var configurationCount = 6;
            var lmfdFileName = "PartialOverridesWork.jpg";
            var rmfdFileName = "filename.jpg";
            var overrideFileName = "RMFDOverride.jpg";
            var leftTopCoordAndOffsetStartsForRmfd = 100;
            var widthHeightAndOffsetFinishForRmfd = 200;

            //ACT and ASSERT
            TestNamedModuleConfiguredCorrectly("OverridesWork.config", moduleName, configurationCount, (module) =>
            {
                Assert.AreEqual(displayName, module.DisplayName);
                Assert.AreEqual(configurationCount, module.Configurations.Count);

                var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == "LMFD").FirstOrDefault();
                Assert.AreEqual(Name1, mfdConfiguration1.Name);
                Assert.AreEqual(lmfdFileName, mfdConfiguration1.FileName);
                Assert.AreEqual(left, mfdConfiguration1.Left);
                Assert.AreEqual(top, mfdConfiguration1.Top);
                Assert.AreEqual(xOffsetStart, mfdConfiguration1.XOffsetStart);
                Assert.AreEqual(yOffsetStart, mfdConfiguration1.YOffsetStart);
                Assert.AreEqual(xOffsetFinish, mfdConfiguration1.XOffsetFinish);
                Assert.AreEqual(yOffsetFinish, mfdConfiguration1.YOffsetFinish);
                Assert.AreEqual(width, mfdConfiguration1.Width);
                Assert.AreEqual(height, mfdConfiguration1.Height);

                var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == "RMFD").FirstOrDefault();
                Assert.AreEqual(Name2, mfdConfiguration2.Name);
                Assert.AreEqual(overrideFileName, mfdConfiguration2.FileName);
                Assert.AreEqual(leftTopCoordAndOffsetStartsForRmfd, mfdConfiguration2.Left);
                Assert.AreEqual(leftTopCoordAndOffsetStartsForRmfd, mfdConfiguration2.Top);
                Assert.AreEqual(leftTopCoordAndOffsetStartsForRmfd, mfdConfiguration2.XOffsetStart);
                Assert.AreEqual(leftTopCoordAndOffsetStartsForRmfd, mfdConfiguration2.YOffsetStart);
                Assert.AreEqual(widthHeightAndOffsetFinishForRmfd, mfdConfiguration2.XOffsetFinish);
                Assert.AreEqual(widthHeightAndOffsetFinishForRmfd, mfdConfiguration2.YOffsetFinish);
                Assert.AreEqual(widthHeightAndOffsetFinishForRmfd, mfdConfiguration2.Width);
                Assert.AreEqual(widthHeightAndOffsetFinishForRmfd, mfdConfiguration2.Height);

                var mfdConfiguration3 = module.Configurations.Where(cfg => cfg.Name == "POSITION").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration3.FileName);
                Assert.AreEqual(300, mfdConfiguration3.Left);
                Assert.AreEqual(300, mfdConfiguration3.Top);
                Assert.AreEqual(xOffsetStart, mfdConfiguration3.XOffsetStart);
                Assert.AreEqual(yOffsetStart, mfdConfiguration3.YOffsetStart);
                Assert.AreEqual(xOffsetFinish, mfdConfiguration3.XOffsetFinish);
                Assert.AreEqual(yOffsetFinish, mfdConfiguration3.YOffsetFinish);
                Assert.AreEqual(width, mfdConfiguration3.Width);
                Assert.AreEqual(height, mfdConfiguration3.Height);

                var mfdConfiguration4 = module.Configurations.Where(cfg => cfg.Name == "SIZE").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration4.FileName);
                Assert.AreEqual(left, mfdConfiguration4.Left);
                Assert.AreEqual(top, mfdConfiguration4.Top);
                Assert.AreEqual(xOffsetStart, mfdConfiguration4.XOffsetStart);
                Assert.AreEqual(yOffsetStart, mfdConfiguration4.YOffsetStart);
                Assert.AreEqual(xOffsetFinish, mfdConfiguration4.XOffsetFinish);
                Assert.AreEqual(yOffsetFinish, mfdConfiguration4.YOffsetFinish);
                Assert.AreEqual(300, mfdConfiguration4.Width);
                Assert.AreEqual(300, mfdConfiguration4.Height);

                var mfdConfiguration5 = module.Configurations.Where(cfg => cfg.Name == "OFFSET").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration5.FileName);
                Assert.AreEqual(left, mfdConfiguration5.Left);
                Assert.AreEqual(top, mfdConfiguration5.Top);
                Assert.AreEqual(10, mfdConfiguration5.XOffsetStart);
                Assert.AreEqual(10, mfdConfiguration5.YOffsetStart);
                Assert.AreEqual(20, mfdConfiguration5.XOffsetFinish);
                Assert.AreEqual(20, mfdConfiguration5.YOffsetFinish);
                Assert.AreEqual(width, mfdConfiguration5.Width);
                Assert.AreEqual(height, mfdConfiguration5.Height);

                var mfdConfiguration6 = module.Configurations.Where(cfg => cfg.Name == "FILENAME").FirstOrDefault();
                Assert.AreEqual(rmfdFileName, mfdConfiguration6.FileName);
                Assert.AreEqual(left, mfdConfiguration6.Left);
                Assert.AreEqual(top, mfdConfiguration6.Top);
                Assert.AreEqual(xOffsetStart, mfdConfiguration6.XOffsetStart);
                Assert.AreEqual(yOffsetStart, mfdConfiguration6.YOffsetStart);
                Assert.AreEqual(xOffsetFinish, mfdConfiguration6.XOffsetFinish);
                Assert.AreEqual(yOffsetFinish, mfdConfiguration6.YOffsetFinish);
                Assert.AreEqual(width, mfdConfiguration6.Width);
                Assert.AreEqual(height, mfdConfiguration6.Height);

            });
        }


        /// <summary>
        /// Performs the actual testing for a specified module in a specified filename
        /// </summary>
        /// <param name="configurationFileName"></param>
        /// <param name="moduleName"></param>
        /// <param name="configurationCount"></param>
        /// <param name="specifcAssert"></param>
        private void TestNamedModuleConfiguredCorrectly(string configurationFileName, string moduleName, int configurationCount, Action<ModuleDefinition> specifcAssert)
        {
            // ARRANGE
            var logger = new MockLogger();
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), configurationFileName);
            var configSection = MFDConfigurationSection.GetConfig(logger, configFilePath);

            // ACT
            var configModel = ConfigSectionModelMapper.MapFromConfigurationSection(configSection, logger);

            // ASSERT
            var module = configModel.Modules.Where(cm => cm.ModuleName == moduleName).FirstOrDefault();
            Assert.IsNotNull(module);
            Assert.AreEqual(configurationCount, module.Configurations.Count);
            specifcAssert?.Invoke(module);
        }
    }
}
