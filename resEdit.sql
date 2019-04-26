select CULTURE_ID from obi.culture where CULTURE_CODE = ;

--delete from OBI.RESOURCE_TEXT where RESOURCE_VALUE = 'v1' and CULTURE_ID = (select * from obi.culture where CULTURE_NAME ='de');
--select * from obi.culture where CULTURE_NAME ='hu';

select * from OBI.RESOURCE_TEXT where  RESOURCE_VALUE = 'v1' and CULTURE_ID = (select CULTURE_ID from obi.culture where CULTURE_NAME ='it');
  
---------------------------

select* from obi.vu_resources where  RESOURCE_KEY_ID = 27477; --  RESOURCE_VALUE ='v5';--RESOURCE_type ='v1' and RESOURCE_KEY =  'v1' and CULTURE_CODE = 'en';  --- VUE

select * from OBI.RESOURCE_TYPE where RESOURCE_TYPE = 'v1'; --RESOURCE_TYPE
select * from OBI.RESOURCE_key where RESOURCE_type = 'v1'; --RESOURCE_TYPE, RESOURCE_KEY
select * from OBI.RESOURCE_text where RESOURCE_KEY_ID = 27477; -- RESOURCE_value = 'v3'; --RESOURCE_VALUE, CULTURE_ID
