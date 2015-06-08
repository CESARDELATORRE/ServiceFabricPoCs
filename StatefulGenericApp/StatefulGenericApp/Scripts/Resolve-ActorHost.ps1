<#
.SYNOPSIS 
Outputs messages indicating information about the resolved location of an actor.

.PARAMETER ServiceName
Name of the Windows Fabric Service that defines the actor.

.PARAMETER ActorId
ID of the actor to be resolved.
#>

[CmdletBinding()]
param
(
    [Parameter(Mandatory = $true)]
    [String]
    $ServiceName,

    [Parameter(Mandatory = $true)]
    [String]
    $ActorId
)

[void](Connect-WindowsFabricCluster)

Write-Output "Resolving Actor Service ${ServiceName} for Actor $ActorId"
$rsp = Resolve-WindowsFabricService -PartitionKindUniformInt64 -ServiceName $ServiceName -PartitionKey $ActorId
$replicas = Get-WindowsFabricReplica $rsp.PartitionId | where {($_.ReplicaRole -eq "Primary")}

if ($replicas.Count -gt 0)
{
    $partitionId = $rsp.PartitionId.ToString();
    $replicaId = $replicas[0].ReplicaOrInstanceId;
    $nodeName = $replicas[0].NodeName;
    $replicaAddress = $replicas[0].ReplicaAddress;
    Write-Output ""
    Write-Output "Actor $ActorId is hosted by replica $replicaId of partition $partitionId of Service ${ServiceName} on $nodeName"
    Write-Output ""
    Write-Output "Address of the ActorHost is $replicaAddress"
    Write-Output ""
}