
$file = 'C:\temp\file.xml'

# method 1

$doc = New-Object System.Xml.XmlDocument
$doc.Load($file)

$node = $doc.SelectSingleNode("//Node[@Id = '1']")
$node.NodeName = $env:COMPUTERNAME
$node = $doc.SelectSingleNode("//Node[@Id = '2']")
$node.NodeName = "Member"

$doc.Save($file)

# method 2

$xml = Select-Xml -Path $file -XPath "//setting[@name='attributeName']/value"
$xml.Node.'#text' = "True"
$xml.Node.OwnerDocument.Save($xml.Path)