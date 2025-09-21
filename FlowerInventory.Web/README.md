
FlowerInventory – Local Data Stack (SQL Server + MinIO)

Spin up a Microsoft SQL Server with optional one-shot seeding and a MinIO S3 bucket preloaded with images.
Designed to be idempotent, easy to reset, and friendly to both EF Core and manual SQL workflows.


dotnet ef migrations script -o migration.sql

```` dockerfile
docker exec -it mssql bash -lc "
>>   set +H                                                                                                                                                      
>>   /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1,1433 -U sa -P 'YourStrong!Passw0rd' -C -l 5 -b -i /sql/init.sql                                                  
>> "                                     
````