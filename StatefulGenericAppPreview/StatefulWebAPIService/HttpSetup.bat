netsh http delete urlacl url="http://+:80/"
netsh http delete urlacl url="http://+:80/data"
netsh http add urlacl url="http://+:80/" user="NT AUTHORITY\NETWORK SERVICE"
netsh http add urlacl url="http://+:80/data" user="NT AUTHORITY\NETWORK SERVICE"
exit /b 0