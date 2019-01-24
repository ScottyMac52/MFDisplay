using MFDSettingsManager.Configuration;
using MFDSettingsManager.Extensions;
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
        private readonly int leftL = 800;
        private readonly int top = 500;
        private readonly int width = 850;
        private readonly int height = 850;
        private readonly int xOffsetStartL = 101;
        private readonly int xOffsetFinishL = 785;
        private readonly int leftR = 1000;
        private readonly int xOffsetStartR = 903;
        private readonly int xOffsetFinishR = 1587;
        private readonly int yOffsetStart = 250;
        private readonly int yOffsetFinish = 901;
        private readonly float opacity = 1.0F;
        private readonly string NameL = "LMFD";
        private readonly string NameR = "RMFD";
        private readonly string NEW_DEFAULT = "NEW_DEFAULT";

        /// <summary>7
        /// TestToEnsureThatDefaultsFunctionCorrectly
        /// </summary>
        [TestMethod]
        public void TestToEnsureThatDefaultsFunctionCorrectly()
        {
            /*
             * This test is designed to surface problems in configuration management related to defaults
             * 
             * Default Configurations:
             * 
             *  <DefaultConfigurations>
                    <add name="LMFD" left="800" top="500" width="850" height="850" xOffsetStart="101" xOffsetFinish="785" yOffsetStart="250" yOffsetFinish="901" opacity="1.0" />
                    <add name="RMFD" left="1000" top="500" width="850" height="850" xOffsetStart="903" xOffsetFinish="1587" yOffsetStart="250" yOffsetFinish="901" opacity="1.0" />
                </DefaultConfigurations>
             * 
             * Module Definition 
             * 
             *  <Modules>
                    <add moduleName="DefaultsWork" displayName="Defaults Work" filename="DefaultsWork.jpg"/>
                </Modules>
             * 
             */
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
                        cf.YOffsetStart == yOffsetStart &&
                        cf.YOffsetFinish == yOffsetFinish &&
                        cf.Opacity == opacity &&
                        cf.FileName == fileName));

                var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == NameL).FirstOrDefault();
                Assert.AreEqual(leftL, mfdConfiguration1.Left);
                Assert.AreEqual(xOffsetStartL, mfdConfiguration1.XOffsetStart);
                Assert.AreEqual(xOffsetFinishL, mfdConfiguration1.XOffsetFinish);

                var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == NameR).FirstOrDefault();
                Assert.AreEqual(leftR, mfdConfiguration2.Left);
                Assert.AreEqual(xOffsetStartR, mfdConfiguration2.XOffsetStart);
                Assert.AreEqual(xOffsetFinishR, mfdConfiguration2.XOffsetFinish);
            });
        }

        /// <summary>
        /// Tests to ensure that configurations defined in modules override all aspects of configurations
        /// defined as defaults.
        /// </summary>
        [TestMethod]
        public void TestToEnsurteThatPartialConfigurationsFunctionCorrectly()
        {
            // ARRANGE
            var moduleName = "OverridesWork";
            var displayName = "Overrides Work";

            var fileNameL = "LMFDOverride.jpg";
            var fileNameR = "RMFDOverride.jpg";
            var configurationCount = 3;

            // ACT and ASSERT
            TestNamedModuleConfiguredCorrectly("OverridesWork.config", moduleName, configurationCount, (module) =>
            {
                Assert.AreEqual(displayName, module.DisplayName);
                Assert.AreEqual(configurationCount, module.Configurations.Count);

                var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == NameL).FirstOrDefault();
                Assert.AreEqual(NameL, mfdConfiguration1.Name);
                Assert.AreEqual(fileNameL, mfdConfiguration1.FileName);
                Assert.AreEqual(1, mfdConfiguration1.Left);
                Assert.AreEqual(1, mfdConfiguration1.Top);
                Assert.AreEqual(1, mfdConfiguration1.XOffsetStart);
                Assert.AreEqual(1, mfdConfiguration1.YOffsetStart);
                Assert.AreEqual(50, mfdConfiguration1.XOffsetFinish);
                Assert.AreEqual(50, mfdConfiguration1.YOffsetFinish);
                Assert.AreEqual(50, mfdConfiguration1.Width);
                Assert.AreEqual(50, mfdConfiguration1.Height);

                var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == NameR).FirstOrDefault();
                Assert.AreEqual(NameR, mfdConfiguration2.Name);
                Assert.AreEqual(fileNameR, mfdConfiguration2.FileName);
                Assert.AreEqual(100, mfdConfiguration2.Left);
                Assert.AreEqual(100, mfdConfiguration2.Top);
                Assert.AreEqual(100, mfdConfiguration2.XOffsetStart);
                Assert.AreEqual(100, mfdConfiguration2.YOffsetStart);
                Assert.AreEqual(200, mfdConfiguration2.XOffsetFinish);
                Assert.AreEqual(200, mfdConfiguration2.YOffsetFinish);
                Assert.AreEqual(200, mfdConfiguration2.Width);
                Assert.AreEqual(200, mfdConfiguration2.Height);

                var mfdConfiguration3 = module.Configurations.Where(cfg => cfg.Name == NEW_DEFAULT).FirstOrDefault();
                Assert.AreEqual(NEW_DEFAULT, mfdConfiguration3.Name);
                Assert.AreEqual($"{moduleName}.jpg", mfdConfiguration3.FileName);
                Assert.AreEqual(1, mfdConfiguration3.Left);
                Assert.AreEqual(5, mfdConfiguration3.Top);
                Assert.AreEqual(90, mfdConfiguration3.XOffsetStart);
                Assert.AreEqual(25, mfdConfiguration3.YOffsetStart);
                Assert.AreEqual(158, mfdConfiguration3.XOffsetFinish);
                Assert.AreEqual(90, mfdConfiguration3.YOffsetFinish);
                Assert.AreEqual(85, mfdConfiguration3.Width);
                Assert.AreEqual(85, mfdConfiguration3.Height);
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
            var configurationCount = 7;
            var lmfdFileName = "PartialOverridesWork.jpg";
            var rmfdFileName = "filename.jpg";
            var overrideFileName = "RMFDOverride.jpg";

            //ACT and ASSERT
            TestNamedModuleConfiguredCorrectly("OverridesWork.config", moduleName, configurationCount, (module) =>
            {
                Assert.AreEqual(displayName, module.DisplayName);
                Assert.AreEqual(configurationCount, module.Configurations.Count);

                // Since the LMFD is defined as a config for the module without any values then those values are all 0
                var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == NameL).FirstOrDefault();
                Assert.AreEqual(NameL, mfdConfiguration1.Name);
                Assert.AreEqual(lmfdFileName, mfdConfiguration1.FileName);
                Assert.AreEqual(leftL, mfdConfiguration1.Left);
                Assert.AreEqual(top, mfdConfiguration1.Top);
                Assert.AreEqual(xOffsetStartL, mfdConfiguration1.XOffsetStart);
                Assert.AreEqual(yOffsetStart, mfdConfiguration1.YOffsetStart);
                Assert.AreEqual(xOffsetFinishL, mfdConfiguration1.XOffsetFinish);
                Assert.AreEqual(yOffsetFinish, mfdConfiguration1.YOffsetFinish);
                Assert.AreEqual(width, mfdConfiguration1.Width);
                Assert.AreEqual(height, mfdConfiguration1.Height);

                var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == NameR).FirstOrDefault();
                Assert.AreEqual(NameR, mfdConfiguration2.Name);
                Assert.AreEqual(overrideFileName, mfdConfiguration2.FileName);
                Assert.AreEqual(100, mfdConfiguration2.Left);
                Assert.AreEqual(100, mfdConfiguration2.Top);
                Assert.AreEqual(100, mfdConfiguration2.XOffsetStart);
                Assert.AreEqual(100, mfdConfiguration2.YOffsetStart);
                Assert.AreEqual(200, mfdConfiguration2.XOffsetFinish);
                Assert.AreEqual(200, mfdConfiguration2.YOffsetFinish);
                Assert.AreEqual(200, mfdConfiguration2.Width);
                Assert.AreEqual(200, mfdConfiguration2.Height);

                var mfdConfiguration3 = module.Configurations.Where(cfg => cfg.Name == "POSITION").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration3.FileName);
                Assert.AreEqual(300, mfdConfiguration3.Left);
                Assert.AreEqual(300, mfdConfiguration3.Top);
                Assert.AreEqual(0, mfdConfiguration3.XOffsetStart);
                Assert.AreEqual(0, mfdConfiguration3.YOffsetStart);
                Assert.AreEqual(0, mfdConfiguration3.XOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration3.YOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration3.Width);
                Assert.AreEqual(0, mfdConfiguration3.Height);

                var mfdConfiguration4 = module.Configurations.Where(cfg => cfg.Name == "SIZE").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration4.FileName);
                Assert.AreEqual(0, mfdConfiguration4.Left);
                Assert.AreEqual(0, mfdConfiguration4.Top);
                Assert.AreEqual(0, mfdConfiguration4.XOffsetStart);
                Assert.AreEqual(0, mfdConfiguration4.YOffsetStart);
                Assert.AreEqual(0, mfdConfiguration4.XOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration4.YOffsetFinish);
                Assert.AreEqual(300, mfdConfiguration4.Width);
                Assert.AreEqual(300, mfdConfiguration4.Height);

                var mfdConfiguration5 = module.Configurations.Where(cfg => cfg.Name == "OFFSET").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration5.FileName);
                Assert.AreEqual(0, mfdConfiguration5.Left);
                Assert.AreEqual(0, mfdConfiguration5.Top);
                Assert.AreEqual(10, mfdConfiguration5.XOffsetStart);
                Assert.AreEqual(10, mfdConfiguration5.YOffsetStart);
                Assert.AreEqual(20, mfdConfiguration5.XOffsetFinish);
                Assert.AreEqual(20, mfdConfiguration5.YOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration5.Width);
                Assert.AreEqual(0, mfdConfiguration5.Height);

                var mfdConfiguration6 = module.Configurations.Where(cfg => cfg.Name == "FILENAME").FirstOrDefault();
                Assert.AreEqual(rmfdFileName, mfdConfiguration6.FileName);
                Assert.AreEqual(0, mfdConfiguration6.Left);
                Assert.AreEqual(0, mfdConfiguration6.Top);
                Assert.AreEqual(0, mfdConfiguration6.XOffsetStart);
                Assert.AreEqual(0, mfdConfiguration6.YOffsetStart);
                Assert.AreEqual(0, mfdConfiguration6.XOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration6.YOffsetFinish);
                Assert.AreEqual(0, mfdConfiguration6.Width);
                Assert.AreEqual(0, mfdConfiguration6.Height);

                var mfdConfiguration7 = module.Configurations.Where(cfg => cfg.Name == "NEW_DEFAULT").FirstOrDefault();
                Assert.AreEqual(lmfdFileName, mfdConfiguration7.FileName);
                Assert.AreEqual(1, mfdConfiguration7.Left);
                Assert.AreEqual(5, mfdConfiguration7.Top);
                Assert.AreEqual(90, mfdConfiguration7.XOffsetStart);
                Assert.AreEqual(25, mfdConfiguration7.YOffsetStart);
                Assert.AreEqual(158, mfdConfiguration7.XOffsetFinish);
                Assert.AreEqual(90, mfdConfiguration7.YOffsetFinish);
                Assert.AreEqual(85, mfdConfiguration7.Width);
                Assert.AreEqual(85, mfdConfiguration7.Height);
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
            var configModel = configSection.ToModel(logger);

            // ASSERT
            var module = configModel.Modules.Where(cm => cm.ModuleName == moduleName).FirstOrDefault();
            Assert.IsNotNull(module);
            Assert.AreEqual(configurationCount, module.Configurations.Count);
            specifcAssert?.Invoke(module);
        }
    }
}
