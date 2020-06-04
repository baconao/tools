Clear-Host
$path='%systemroot%\system32;%systemroot%;%systemroot%\system32\wbem'
[Environment]::SetEnvironmentVariable("PATH", $path, "Machine")