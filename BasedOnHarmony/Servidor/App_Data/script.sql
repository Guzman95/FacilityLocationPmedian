/****** Object:  Table [dbo].[DistributeTask]    Script Date: 08/25/2016 10:56:25 ******/

DROP TABLE IF EXISTS dbo.[DistributeTask]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DistributeTask] (
    [Dt_Id]          BIGINT  NOT NULL IDENTITY(1,1),
    [Dt_Problem]     VARCHAR (300) NOT NULL,
	[Dt_Seed]        INT NOT NULL,
    [Dt_Algorithm]   VARCHAR (100) NOT NULL,
	[Dt_Date] [datetime] NULL,
	[Dt_Optimal] INT   NOT NULL,
	[Dt_Result_Best] NUMERIC (18)  NULL,
    [Dt_Status]     VARCHAR (1)   NULL,
    CONSTRAINT [PK_DistributeTask] PRIMARY KEY CLUSTERED ([Dt_Id] ASC)
);

declare @cnt int = 1;

declare @opt1  int=     5819;
declare @opt2  int=     4093;
declare @opt3   int=    4250;
declare @opt4   int=    3034;
declare @opt5   int=    1355;
declare @opt6    int=   7824;
declare @opt7    int=   5631;
declare @opt8    int=   4445;
declare @opt9    int=   2734;
declare @opt10    int=  1255;
declare @opt11    int=  7696;
declare @opt12     int= 6634;
declare @opt13     int= 4374;
declare @opt14      int=2968;
declare @opt15      int=1729;
declare @opt16      int=8162;
declare @opt17      int=6999;
declare @opt18      int=4809;
declare @opt19      int=2845;
declare @opt20      int=1789;
declare @opt21      int=9138;
declare @opt22      int=8579;
declare @opt23      int=4619;
declare @opt24      int=2961;
declare @opt25      int=1828;
declare @opt26      int=9917;
declare @opt27      int=8307;
declare @opt28      int=4498;
declare @opt29      int=3033;
declare @opt30      int=1989;

while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed1', @cnt, 'SBHS', @opt1, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;

while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed1', @cnt, 'HSOS', @opt1, 'N') 
	set @cnt = @cnt+1;
end

set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed2', @cnt, 'SBHS', @opt2, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed2', @cnt, 'HSOS', @opt2, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed3', @cnt, 'SBHS', @opt3, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed3', @cnt, 'HSOS', @opt3, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed4', @cnt, 'SBHS', @opt4, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed4', @cnt, 'HSOS', @opt4, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed5', @cnt, 'SBHS', @opt5, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed5', @cnt, 'HSOS', @opt5, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed6', @cnt, 'SBHS', @opt6, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed6', @cnt, 'HSOS', @opt6, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed7', @cnt, 'SBHS', @opt7, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed7', @cnt, 'HSOS', @opt7, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed8', @cnt, 'SBHS', @opt8, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed8', @cnt, 'HSOS', @opt8, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed9', @cnt, 'SBHS', @opt9, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed9', @cnt, 'HSOS', @opt9, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed10', @cnt, 'SBHS', @opt10, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed10', @cnt, 'HSOS', @opt10, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed11', @cnt, 'SBHS', @opt11, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed11', @cnt, 'HSOS', @opt11, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed12', @cnt, 'SBHS', @opt12, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed12', @cnt, 'HSOS', @opt12, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed13', @cnt, 'SBHS', @opt13, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed13', @cnt, 'HSOS', @opt13, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed14', @cnt, 'SBHS', @opt14, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed14', @cnt, 'HSOS', @opt14, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed15', @cnt, 'SBHS', @opt15, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed15', @cnt, 'HSOS', @opt15, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed16', @cnt, 'SBHS', @opt16, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed16', @cnt, 'HSOS', @opt16, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed17', @cnt, 'SBHS', @opt17, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed17', @cnt, 'HSOS', @opt17, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed18', @cnt, 'SBHS', @opt18, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed18', @cnt, 'HSOS', @opt18, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed19', @cnt, 'SBHS', @opt19, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed19', @cnt, 'HSOS', @opt19, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed20', @cnt, 'SBHS', @opt20, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed20', @cnt, 'HSOS', @opt20, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed21', @cnt, 'SBHS', @opt21, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed21', @cnt, 'HSOS', @opt21, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed22', @cnt, 'SBHS', @opt22, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed22', @cnt, 'HSOS', @opt22, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed23', @cnt, 'SBHS', @opt23, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed23', @cnt, 'HSOS', @opt23, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed24', @cnt, 'SBHS', @opt24, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed24', @cnt, 'HSOS', @opt24, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed25', @cnt, 'SBHS', @opt25, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed25', @cnt, 'HSOS', @opt25, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed26', @cnt, 'SBHS', @opt26, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed26', @cnt, 'HSOS', @opt26, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed27', @cnt, 'SBHS', @opt27, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed27', @cnt, 'HSOS', @opt27, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed28', @cnt, 'SBHS', @opt28, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed28', @cnt, 'HSOS', @opt28, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed29', @cnt, 'SBHS', @opt29, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed29', @cnt, 'HSOS', @opt29, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed30', @cnt, 'SBHS', @opt30, 'N') 
	set @cnt = @cnt+1;
end
set @cnt = 1;
while @cnt < 31  begin
	insert into dbo.DistributeTask([Dt_Problem], [Dt_Seed], [Dt_Algorithm], [Dt_Optimal],[Dt_Status])
	values
	('pmed30', @cnt, 'HSOS', @opt30, 'N') 
	set @cnt = @cnt+1;
end