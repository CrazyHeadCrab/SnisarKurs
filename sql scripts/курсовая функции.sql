use barber

if exists (select * from sysobjects where name = 'reception_end_time' and type='FN')
drop function reception_end_time
go
create function reception_end_time(@record_id int)
returns time
begin
declare @time_plus int
select  @time_plus=sum(srvice_duration) from record inner join srvice on record.srvice_id = srvice.srvice_id where record_id = @record_id group by record_id
declare @reception_end_time time
set @reception_end_time  = dateadd(mi,@time_plus,(select start_time from record where record_id = @record_id))
return @reception_end_time
end 
go
go
select dbo.reception_end_time(2)
go


if exists (select * from sysobjects where name = 'free_emp_time' and type='FN')
drop function free_emp_time
go
create function free_emp_time(@employer_id int, @date date,@time time )
returns int 
begin
declare @free_emp_time int
if(exists(select record.record_date  from record where record.record_date = @date))
begin
	if (exists (
	select  record.employer_id
	from record 
	inner join schedule  on schedule.employer_id = record.employer_id and schedule.day_num = datepart(dw,record.record_date)
	where record.employer_id = @employer_id 
			and record_date =@date
			and (@time < start_time or @time >= dbo.reception_end_time(record.record_id))
			and (@time >= schedule.time_start and @time < schedule.time_end and DATEPART(dw, @date) = schedule.day_num)
	) and not exists (
	select  record.employer_id
	from record 
	inner join schedule  on schedule.employer_id = record.employer_id and schedule.day_num = datepart(dw,record.record_date)
	where record.employer_id = @employer_id 
	and (@time < schedule.time_start or @time >= schedule.time_end )))

	set @free_emp_time = 1
	ELSE
	set  @free_emp_time = 0
end
else 
begin
if exists(select schedule.employer_id 
from schedule
where @time >= schedule.time_start and @time < schedule.time_end and employer_id = @employer_id and day_num = datepart(dw,@date))
set @free_emp_time = 1
	ELSE
	set  @free_emp_time = 0
end
return @free_emp_time
end 
go
go
select dbo.free_emp_time(1,'2021-04-22','18:53:00')
go

IF EXISTS (SELECT name FROM sysobjects WHERE name = 'free_emp_time_date' AND type = 'P')
DROP PROCEDURE free_emp_time_date
GO
go
CREATE PROCEDURE free_emp_time_date
@client_id int, 
@datedate date
AS
declare @i time 
set @i = '08:00:00'
while @i != '22:00:00' 
begin
select @i, dbo.free_emp_time(@client_id,@datedate,@i)
set @i = DATEADD(hh,1,@i)
end
go


go
exec dbo.free_emp_time_date 1, '2021-04-22'
go


if exists (select * from sysobjects where name = 'all_branch_srvice' and type='FN')
drop function all_branch_srvice
go
create function all_branch_srvice(@branch_id int)
returns table
As 
return
select srvice_id 
from post_srvice
	inner join post on post.post_id = post_srvice.post_id
	inner join employer on employer.post_id = post.post_id
	inner join branch on branch.branch_id = employer.branch_id
where branch.branch_id = @branch_id
go

 




IF EXISTS (SELECT name FROM sysobjects WHERE name = 'order_creation' AND type = 'P')
DROP PROCEDURE order_creation
GO
go
CREATE PROCEDURE order_creation
@client_id int,
@employer_id int,
@branch_id int,
@start_time time,
@srvice_id int,
@record_date date
AS
declare @errormas nvarchar(1000), @errorsev int
begin try

if not exists(select srvice.srvice_id from srvice inner join dbo.all_branch_srvice(@branch_id) on srvice.srvice_id =all_branch_srvice.srvice_id and @srvice_id=srvice.srvice_id )
begin
RAISERROR('Такой услуги нет в данном отделе', 16, 1)
end

if @employer_id not in (select employer_id from employer  where employer.branch_id  = @branch_id and @employer_id =employer_id )
begin
RAISERROR('Нет такого сотрудника в данном отделе', 16, 1)
end

if @srvice_id not in (select srvice_id from post_srvice inner join employer on employer.post_id = post_srvice.post_id where employer_id = @employer_id )
begin
RAISERROR('Этот сотрудник  не может выполнять данную услугу', 16, 1)
end


if (dbo.free_emp_time(@employer_id, @record_date,@start_time) != 1)
begin
RAISERROR('Сотрудник не работает в это время', 16, 1)
end

begin tran
declare @r_id int 
set @r_id = (select max(record_id) from record)
set @r_id = @r_id +1
insert into record
	values (@r_id,@client_id,@record_date,@branch_id,@start_time,@srvice_id, @employer_id)
	commit 
end try

begin catch
if  @@TRANCOUNT > 0 ROLLBACK
select @errormas = ERROR_MESSAGE(), @errorsev = ERROR_SEVERITY()
RAISERROR(@errormas, @errorsev, 1)
end catch
go





IF EXISTS (SELECT name FROM sysobjects WHERE name = 'free_emp_time_date' AND type = 'P')
DROP PROCEDURE free_emp_time_date
GO
go
CREATE PROCEDURE free_emp_time_date
@client_id int, 
@datedate date
AS
declare @i time 


select @client_id, @datedate, times.val 
from times 
where dbo.free_emp_time(@client_id,@datedate,times.val) != 0
go


go
exec dbo.free_emp_time_date 1, '2022-04-22'
go



use barber
go
exec dbo.order_creation 1, 1, 1,'19:2:00',1, '2022-04-22' 
go

go
select log_pass.log_pass_id, employer_id, client_id
from log_pass 
	left join employer on log_pass.log_pass_id=employer.log_pass_id
	left join client on log_pass.log_pass_id=client.log_pass_id
go


if exists (select * from sysobjects where name = 'login_cheaks' and type='P')
drop proc login_cheaks
go
create proc login_cheaks
@log nvarchar(20),
@pass nvarchar(20)
as
select log_pass.log_pass_id, employer_id, client_id
from log_pass 
	left join employer on log_pass.log_pass_id=employer.log_pass_id
	left join client on log_pass.log_pass_id=client.log_pass_id
	where log_pass.logn = @log and log_pass.pass = @pass

	go
	
	 exec dbo.login_cheaks 'wex', '123'
	go