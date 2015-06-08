<#
.SYNOPSIS 
Restarts the Windows Fabric replica hosting the specified actor.

.PARAMETER ServiceName
Name of the Windows Fabric Service that defines the actor.

.PARAMETER ActorId
ID of the actor to be resolved.

.PARAMETER HasPersistedState
A value indicating whether the service has persisted state.
#>

[CmdletBinding()]
param
(
    [Parameter(Mandatory = $true)]
    [String]
    $ServiceName,

    [Parameter(Mandatory = $true)]
    [String]
    $ActorId,

    [Boolean]
    $HasPersistedState = $false
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
    Write-Output ""
    Write-Output "Restarting Actor Host ${ServiceName}:$partitionId on $nodeName"
    if ($HasPersistedState)
    {
        Restart-WindowsFabricReplica -NodeName $nodeName -PartitionId $partitionId -ReplicaOrInstanceId $replicaId
    }
    else
    {
        Remove-WindowsFabricReplica -NodeName $nodeName -PartitionId $partitionId -ReplicaOrInstanceId $replicaId
    }

    Write-Output ""
}