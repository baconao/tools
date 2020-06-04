
Clear-Host

$computer = $env:COMPUTERNAME
$namespace = "ROOT\Microsoft\HomeNet"
$classname = "HNet_Connection"

Write-Output "====================================="
Write-Output "COMPUTER : $computer "
Write-Output "CLASS    : $classname "
Write-Output "====================================="

Get-WmiObject -Class $classname -ComputerName $computer -Namespace $namespace |
    Select-Object * -ExcludeProperty PSComputerName, Scope, Path, Options, ClassPath, Properties, SystemProperties, Qualifiers, Site, Container |
    Format-List -Property [a-z]*