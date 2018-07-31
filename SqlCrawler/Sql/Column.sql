select object_name(object_id) as ParentName
, column_id as 'Order'
, name as 'Name'
, type_name(user_type_id) as 'Type'
, max_length AS 'Size'
, Precision
, Scale
, is_nullable as IsNullable
from sys.columns
where object_id in (select object_id from sys.objects o where O.is_ms_shipped = 0)