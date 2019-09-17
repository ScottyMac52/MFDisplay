using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;

namespace CustomActions
{
    [RunInstaller(true)]
    public partial class SetUserPerms : Installer
    {
        public SetUserPerms()
        {
            InitializeComponent();
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // This gets the named parameters passed in from your custom action
            string folder = Context.Parameters["folder"];
			var cacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Vyper Industries\MFD4CTS\cache");

			if(!Directory.Exists(cacheFolder))
			{
				Directory.CreateDirectory(cacheFolder);
				File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : Created cache folder: {cacheFolder}.");
			}

			if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
			{
				// Get your file's ACL
				DirectorySecurity fsecurity = Directory.GetAccessControl(folder);
				DirectorySecurity cacheSecurity = Directory.GetAccessControl(cacheFolder);

				File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : Processing: {folder}");
				File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : Processing: {cacheFolder}");
				var sidsToProcess = new SortedList<string, SecurityIdentifier>()
				{
					{ "PowerUser", new SecurityIdentifier(WellKnownSidType.BuiltinPowerUsersSid, null) },
					{ "Users", new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null) },
					{ "CreatorOwner", new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null) }
				};

				foreach(var sid in sidsToProcess)
				{
					var fullControlRule = new FileSystemAccessRule(sid.Value, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow);
					File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : Adding SID: {sid.Value?.Value ?? "Unknown"} Named: {sid.Key}");
					fsecurity.AddAccessRule(fullControlRule);
					cacheSecurity.AddAccessRule(fullControlRule);
				}

				Directory.SetAccessControl(folder, fsecurity);
				File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : folder: {folder} was processed for full control.");
				Directory.SetAccessControl(cacheFolder, cacheSecurity);
				File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : folder: {cacheFolder} was processed for full control.{Environment.NewLine}");
			}
		}

        [SecurityPermission(SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            var site = Context.Parameters["site"] ?? "https://github.com/ScottyMac52/MFDisplay/wiki";
            System.Diagnostics.Process.Start(site);
        }
    }
}
