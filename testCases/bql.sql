
--Update all employee
update dbo.employee set employeeid = 100

--for loop, print index

DECLARE @index_middle
SET @index_middle = 1
WHILE @index_middle < 3
BEGIN
	print @index_middle
	SET @index_middle = @index_middle + 1
END
