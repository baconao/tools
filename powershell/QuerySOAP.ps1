function Execute-SOAPRequest 
( 
        [Xml]    $SOAPRequest, 
        [String] $URL 
) 
{ 
        write-host "Sending SOAP Request To Server: $URL" 
        $soapWebRequest = [System.Net.WebRequest]::Create($URL) 
        $soapWebRequest.Headers.Add("SOAPAction","`"http://db-spira2/SpiraTest/Services/v3_0/`"")

        $soapWebRequest.ContentType = "text/xml;charset=`"utf-8`"" 
        $soapWebRequest.Accept      = "text/xml" 
        $soapWebRequest.Method      = "POST" 
        
        write-host "Initiating Send." 
        $requestStream = $soapWebRequest.GetRequestStream() 
        $SOAPRequest.Save($requestStream) 
        $requestStream.Close() 
        
        write-host "Send Complete, Waiting For Response." 
        $resp = $soapWebRequest.GetResponse() 
        $responseStream = $resp.GetResponseStream() 
        $soapReader = [System.IO.StreamReader]($responseStream) 
        $ReturnXml = [Xml] $soapReader.ReadToEnd() 
        $responseStream.Close() 
        
        write-host "Response Received."

        return $ReturnXml 
}

$url = 'http://db-spira2/SpiraTest/Services/v3_0/ImportExport.svc?wsdl'
$soap = [xml]@'
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://www.w3.org/2003/05/soap-envelope">
  <soap:Body>
    <Incident_RetrieveById xmlns="http://db-spira2/SpiraTest/Services/v3_0/">
      <incidentId>13235</incidentId>      
    </Incident_RetrieveById>
  </soap12:Body>
</soap12:Envelope>
'@

Clear-Host
$ret = Execute-SOAPRequest $soap $url 
Write-Output $ret

$ret | Export-Clixml c:\temp\res.xml
