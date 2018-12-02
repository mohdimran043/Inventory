--dotnet ef dbcontext scaffold "Host=localhost;Database=Patrols;Username=postgres;Password=12345" Npgsql.EntityFrameworkCore.PostgreSQL -o PatrolEF -f
--AFTER UPGRADING ENTITY FRAMEWORK please OpenIdDict in patrolContext from the backup since Entity framework is not properly able to update it


--**********************************************************************************
--1st December changes
--**********************************************************************************
--CREATE OR REPLACE FUNCTION FetchEmployeeStatsByAhwal(aid integer)
-- RETURNS TABLE ( name VARCHAR, ONDUTY BIGint,ONLEAVE BIGint) 
-- AS  $BODY$ 
--BEGIN
--RETURN  QUERY
--select os.shortname as name,(select count(1)  from sectors s left  join ahwalmapping am on s.sectorid=am.sectorid
--where am.sectorid=os.sectorid and am.patrolpersonstateid  not in (80,90,100)) AS ONDUTY,
--(select count(1) from sectors s left  join ahwalmapping am on s.sectorid=am.sectorid
--where am.sectorid=os.sectorid and am.patrolpersonstateid   in (80,90,100))  AS ONLEAVE
--from sectors os where os.ahwalid=aid;															   
--END;
--$BODY$
-- LANGUAGE 'plpgsql';



---- Table: public.userpreference
---- DROP TABLE public.userpreference;
--CREATE TABLE public.userpreference
--(
--    userpreferenceid integer NOT NULL,
--    userid bigint,
--    defaulturl character varying(50) COLLATE pg_catalog."default",
--    theme character varying(10) COLLATE pg_catalog."default",
--    CONSTRAINT pk_userpreferenceid PRIMARY KEY (userpreferenceid),
--    CONSTRAINT fk_userpreferenceid_users FOREIGN KEY (userid)
--        REFERENCES public.users (userid) MATCH SIMPLE
--        ON UPDATE NO ACTION
--        ON DELETE NO ACTION
--)
--WITH (
--    OIDS = FALSE
--)
--TABLESPACE pg_default;

--ALTER TABLE public.userpreference
--    OWNER to postgres;




--insert into usersroles values (0	,"Admin")
--insert into usersroles values (10,	"ManageOrganization"
--insert into usersroles values (20,	"ManageOperation"
--insert into usersroles values (30,	"ManageDispatcher")
--insert into usersroles values (40,	"ManageMaintainance")
--insert into usersroles values (50,	"ManageScheduling")
--insert into usersroles values (60,	"ManageCharts")

--**********************************************************************************
--2nd December changes
--**********************************************************************************


--CREATE OR REPLACE FUNCTION FetchIncidentCount(aid integer)
-- RETURNS TABLE ( Month VARCHAR, IncidentCount bigint) 
-- AS  $BODY$ 
--BEGIN
--RETURN  QUERY
--select to_month(date_part('month', timestamp)::int) as Month,count(1) as IncidentCount from incidents i join usersrolesmap u on i.userid=u.userid
--where u.ahwalid=aid and date_part('year', timestamp) = date_part('year', CURRENT_DATE)
--group by to_month(date_part('month', timestamp)::int);														   
--END;
--$BODY$
--LANGUAGE 'plpgsql';

--create function to_month(integer) returns varchar as
--$$
--    select to_char(to_timestamp(to_char($1, '999'), 'MM'), 'Mon');
--$$ language sql


--CREATE OR REPLACE FUNCTION FetchPatrolStatusByAhwal(aid integer)
-- RETURNS TABLE ( name VARCHAR, patrolstatuscount BIGint) 
-- AS  $BODY$ 
--BEGIN
--RETURN  QUERY
--select ps.name,count(1) as patrolstatuscount from ahwalmapping am join patrolpersonstates ps on am.patrolpersonstateid=ps.patrolpersonstateid
--where am.ahwalid=aid
--group by ps.name;															   
--END;
--$BODY$
--LANGUAGE 'plpgsql';
--**********************************************************************************
--1st December changes
--**********************************************************************************