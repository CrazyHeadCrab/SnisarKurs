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
if(exists(select record.record_date  from record where record.record_date = @date and @employer_id=employer_id))
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
select dbo.free_emp_time(1,'2021-02-22','18:53:00')
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

 

all_branch_srvice(1)


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
select log_pass.log_pass_id, employer_id, client_id, income_lvl
from log_pass 
	left join employer on log_pass.log_pass_id=employer.log_pass_id
	left join client on log_pass.log_pass_id=client.log_pass_id
	where log_pass.logn = @log and log_pass.pass = @pass

	go
	
	 exec dbo.login_cheaks 'wex', '123'
	go



if exists (select * from sysobjects where name = 'regist_client' and type='P')
drop proc regist_client
go
create proc regist_client
@log nvarchar(20),
@pass nvarchar(20),
@name nvarchar(20),
@sur_n nvarchar(20),
@patr_n nvarchar(20),
@email nvarchar(30),
@phone nvarchar(12)
as
begin try
if not(@email like('%@%.%') or @email = 'null')
begin
	raiserror('Неправильный емаил',16,1)
end

if  not(@phone like('+7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]') or @phone = 'null')
begin
	raiserror('неправильный номер',16,1)
end

declare @email2 nvarchar(30)
declare @phone2 nvarchar(12)
declare @patr_n2 nvarchar(20)

if (@patr_n = 'null') 
begin
	set @patr_n2 = null
end
else
begin 
	set @patr_n2 = @patr_n
end

if (@email = 'null') 
begin
	set @email2 = null
end
else
begin 
	set @email2 = @email
end

if (@phone = 'null') 
begin
	set @phone2 = null
end
else
begin 
	set @phone2 = @phone
end

begin tran
insert into log_pass
	values ((select max(log_pass_id)from log_pass)+1, @log, @pass, 'C')
insert into client
	values ((select max(client_id)from client)+1,@name,@sur_n,@patr_n,@email2,@phone2,(select max(log_pass_id)from log_pass))
commit

end try

begin catch
declare @errormas nvarchar(50)
declare @errorsev int
if  @@TRANCOUNT > 0 ROLLBACK
select @errormas = ERROR_MESSAGE(), @errorsev = ERROR_SEVERITY()
RAISERROR(@errormas, @errorsev, 1)
end catch
go



if exists (select * from sysobjects where name = 'all_srvice_post' and type='FN')
drop function all_srvice_post
go
create function all_srvice_post(@srvice_id int)
returns table
As 
return
select post_srvice.post_id 
from post_srvice
	inner join srvice on srvice.srvice_id = post_srvice.srvice_id
where srvice.srvice_id = @srvice_id
go


select post_srvice.post_id 
from post_srvice
	inner join srvice on srvice.srvice_id = post_srvice.srvice_id
where srvice.srvice_id = 1

 
 select srvice.srvice_id, srvice_name, srvice_cost from srvice inner join dbo.all_branch_srvice(1) on all_branch_srvice.srvice_id = srvice.srvice_id 


 select employer_id, emp_name, emp_surname, emp_patronic, post_name, employer.post_id from employer inner join branch on branch.branch_id = employer.branch_id inner join dbo.all_srvice_post(1) on employer.post_id = all_srvice_post.post_id inner join post on post.post_id = employer.post_id where employer.branch_id =1







 
select times.val from times where dbo.free_emp_time(7,'2021-5-7',times.val) != 0

select dbo.free_emp_time(7,'2021-5-21','10:00:00')
select times.val from times where dbo.free_emp_time(8,'2021-5-20',times.val) != 0
select dbo.free_emp_time(3,'2021-5-10','10:00:00')
go

/*
create trigger update_count_stars 
on record
for insert, update
as
*/


if exists (select * from sysobjects where name = 'all_client_record' and type='FN')
drop function all_client_record
go
create function all_client_record(@client_id int)
returns table
As 
return
SELECT        dbo.record.record_id, dbo.record.employer_id, dbo.record.client_id, dbo.record.branch_id, dbo.record.srvice_id, dbo.record.start_time, dbo.record.record_date, dbo.srvice.srvice_name, dbo.srvice.srvice_cost, 
                         dbo.branch.branch_id AS Expr1, dbo.branch.br_address, dbo.employer.emp_name, dbo.employer.emp_surname, dbo.post.post_name, town_name
FROM            dbo.record INNER JOIN
                         dbo.branch   ON dbo.record.branch_id = dbo.branch.branch_id INNER JOIN
                         dbo.town ON dbo.branch.town_id = dbo.town.town_id INNER JOIN
                         dbo.employer ON dbo.record.employer_id = dbo.employer.employer_id AND dbo.branch.branch_id = dbo.employer.branch_id INNER JOIN
                         dbo.srvice ON dbo.record.srvice_id = dbo.srvice.srvice_id INNER JOIN
                         dbo.post ON dbo.employer.post_id = dbo.post.post_id
						 where dbo.record.client_id = @client_id
go


						 select * from  all_client_record(1) 
