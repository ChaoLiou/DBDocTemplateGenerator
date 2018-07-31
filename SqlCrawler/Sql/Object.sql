select o.Name
, o.Type
, o.type_desc as TypeDesc
, r.routine_definition as Definition
from sys.objects o
left outer join information_schema.routines r on r.routine_name = o.name
where O.is_ms_shipped = 0
and O.type_desc in ('SQL_TABLE_VALUED_FUNCTION', 'SQL_STORED_PROCEDURE', 'USER_TABLE')