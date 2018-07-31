SELECT object_name(object_id) AS ParentName
, parameter_id AS 'Order'
, name AS 'Name'
, type_name(user_type_id) AS 'Type'
, max_length AS 'Size'
, Precision
, Scale
, is_output as IsReturnParameter
FROM sys.parameters
where object_id in (select object_id from sys.objects o where O.is_ms_shipped = 0)