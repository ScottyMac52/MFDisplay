using System;
using System.Collections;
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

            var powerUserSid = new SecurityIdentifier(WellKnownSidType.BuiltinPowerUsersSid, null);
            var userSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            var creatorOwnerSid = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);

            // This gets the built in users group
            File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : This is output from the installer, Creator: {creatorOwnerSid?.Value} PowerUser: {powerUserSid} User: {userSid} the folder: {folder}{Environment.NewLine}");
            var writerulePowerUsers = new FileSystemAccessRule(powerUserSid, FileSystemRights.FullControl, AccessControlType.Allow);
            var writeruleUsers = new FileSystemAccessRule(userSid, FileSystemRights.FullControl, AccessControlType.Allow);
            var writeruleCreator = new FileSystemAccessRule(creatorOwnerSid, FileSystemRights.FullControl, AccessControlType.Allow);

            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                // Get your file's ACL
                DirectorySecurity fsecurity = Directory.GetAccessControl(folder);

                // Add the new rule to the ACL
                fsecurity.AddAccessRule(writerulePowerUsers);
                fsecurity.AddAccessRule(writeruleUsers);
                fsecurity.AddAccessRule(writeruleCreator);

                // Set the ACL back to the file
                Directory.SetAccessControl(folder, fsecurity);

                File.AppendAllText(Path.Combine(folder, "customactions.log"), $"{Environment.NewLine}{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} : folder: {folder} was processed.{Environment.NewLine}");
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
