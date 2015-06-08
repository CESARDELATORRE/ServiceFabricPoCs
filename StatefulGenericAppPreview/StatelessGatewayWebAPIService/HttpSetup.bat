netsh http delete urlacl url="http://+:80/"
netsh http add urlacl url="http://+:80/" user="NT AUTHORITY\NETWORK SERVICE"
exit /b 0