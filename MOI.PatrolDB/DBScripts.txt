ALTER TABLE PatrolCars Add delflag character(1);


CREATE TABLE "OpenIddictApplications"(
	"ClientId" Varchar(450) NOT NULL,
	"ClientSecret" Text NULL,
	"ConcurrencyToken" Text NULL,
	"ConsentType" Text NULL,
	"DisplayName" Text NULL,
	"Id" Varchar(450) NOT NULL,
	"Permissions" Text NULL,
	"PostLogoutRedirectUris" Text NULL,
	"Properties" Text NULL,
	"RedirectUris" Text NULL,
	"Type" Text NOT NULL,
 CONSTRAINT PK_OpenIddictApplications PRIMARY KEY 
(
	"Id" 
) 
); 
 
CREATE TABLE "OpenIddictAuthorizations"(
	"ApplicationId" Varchar(450) NULL,
	"ConcurrencyToken" Text NULL,
	"Id" Varchar(450) NOT NULL,
	"Properties" Text NULL,
	"Scopes" Text NULL,
	"Status" Text NOT NULL,
	"Subject" Text NOT NULL,
	"Type" Text NOT NULL,
 CONSTRAINT PK_OpenIddictAuthorizations PRIMARY KEY 
(
	"Id"
) 
); 
 
CREATE TABLE "OpenIddictScopes"(
	"ConcurrencyToken" Text NULL,
	"Description" Text NULL,
	"DisplayName" Text NULL,
	"Id" Varchar(450) NOT NULL,
	"Name" Varchar(450) NOT NULL,
	"Properties" Text NULL,
	"Resources" Text NULL,
 CONSTRAINT PK_OpenIddictScopes PRIMARY KEY 
(
	"Id" 
) 
);
 
CREATE TABLE "OpenIddictTokens"(
	"ApplicationId" Varchar(450) NULL,
	"AuthorizationId" Varchar(450) NULL,
	"CreationDate" Timestamp(6) WITH TIME ZONE NULL,
	"ExpirationDate" Timestamp(6) WITH TIME ZONE NULL,
	"ConcurrencyToken" Text NULL,
	"Id" Varchar(450) NOT NULL,
	"Payload" Text NULL,
	"Properties" Text NULL,
	"ReferenceId" Varchar(450) NULL,
	"Status" Text NULL,
	"Subject" Text NOT NULL,
	"Type" Text NOT NULL,
 CONSTRAINT PK_OpenIddictTokens PRIMARY KEY 
(
	"Id"
) 
); 



ALTER TABLE "OpenIddictAuthorizations"    ADD  CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId" FOREIGN KEY("ApplicationId")
REFERENCES "OpenIddictApplications"("Id")

ALTER TABLE "OpenIddictTokens"    ADD  CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId" FOREIGN KEY("ApplicationId")
REFERENCES "OpenIddictApplications" ("Id")

ALTER TABLE "OpenIddictTokens"    ADD  CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId" FOREIGN KEY("AuthorizationId")
REFERENCES "OpenIddictAuthorizations" ("Id")
