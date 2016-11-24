use WpfData
go

IF EXISTS( select * from sys.views where name = 'notities')
begin
	drop table notities;
end
go



CREATE TABLE dbo.Sample_Table
(
    id int identity(1,1) NOT NULL,
    titel varchar(50) NOT NULL,
	tekst varchar(50) NULL
);

go 

IF EXISTS( select * from sys.views where name = 'notities')
begin
	drop table notities;
end

go


CREATE TABLE dbo.categorie
(
    id int identity(1,1) NOT NULL,
    column_2 int NULL
);