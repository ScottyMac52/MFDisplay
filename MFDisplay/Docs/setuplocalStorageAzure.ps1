$resourceGroupName = "dev-VyperTestLab"
$storageAccountName = "vyperdevstorageaccount"

Connect-AzAccount

# These commands require you to be logged into your Azure account, run Login-AzAccount if you haven't
# already logged in.
$storageAccount = Get-AzStorageAccount -ResourceGroupName $resourceGroupName -Name $storageAccountName
$storageAccountKeys = Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageAccountName

# The ComputerName, or host, is <storage-account>.file.core.windows.net for Azure Public Regions.
# $storageAccount.Context.FileEndpoint is used because non-Public Azure regions, such as sovereign clouds
# or Azure Stack deployments, will have different hosts for Azure file shares (and other storage resources).
$fileHost=[System.Uri]::new($storageAccount.Context.FileEndPoint).Host
Test-NetConnection -ComputerName $fileHost  -Port 445


# The cmdkey utility is a command-line (rather than PowerShell) tool. We use Invoke-Expression to allow us to 
# consume the appropriate values from the storage account variables. The value given to the add parameter of the
# cmdkey utility is the host address for the storage account, <storage-account>.file.core.windows.net for Azure 
# Public Regions. $storageAccount.Context.FileEndpoint is used because non-Public Azure regions, such as sovereign 
# clouds or Azure Stack deployments, will have different hosts for Azure file shares (and other storage resources).
# Invoke-Expression -Command "cmdkey /add:$([System.Uri]::new($storageAccount.Context.FileEndPoint).Host) " + "/user:AZURE\$($storageAccount.StorageAccountName) /pass:$($storageAccountKeys[0].Value)"