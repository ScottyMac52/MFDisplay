using log4net;
using log4net.Core;
using System;

namespace Tests_MFDisplay.Mocks
{
    /// <summary>
    /// Mock for logging
    /// </summary>
    public class MockLogger : ILog
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDebugEnabled => true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsInfoEnabled => true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsWarnEnabled => true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsErrorEnabled => true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFatalEnabled => true;

        /// <summary>
        /// 
        /// </summary>
        public ILogger Logger => null;

        /// <summary>
        /// 
        /// </summary>
        public void Debug(object message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Debug(object message, Exception exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void DebugFormat(string format, params object[] args)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void DebugFormat(string format, object arg0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Error(object message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Error(object message, Exception exception)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void ErrorFormat(string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void ErrorFormat(string format, object arg0)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Fatal(object message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Fatal(object message, Exception exception)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void FatalFormat(string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void FatalFormat(string format, object arg0)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Info(object message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Info(object message, Exception exception)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void InfoFormat(string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void InfoFormat(string format, object arg0)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Warn(object message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Warn(object message, Exception exception)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void WarnFormat(string format, params object[] args)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void WarnFormat(string format, object arg0)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            
        }
    }
}
