<#
.SYNOPSIS 
Returns objects representing all the replicas associated with a Windows Fabric application.

.PARAMETER ApplicationManifestPath
Path to the application manifest of the Windows Fabric application.
#>

[CmdletBinding()]
param
(
    [String]
    $ApplicationManifestPath
)

$LocalFolder = (Split-Path $MyInvocation.MyCommand.Path)

if (!$ApplicationManifestPath)
{
    $ApplicationManifestPath = "$LocalFolder\..\ApplicationManifest.xml"
}

[void](Connect-WindowsFabricCluster)

$names = Get-Names -ApplicationManifestPath $ApplicationManifestPath
if (!$names)
{
    return
}

Get-WindowsFabricApplication $names.ApplicationName | Get-WindowsFabricService | Get-WindowsFabricPartition | Get-WindowsFabricReplica 