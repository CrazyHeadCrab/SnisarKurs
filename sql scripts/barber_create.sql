go
if exists (select * from sys.databases where name = 'barber') 
drop database barber
go
go
create database barber
ON PRIMARY 
( NAME= SnisarDate,FILENAME= "D:\databases\barberdate.mdf", SIZE=3 MB, MAXSIZE=100, FILEGROWTH=10) 
LOG ON 
(NAME=Snisarlog,FILENAME= "D:\databases\barberslog.mdf", SIZE=3MB, MAXSIZE=100, FILEGROWTH=10) 
go
go
use barber

create table log_pass
(
log_pass_id int not null,
logn nvarchar(20) not null Unique,
pass nvarchar(20) not null,
income_lvl varchar(1) not null check (income_lvl like ('[ABC]')),
primary key(log_pass_id)
)


create table client
(
client_id int not null ,
cl_name nvarchar(20) not null,
cl_surname nvarchar(20) not null,
cl_patronic nvarchar(20) null,
cl_email varchar(30)  null check (cl_email like('%@%.%')),
cl_phone varchar(12)  null check (cl_phone like('+7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')),
log_pass_id int not null,
primary key(client_id),
foreign key (log_pass_id) references log_pass(log_pass_id)
)
create table town
(
town_id int not null ,
town_name nvarchar(30) not null,
primary key (town_id)
)
create table branch
(
branch_id int not null ,
town_id int not null,
br_address nvarchar(80) null,
br_name nvarchar (40) null,
primary key (branch_id),
foreign key(town_id) references town(town_id)
)
create table post
(
post_id int not null ,
post_name nvarchar(50) not null,
salary money not null,
primary key (post_id)
)
create table employer
(
employer_id int not null ,
emp_name nvarchar(20) not null,
emp_surname nvarchar(20) not null,
emp_patronic nvarchar(20) null,
branch_id int not null,
post_id int not null,
log_pass_id int not null,
count_stars float not null check (count_stars >= 1 and count_stars <=5) DEFAULT 4,

primary key (employer_id),
foreign key (branch_id) references branch(branch_id),
foreign key (post_id) references post(post_id),
foreign key (log_pass_id) references log_pass(log_pass_id)
)
create table srvice
(
srvice_id int not null ,
srvice_name nvarchar(50) not null,
srvice_cost money not null,
srvice_duration int null,
primary key (srvice_id)
)
create table record
(
record_id int not null ,
client_id int not null,
record_date date not null,
branch_id int not null,
start_time time not null,
srvice_id int not null ,
employer_id int not null ,
primary key (record_id),
foreign key (client_id) references client(client_id),
foreign key (branch_id) references branch(branch_id),
foreign key (srvice_id) references srvice(srvice_id),
foreign key (employer_id) references employer(employer_id),
)
create table post_srvice
(
post_id int not null,
srvice_id int not null ,
primary key (post_id, srvice_id),
foreign key (post_id) references post(post_id),
foreign key (srvice_id) references srvice(srvice_id),
)

create table schedule
(
employer_id int not null ,
day_num int not null  check (day_num >= 1 and day_num <=7),
time_start time not null,
time_end time not null
primary key(employer_id, day_num),
foreign key (employer_id) references employer(employer_id)
)



create table times
(
val time not null,
primary key (val)
)
create table review
(
review_id int not null  identity(1,1),
record_id int not null ,
count_stars float not null check (count_stars >= 1 and count_stars <=5),
review nvarchar(500) null,
primary key(review_id),
foreign key (record_id) references record(record_id)
)
go


create trigger update_count_stars 
on review
for insert, update
as 
declare @emp_id int
set @emp_id = (select employer_id from inserted inner join record on record.record_id =inserted.record_id)
update employer
set count_stars = (select ((sum(count_stars)+4)/(count(count_stars)+1)) from review inner join record on record.record_id =review.record_id where record.employer_id = @emp_id )
where employer_id = @emp_id

go
use barber

insert into  log_pass
	values  (1, 'wex', '123', 'B'),
			(2, 'exort', '123', 'B'),
			(3, 'qwas', '123', 'B'),
			(4, 'blast', '123', 'B'),
			(5, 'kotleta', '123', 'B'),
			(6, 'tornado', '123', 'B'),
			(7, 'inviz', '123', 'B'),
			(8, 'cold', '123', 'B'),
			(9, 'alacriti', '123', 'B'),
			(10, 'manableed', '123', 'B'),
			(11, 'invoker', '123', 'B'),
			(12, 'sunstrike', '123', 'B'),
			(13, 'oganim', '123', 'B'),
			(14, 'midas', '123', 'B'),
			(15, 'hex', '123', 'B'),
			(16, 'phase', '123', 'B'),
			(17, 'blink', '123', 'B'),
			(18, 'satanic', '123', 'B'),
			(19, 'tangusi', '123', 'B'),
			(20, 'null', '123', 'B'),
			(21, 'amulet', '123', 'C'),
			(22, 'sasha', '123', 'C'),
			(23, 'ysha', '123', 'C'),
			(24, 'Kya', '123', 'C'),
			(26, 'maddnes', '123', 'C'),
			(25, 'meteor', '123', 'C'),
			(27, 'blade', '123', 'C'),
			(28, 'mail', '123', 'C'),
			(29, 'flasca', '123', 'C'),
			(30, 'force', '123', 'C'),
			(31, 'stuffic', '123', 'C'),
			(32, 'crystaliz', '123', 'C'),
			(33, 'daydalus', '123', 'C'),
			(34, 'Axe', '123', 'C'),
			(35, 'bloodtorn', '123', 'C'),
			(36, 'celesta', '123', 'C'),
			(37, 'mount', '123', 'C'),
			(38, 'medalin', '123', 'C'),
			(39, 'mango', '123', 'C'),
			(40, 'gem', '123', 'C'),
			(41, 'kokos', '123' ,'A')
		

insert into town
	values  (1,'??????'),
			(2,'?????-?????????'),
			(3,'???????'),
			(4,'????????'),
			(5,'???????????')

insert into branch(branch_id ,town_id ,br_address,br_name )
	values  (1,1,'??. ????????, ??? 15', 'FoxTail'),
			(2,1,'??. ???????, ??? 3', 'FoxTail'),
			(3,2,'??. ??????????, ??? 9', 'FoxTail'),
			(4,2,'??. ??????, ??? 31', 'FoxTail'),
			(5,3,'??. ???????, ??? 1', 'FoxTail'),
			(6,3,'??. ???????? ??????? ????, ??? 21', 'FoxTail'),
			(7,4,'??. ???????????, ??? 6', 'FoxTail'),
			(8,4,'??. ??????, ??? 11', 'FoxTail'),
			(9,5,'??. ?????????, ??? 32', 'FoxTail'),
			(10,5,'??. ??????????, ??? 19', 'FoxTail')
insert into post
	values  (1,'??????????',20000),
			(2,'????????',15000),
			(3,'????????????? ????????',30000),
			(4,'?????????',30000),
			(5,'?????? ????????? ???????',25000),
			(6,'????????',100000)

insert into employer(employer_id,emp_name,emp_surname,emp_patronic,branch_id,post_id,log_pass_id)
	values  (1,'???????','??????','?????????????',1,1,1),
			(2,'????','??????','????????',1,2,2),
			(3,'???????','??????','?????????',2,3,3),
			(4,'????','??????','?????????',2,2,4),
			(5,'????','????????','????????',3,1,5),
			(6,'??????','??????','????????',3,2,6),
			(7,'???????','???????','????????',4,1,7),
			(8,'???????','??????','????????',4,1,8),
			(9,'?????','????????','??????????',5,3,9),
			(10,'?????','???????','????????',5,3,10),
			(11,'???????','????????','????????',6,2,11),
			(12,'???????','????????','????????',6,2,12),
			(13,'?????????','???????','????????',7,3,13),
			(14,'???????','???????','????????',7,3,14),
			(15,'?????????','????????','?????????',8,3,15),
			(16,'??????','??????????','??????????',8,2,16),
			(17,'?????','?????????','????????',9,3,17),
			(18,'?????','????????','?????????????',9,3,18),
			(19,'?????','???????','??????????',10,1,19),
			(20,'???????','?????????','?????????',10,2,20),
			(21,'???????????','??????','?????????',10,2,41)

insert into srvice
	values  (1, '??????? ??????', 1000, 60),
			(2, '??????? ??????', 1200, 60),
			(3, '?????? ???????', 500, 30),
			(4, '?????? ?????', 1300, 40),
			(5, '???????', 1000, 60),
			(6, '???????', 1000, 60),
			(7, '????????????? ??????', 2000, 60),
			(8, '??????? ?????? ? ????', 1000, 60),
			(9, '??????? ?????', 500, 60),
			(10, '??????', 1500, 60),
			(11, '??????', 2500, 60)
insert into post_srvice
	values  (1,2),
			(1,1),
			(1,3),
			(1,4),
			(1,7),
			(1,8),
			(1,9),
			(2,10),
			(4,11),
			(5,5),
			(5,6),
			(3,1),
			(3,2),
			(3,3),
			(3,4),
			(3,5),
			(3,6),
			(3,7),
			(3,8),
			(3,9),
			(3,10),
			(3,11)

insert into client(client_id, cl_surname, cl_name, cl_patronic,log_pass_id)
	values  (1,'???????', '????', '???????',21),
			(2,'?????????','???', '?????????',22),
			(3,'??????', '?????????', '?????????????',23),
			(4,'??????', '??????', '??????????',24),
			(5,'????????', '???????', '??????????',25),
			(6,'???????', '????????', '????????????',26),
			(7,'?????????', '????', '?????????',27),
			(8,'???????', '?????', '???????',28),
			(9,'??????????', '????', '?????????',29),
			(10,'????????', '??????', '?????????',30),
			(11,'?????', '??????', '??????????',31),
			(12,'??????', '????', '??????????',32),
			(13,'??????', '????', '??????????',33),
			(14,'????????', '???', '??????????',34),
			(15,'?????', '???????', '??????????',35),
			(16,'????????', '????????', '???????????',36),
			(17,'??????', '??????', '??????????',37),
			(18,'?????', '???????', '????????',38),
			(19,'?????????', '?????', '???????????',39),
			(20,'????????', '?????', '??????????',40)

insert into record
	values  (1,1,DATEADD(month, 1, getdate()),1,'17:00:00',2,1),
			(2,2,DATEADD(day, 7, getdate()),1,'13:00:00',10,2),
			(3,3,DATEADD(day, 13, getdate()),2,'15:00:00',4,3),
			(4,4,DATEADD(day, 15, getdate()),2,'10:00:00',10,4),
			(5,5,DATEADD(day, 2, getdate()),3,'18:00:00',2,5),
			(6,6,DATEADD(day, 1, getdate()),3,'13:00:00',10,6),
			(7,7,DATEADD(day, 3, getdate()),4,'15:00:00',8,7),
			(8,8,DATEADD(day, 5, getdate()),4,'16:00:00',2,8),
			(9,9,DATEADD(day, 7, getdate()),5,'19:00:00',11,9),
			(10,10,DATEADD(day, 2, getdate()),5,'17:00:00',11,10),
			(11,11,DATEADD(day, 6, getdate()),6,'11:00:00',7,11),
			(12,12,DATEADD(month, 1, getdate()),6,'12:00:00',10,12),
			(13,13,DATEADD(day, 9, getdate()),7,'14:00:00',5,13),
			(14,14,DATEADD(day, 6, getdate()),7,'18:00:00',6,14),
			(15,15,DATEADD(day, 8, getdate()),8,'14:00:00',9,15),
			(16,16,DATEADD(day, 13, getdate()),8,'12:00:00',10,16),
			(17,17,DATEADD(day, 11, getdate()),9,'16:00:00',3,17),
			(18,18,DATEADD(month, 1, getdate()),9,'18:00:00',4,18),
			(19,19,DATEADD(day, 5, getdate()),10,'10:00:00',1,19),
			(20,20,DATEADD(day, 1, getdate()),10,'13:00:00',10,20),
			(21,1,DATEADD(month, -1, getdate()),1,'17:00:00',2,1)


insert into schedule
	values  (1,1,'09:00','21:00'),
			(1,2,'09:00','21:00'),
			(1,3,'09:00','21:00'),
			(1,4,'09:00','21:00'),
			(1,5,'09:00','21:00'),
			(1,6,'09:00','21:00'),
			(1,7,'09:00','21:00'),
			(2,1,'09:00','21:00'),
			(2,2,'09:00','21:00'),
			(2,3,'09:00','21:00'),
			(2,4,'09:00','21:00'),
			(2,5,'09:00','21:00'),
			(2,6,'09:00','21:00'),
			(3,1,'09:00','21:00'),
			(3,2,'09:00','21:00'),
			(3,3,'09:00','21:00'),
			(3,4,'09:00','21:00'),
			(3,5,'09:00','21:00'),
			(3,6,'09:00','21:00'),
			(4,1,'09:00','21:00'),
			(4,2,'09:00','21:00'),
			(4,3,'09:00','21:00'),
			(4,4,'09:00','21:00'),
			(4,5,'09:00','21:00'),
			(4,6,'09:00','21:00'),
			(5,1,'09:00','21:00'),
			(5,2,'09:00','21:00'),
			(5,3,'09:00','21:00'),
			(5,4,'09:00','21:00'),
			(5,5,'09:00','21:00'),
			(5,6,'09:00','21:00'),
			(6,1,'09:00','21:00'),
			(6,2,'09:00','21:00'),
			(6,3,'09:00','21:00'),
			(6,4,'09:00','21:00'),
			(6,5,'09:00','21:00'),
			(6,6,'09:00','21:00'),
			(7,1,'09:00','21:00'),
			(7,2,'09:00','21:00'),
			(7,3,'09:00','21:00'),
			(7,4,'09:00','21:00'),
			(7,5,'09:00','21:00'),
			(7,6,'09:00','21:00'),
			(8,1,'09:00','21:00'),
			(8,2,'09:00','21:00'),
			(8,3,'09:00','21:00'),
			(8,4,'09:00','21:00'),
			(8,5,'09:00','21:00'),
			(8,6,'09:00','21:00'),
			(9,1,'09:00','21:00'),
			(9,2,'09:00','21:00'),
			(9,3,'09:00','21:00'),
			(9,4,'09:00','21:00'),
			(9,5,'09:00','21:00'),
			(9,6,'09:00','21:00'),
			(10,1,'09:00','21:00'),
			(10,2,'09:00','21:00'),
			(10,3,'09:00','21:00'),
			(10,4,'09:00','21:00'),
			(10,5,'09:00','21:00'),
			(10,6,'09:00','21:00'),
			(11,1,'09:00','21:00'),
			(11,2,'09:00','21:00'),
			(11,3,'09:00','21:00'),
			(11,4,'09:00','21:00'),
			(11,5,'09:00','21:00'),
			(11,6,'09:00','21:00'),
			(12,1,'09:00','21:00'),
			(12,2,'09:00','21:00'),
			(12,3,'09:00','21:00'),
			(12,4,'09:00','21:00'),
			(12,5,'09:00','21:00'),
			(12,6,'09:00','21:00'),
			(12,7,'09:00','21:00'),
			(13,1,'09:00','21:00'),
			(13,2,'09:00','21:00'),
			(13,3,'09:00','21:00'),
			(13,4,'09:00','21:00'),
			(13,5,'09:00','21:00'),
			(13,6,'09:00','21:00'),
			(13,7,'09:00','21:00'),
			(14,1,'09:00','21:00'),
			(14,2,'09:00','21:00'),
			(14,3,'09:00','21:00'),
			(14,4,'09:00','21:00'),
			(14,5,'09:00','21:00'),
			(14,6,'09:00','21:00'),
			(14,7,'09:00','21:00'),
			(15,1,'09:00','21:00'),
			(15,2,'09:00','21:00'),
			(15,3,'09:00','21:00'),
			(15,4,'09:00','21:00'),
			(15,5,'09:00','21:00'),
			(15,6,'09:00','21:00'),
			(15,7,'09:00','21:00'),
			(16,1,'09:00','21:00'),
			(16,2,'09:00','21:00'),
			(16,3,'09:00','21:00'),
			(16,4,'09:00','21:00'),
			(16,5,'09:00','21:00'),
			(16,6,'09:00','21:00'),
			(16,7,'09:00','21:00'),
			(17,1,'09:00','21:00'),
			(17,2,'09:00','21:00'),
			(17,3,'09:00','21:00'),
			(17,4,'09:00','21:00'),
			(17,5,'09:00','21:00'),
			(17,6,'09:00','21:00'),
			(17,7,'09:00','21:00'),
			(18,1,'09:00','21:00'),
			(18,2,'09:00','21:00'),
			(18,3,'09:00','21:00'),
			(18,4,'09:00','21:00'),
			(18,5,'09:00','21:00'),
			(18,6,'09:00','21:00'),
			(18,7,'09:00','21:00'),
			(19,1,'09:00','21:00'),
			(19,2,'09:00','21:00'),
			(19,3,'09:00','21:00'),
			(19,4,'09:00','21:00'),
			(19,5,'09:00','21:00'),
			(19,6,'09:00','21:00'),
			(19,7,'09:00','21:00'),
			(20,1,'09:00','21:00'),
			(20,2,'09:00','21:00'),
			(20,3,'09:00','21:00'),
			(20,4,'09:00','21:00'),
			(20,5,'09:00','21:00'),
			(20,6,'09:00','21:00'),
			(20,7,'09:00','21:00')

insert into times
	values  ('00:00'),
			('01:00'),
			('02:00'),
			('03:00'),
			('04:00'),
			('05:00'),
			('06:00'),
			('07:00'),
			('08:00'),
			('09:00'),
			('10:00'),
			('11:00'),
			('12:00'),
			('13:00'),
			('14:00'),
			('15:00'),
			('16:00'),
			('17:00'),
			('18:00'),
			('19:00'),
			('20:00'),
			('21:00'),
			('22:00'),
			('23:00')


go