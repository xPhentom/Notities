use WpfData
go

IF EXISTS( select * from sys.tables where name = 'notitie')
begin
	drop table notitie;
end
go



CREATE TABLE dbo.notitie	
(
    id int identity(1,1) NOT NULL primary key,
    titel varchar(50) NOT NULL,
	tekst varchar(400) NULL,
	cat_id int NOT NULL
);

go 

IF EXISTS( select * from sys.tables where name = 'categorie')
begin
	drop table categorie;
end

go


CREATE TABLE dbo.categorie
(
    id int identity(1,1) NOT NULL primary key,
    titel varchar(50) NOT NULL
);

go

alter table notitie add constraint fk_categorie foreign key (cat_id )references categorie(id)