using MFDSettingsManager;
using MFDSettingsManager.Configuration;
using MFDSettingsManager.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
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
        private readonly string filePath = @"TestData\";
        private readonly string defaultConfig = "TestModule";
        private readonly int moduleCount = 1;
        private readonly string moduleName = "TestModule";
        private readonly string displayName = "Test Module";
        private readonly string fileName1 = "LMFD.jpg";
        private readonly int height = 720;
        private readonly int width = 850;
        private readonly int top = 615;
        private readonly int left1 = 3000;
        private readonly int left2 = 3825;
        private readonly float opacity = 1.0F;
        private readonly string Name1 = "LMFD";
        private readonly string Name2 = "RMFD";

        /// <summary>
        /// TestToEnsureConfigSectionIsMappedToModel
        /// </summary>
        [TestMethod]
        public void TestToEnsureConfigSectionIsMappedToModel()
        {
            // ARRANGE
            var logger = new MockLogger();
            var configSection = MFDConfigurationSection.GetConfig(logger);

            // ACT
            var configModel = ConfigSectionModelMapper.MapFromConfigurationSection(configSection, logger);

            // ASSERT
            Assert.AreEqual(filePath, configModel.FilePath);
            Assert.AreEqual(defaultConfig, configModel.DefaultConfig);
            Assert.AreEqual(moduleCount, configModel.Modules.Count);
            var module = configModel.Modules.Where(cm => cm.ModuleName == moduleName).FirstOrDefault();
            Assert.IsNotNull(module);
            Assert.AreEqual(displayName, module.DisplayName);
            var mfdConfiguration1 = module.Configurations.Where(cfg => cfg.Name == "LMFD").FirstOrDefault();
            var mfdConfiguration2 = module.Configurations.Where(cfg => cfg.Name == "RMFD").FirstOrDefault();
            Assert.AreEqual(Name1, mfdConfiguration1.Name);
            Assert.AreEqual(Name2, mfdConfiguration2.Name);
            Assert.AreEqual(fileName1, mfdConfiguration1.FileName);
            Assert.AreEqual(fileName1, mfdConfiguration2.FileName);
            Assert.AreEqual(true, module.Configurations.All(cf => cf.Height == height && cf.Top == top && cf.Width == width && cf.Opacity == opacity));
            Assert.AreEqual(left1, mfdConfiguration1.Left);
            Assert.AreEqual(left2, mfdConfiguration2.Left);
        }
    }
}
