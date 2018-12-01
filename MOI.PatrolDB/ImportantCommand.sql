﻿--dotnet ef dbcontext scaffold "Host=localhost;Database=Patrols;Username=postgres;Password=12345" Npgsql.EntityFrameworkCore.PostgreSQL -o PatrolEF -f
--AFTER UPGRADING ENTITY FRAMEWORK please OpenIdDict in patrolContext from the backup since Entity framework is not properly able to update it

--CREATE OR REPLACE FUNCTION FetchEmployeeStatsByAhwal(aid integer)
-- RETURNS TABLE ( name VARCHAR, ONDUTY INT,ONLEAVE INT) AS $$
--BEGIN
--select os.shortname as name,(select count(1)  from sectors s left  join ahwalmapping am on s.sectorid=am.sectorid
--where am.sectorid=os.sectorid and am.patrolpersonstateid  not in (80,90,100)) AS ONDUTY,
--(select count(1) from sectors s left  join ahwalmapping am on s.sectorid=am.sectorid
--where am.sectorid=os.sectorid and am.patrolpersonstateid   in (80,90,100))  AS ONLEAVE
--from sectors os where os.ahwalid=aid;															   
--END;
--$$ LANGUAGE 'plpgsql';