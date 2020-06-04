
Clear-Host
$User = 'dbMotion\bldadm'
$PWord = ConvertTo-SecureString –String '1qaz@@wsx' –AsPlainText -Force
$Credential = New-Object –TypeName System.Management.Automation.PSCredential –ArgumentList $User, $PWord
$args = '/c rmdir /Q /S \\jenbld1\backup\test'
Start-Process -FilePath $Env:ComSpec -ArgumentList $args -Credential $Credential -NoNewWindow -Wait -Verbose  -WorkingDirectory C:\temp