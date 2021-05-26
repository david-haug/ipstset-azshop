--STORED PROCEDURE SCRIPTS for DEV,TEST	2/23/21
/* DEV SCHEMA								*/
/*********************************************
delete_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [dev].[delete_json]
END
GO

CREATE procedure [dev].[delete_json]
@table varchar(50),
@id varchar(50)
as

if(@table = 'shop')
begin
	delete from [dev].[shop] where id = @id
end

if(@table = 'user')
begin
	delete from [dev].[user] where id = @id
end

if(@table = 'product')
begin
	delete from [dev].[product] where id = @id
end

GO
/*********************************************
get_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [dev].[get_json]
END
GO

CREATE procedure [dev].[get_json]
@table varchar(50),
@id varchar(50)
as


if(@table = 'shop')
begin
	select id, [data] from [dev].[shop] where id = @id
end

if(@table = 'user')
begin
	select id, [data] from [dev].[user] where id = @id
end

if(@table = 'event')
begin
	select id, [data] from [dev].[event] where id = @id
end

if(@table = 'product')
begin
	select id, [data] from [dev].[product] where id = @id
end

GO

/*********************************************
get_json_all
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [dev].[get_json_all]
END
GO

CREATE procedure [dev].[get_json_all]
@table varchar(50),
@startAfter varchar(50) = null
as

declare @results table
(
row_id int,
id varchar(50),
[data] varchar(MAX),
date_created datetimeoffset(7)
)

if(@table = 'shop')
begin
	insert into @results
	select row_id, id, [data], date_created from [dev].[shop]
end

if(@table = 'user')
begin
	insert into @results
	select row_id, id, [data], date_created from [dev].[user]
end

if(@table = 'event')
begin
	insert into @results
	select row_id, id, [data], date_created from [dev].[event]
end

if(@table = 'product')
begin
	insert into @results
	select row_id, id, [data], date_created from [dev].[product]
end

declare @startAfterRowId int = 0
if(ISNULL(@startAfter,'') <> '')
	select @startAfterRowId = row_id from @results where id = @startAfter

select row_id as rowId, id, [data], date_created
from @results 
where row_id > @startAfterRowId
order by row_id
GO

/*********************************************
save_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [dev].[save_json] 
END
GO

CREATE procedure [dev].[save_json]
@table varchar(50),
@id varchar(50),
@data varchar(MAX)
as

if(@table = 'shop')
begin
	if (select count(id) from [dev].[shop] where id = @id) = 0
		insert into [dev].[shop] (id,[data]) values (@id,@data)
	else
		update [dev].[shop]
		set [data] = @data
		where id = @id
end

if(@table = 'user')
begin
	if (select count(id) from [dev].[user] where id = @id) = 0
		insert into [dev].[user] (id,[data]) values (@id,@data)
	else
		update [dev].[user]
		set [data] = @data
		where id = @id
end

if(@table = 'event')
begin
	if (select count(id) from [dev].[event] where id = @id) = 0
		insert into [dev].[event] (id,[data]) values (@id,@data)
	else
		update [dev].[event]
		set [data] = @data
		where id = @id
end

if(@table = 'product')
begin
	if (select count(id) from [dev].[product] where id = @id) = 0
		insert into [dev].[product] (id,[data]) values (@id,@data)
	else
		update [dev].[product]
		set [data] = @data
		where id = @id
end

/*********************************************
request_log_insert
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [dev].[request_log_insert] 
END
GO

CREATE procedure [dev].[request_log_insert]
@logDate datetime,
@parameters varchar(max),
@route varchar(1000)
as

insert into [dev].[request_log]
([LogDate], [Parameters], [Route])
values
(@logDate,
@parameters,
@route)

/* TEST SCHEMA								*/
/*********************************************
delete_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [test].[delete_json]
END
GO

CREATE procedure [test].[delete_json]
@table varchar(50),
@id varchar(50)
as

if(@table = 'shop')
begin
	delete from [test].[shop] where id = @id
end

if(@table = 'user')
begin
	delete from [test].[user] where id = @id
end

if(@table = 'product')
begin
	delete from [test].[product] where id = @id
end

GO
/*********************************************
get_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [test].[get_json]
END
GO

CREATE procedure [test].[get_json]
@table varchar(50),
@id varchar(50)
as


if(@table = 'shop')
begin
	select id, [data] from [test].[shop] where id = @id
end

if(@table = 'user')
begin
	select id, [data] from [test].[user] where id = @id
end

if(@table = 'event')
begin
	select id, [data] from [test].[event] where id = @id
end

if(@table = 'product')
begin
	select id, [data] from [test].[product] where id = @id
end

GO

/*********************************************
get_json_all
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [test].[get_json_all]
END
GO

CREATE procedure [test].[get_json_all]
@table varchar(50),
@startAfter varchar(50) = null
as

declare @results table
(
row_id int,
id varchar(50),
[data] varchar(MAX),
date_created datetimeoffset(7)
)

if(@table = 'shop')
begin
	insert into @results
	select row_id, id, [data], date_created from [test].[shop]
end

if(@table = 'user')
begin
	insert into @results
	select row_id, id, [data], date_created from [test].[user]
end

if(@table = 'event')
begin
	insert into @results
	select row_id, id, [data], date_created from [test].[event]
end

if(@table = 'product')
begin
	insert into @results
	select row_id, id, [data], date_created from [test].[product]
end

declare @startAfterRowId int = 0
if(ISNULL(@startAfter,'') <> '')
	select @startAfterRowId = row_id from @results where id = @startAfter

select row_id as rowId, id, [data], date_created
from @results 
where row_id > @startAfterRowId
order by row_id
GO

/*********************************************
save_json
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [test].[save_json] 
END
GO

CREATE procedure [test].[save_json]
@table varchar(50),
@id varchar(50),
@data varchar(MAX)
as

if(@table = 'shop')
begin
	if (select count(id) from [test].[shop] where id = @id) = 0
		insert into [test].[shop] (id,[data]) values (@id,@data)
	else
		update [test].[shop]
		set [data] = @data
		where id = @id
end

if(@table = 'user')
begin
	if (select count(id) from [test].[user] where id = @id) = 0
		insert into [test].[user] (id,[data]) values (@id,@data)
	else
		update [test].[user]
		set [data] = @data
		where id = @id
end

if(@table = 'event')
begin
	if (select count(id) from [test].[event] where id = @id) = 0
		insert into [test].[event] (id,[data]) values (@id,@data)
	else
		update [test].[event]
		set [data] = @data
		where id = @id
end

if(@table = 'product')
begin
	if (select count(id) from [test].[product] where id = @id) = 0
		insert into [test].[product] (id,[data]) values (@id,@data)
	else
		update [test].[product]
		set [data] = @data
		where id = @id
end

/*********************************************
request_log_insert
*********************************************/
BEGIN
DROP PROCEDURE IF EXISTS [test].[request_log_insert] 
END
GO

CREATE procedure [test].[request_log_insert]
@logDate datetime,
@parameters varchar(max),
@route varchar(1000)
as

insert into [test].[request_log]
([LogDate], [Parameters], [Route])
values
(@logDate,
@parameters,
@route)
