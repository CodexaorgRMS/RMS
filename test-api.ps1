$body = @{
    name = "Test Customer"
    phone = "0123456789"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:5152/api/customers" -Method Post -ContentType "application/json" -Body $body -StatusCodeVariable "status" -ResponseHeadersVariable "headers"

Write-Output "Status: $status"
Write-Output "Headers:"
foreach ($key in $headers.Keys) { Write-Output "$key : $($headers[$key])" }
Write-Output "Body:"
$response | ConvertTo-Json -Depth 10
