use WpfData
go

IF EXISTS( select * from sys.tables where name = 'notitie')
begin
	drop table notitie;
end
go



CREATE TABLE dbo.notitie
(
    id int identity(1,1) NOT NULL,
    titel varchar(50) NOT NULL,
	tekst varchar(50) NULL
);

go 

IF EXISTS( select * from sys.tables where name = 'categorie')
begin
	drop table categorie;
end

go


CREATE TABLE dbo.categorie
(
    id int identity(1,1) NOT NULL,
    titel int NULL
);