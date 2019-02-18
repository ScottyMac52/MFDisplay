using System;
using System.Configuration;

namespace MFDSettingsManager.Configuration
{
    /// <summary>
    /// Encapsulates a MFD configuration
    /// </summary>
    public class MFDDefaultDefintion : MFDDefintion
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public MFDDefaultDefintion()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="dc"></param>
        public MFDDefaultDefintion(MFDDefaultDefintion dc)
        {
            FileName = dc.FileName;
            Height = dc.Height;
            Width = dc.Width;
            Left = dc.Left;
            Top = dc.Top;
            Name = dc.Name;
            XOffsetStart = dc.XOffsetStart;
            XOffsetFinish = dc.XOffsetFinish;
            YOffsetStart = dc.YOffsetStart;
            YOffsetFinish = dc.YOffsetFinish;
            Opacity = dc.Opacity;
            UseCougar = dc.UseCougar;
        }

        #region Additional configuration properties

        /// <summary>
        /// Name of the Configuration, LMFD, RMFD etc.
        /// </summary>
        [ConfigurationProperty("useCougar", IsRequired = true)]
        public string UseCougar
        {
            get
            {
                return this["useCougar"] as string;
            }
            set
            {
                this["useCougar"] = value;
            }
        }

        #endregion Additional configuration properties
    }
}